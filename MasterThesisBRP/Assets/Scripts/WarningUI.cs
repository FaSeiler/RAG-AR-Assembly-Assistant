using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningUI : WindowManager
{
    public static WarningUI instance;
    public TextMeshProUGUI warningText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowWarning(string warningMessage)
    {
        warningText.text = "<b>Warning:</b> " + warningMessage;
        
        EnableWindow();
    }

    public void HideWarning()
    {
        DisableWindow();
    }
}
