using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab; // The arrow prefab to instantiate
    public GameObject spherePrefab; // The small black sphere prefab
    public Transform targetObject; // The target object the arrows will point to

    public Color frontFaceColor = Color.red;
    public Color backFaceColor = Color.blue;
    public Color topFaceColor = Color.green;
    public Color bottomFaceColor = Color.yellow;
    public Color leftFaceColor = Color.magenta;
    public Color rightFaceColor = Color.cyan;

    private int arrowID = 0;

    void Start()
    {
        // Calculate the combined bounding box of the target object and its children
        Bounds combinedBounds = CalculateCombinedBounds(targetObject);

        // Spawn arrows on all six faces of the bounding box
        SpawnArrows(combinedBounds, Vector3.forward, frontFaceColor);  // Front face
        SpawnArrows(combinedBounds, Vector3.back, backFaceColor);     // Back face
        SpawnArrows(combinedBounds, Vector3.up, topFaceColor);        // Top face
        SpawnArrows(combinedBounds, Vector3.down, bottomFaceColor);    // Bottom face
        SpawnArrows(combinedBounds, Vector3.left, leftFaceColor);      // Left face
        SpawnArrows(combinedBounds, Vector3.right, rightFaceColor);    // Right face

        // Spawn spheres at all 8 corners of the bounding box
        SpawnCorners(combinedBounds);
    }

    Bounds CalculateCombinedBounds(Transform root)
    {
        // Initialize the combined bounds with an invalid value
        Bounds combinedBounds = new Bounds(root.position, Vector3.zero);

        // Include the bounds of the root object
        Renderer rootRenderer = root.GetComponent<Renderer>();
        if (rootRenderer != null)
        {
            combinedBounds = rootRenderer.bounds;
        }

        // Include the bounds of all child objects
        foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>())
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        return combinedBounds;
    }

    void SpawnArrows(Bounds bounds, Vector3 direction, Color color)
    {
        Vector3 faceCenter = bounds.center + Vector3.Scale(bounds.extents, direction);

        Vector3 perpendicular1, perpendicular2;

        if (direction == Vector3.up || direction == Vector3.down)
        {
            // For top and bottom faces, use the right and forward directions
            perpendicular1 = Vector3.right * bounds.extents.x;
            perpendicular2 = Vector3.forward * bounds.extents.z;
        }
        else if (direction == Vector3.right || direction == Vector3.left)
        {
            // For right and left faces, use up and forward directions
            perpendicular1 = Vector3.up * bounds.extents.y;
            perpendicular2 = Vector3.forward * bounds.extents.z;

            if (direction == Vector3.left)
            {
                // Invert the perpendicular directions for the left face
                perpendicular1 = -perpendicular1;
                perpendicular2 = -perpendicular2;
            }
        }
        else
        {
            // For front and back faces, use up and right directions
            perpendicular1 = Vector3.up * bounds.extents.y;
            perpendicular2 = Vector3.right * bounds.extents.x;
        }

        // Calculate the positions on the face
        Vector3 top = faceCenter + perpendicular1;
        Vector3 bottom = faceCenter - perpendicular1;
        Vector3 left = faceCenter - perpendicular2;
        Vector3 right = faceCenter + perpendicular2;
        Vector3 center = faceCenter;

        // Instantiate arrows at these positions
        InstantiateArrow(top, direction, color);
        InstantiateArrow(bottom, direction, color);
        InstantiateArrow(left, direction, color);
        InstantiateArrow(right, direction, color);
        InstantiateArrow(center, direction, color);
    }

    void SpawnCorners(Bounds bounds)
    {
        // Calculate the 8 corners of the bounding box
        Vector3[] corners = new Vector3[8];
        corners[0] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[1] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[2] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[3] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[4] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[5] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[6] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[7] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

        // Instantiate a sphere at each corner position
        foreach (Vector3 corner in corners)
        {
            Instantiate(spherePrefab, corner, Quaternion.identity);
        }
    }

    void InstantiateArrow(Vector3 position, Vector3 direction, Color color)
    {
        // Instantiate the arrow at the given position
        GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);

        // Set the arrow's rotation based on the face direction
        arrow.transform.rotation = Quaternion.LookRotation(direction);

        // Set the arrow's color
        Renderer arrowRenderer = arrow.GetComponent<Renderer>();
        if (arrowRenderer != null)
        {
            arrowRenderer.material.color = color;
        }
        else
        {
            Debug.LogWarning("Arrow prefab does not have a Renderer component.");
        }

        // Set a unique name and ID
        arrow.name = $"Arrow_{arrowID++}";
    }
}
