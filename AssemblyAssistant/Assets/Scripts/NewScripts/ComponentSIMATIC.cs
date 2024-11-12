using Old_Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class ComponentSIMATIC : MonoBehaviour
{
    [Header("Static Properties")] // Properties that have to be set manually
    public string componentName; // Static
    public string articleNumber; // Static
    public ComponentTypes.ComponentType componentType; // Static (in future extract it somehow from the article number)
    public GameObject modelPrefab; // Static (in future it should come from a database)
    public ModelTargetBehaviour modelTargetBehaviour; // Static
    public Material materialHologram; // Static
    public Vector3 offsetOnRail; // Static: The offset of the component on the rail in respect to the first component

    [Header("Dynamic Properties")] // Properties that are set dynamically
    public GameObject modelTargetPreviewHologramGO; // Dynamic: Comes from the ModelDatabase (in future it should come from a Siemens database)
    public GameObject modelTargetPreviewTexturedGO; // Dynamic: Comes from the ModelDatabase (in future it should come from a Siemens database)
    public Dictionary<string, string> properties = new Dictionary<string, string>(); // Dynamic: Comes from WebScraper (Siemens Industry Mall)

    public Instruction scanInstruction; // Dynamic: Comes from InstructionGenerator. Every component has one scan instruction
    public Instruction assemblyInstruction; // Dynamic: Comes from InstructionGenerator. Every component has one assembly instruction (In fute it should be a list of instructions)

    [Header("Debugging")]
    [SerializeField] public bool propertiesInitialized = false;
    [SerializeField] public bool scanInstructionsInitialized = false;
    [SerializeField] public bool assemblyInstructionInitialized = false;

    private void Awake()
    {
        CreateModelTargetPreview(); // Create the hologram preview GameObject of the model
        GetProperties(articleNumber); // Get the properties of the component from the Siemens Industry Mall
        GetScanInstructions(); // Get the scan instructions for the component
        InstructionGenerator.instance.OnNewAssemblyInstructionGeneratedOrLoaded.AddListener(OnNewAssemblyInstructionGenerated); // Listen for new instructions
    }

    /// <summary>
    /// If the InstructionGenerator created a new instruction in the background, check if it is for this component and add it to the list of instructions
    /// </summary>
    private void OnNewAssemblyInstructionGenerated(Instruction newInstruction)
    {
        if (componentType == newInstruction.componentType)
        {
            assemblyInstruction = newInstruction;
            assemblyInstructionInitialized = true;
        }
    }

    /// <summary>
    /// Get the scan instructions for the component
    /// </summary>
    private void GetScanInstructions()
    {
        Instruction scanInstruction = InstructionGenerator.GenerateScanInstruction(this);
        this.scanInstruction = scanInstruction;
        scanInstructionsInitialized = true;
    }

    /// <summary>
    /// Get the properties of the component from the Siemens Industry Mall
    /// </summary>
    private void GetProperties(string articleNumber)
    {
        WebScraperSIMATIC.instance.StartScraping(articleNumber, (dataDictionary) =>
        {
            properties = dataDictionary;
            propertiesInitialized = true;
        });
    }

    /// <summary>
    /// Create a hologram preview of the model
    /// </summary>
    private void CreateModelTargetPreview()
    {
        modelTargetPreviewHologramGO = Instantiate(modelPrefab, transform);
        modelTargetPreviewHologramGO.SetActive(false);

        SetMaterials(modelTargetPreviewHologramGO, materialHologram);

        modelTargetPreviewTexturedGO = Instantiate(modelPrefab, transform);
        modelTargetPreviewTexturedGO.SetActive(false);
    }

    #region HELPERS
    public void SetMaterials(GameObject parent, Material newMaterial)
    {
        // Get all MeshRenderer components in the parent object and its children
        MeshRenderer[] meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();

        // Loop through each MeshRenderer and set its materials to the new material
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // Create a new array of materials with the same length as the original
            Material[] newMaterials = new Material[meshRenderer.materials.Length];

            // Assign the new material to each slot in the new array
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = newMaterial;
            }

            // Set the MeshRenderer's materials to the new array
            meshRenderer.materials = newMaterials;
        }
    }

    // NOTE: This method does not work properly if on Android.
    public void SetMaterialsAlpha(GameObject parent, float v)
    {
        // Get all MeshRenderer components in the parent object and its children
        MeshRenderer[] meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();

        // Loop through each MeshRenderer and update its materials
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // Get the materials array
            Material[] materials = meshRenderer.materials;

            // Loop through each material and modify it
            for (int i = 0; i < materials.Length; i++)
            {
                // Copy the original material
                Material copiedMaterial = new Material(materials[i]);

                // Set the rendering mode to transparent
                copiedMaterial.SetFloat("_Mode", 3); // 3 corresponds to Transparent in Unity's Standard Shader
                copiedMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                copiedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                copiedMaterial.SetInt("_ZWrite", 0);
                copiedMaterial.DisableKeyword("_ALPHATEST_ON");
                copiedMaterial.EnableKeyword("_ALPHABLEND_ON");
                copiedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                copiedMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                // Modify the alpha value
                Color color = copiedMaterial.color;
                color.a = v;
                copiedMaterial.color = color;

                // Assign the modified material back to the array
                materials[i] = copiedMaterial;
            }

            // Reassign the modified materials array back to the MeshRenderer
            meshRenderer.materials = materials;
        }
    }
    #endregion
}
