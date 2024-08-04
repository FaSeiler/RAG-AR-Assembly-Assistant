using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ComponentPreviewCamera : MonoBehaviour
{
    public float margin = 10f; // Margin of camera to model
    public float height = 10f; // Height of camera from model


    public RotatingPreviewComponent rotatingPreviewComponent;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        rotatingPreviewComponent.OnPreviewComponentUpdated.AddListener(UpdateCameraPositionAndSize);
    }

    private void UpdateCameraPositionAndSize(Bounds bounds)
    {
        float size = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) / 2f;

        SetPivot(bounds.center);
        SetOrthographicSize(size);
    }

    public void SetPivot(Vector3 pivot)
    {

        transform.position = pivot;
        // include height
        transform.position = transform.position + Vector3.up * height;
    }

    public void SetOrthographicSize(float size)
    {
        cam.orthographicSize = size + margin;
    }
}
