using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NASA_TLX
{
    public class ConfirmationDialog_NASA_TLX : WindowManager
    {
        public TextMeshProUGUI text;
        public Button buttonNo;
        public Button buttonYes;

        // Delegate to store the confirmation action
        public UnityEvent onConfirmEvent;

        private void Start()
        {
            buttonNo.onClick.AddListener(() =>
            {
                DisableWindow();
            });

            buttonYes.onClick.AddListener(() =>
            {
                // Execute the stored action
                onConfirmEvent?.Invoke();
                DisableWindow();
            });
        }

        public void ShowConfirmationDialog()
        {
            EnableWindow();
        }

        public void HideConfirmationDialog()
        {
            DisableWindow();
        }
    }
}
