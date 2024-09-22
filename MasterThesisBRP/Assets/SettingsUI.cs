using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : WindowManager
{
    [Header("Settings UI")]
    public TMP_InputField ipInputField;
    public Toggle localhostToggle;
    
    private string lastIPAdress; // Save this to switch between localhost and the last IP adress

    private void Start()
    {
        ipInputField.text = ClientRAG.instance.BASE_URL;
        lastIPAdress = ipInputField.text;
        ipInputField.onEndEdit.AddListener(UpdateIP);
        localhostToggle.onValueChanged.AddListener(OnLocalhostToggleValueChanged);
    }

    private void OnLocalhostToggleValueChanged(bool enabled)
    {
        if (enabled)
        {
            lastIPAdress = ipInputField.text;
            UpdateIP("http://127.0.0.1:5000");
        }
        else
        {
            UpdateIP(lastIPAdress);
        }
    }

    private void UpdateIP(string newIPAdress)
    {
        ipInputField.text = newIPAdress;
        ClientRAG.instance.BASE_URL = newIPAdress;
    }
}
