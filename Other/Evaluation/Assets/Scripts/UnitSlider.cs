using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Evaluation
{
    public class UnitSlider : MonoBehaviour//, IPointerDownHandler
    {
        public Slider slider;
        public Color interactedColor;

        public void Start()
        {
            slider.onValueChanged.AddListener((float value) =>
            {
                slider.targetGraphic.color = interactedColor;
            });
        }

        //// This will trigger when the user touches the slider handle
        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    slider.targetGraphic.color = interactedColor;
        //}
    }
}
