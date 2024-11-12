using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationDialogUI : WindowManager
{
    public TextMeshProUGUI text;
    public Button buttonNo;
    public Button buttonYes;

    // Delegate to store the confirmation action
    private System.Action onConfirmAction;

    private void Start()
    {
        buttonNo.onClick.AddListener(() =>
        {
            DisableWindow();
        });

        buttonYes.onClick.AddListener(() =>
        {
            // Execute the stored action
            onConfirmAction?.Invoke();
            DisableWindow();
        });
    }

    public void ShowConfirmationDialog(string message, System.Action confirmAction)
    {
        this.text.text = message;
        // Store the action to be executed when confirmed
        onConfirmAction = confirmAction;
        EnableWindow();
    }

    public void HideConfirmationDialog()
    {
        DisableWindow();
    }
}
