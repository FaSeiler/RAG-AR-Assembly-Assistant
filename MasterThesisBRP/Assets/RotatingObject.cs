using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 10f;

    private void Update()
    {
        //RotateObject();
        RotateObjectAroundCenter();
    }

    // Rotate the object around a give axis over time
    private void RotateObject()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    // Rotate the object around its own pivot center over time
    private void RotateObjectAroundPivot()
    {
        transform.RotateAround(transform.position, rotationAxis, rotationSpeed * Time.deltaTime);
    }

    private void RotateObjectAroundCenter()
    {


        // Step 1: Calculate the bounds of all child renderers
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return; // No child renderers found

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 pivot = bounds.center;

        // Step 2: Perform the rotation around the pivot
        transform.RotateAround(pivot, rotationAxis, rotationSpeed * Time.deltaTime);
    }


}
