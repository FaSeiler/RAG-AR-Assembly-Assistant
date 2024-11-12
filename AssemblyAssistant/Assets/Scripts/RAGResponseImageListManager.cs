using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RAGResponseImageListManager : MonoBehaviour
{
    public GameObject rawImagePrefab;

    public void AddRawImage(Texture2D imageTexture)
    {
        GameObject rawImageGO = Instantiate(rawImagePrefab, transform, false);
        RawImage rawImage = rawImageGO.GetComponent<RawImage>();
        rawImage.texture = imageTexture;
    }
}
