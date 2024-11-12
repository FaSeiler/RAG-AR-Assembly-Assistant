//using BarcodeScanner.Scanner;
//using BarcodeScanner;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using Wizcorp.Utils.Logger;
//using System;

//public class BarcodeScannerSIMATIC : MonoBehaviour
//{
//    private IScanner BarcodeScanner;
//    public Text TextHeader;
//    public RawImage Image;
//    public WebScraperSIMATIC webScraperSIMATIC;

//    // Disable Screen Rotation on that screen
//    //void Awake()
//    //{
//    //    Screen.autorotateToPortrait = false;
//    //    Screen.autorotateToPortraitUpsideDown = false;
//    //}

//    void Start()
//    {
//        webScraperSIMATIC = GetComponent<WebScraperSIMATIC>();

//        // Create a basic scanner
//        BarcodeScanner = new Scanner();
//        BarcodeScanner.Camera.Play();

//        // Display the camera texture through a RawImage
//        BarcodeScanner.OnReady += (sender, arg) => {
//            // Set Orientation & Texture
//            Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
//            Image.transform.localScale = BarcodeScanner.Camera.GetScale();
//            Image.texture = BarcodeScanner.Camera.Texture;

//            // Keep Image Aspect Ratio
//            var rect = Image.GetComponent<RectTransform>();
//            var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
//            rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
//        };

//        // Track status of the scanner
//        BarcodeScanner.StatusChanged += (sender, arg) => {
//            TextHeader.text = "Status: " + BarcodeScanner.Status;
//        };
//    }
//    private void OnEnable()
//    {
//        if (BarcodeScanner != null)
//        {
//            BarcodeScanner.Camera.Play();
//            StartScanning();
//        }
//    }

//    private void OnDisable()
//    {
//        // Destroy the camera to allow Vuforia to use it
//        BarcodeScanner.Camera.Stop();
//        BarcodeScanner.Camera.Destroy();
//    }

//    /// <summary>
//    /// The Update method from unity need to be propagated to the scanner
//    /// </summary>
//    void Update()
//    {
//        if (BarcodeScanner == null)
//        {
//            return;
//        }

//        BarcodeScanner.Update();
//    }

//    public void StartScanning()
//    {
//        if (BarcodeScanner == null)
//        {
//            Log.Warning("No valid camera - Click Start");
//            return;
//        }

//        // Start Scanning
//        BarcodeScanner.Scan((barCodeType, barCodeValue) => {
//            BarcodeScanner.Stop();
//            TextHeader.text = "Found: " + barCodeType + " / " + barCodeValue;

//            string articleNumber = GetArticleNumberFromBarcode(barCodeValue);
//            webScraperSIMATIC.StartScraping(articleNumber);

//#if UNITY_ANDROID || UNITY_IOS
//            Handheld.Vibrate();
//#endif
//        });
//    }

//    public void StopScanning()
//    {
//        if (BarcodeScanner == null)
//        {
//            Log.Warning("No valid camera - Click Stop");
//            return;
//        }

//        // Stop Scanning
//        BarcodeScanner.Stop();
//    }

//    /// <summary>
//    /// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
//    /// Trying to stop the camera in OnDestroy provoke random crash on Android
//    /// </summary>
//    /// <param name="callback"></param>
//    /// <returns></returns>
//    public IEnumerator StopCamera(Action callback)
//    {
//        // Stop Scanning
//        Image = null;
//        BarcodeScanner.Destroy();
//        BarcodeScanner = null;

//        // Wait a bit
//        yield return new WaitForSeconds(0.1f);

//        callback.Invoke();
//    }

//    public string GetArticleNumberFromBarcode(string barcode)
//    {
//        // Sample barcode 1: 1P6AG1212-1AE31-2XB0+ -> Article Number 1: 6AG1212-1AE31-2XB0
//        // Sample barcode 2: 1P6ES7155-6AR00-0AN0+23S286336D72F96+SC-J8MK1502 -> Article Number 2: 6ES7155-6AR00-0AN0
//        // Infer article number from barcode
//        string articleNumber = barcode.Substring(2, barcode.IndexOf('+') - 2);
//        return articleNumber;
//    }
//}
