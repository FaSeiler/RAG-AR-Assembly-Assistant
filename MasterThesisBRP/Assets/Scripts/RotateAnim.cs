using UnityEngine;

public class RotateAnim : MonoBehaviour {
    public float speed = 1;
    public Vector3 axis = Vector3.forward;
    public bool rotateAroundPivot = false;
    private Vector3 pivot;

    private void Start()
    {
        if (rotateAroundPivot)
        {
            SetPivot();
        }
    }

    void Update () 
    {
        if (rotateAroundPivot)
        {
            transform.RotateAround(pivot, axis, speed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(axis * Time.deltaTime * speed);
        }
    }


    public void SetPivot()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return; // No child renderers found

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        pivot = bounds.center;
    }
}
