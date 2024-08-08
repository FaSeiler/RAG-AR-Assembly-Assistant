using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionStepUIManager : WindowManager
{
    public GameObject chatEntriesParent;
    public GameObject chatEntryPrefab;
    public GameObject fullscreenImageWindowGO; // Needed?
    public GameObject responseImageListPrefab;
    public GameObject responsePageNumberListPrefab;
    public GameObject chatPlaceHolderPrefab;
    public GameObject instructionStepHeaderPrefab;
    public ScrollRect scrollRect;

    public void UpdateInstructionUI(InstructionStep instructionStep, int instructionStepIndex, int totalInstuctionStepCount)
    {
        ClearText();
        AddInstructionStepHeader(instructionStep, instructionStepIndex, totalInstuctionStepCount);
        //AddPlaceHolder();
        AddChatEntry(instructionStep.instruction.text, instructionStep.instruction.images, instructionStep.instruction.pageNumbers);
    }

    private void AddInstructionStepHeader(InstructionStep instructionStep, int currentInstructionStepIndex, int totalInstuctionSepCount)
    {
        GameObject instructionStepHeaderGO = Instantiate(instructionStepHeaderPrefab, chatEntriesParent.transform);

        string newText = "Step {0}/{1} <indent=15%>Art. Name:</indent><indent=35%> {2}</indent>\r\n<indent=15%>Art. Number:</indent><indent=35%> {3}</indent>";
        newText = string.Format(newText, currentInstructionStepIndex, totalInstuctionSepCount, instructionStep.component.componentName, instructionStep.component.articleNumber);

        instructionStepHeaderGO.GetComponent<TextMeshProUGUI>().text = newText;
    }

    private void AddPlaceHolder()
    {
        Instantiate(chatPlaceHolderPrefab, chatEntriesParent.transform);
    }

    private void AddChatEntry(string text, List<Texture2D> imageTextures, List<int> page_numbers)
    {
        GameObject chatEntryGO = Instantiate(chatEntryPrefab, chatEntriesParent.transform);
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

    public void ClearText()
    {
        foreach (Transform child in chatEntriesParent.transform)
            Destroy(child.gameObject);
    }
}
