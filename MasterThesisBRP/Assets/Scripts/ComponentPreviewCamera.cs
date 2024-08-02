using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentPreviewCamera : MonoBehaviour
{
    public float margin = 10f; // Margin of camera to model
    public float height = 10f; // Height of camera from model

    public Vector3 pivot;
    public bool rotate = false;

    public void Update()
    {
        if (rotate)
        {
            transform.position = pivot;
            // include height
            transform.position = transform.position + Vector3.up * height;
        }
    }

    public void SetPivot(Vector3 pivot)
    {
        this.pivot = pivot;
        rotate = true;
    }

    public void SetOrthographicSize(float size)
    {
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = size + margin;
    }
}
