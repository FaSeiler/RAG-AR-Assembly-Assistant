using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using System.Collections;

public class ChatWithRAGUI : MonoBehaviour, ISpeechToTextListener
{
    [Header("Speech to Text")]
    public TMP_InputField questionInputField;
    public Button startSpeechToTextButton, stopSpeechToTextButton, sendRequestButton;
    public Slider voiceLevelSlider;
    public GameObject loadingRing;
    public bool preferOfflineRecognition;
    private float normalizedVoiceLevel;

    [Header("Properties")]
    public GameObject chatEntriesParent;
    public GameObject chatEntryPrefab;
    public GameObject placeHolderPrefab;
    public GameObject responseImageListPrefab;
    public GameObject responsePageNumberListPrefab;
    public ScrollRect scrollRect;

    private void Awake()
    {
        SpeechToText.Initialize("en-US");

        startSpeechToTextButton.onClick.AddListener(StartSpeechToText);
        sendRequestButton.onClick.AddListener(SendRAGRequest);

        if (stopSpeechToTextButton != null)
            stopSpeechToTextButton.onClick.AddListener(StopSpeechToText);
    }

    void Start()
    {
        ClearChat();
    }

    private void Update()
    {
        startSpeechToTextButton.interactable = SpeechToText.IsServiceAvailable(preferOfflineRecognition) && !SpeechToText.IsBusy();
        startSpeechToTextButton.gameObject.GetComponent<Image>().enabled = !startSpeechToTextButton.interactable;

        if (stopSpeechToTextButton != null)
            stopSpeechToTextButton.interactable = SpeechToText.IsBusy();

        if (ClientRAG.instance.isBusy)
        {
            sendRequestButton.interactable = false;
            loadingRing.SetActive(true);
        }
        else
        {
            sendRequestButton.interactable = !string.IsNullOrEmpty(questionInputField.text);
            loadingRing.SetActive(false);
        }

        // You may also apply some noise to the voice level for a more fluid animation (e.g. via Mathf.PerlinNoise)
        voiceLevelSlider.value = Mathf.Lerp(voiceLevelSlider.value, normalizedVoiceLevel, 15f * Time.unscaledDeltaTime);
    }

    public void ChangeLanguage(string preferredLanguage)
    {
        if (!SpeechToText.Initialize(preferredLanguage))
            questionInputField.text = "Couldn't initialize with language: " + preferredLanguage;
    }

    public void StartSpeechToText()
    {
        SpeechToText.RequestPermissionAsync((permission) =>
        {
            if (permission == SpeechToText.Permission.Granted)
            {
                if (SpeechToText.Start(this, preferOfflineRecognition: preferOfflineRecognition))
                    questionInputField.text = "";
                else
                    questionInputField.text = "Couldn't start speech recognition session!";
            }
            else
                questionInputField.text = "Permission is denied!";
        });
    }

    public void StopSpeechToText()
    {
        SpeechToText.ForceStop();
    }

    void ISpeechToTextListener.OnReadyForSpeech()
    {
        Debug.Log("OnReadyForSpeech");
    }

    void ISpeechToTextListener.OnBeginningOfSpeech()
    {
        Debug.Log("OnBeginningOfSpeech");
    }

    void ISpeechToTextListener.OnVoiceLevelChanged(float normalizedVoiceLevel)
    {
        // Note that On Android, voice detection starts with a beep sound and it can trigger this callback. You may want to ignore this callback for ~0.5s on Android.
        this.normalizedVoiceLevel = normalizedVoiceLevel;
    }

    void ISpeechToTextListener.OnPartialResultReceived(string spokenText)
    {
        //Debug.Log("OnPartialResultReceived: " + spokenText);
        questionInputField.text = spokenText;
    }

    void ISpeechToTextListener.OnResultReceived(string spokenText, int? errorCode)
    {
        //Debug.Log("OnResultReceived: " + spokenText + (errorCode.HasValue ? (" --- Error: " + errorCode) : ""));
        questionInputField.text = spokenText;
        normalizedVoiceLevel = 0f;

        // Recommended approach:
        // - If errorCode is 0, session was aborted via SpeechToText.Cancel. Handle the case appropriately.
        // - If errorCode is 9, notify the user that they must grant Microphone permission to the Google app and call SpeechToText.OpenGoogleAppSettings.
        // - If the speech session took shorter than 1 seconds (should be an error) or a null/empty spokenText is returned, prompt the user to try again (note that if
        //   errorCode is 6, then the user hasn't spoken and the session has timed out as expected).
    }

    DateTime before;
    DateTime after;

    public void SendRAGRequest()
    {
        string request = questionInputField.text;
        AddChatEntry(request, null, null, true);
        questionInputField.text = "";

        before = DateTime.Now; // Start of RAG request

        ClientRAG.instance.SendRequest(request, (responseData) =>
        {
            after = DateTime.Now; // End of RAG request
            AddChatEntry(responseData.text, responseData.decoded_images, responseData.page_numbers, false);
        });
    }

    public void AddChatEntry(string text, List<Texture2D> imageTextures, List<int> page_numbers, bool isUser)
    {
        if (!isUser)
        {
            TimeSpan duration = after.Subtract(before);
            Debug.Log("Duration in seconds: " + duration.Seconds);

            text += "\n\n<size=65%>(Response time: " + duration.Seconds + " seconds)";
        }



        GameObject chatEntryGO = Instantiate(chatEntryPrefab, chatEntriesParent.transform);
        ChatEntry chatEntry = chatEntryGO.GetComponent<ChatEntry>();

        chatEntry.SetText(text, isUser);

        if (imageTextures != null )
        {
            AddImageList(imageTextures);
        }

        if (page_numbers != null)
        {
            AddPageNumbersList(page_numbers);
        }

        if (!isUser) // Add placeholder after assistant response
        {
            Instantiate(placeHolderPrefab, chatEntriesParent.transform);
        }

        StartCoroutine(ScrollToBottom());
    }



    public void AddImageList(List<Texture2D> imageTextures)
    {
        if (imageTextures.Count > 0)
        {
            GameObject responseImageListGO = Instantiate(responseImageListPrefab, chatEntriesParent.transform);
            responseImageListGO.GetComponent<GridLayoutGroup>().enabled = true;
            responseImageListGO.GetComponent<RAGResponseImageListManager>().enabled = true;

            foreach (Texture2D imageTexture in imageTextures)
            {
                responseImageListGO.GetComponent<RAGResponseImageListManager>().AddRawImage(imageTexture);
            }
        }
    }

    private void AddPageNumbersList(List<int> page_numbers)
    {
        if (page_numbers.Count > 0)
        {
            GameObject responsePageNumberListGO = Instantiate(responsePageNumberListPrefab, chatEntriesParent.transform);   
            responsePageNumberListGO.GetComponent<GridLayoutGroup>().enabled = true;
            responsePageNumberListGO.GetComponent<RAGResponsePageNumberListManager>().enabled = true;

            foreach (int page_number in page_numbers)
            {
                responsePageNumberListGO.GetComponent<RAGResponsePageNumberListManager>().AddPageNumberButton(page_number);
            }
        }
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ClearChat()
    {
        foreach (Transform child in chatEntriesParent.transform)
            Destroy(child.gameObject);
    }
}