using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailVisualizationManager : MonoBehaviour
{
    public bool showRail = true;


    // The rail model should only be visible in the Editor for debugging purposes
    private void Start()
    {
        if (showRail)
        {
            gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        // Do nothing, keep the GameObject active for debugging
#else
        gameObject.SetActive(false); // Disable the GameObject for non-editor builds
#endif
    }
}
