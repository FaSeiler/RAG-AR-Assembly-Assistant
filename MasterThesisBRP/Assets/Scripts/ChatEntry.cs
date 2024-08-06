using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatEntry : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SetText(string text, bool isUser)
    {
        this.text.text = text;
        string modifiedText = URLDetector.FormatTextMeshProForRichTextFormat(this.text);

        if (isUser)
            this.text.text = "<b>You:</b><indent=12%>" + modifiedText + "</indent>";
        else
            this.text.text = "<b>Assistant:</b><indent=12%>" + modifiedText + "</indent>";
    }
}
