using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionStepUIManager : WindowManager
{
    public GameObject instructionUIParent;
    public GameObject chatEntryPrefab;
    public GameObject responseImageListPrefab;
    public GameObject responsePageNumberListPrefab;
    public GameObject chatPlaceHolderPrefab;
    public GameObject instructionStepHeaderPrefab;
    public ScrollRect scrollRect;
    public Button nextButton;
    public Button previousButton;

    private List<GameObject> instructionBody = new List<GameObject>(); // Everything that is part of the instruction except the header
    private GameObject chatPlaceHolderInstance;

    public void UpdateInstructionUI(InstructionStep instructionStep, int instructionStepIndex, int totalInstuctionStepCount)
    {
        ClearInstructionContent();
        AddInstructionStepHeader(instructionStep, instructionStepIndex + 1, totalInstuctionStepCount);
        AddPlaceHolder();
        SetInstructionContent(instructionStep.instruction.text, instructionStep.instruction.images, instructionStep.instruction.pageNumbers);
        UpdateStepControlButtons(instructionStepIndex, totalInstuctionStepCount);
    }

    private void AddInstructionStepHeader(InstructionStep instructionStep, int currentInstructionStepIndex, int totalInstuctionSepCount)
    {
        GameObject instructionStepHeaderGO = Instantiate(instructionStepHeaderPrefab, instructionUIParent.transform);

        string newText = "Step {0}/{1} <indent=15%>Art. Name:</indent><indent=35%> {2}</indent>\r\n<indent=15%>Art. Number:</indent><indent=35%> {3}</indent>";
        newText = string.Format(newText, currentInstructionStepIndex, totalInstuctionSepCount, instructionStep.component.componentName, instructionStep.component.articleNumber);

        instructionStepHeaderGO.GetComponent<TextMeshProUGUI>().text = newText;
    }

    private void AddPlaceHolder()
    {
        chatPlaceHolderInstance = Instantiate(chatPlaceHolderPrefab, instructionUIParent.transform);
        chatPlaceHolderInstance.SetActive(false);
    }

    private void SetInstructionContent(string text, List<Texture2D> imageTextures, List<int> page_numbers)
    {
        instructionBody.Clear();

        GameObject chatEntryGO = Instantiate(chatEntryPrefab, instructionUIParent.transform);
        instructionBody.Add(chatEntryGO);

        ChatEntry chatEntry = chatEntryGO.GetComponent<ChatEntry>();

        chatEntry.SetTextInstruction(text);

        if (imageTextures != null)
        {
            AddImageList(imageTextures);
        }

        if (page_numbers != null)
        {
            AddPageNumbersList(page_numbers);
        }

        StartCoroutine(ScrollToTop());
    }

    private void AddImageList(List<Texture2D> imageTextures)
    {
        if (imageTextures.Count > 0)
        {
            GameObject responseImageListGO = Instantiate(responseImageListPrefab, instructionUIParent.transform);
            responseImageListGO.GetComponent<GridLayoutGroup>().enabled = true;
            responseImageListGO.GetComponent<RAGResponseImageListManager>().enabled = true;

            foreach (Texture2D imageTexture in imageTextures)
            {
                responseImageListGO.GetComponent<RAGResponseImageListManager>().AddRawImage(imageTexture);
            }

            instructionBody.Add(responseImageListGO);
        }
    }

    private void AddPageNumbersList(List<int> page_numbers)
    {
        if (page_numbers.Count > 0)
        {
            GameObject responsePageNumberListGO = Instantiate(responsePageNumberListPrefab, instructionUIParent.transform);
            responsePageNumberListGO.GetComponent<GridLayoutGroup>().enabled = true;
            responsePageNumberListGO.GetComponent<RAGResponsePageNumberListManager>().enabled = true;

            List<int> addedPageNumbers = new List<int>();
            foreach (int page_number in page_numbers)
            {
                if (!addedPageNumbers.Contains(page_number))
                {
                    responsePageNumberListGO.GetComponent<RAGResponsePageNumberListManager>().AddPageNumberButton(page_number);
                    addedPageNumbers.Add(page_number);
                }
            }

            instructionBody.Add(responsePageNumberListGO);
        }
    }

    public void EnableInstructionBody()
    {
        foreach (GameObject instructionBodyGO in instructionBody)
        {
            instructionBodyGO.SetActive(true);
        }

        chatPlaceHolderInstance.SetActive(false);
    }

    public void DisableInstructionBody()
    {
        foreach (GameObject instructionBodyGO in instructionBody)
        {
            instructionBodyGO.SetActive(false);
        }

        chatPlaceHolderInstance.SetActive(true);
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private IEnumerator ScrollToTop()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void ClearInstructionContent()
    {
        foreach (Transform child in instructionUIParent.transform)
            Destroy(child.gameObject);
    }

    internal void UpdateStepControlButtons(int currentInstructionStepIndex, int totalInstructionStepCount)
    {
        if (currentInstructionStepIndex == 0)
        {
            HidePreviousButton();
        }
        else
        {
            ShowPreviousButton();
        }

        if (currentInstructionStepIndex == totalInstructionStepCount - 1)
        {
            HideNextButton();
        }
        else
        {
            ShowNextButton();
        }
    }


    public void ShowNextButton()
    {
        nextButton.gameObject.SetActive(true);
    }

    public void ShowPreviousButton()
    {
        previousButton.gameObject.SetActive(true);
    }

    public void HideNextButton()
    {
        nextButton.gameObject.SetActive(false);
    }

    public void HidePreviousButton()
    {
        previousButton.gameObject.SetActive(false);
    }
}
