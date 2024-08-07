using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RAGResponsePageNumberListManager : MonoBehaviour
{
    public GameObject pageNumberButtonPrefab;

    public void AddPageNumberButton(int pageNumber)
    {
        GameObject pageNumberButtonGO = Instantiate(pageNumberButtonPrefab, transform, false);
        ResponsePageNumberButton responsePageNumberButton = pageNumberButtonGO.GetComponent<ResponsePageNumberButton>();
        responsePageNumberButton.SetPageNumber(pageNumber);
    }
}
