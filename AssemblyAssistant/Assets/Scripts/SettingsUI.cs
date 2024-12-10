using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : WindowManager
{
    [Header("Settings UI")]
    [Header("IP")]
    public TMP_InputField ipInputField;
    public Toggle localhostToggle;
    private string lastIPAdress; // Save this to switch between localhost and the last IP adress

    [Header("Visualizations")]
    public Toggle toggleAssembledComponents;
    public Toggle toggleScanPreviews;
    public GameObject scanPreviewsParent;

    [Header("TrackingWarning")]
    public Toggle toggleTrackingWarning;

    [Header("Regenerate Instructions")]
    public Button regenerateInstructionsButton;


    private void Start()
    {
        InitIpUI();
        InitVisualizationsUI();
        InitTrackingWarningUI();
        InitInstructionRegenerationUI();
    }

    #region VISUALIZATIONS
    private void InitVisualizationsUI()
    {
        toggleAssembledComponents.onValueChanged.AddListener(OnAssembledComponentsToggleChanged);
        toggleScanPreviews.onValueChanged.AddListener(OnScanPreviewsToggleChanged);
    }

    private void OnScanPreviewsToggleChanged(bool value)
    {
        if (value)
        {
            scanPreviewsParent.SetActive(true);
        }
        else
        {
            scanPreviewsParent.SetActive(false);
        }
    }

    private void OnAssembledComponentsToggleChanged(bool value)
    {
        if (value)
        {
            TrackingManager.instance.ShowLoggedInComponents();
        }
        else
        {
            TrackingManager.instance.HideLoggedInComponents();
        }
    }
    #endregion

    #region TRACKING_WARNING
    private void InitTrackingWarningUI()
    {
        if (toggleTrackingWarning.isOn)
        {
            PlayerPrefs.SetInt("TrackingWarning", 1);
        }
        else
        {
            PlayerPrefs.SetInt("TrackingWarning", 0);
        }

        toggleTrackingWarning.onValueChanged.AddListener(OnTrackingWarningToggleChanged);
    }

    private void OnTrackingWarningToggleChanged(bool value)
    {
        if (value)
        {
            PlayerPrefs.SetInt("TrackingWarning", 1);
        }
        else
        {
            PlayerPrefs.SetInt("TrackingWarning", 0);
        }
    }
    #endregion

    #region IP_ADRESS
    private void InitIpUI()
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

    private void OnEnable()
    {
        if (ClientRAG.instance.BASE_URL != ipInputField.text)
        {
            ipInputField.text = ClientRAG.instance.BASE_URL;
        }
    }

    private void UpdateIP(string newIPAdress)
    {
        ipInputField.text = newIPAdress;
        ClientRAG.instance.BASE_URL = newIPAdress;
        
        PlayerPrefs.SetString("ServerIP", ClientRAG.instance.BASE_URL);
        PlayerPrefs.Save();

    }
    #endregion

    #region REGENERATE_INSTRUCTIONS
    private void InitInstructionRegenerationUI()
    {
        regenerateInstructionsButton.onClick.AddListener(OnRegenerateInstructionsButtonClicked);
    }

    private void OnRegenerateInstructionsButtonClicked()
    {
        InstructionGenerator.instance.ReGenerateAssemblyInstructions();
    }
    #endregion
}
