using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatWithRAGUI : MonoBehaviour, ISpeechToTextListener
{
    public TextMeshProUGUI speechText;
    public Button startSpeechToTextButton, stopSpeechToTextButton, sendRequestButton;
    public Slider voiceLevelSlider;
    public bool preferOfflineRecognition;

    private float normalizedVoiceLevel;

    public GameObject chatEntriesParent;
    public GameObject chatEntryPrefab;
    public GameObject placeHolderPrefab;

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

    // TODO Disable controls while processing the request and display loading animation

    private void Update()
    {
        startSpeechToTextButton.interactable = SpeechToText.IsServiceAvailable(preferOfflineRecognition) && !SpeechToText.IsBusy();
        
        if (stopSpeechToTextButton != null)
            stopSpeechToTextButton.interactable = SpeechToText.IsBusy();

        // You may also apply some noise to the voice level for a more fluid animation (e.g. via Mathf.PerlinNoise)
        voiceLevelSlider.value = Mathf.Lerp(voiceLevelSlider.value, normalizedVoiceLevel, 15f * Time.unscaledDeltaTime);
    }

    public void ChangeLanguage(string preferredLanguage)
    {
        if (!SpeechToText.Initialize(preferredLanguage))
            speechText.text = "Couldn't initialize with language: " + preferredLanguage;
    }

    public void StartSpeechToText()
    {
        SpeechToText.RequestPermissionAsync((permission) =>
        {
            if (permission == SpeechToText.Permission.Granted)
            {
                if (SpeechToText.Start(this, preferOfflineRecognition: preferOfflineRecognition))
                    speechText.text = "";
                else
                    speechText.text = "Couldn't start speech recognition session!";
            }
            else
                speechText.text = "Permission is denied!";
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
        Debug.Log("OnPartialResultReceived: " + spokenText);
        speechText.text = spokenText;
    }

    void ISpeechToTextListener.OnResultReceived(string spokenText, int? errorCode)
    {
        Debug.Log("OnResultReceived: " + spokenText + (errorCode.HasValue ? (" --- Error: " + errorCode) : ""));
        speechText.text = spokenText;
        normalizedVoiceLevel = 0f;

        // Recommended approach:
        // - If errorCode is 0, session was aborted via SpeechToText.Cancel. Handle the case appropriately.
        // - If errorCode is 9, notify the user that they must grant Microphone permission to the Google app and call SpeechToText.OpenGoogleAppSettings.
        // - If the speech session took shorter than 1 seconds (should be an error) or a null/empty spokenText is returned, prompt the user to try again (note that if
        //   errorCode is 6, then the user hasn't spoken and the session has timed out as expected).
    }

    public void SendRAGRequest()
    {
        string request = speechText.text;
        AddChatEntry(request, true);


        //ClientRAG.instance.SendRequest(request, (response) =>
        //{
        //    AddChatEntry(response, false);
        //});
    }

    public void AddChatEntry(string text, bool isUser)
    {
        GameObject chatEntryGO = Instantiate(chatEntryPrefab, chatEntriesParent.transform);
        ChatEntry chatEntry = chatEntryGO.GetComponent<ChatEntry>();

        chatEntry.SetText(text, isUser);

        if (!isUser) // Add placeholder after assistant response
        {
            Instantiate(placeHolderPrefab, chatEntriesParent.transform);
        }
    }

    public void ClearChat()
    {
        foreach (Transform child in chatEntriesParent.transform)
            Destroy(child.gameObject);
    }
}