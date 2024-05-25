using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    // This script is used to manage UI windows
    // It is used to open and close windows
    // Classes can be derived from this class to create custom windows

    public Button openButton, closeButton;

    private void Awake()
    {
        if (openButton != null)
            openButton.onClick.AddListener(EnableWindow);
        if (closeButton != null)
            closeButton.onClick.AddListener(DisableWindow);
    }

    public void EnableWindow()
    {
        gameObject.SetActive(true);
    }

    public void DisableWindow()
    {
        gameObject.SetActive(false);
    }
}
