using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseImageFullScreenManager : Singleton<ResponseImageFullScreenManager>
{
    public GameObject root;
    public RawImage rawImage;

    public void OpenFullScreenWindow(Texture2D texture)
    {
        root.SetActive(true);
        rawImage.texture = texture;
        rawImage.SetNativeSize();
    }

    public void CloseFullScreenWindow()
    {
        root.SetActive(false);
    }
}
