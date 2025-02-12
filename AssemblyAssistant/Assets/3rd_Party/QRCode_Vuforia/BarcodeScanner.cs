using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.UI;
using Vuforia;

using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using TMPro;

[RequireComponent(typeof(WebScraperSIMATIC))]
public class BarcodeScanner : MonoBehaviour
{

    public bool lookingForQRCode = false;
    public TextMeshProUGUI qrCodeText;
    public TextMeshProUGUI articleInfoText;

    public VuforiaBehaviour vuforiaBehaviour;

    private bool cameraInitialized;
    private BarcodeReader barCodeReader;
    private WebScraperSIMATIC webScraperSIMATIC;


    // Use this for initialization
    void Start()
    {
        webScraperSIMATIC = GetComponent<WebScraperSIMATIC>();
        //Barcode reader instance
        barCodeReader = new BarcodeReader();
        StartCoroutine(InitializeCamera());
    }

    //Connect the camera with the barcode scanner
    private IEnumerator InitializeCamera()
    {
        // Waiting a little seem to avoid the Vuforia's crashes.
        yield return new WaitForSeconds(1.25f);


        var isFrameFormatSet = vuforiaBehaviour.CameraDevice.SetFrameFormat(PixelFormat.RGB888, true);

        // Force autofocus.
        var isAutoFocus = vuforiaBehaviour.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (!isAutoFocus)
        {
            vuforiaBehaviour.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_FIXED);
        }
        cameraInitialized = true;
        lookingForQRCode = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (cameraInitialized && lookingForQRCode)
        {
            try
            {
                var cameraFeed = vuforiaBehaviour.CameraDevice.GetCameraImage(PixelFormat.RGB888);
                if (cameraFeed == null)
                {
                    return;
                }
                var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.RGB24);
                    if (data != null)
                {
                    // QRCode detected.
                    qrCodeText.text = data.Text;

                    //string articleNumber = GetArticleNumberFromBarcode(data.Text);
                    //articleInfoText.text = articleNumber;

                    //webScraperSIMATIC.StartScraping(articleNumber);


                    lookingForQRCode = false;
                }
            }
            catch (Exception e)
            {
                //Debug.LogError(e.Message);
            }
        }
    }

    public string GetArticleNumberFromBarcode(string barcode)
    {
        // Sample barcode 1: 1P6AG1212-1AE31-2XB0+ -> Article Number 1: 6AG1212-1AE31-2XB0
        // Sample barcode 2: 1P6ES7155-6AR00-0AN0+23S286336D72F96+SC-J8MK1502 -> Article Number 2: 6ES7155-6AR00-0AN0
        // Infer article number from barcode
        string articleNumber = barcode.Substring(2, barcode.IndexOf('+') - 2);
        return articleNumber;
    }

    public void ResetFoundQRCode()
    {
        lookingForQRCode = true;
        qrCodeText.text = "";
        articleInfoText.text = "";
    }
}
