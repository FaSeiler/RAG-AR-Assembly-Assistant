using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsUI : WindowManager
{
    [Header("Settings UI")]
    public TMP_InputField ipInputField;

    private void Start()
    {
        ipInputField.text = ClientRAG.instance.BASE_URL;
        ipInputField.onEndEdit.AddListener(UpdateIP);
    }

    private void UpdateIP(string newIPAdress)
    {
        ClientRAG.instance.BASE_URL = newIPAdress;
    }
}
