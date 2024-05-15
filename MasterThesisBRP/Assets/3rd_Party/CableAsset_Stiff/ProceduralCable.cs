using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralCable : MonoBehaviour {

    public Vector3 a;
    public Vector3 b;
    public int step = 20;
    public float curvature = 1;
    public float radius = 0.2f;
    public int radiusStep = 6;
    public Vector2 uvMultiply = Vector2.one;
    public GameObject endSphereA;
    public GameObject endSphereB;
    public Transform aTransform;
    public Transform bTransform;
    public GameObject connectorA;
    public GameObject connectorB;
    public Vector3 cableVector; // The vector from a to b

    public bool drawEditorLines = false;

    MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    void OnEnable() {

        meshFilter = GetComponent<MeshFilter>() == null ? gameObject.AddComponent<MeshFilter>() : GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>() == null ? gameObject.AddComponent<MeshRenderer>() : GetComponent<MeshRenderer>();
        UpdateObject();

    }

	void Update () {
        if (aTransform != null && aTransform.hasChanged &&
            bTransform != null && bTransform.hasChanged)
        {
            a = aTransform.position;
            b = bTransform.position;
            UpdateObject();
        }
	}

    public float CurveHeight(int i)
    {
        i = Mathf.Clamp(i,0,step);
        float normalizedStep = (float)i / step;
        return (Mathf.Pow((normalizedStep * 2) - 1, 2) - 1)*curvature;
    }

    public Vector3 PointPosition(int i)
    {
        Vector3 segment = (b - a) / step;
        return a + segment * i + new Vector3(0,CurveHeight(i),0);
    }

    public Vector3[] VerticesForPoint(int i)
    {
        Vector3 pointPosition = PointPosition(i);
        Vector3 orientation;

        if (i == 0)
            orientation = PointPosition(1) - PointPosition(0);
        else if(i == step)
            orientation = PointPosition(step) - PointPosition(step-1);
        else
            orientation = PointPosition(i + 1) - PointPosition(i - 1);

        Quaternion rotation = Quaternion.LookRotation(orientation, Vector3.Cross(Vector3.down, b - a)); 

        List<Vector3> vertices = new List<Vector3>();
        float angleStep = 360f / (radiusStep-1);

        for(int h = 0; h < radiusStep; h++)
        {
            float angle = angleStep * h * Mathf.Deg2Rad;
            vertices.Add(pointPosition + rotation * (new Vector3( Mathf.Cos(angle)*radius , Mathf.Sin(angle)*radius , 0 )));
        }

        return vertices.ToArray();
    }

    public void UpdateObject()
    {
        meshFilter.sharedMesh = GenerateMesh();
    }

    public void Connect(Transform a, Transform b)
    {
        this.aTransform = a;
        this.bTransform = b;
        this.a = a.position;
        this.b = b.position;

        UpdateObject();
    }

    public Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Cable mesh";

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        float lenght = 0;

        for (int i = 0; i <= step; i++)
        {
            Vector3[] verticesForPoint = VerticesForPoint(i);
            for (int h = 0; h < verticesForPoint.Length; h++)
            {
                vertices.Add(verticesForPoint[h]);
                normals.Add((verticesForPoint[h] - PointPosition(i)).normalized);

                uvs.Add(new Vector2(lenght * uvMultiply.x,(float)h / (verticesForPoint.Length-1) * uvMultiply.y));

                if (i < step)
                {
                    int index = h + (i * radiusStep);

                    triangles.Add(index);
                    triangles.Add(index + 1);
                    triangles.Add(index + radiusStep);

                    triangles.Add(index);
                    triangles.Add(index + radiusStep);
                    triangles.Add(index + radiusStep - 1);

                }
            }
            lenght += SegmentLenght(i, i + 1);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateBounds();
        
        mesh.RecalculateTangents();


        // If there are already end spheres update their position and scale
        if (endSphereA != null && endSphereB != null)
        {
            endSphereA.transform.position = a;
            endSphereB.transform.position = b;
            endSphereA.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
            endSphereB.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        }
        else
        {
            endSphereA = CreateSphere(a, radius, meshRenderer.material);
            endSphereB = CreateSphere(b, radius, meshRenderer.material);
        }

        if (connectorA != null && connectorB != null)
        {
            connectorA.transform.rotation = Quaternion.LookRotation(cableVector, Vector3.Cross(Vector3.down, b - a));
            connectorB.transform.rotation = Quaternion.LookRotation(-cableVector, Vector3.Cross(Vector3.down, b - a));
        }

        cableVector = b - a;
        
        return mesh;
    }

    public float SegmentLenght(int a,int b)
    {
        return (PointPosition(b) - PointPosition(a)).magnitude;
    }

    public GameObject CreateSphere(Vector3 position, float radius, Material material)
    {
        // Create a sphere primitive
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.parent = transform;
        sphere.name = "End Sphere";
        // Set the position
        sphere.transform.position = position;

        // Set the scale (radius)
        sphere.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);

        MeshRenderer meshRenderer = sphere.GetComponent<MeshRenderer>() == null ? sphere.AddComponent<MeshRenderer>() : sphere.GetComponent<MeshRenderer>();

        // Set the material
        meshRenderer.material = material;

        return sphere;
    }
}
