using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RotatingPreviewComponent : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 10f;

    [HideInInspector]
    public UnityEvent<Bounds> OnPreviewComponentUpdated = new UnityEvent<Bounds>();

    [HideInInspector]
    public GameObject activePreviewComponent;

    private bool pivotSet = false;
    private float oldSize;

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
        UpdateBounds(); // While rotating the bounds need to be updated constantly
    }

    public GameObject SetActivePreview(GameObject prefab)
    {
        RemoveActivePreview();

        GameObject model = Instantiate(prefab);
        // Set model and all its children layers to this.gameObject.layer
        SetLayerRecursively(model, gameObject.layer);

        model.SetActive(true);
        activePreviewComponent = model;
        
        Bounds bounds = CalculateBounds(model);
        transform.position = bounds.center;
        model.transform.parent = transform;
        oldSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) / 2f;

        pivotSet = true;

        Debug.Log("SetActivePreview - New Preview: " + activePreviewComponent.name);

        OnPreviewComponentUpdated.Invoke(bounds);

        return model;
    }

    private void SetLayerRecursively(GameObject model, int layer)
    {
        // Set the layer of the model and all its children to the given layer
        model.layer = layer;
        foreach (Transform child in model.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    private Bounds CalculateBounds(GameObject model)
    {
        // Step 1: Calculate the bounds of all child renderers
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("No renderers found in model: " + model.name);
            return new Bounds(); // No child renderers found
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }

    private void UpdateBounds()
    {
        // Check if the bounds of the active preview component are larger than the largest bounds
        // If so, update the largest bounds, otherwise use the largest bounds

        Bounds bounds = CalculateBounds(activePreviewComponent);
        float newSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) / 2f;

        if (newSize > oldSize)
        {
            oldSize = newSize;
            transform.position = bounds.center;
            OnPreviewComponentUpdated.Invoke(bounds);
        }
    }

    public void RemoveActivePreview()
    {

        pivotSet = false;

        if (activePreviewComponent != null)
        {
            Destroy(activePreviewComponent);
            Debug.Log("RemoveActivePreview - Removing: " + activePreviewComponent.name);
        }
        else
        {
            Debug.Log("RemoveActivePreview - Nothing to remove!");
        }
    }
}
