using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    // This script is used to manage UI windows
    // It is used to open and close windows
    // Classes can be derived from this class to create custom windows

    [Header("Window Manager")]
    public GameObject window;
    [HideInInspector]
    public UnityEvent OnWindowEnabled = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnWindowDisabled = new UnityEvent();


    public void EnableWindow()
    {
        window.SetActive(true);
        OnWindowEnabled.Invoke();
    }

    public void DisableWindow()
    {
        window.SetActive(false);
        OnWindowDisabled.Invoke();
    }
}
