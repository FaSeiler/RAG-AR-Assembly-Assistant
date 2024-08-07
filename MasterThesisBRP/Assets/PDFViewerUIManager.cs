using Paroxe.PdfRenderer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDFViewerUIManager : WindowManager
{
    public static PDFViewerUIManager instance; // Workaround because we can't use Singleton pattern because of WindowManager inheritance

    private void Awake()
    {
        instance = this;
    }

    public PDFViewer pdfViewer;

    public void OpenPDFViewerOnPager(int pageNumber)
    {
        StartCoroutine(OpenPDFViewerOnPagerCoroutine(pageNumber));
    }

    private IEnumerator OpenPDFViewerOnPagerCoroutine(int pageNumber)
    {
        EnableWindow();
        while (!pdfViewer.IsLoaded)
            yield return null;

        pdfViewer.GoToPage(pageNumber - 1);
    }
}
