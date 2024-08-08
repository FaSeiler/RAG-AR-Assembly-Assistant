using Old_Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Dynamic Properties")] // Properties that are set dynamically
    public GameObject modelTargetPreview; // Dynamic: Comes from the ModelDatabase (in future it should come from a Siemens database)
    public Dictionary<string, string> properties = new Dictionary<string, string>(); // Dynamic: Comes from WebScraper (Siemens Industry Mall)

    public Instruction scanInstruction; // Dynamic: Comes from InstructionGenerator. Every component has one scan instruction
    public Instruction assemblyInstruction; // Dynamic: Comes from InstructionGenerator. Every component has one assembly instruction (In fute it should be a list of instructions)

    [Header("Debugging")]
    [SerializeField] public bool propertiesInitialized = false;
    [SerializeField] public bool scanInstructionsInitialized = false;
    [SerializeField] public bool assemblyInstructionInitialized = false;

    private void Awake()
    {
        Debug.Log(componentName + " created"); ;
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
        modelTargetPreview = Instantiate(modelPrefab, transform);

        SetMaterials(modelTargetPreview, materialHologram);
    }

    #region HELPERS
    private void SetMaterials(GameObject parent, Material newMaterial)
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
    #endregion
}
