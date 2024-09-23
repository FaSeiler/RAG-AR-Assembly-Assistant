using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceComponentAlign : MonoBehaviour
{
    /// <summary>
    /// Workaround: Rotate this reference component at start to align with the coordinate system of the other components
    /// </summary>
    private void Awake()
    {
        this.gameObject.transform.Rotate(90, 0, 0);
        this.gameObject.SetActive(false); 
    }
}
