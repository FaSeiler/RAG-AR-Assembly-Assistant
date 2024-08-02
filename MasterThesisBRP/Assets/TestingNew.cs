using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingNew : MonoBehaviour
{
    public List<GameObject> modelTargetGO;
    public int currentIndex = 0;
    public GameObject currentActiveModelTargetGO = null;

    private void Start()
    {
        currentActiveModelTargetGO = modelTargetGO[currentIndex];
    }

    public void ShowNextModelTargetGO()
    {
        if (modelTargetGO.Count == 0)
        {
            Debug.Log("No more model target GOs to show.");
            return;
        }

        GameObject nextModelTargetGO = modelTargetGO[currentIndex];
        currentIndex = (currentIndex + 1) % modelTargetGO.Count;

        if (currentActiveModelTargetGO != null)
        {
            currentActiveModelTargetGO.SetActive(false);
        }

        currentActiveModelTargetGO = nextModelTargetGO;
        nextModelTargetGO.SetActive(true);

        DebugUI.WriteLog("Active model target GO: " + nextModelTargetGO.name);
    }
}
