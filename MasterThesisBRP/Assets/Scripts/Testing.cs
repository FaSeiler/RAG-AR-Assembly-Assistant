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

    public TextMeshProUGUI text;

    public void Start()
    {
        //string originalText = "Here is a link to a website: ({0}).";
        //originalText = string.Format(originalText, WebsiteUrl);

        string modifiedText = URLDetector.ReplaceUrlsWithLinks(text);
        text.text = modifiedText;

    }
}
