using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingPreviewComponent : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 10f;
    public bool pivotSet = false;
    public ComponentPreviewCamera previewCamera;


    private void Update()
    {
        if (pivotSet)
        {
            RotateObject();
        }
    }

    private void RotateObject()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    public void UpdatePreviewComponent(GameObject model)
    {
        // Step 1: Calculate the bounds of all child renderers
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return; // No child renderers found

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        transform.position = bounds.center;
        model.transform.parent = transform;
        previewCamera.SetPivot(bounds.center);

        pivotSet = true;
    }
}
