using System.Collections;
using System.Net.Http;
using UnityEngine;
using HtmlAgilityPack;
using Vuforia;

public class Testing : MonoBehaviour
{
    public GameObject scannerPanel;


    // Function to pause Vuforia and release the camera device
    public void PauseVuforia()
    {
        // Stop Vuforia engine
        VuforiaBehaviour.Instance.enabled = false;
    }

    // Function to resume Vuforia
    public void ResumeVuforia()
    {
        // Restart Vuforia engine
        VuforiaBehaviour.Instance.enabled = true;
    }

    public void ToggleScannerPanel()
    {
        if (scannerPanel.activeSelf)
        {
            scannerPanel.SetActive(false);
            ResumeVuforia();
        }
        else
        {
            PauseVuforia();
            scannerPanel.SetActive(true);
        }
    }
}
