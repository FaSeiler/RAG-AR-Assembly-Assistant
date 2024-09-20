using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [Serializable]
    public struct ArrowData
    {
        public Face face;
        public Position position;
        public string name;
        public bool enabled;
    }

    [Serializable]
    // The faces of the bounding box
    public enum Face { Front, Back, Top, Bottom, Left, Right }

    [Serializable]
    // The position of the arrow on one face
    public enum Position { Center, Top, Bottom, Left, Right }

    [Header("Set Arrows")]
    public List<ArrowData> arrowPositions = new List<ArrowData>();

    [Header("Variables")]
    public GameObject arrowPrefab; // The arrow prefab to instantiate
    public GameObject spherePrefab; // The small black sphere prefab
    public Transform targetObject; // The target object the arrows will point to
    public bool addArrowsAtStart = false;
    public bool showAllArrows = false;
    public bool visualizeBoundingBox = false;

    public Color frontFaceColor = Color.red;
    public Color backFaceColor = Color.blue;
    public Color topFaceColor = Color.green;
    public Color bottomFaceColor = Color.yellow;
    public Color leftFaceColor = Color.magenta;
    public Color rightFaceColor = Color.cyan;

    private int arrowID = 0;

    private Bounds combinedBounds;
    private Vector3[] corners;

    //void Start()
    //{
    //    combinedBounds = CalculateCombinedBounds(targetObject);

    //    if (addArrowsAtStart)
    //    {
    //        if (showAllArrows)
    //        {
    //            AddAllArrows();
    //        }
    //        else
    //        {
    //            //Add arrows at the specified positions
    //            foreach (ArrowData arrowPosition in arrowPositions)
    //            {
    //                AddArrowAtPosition(arrowPosition.face, arrowPosition.position, arrowPosition.name, arrowPosition.enabled);
    //            }
    //        }

    //        if (visualizeBoundingBox)
    //        {
    //            //Spawn spheres at all 8 corners of the bounding box for debugging
    //            SpawnCorners(combinedBounds);
    //        }
    //    }
    //}

    private void Update()
    {
        if (visualizeBoundingBox)
        {
            VisualizeBoundingBox();
        }
    }

    // Calculate Bounds based on renderer
    //Bounds CalculateCombinedBounds(Transform root)
    //{
    //    // Initialize the combined bounds with an invalid value
    //    Bounds combinedBounds = new Bounds(root.position, Vector3.zero);

    //    // Include the bounds of the root object
    //    Renderer rootRenderer = root.GetComponent<Renderer>();
    //    if (rootRenderer != null)
    //    {
    //        combinedBounds = rootRenderer.bounds;
    //    }

    //    // Include the bounds of all child objects
    //    foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>())
    //    {
    //        combinedBounds.Encapsulate(renderer.bounds);
    //    }

    //    return combinedBounds;
    //}

    // Calculate Bounds based on Mesh Vertices
    Bounds CalculateCombinedBounds(Transform root)
    {
        // Initialize the combined bounds with an invalid value
        Bounds combinedBounds = new Bounds();
        bool boundsInitialized = false;

        // Go through all the MeshFilter components in the root and its children
        foreach (MeshFilter meshFilter in root.GetComponentsInChildren<MeshFilter>())
        {
            Mesh mesh = meshFilter.sharedMesh;

            if (mesh != null)
            {
                // Iterate over the vertices of the mesh and transform them to world space
                foreach (Vector3 vertex in mesh.vertices)
                {
                    Vector3 worldVertex = meshFilter.transform.TransformPoint(vertex);

                    if (!boundsInitialized)
                    {
                        // Initialize bounds with the first vertex
                        combinedBounds = new Bounds(worldVertex, Vector3.zero);
                        boundsInitialized = true;
                    }
                    else
                    {
                        // Encapsulate the current vertex into the bounds
                        combinedBounds.Encapsulate(worldVertex);
                    }
                }
            }
        }

        // Return valid bounds if there were any vertices, otherwise return an empty bounds
        return boundsInitialized ? combinedBounds : new Bounds(root.position, Vector3.zero);
    }


    public void AddArrowAtPosition(ArrowData arrowData, Transform parentTransform)
    {
        AddArrowAtPosition(arrowData.face, arrowData.position, arrowData.name, arrowData.enabled, parentTransform);
    }

    // New method to add an arrow using enums
    public void AddArrowAtPosition(Face face, Position position, string name, bool enabled, Transform parent = null)
    {
        if (parent != null)
        {
            targetObject = parent;
        }

        // Calculate the combined bounding box of the target object and its children
        combinedBounds = CalculateCombinedBounds(targetObject);

        // Determine the face and direction
        Vector3 faceDirection;
        Color faceColor;

        switch (face)
        {
            case Face.Back:
                faceDirection = Vector3.forward;
                faceColor = backFaceColor;
                break;
            case Face.Front:
                faceDirection = Vector3.back;
                faceColor = frontFaceColor;
                break;
            case Face.Top:
                faceDirection = Vector3.up;
                faceColor = topFaceColor;
                break;
            case Face.Bottom:
                faceDirection = Vector3.down;
                faceColor = bottomFaceColor;
                break;
            case Face.Left:
                faceDirection = Vector3.left;
                faceColor = leftFaceColor;
                break;
            case Face.Right:
                faceDirection = Vector3.right;
                faceColor = rightFaceColor;
                break;
            default:
                Debug.LogError("Invalid face specified");
                return;
        }

        // Calculate face center
        Vector3 faceCenter = combinedBounds.center + Vector3.Scale(combinedBounds.extents, faceDirection);
        Vector3 perpendicular1, perpendicular2;

        // Calculate perpendicular directions for the face
        if (faceDirection == Vector3.up || faceDirection == Vector3.down)
        {
            perpendicular1 = Vector3.right * combinedBounds.extents.x;
            perpendicular2 = Vector3.forward * combinedBounds.extents.z;
        }
        else if (faceDirection == Vector3.left || faceDirection == Vector3.right)
        {
            perpendicular1 = Vector3.up * combinedBounds.extents.y;
            perpendicular2 = Vector3.forward * combinedBounds.extents.z;
        }
        else
        {
            perpendicular1 = Vector3.up * combinedBounds.extents.y;
            perpendicular2 = Vector3.right * combinedBounds.extents.x;
        }

        // Define positions on the face
        Vector3 top = faceCenter + perpendicular1;
        Vector3 bottom = faceCenter - perpendicular1;
        Vector3 left = faceCenter - perpendicular2;
        Vector3 right = faceCenter + perpendicular2;
        Vector3 center = faceCenter;

        // Instantiate an arrow at the specified position
        switch (position)
        {
            case Position.Top:
                InstantiateArrow(top, faceDirection, faceColor, name, enabled);
                break;
            case Position.Bottom:
                InstantiateArrow(bottom, faceDirection, faceColor, name, enabled);
                break;
            case Position.Left:
                InstantiateArrow(left, faceDirection, faceColor, name, enabled);
                break;
            case Position.Right:
                InstantiateArrow(right, faceDirection, faceColor, name, enabled);
                break;
            case Position.Center:
                InstantiateArrow(center, faceDirection, faceColor, name, enabled);
                break;
            default:
                Debug.LogError("Invalid position specified");
                break;
        }
    }

    void InstantiateArrow(Vector3 position, Vector3 direction, Color color, string name, bool enabled)
    {
        // Instantiate the arrow at the given position
        GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity, targetObject);

        // Set the arrow's rotation based on the face direction
        arrow.transform.rotation = Quaternion.LookRotation(direction);

        if (color == leftFaceColor || color == rightFaceColor)
        {
            // Rotate the arrow by 90 degrees for the left and right faces
            //arrow.transform.Rotate(Vector3.forward, 90);
        }


        // Set the arrow's color for all Renderer components in the children
        foreach (Renderer renderer in arrow.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = color;
        }

        // Set a unique name and ID
        if (arrow.name == "")
        {
            arrow.name = $"Arrow_{arrowID++}";
        }
        else
        {
            arrow.name = name;
        }

        arrow.SetActive(enabled);
    }

    void SpawnCorners(Bounds bounds)
    {
        // Calculate the 8 corners of the bounding box
        corners = new Vector3[8];
        corners[0] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[1] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[2] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[3] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[4] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[5] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[6] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[7] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

        // Create new empty gameObject with the name "Corners"
        GameObject cornersGO = new GameObject("Corners");
        cornersGO.transform.parent = targetObject;

        // Instantiate a sphere at each corner position
        foreach (Vector3 corner in corners)
        {
            Instantiate(spherePrefab, corner, Quaternion.identity, cornersGO.transform);
        }
    }

    /// <summary>
    /// Adds all arrows possible for debugging
    /// </summary>
    private void AddAllArrows()
    {
        // Calculate the combined bounding box of the target object and its children
        combinedBounds = CalculateCombinedBounds(targetObject);

        // Iterate over all faces
        foreach (Face face in Enum.GetValues(typeof(Face)))
        {
            // Iterate over all positions for each face
            foreach (Position position in Enum.GetValues(typeof(Position)))
            {
                // Add an arrow at the current face and position
                AddArrowAtPosition(face, position, "", true);
            }
        }
    }

    private void VisualizeBoundingBox()
    {
        if (corners != null)
        {
            // Draw lines between the corners to form the edges of the bounding box
            Debug.DrawLine(corners[0], corners[1], Color.white);
            Debug.DrawLine(corners[0], corners[2], Color.white);
            Debug.DrawLine(corners[0], corners[4], Color.white);
            Debug.DrawLine(corners[1], corners[3], Color.white);
            Debug.DrawLine(corners[1], corners[5], Color.white);
            Debug.DrawLine(corners[2], corners[3], Color.white);
            Debug.DrawLine(corners[2], corners[6], Color.white);
            Debug.DrawLine(corners[3], corners[7], Color.white);
            Debug.DrawLine(corners[4], corners[5], Color.white);
            Debug.DrawLine(corners[4], corners[6], Color.white);
            Debug.DrawLine(corners[5], corners[7], Color.white);
            Debug.DrawLine(corners[6], corners[7], Color.white);
        }
    }
}
