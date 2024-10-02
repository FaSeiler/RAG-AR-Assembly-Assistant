using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SUS
{
    public class Navigation_SUS : MonoBehaviour
    {
        public Button buttonNext;
        public Button buttonPrevious;

        public List<GameObject> windows = new List<GameObject>();

        private int currentWindowIndex = 0;

        public void Start()
        {
            buttonNext.onClick.AddListener(NextButtonClicked);
            buttonPrevious.onClick.AddListener(PreviousButtonClicked);

            ShowWindow(currentWindowIndex);
            UpdateNextPreviousButtonVisibility();
        }

        private void NextButtonClicked()
        {
            if (currentWindowIndex < windows.Count - 1)
            {
                HideWindow(currentWindowIndex);
                currentWindowIndex++;
                ShowWindow(currentWindowIndex);
            }

            UpdateNextPreviousButtonVisibility();
        }

        private void PreviousButtonClicked()
        {
            if (currentWindowIndex > 0)
            {
                HideWindow(currentWindowIndex);
                currentWindowIndex--;
                ShowWindow(currentWindowIndex);
            }

            UpdateNextPreviousButtonVisibility();
        }

        private void UpdateNextPreviousButtonVisibility()
        {
            if (currentWindowIndex == windows.Count - 1)
            {
                buttonNext.gameObject.SetActive(false);
            }
            else
            {
                buttonNext.gameObject.SetActive(true);
            }

            if (currentWindowIndex == 0)
            {
                buttonPrevious.gameObject.SetActive(false);
            }
            else
            {
                buttonPrevious.gameObject.SetActive(true);
            }
        }


        private void ShowWindow(int index)
        {
            windows[index].SetActive(true);
        }

        private void HideWindow(int index)
        {
            windows[index].SetActive(false);
        }
    }
}
