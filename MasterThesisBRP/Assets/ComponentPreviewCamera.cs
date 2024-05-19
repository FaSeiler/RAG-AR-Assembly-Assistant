using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentPreviewCamera : MonoBehaviour
{
    public float height = 200.0f;
    public GameObject referenceModel;

    public void Update()
    {
        if (referenceModel != null)
        {
            Vector3 pivot = GetMeshRendererCenterPivot(referenceModel);
            transform.position = pivot;
            transform.position = transform.position + Vector3.up * height;
            //transform.LookAt(referenceModel.transform.position);

            //var pivot = GetPivot(referenceModel.transform);
            //transform.position = pivot;
            //transform.position = transform.position + Vector3.up * height;
            //transform.LookAt(referenceModel.transform.position);
        }
    }


    public Vector3 GetMeshRendererCenterPivot(GameObject model)
    {
        var bounds = new Bounds(model.transform.position, Vector3.zero);
        var renderers = model.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        Vector3 pivot = bounds.center;
        return pivot;
    }

    public static Vector3 GetPivot(Transform model)
    {
        var point = new GameObject("PIVOT");
        point.transform.SetParent(model);
        point.transform.localPosition = Vector3.zero;
        point.transform.SetParent(null);
        var pivot = point.transform.localPosition;
        Object.Destroy(point);
        return pivot;
    }
}
