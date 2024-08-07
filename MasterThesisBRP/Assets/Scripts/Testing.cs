using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.EventSystems;

public class Testing : MonoBehaviour
{
    public RectTransform contentRectTransform; // The RectTransform of the Content GameObject
    public RectTransform scrollViewRectTransform; // The RectTransform of the ScrollView GameObject

    void Start()
    {
        AdjustScrollViewSize();
    }

    void Update()
    {
        AdjustScrollViewSize();
    }

    void AdjustScrollViewSize()
    {
        // Ensure the scroll view size matches the content size
        float contentHeight = contentRectTransform.rect.height;
        scrollViewRectTransform.sizeDelta = new Vector2(scrollViewRectTransform.sizeDelta.x, contentHeight);
    }
}
