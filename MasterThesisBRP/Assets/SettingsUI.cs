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

    private void Start()
    {
        ipInputField.text = ClientRAG.instance.BASE_URL;
        ipInputField.onEndEdit.AddListener(UpdateIP);
        localhostToggle.onValueChanged.AddListener(OnLocalhostToggleValueChanged);
    }

    private void OnLocalhostToggleValueChanged(bool newValue)
    {
        if (newValue)
        {
            UpdateIP("http://127.0.0.1:5000");
        }
    }

    private void UpdateIP(string newIPAdress)
    {
        ipInputField.text = newIPAdress;
        ClientRAG.instance.BASE_URL = newIPAdress;
    }
}
