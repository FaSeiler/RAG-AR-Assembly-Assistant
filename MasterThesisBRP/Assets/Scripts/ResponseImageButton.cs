using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseImageButton : MonoBehaviour
{
    public void OpenInFullScreen()
    {
        Texture2D texture = GetComponent<RawImage>().texture as Texture2D;
        ResponseImageFullScreenManager.instance.OpenFullScreenWindow(texture);
    }
}
