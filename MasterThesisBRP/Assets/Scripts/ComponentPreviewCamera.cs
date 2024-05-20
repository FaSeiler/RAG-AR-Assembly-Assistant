using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentPreviewCamera : MonoBehaviour
{
    public float height = 200.0f;
    public GameObject referenceModel;
    public Slider heightSlider;

    public Vector3 pivot;
    public bool rotate = false;

    private void Start()
    {
        heightSlider.onValueChanged.AddListener(OnHeightSliderValueChanged);
    }

    private void OnHeightSliderValueChanged(float height)
    {
        this.height = height;
    }

    public void Update()
    {
        if (rotate)
        {
            transform.position = pivot;
            transform.position = transform.position + Vector3.up * height;
        }
    }

    public void SetPivot(Vector3 pivot)
    {
        this.pivot = pivot;
        rotate = true;
    }



    //public Vector3 GetMeshRendererCenterPivot(GameObject model)
    //{
    //    var bounds = new Bounds(model.transform.position, Vector3.zero);
    //    var renderers = model.GetComponentsInChildren<MeshRenderer>();
    //    foreach (var renderer in renderers)
    //    {
    //        bounds.Encapsulate(renderer.bounds);
    //    }
    //    Vector3 pivot = bounds.center;
    //    return pivot;
    //}

    //public static Vector3 GetPivot(Transform model)
    //{
    //    var point = new GameObject("PIVOT");
    //    point.transform.SetParent(model);
    //    point.transform.localPosition = Vector3.zero;
    //    point.transform.SetParent(null);
    //    var pivot = point.transform.localPosition;
    //    Object.Destroy(point);
    //    return pivot;
    //}
}
