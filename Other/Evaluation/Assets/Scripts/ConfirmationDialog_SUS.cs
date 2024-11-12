using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SUS
{
    public class ConfirmationDialog_SUS : WindowManager
    {
        public TextMeshProUGUI text;
        public Button buttonNo;
        public Button buttonYes;

        // Delegate to store the confirmation action
        public UnityEvent onConfirmEvent;
        public UnityEvent onConfirmCancelEvent;

        private void Start()
        {
            buttonNo.onClick.AddListener(() =>
            {
                onConfirmCancelEvent?.Invoke();
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
