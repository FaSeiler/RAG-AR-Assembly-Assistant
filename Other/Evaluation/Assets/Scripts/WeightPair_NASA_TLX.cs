using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NASA_TLX
{
    public class WeightPair_NASA_TLX : MonoBehaviour
    {
        public Color enabledColor;
        public Color disabledColor;

        public Button buttonLeft;
        public Button buttonRight;

        public UnityEvent<int> OnSelectionChanged = new UnityEvent<int>();

        public void Start()
        {
            buttonLeft.image.color = disabledColor;
            buttonRight.image.color = disabledColor;

            buttonLeft.onClick.AddListener(LeftButtonClicked);
            buttonRight.onClick.AddListener(RightButtonClicked);
        }

        private void LeftButtonClicked()
        {
            buttonLeft.image.color = enabledColor;
            buttonRight.image.color = disabledColor;

            OnSelectionChanged.Invoke(0);
        }

        private void RightButtonClicked()
        {
            buttonRight.image.color = enabledColor;
            buttonLeft.image.color = disabledColor;

            OnSelectionChanged.Invoke(1);
        }
    }
}
