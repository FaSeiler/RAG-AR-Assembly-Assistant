using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextMeshProLinkClickDetector : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(GetComponent<TextMeshProUGUI>(), Input.mousePosition, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = GetComponent<TextMeshProUGUI>().textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
