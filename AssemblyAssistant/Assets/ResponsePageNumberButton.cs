using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResponsePageNumberButton : MonoBehaviour
{
    public int pageNumber;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    internal void SetPageNumber(int pageNumber)
    {
        this.pageNumber = pageNumber;
        buttonText.text = "Page " + pageNumber.ToString();
    }

    public void OpenPDFOnPageNumber()
    {
        PDFViewerUIManager.instance.OpenPDFViewerOnPager(pageNumber);
    }
}
