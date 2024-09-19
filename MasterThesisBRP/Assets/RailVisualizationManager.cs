using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailVisualizationManager : MonoBehaviour
{
    // The rail model should only be visible in the Editor for debugging purposes
    private void Start()
    {
#if !UNITY_EDITOR
    gameObject.SetActive(false);
#endif
    }
}
