using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public static DebugUI uiDebugText;

    private static TextMeshProUGUI tmproText;

    void Awake()
    {
        if (uiDebugText != null)
            GameObject.Destroy(uiDebugText);
        else
            uiDebugText = this;

        DontDestroyOnLoad(this);

        tmproText = GetComponent<TextMeshProUGUI>();
        tmproText.text = "";
    }

    /// <summary>
    /// Overrides the old logged texts with the given string.
    /// </summary>
    /// <param name="debugText"></param>
    public static void WriteLog(string debugText)
    {
        tmproText.text = debugText;
    }

    /// <summary>
    /// Adds the new log string to the existing log.
    /// </summary>
    /// <param name="debugText"></param>
    public static void AddLog(string debugText)
    {
        tmproText.text += debugText + "\n";
    }

    /// <summary>
    /// Resets the log text.
    /// </summary>
    public static void ResetLog()
    {
        tmproText.text = "";
    }
}
