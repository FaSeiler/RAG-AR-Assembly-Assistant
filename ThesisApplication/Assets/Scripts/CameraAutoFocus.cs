using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraAutoFocus : MonoBehaviour
{
    void Start()
    {
        var vuforia = VuforiaApplication.Instance;
        vuforia.OnVuforiaStarted += OnVuforiaStarted;
        vuforia.OnVuforiaPaused += OnPaused;
    }

    private void OnVuforiaStarted()
    {
        VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }

    private int currentFocusMode = 1;

    public void IterateFocusMode()
    {
        SetFocusMode((currentFocusMode % 5) + 1);
    }

    public void SetFocusMode(int mode) // 1 to 5
    {
        VuforiaBehaviour.Instance.CameraDevice.SetFocusMode((FocusMode)mode);
        currentFocusMode = mode;
        DebugUI.WriteLog("CameraAutoFocus: Set focus mode to " + (FocusMode)mode);
    }
}
