using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralCableWrapper : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public ProceduralCable proceduralCable;

    private void Update()
    {
        if (pointA.hasChanged || pointB.hasChanged)
        {
            proceduralCable.a = pointA.position;
            proceduralCable.b = pointB.position;

            if (proceduralCable.endSphereA == null && proceduralCable.endSphereB == null)
            {
                proceduralCable.endSphereA = CreateSphere(proceduralCable.transform, pointA.position, proceduralCable.radius, proceduralCable.meshRenderer.material);
                proceduralCable.endSphereB = CreateSphere(proceduralCable.transform, pointB.position, proceduralCable.radius, proceduralCable.meshRenderer.material);
            }
            else
            {
                proceduralCable.endSphereA.transform.position = pointA.position;
                proceduralCable.endSphereB.transform.position = pointB.position;
            }

            proceduralCable.UpdateObject();
        }
    }

    public GameObject CreateSphere(Transform parent, Vector3 position, float radius, Material material)
    {
        // Create a sphere primitive
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.parent = parent;

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
