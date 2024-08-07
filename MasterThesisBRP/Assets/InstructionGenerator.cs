using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstructionGenerator : Singleton<InstructionGenerator>
{

    public Dictionary<ComponentTypes.ComponentType, Instruction> generatedAssemblyInstructions = // For each component type, store the generated instruction
        new Dictionary<ComponentTypes.ComponentType, Instruction>();
    public static UnityEvent<Instruction> OnNewAssemblyInstructionGenerated = new UnityEvent<Instruction>();
    public string instructionTemplateAssembly =  // Template for RAG query on how the instruction should be generated
        "How do I mount/install the {0}? Answer in short concise steps! Dont' add any other additional information.";
    public static string instructionTemplateScan = // Template for the scan instruction
        "Point the tablet camera at the component to scan it.";

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(GenerateAllAssemblyInstructionsCoroutine());
    }

    /// <summary>
    /// Fetches all component types and generates instructions for each of them
    /// </summary>
    public IEnumerator GenerateAllAssemblyInstructionsCoroutine()
    {
        foreach (ComponentSIMATIC componentSIMATIC in ComponentDatabase.instance.GetAllComponents())
        {
            // Wait until the instruction for the current componentType has been generated
            yield return StartCoroutine(GenerateAssemblyInstructionCoroutine(componentSIMATIC));
        }
    }

    public IEnumerator GenerateAssemblyInstructionCoroutine(ComponentSIMATIC componentSIMATIC, Action<Instruction> callback = null)
    {
        Debug.Log("Starting generating instruction for " + componentSIMATIC.componentType + "...");

        // If the instruction for the component type has already been generated, skip it
        if (generatedAssemblyInstructions.ContainsKey(componentSIMATIC.componentType))
        {
            yield break;
        }

        Instruction instruction = new Instruction();
        string query = string.Format(instructionTemplateAssembly, componentSIMATIC.componentType);
        bool isRequestCompleted = false;

        ClientRAG.instance.SendRequest(query, (responseData) =>
        {
            instruction.componentType = componentSIMATIC.componentType;
            instruction.text = responseData.text;
            instruction.images = responseData.decoded_images;
            instruction.pageNumbers = responseData.page_numbers;
            // TODO: Animation
            isRequestCompleted = true;
        });

        // Wait until the request is completed
        while (!isRequestCompleted)
        {
            yield return null;
        }

        Debug.Log("Generated instruction for " + instruction.componentType + ": \n\n" + instruction.text);

        generatedAssemblyInstructions.Add(instruction.componentType, instruction);

        OnNewAssemblyInstructionGenerated.Invoke(instruction);
        callback?.Invoke(instruction);

        yield return instruction;
    }


    public static Instruction GenerateScanInstruction(ComponentSIMATIC componentSIMATIC)
    {
        string formattedInstructionText = string.Format(instructionTemplateScan, componentSIMATIC.componentName);

        Instruction instruction = new Instruction();
        instruction.text = formattedInstructionText;
        instruction.componentType = componentSIMATIC.componentType;
        return instruction;
    }




    //    public bool generateNewInstructions = false;

    //    public string instructionTemplate = "How do I mount/install the {0}? Answer in short concise steps! Dont' add any other additional information.";

    //    public Dictionary<ComponentTypes.ComponentType, Instruction> generatedInstructions = new Dictionary<ComponentTypes.ComponentType, Instruction>();

    //    public static UnityEvent<Instruction> OnNewInstructionGenerated = new UnityEvent<Instruction>();

    //    private void Start()
    //    {
    //        if (generateNewInstructions)
    //        {
    //            StartCoroutine(GenerateAllInstructionsCoroutine());
    //        }
    //    }

    //    public IEnumerator GenerateAllInstructionsCoroutine()
    //    {
    //        foreach (ComponentTypes.ComponentType componentType in ComponentTypes.GetAllComponentTypeEnums())
    //        {
    //            // Warte, bis die Anweisung für den aktuellen componentType generiert wurde
    //            yield return StartCoroutine(GenerateInstructionCoroutine(componentType));
    //        }
    //    }

    //    public IEnumerator GenerateInstructionCoroutine(ComponentTypes.ComponentType componentType, Action<Instruction> callback = null)
    //    {
    //        Debug.Log("Generating instruction for " + componentType);

    //        Instruction instruction = new Instruction();
    //        instruction.componentTypeEnum = componentType;

    //        string query = string.Format(instructionTemplate, componentType);

    //        bool isRequestCompleted = false;

    //        ClientRAG.instance.SendRequest(query, (responseData) =>
    //        {
    //            instruction.instructionText = responseData.text;
    //            instruction.imageTextures = responseData.decoded_images;
    //            instruction.page_numbers = responseData.page_numbers;
    //            isRequestCompleted = true;
    //        });

    //        // Wait until the request is completed
    //        while (!isRequestCompleted)
    //        {
    //            yield return null; // Wait for the next frame
    //        }

    //        // Add the generated instruction to the dictionary
    //        generatedInstructions.Add(componentType, instruction);

    //        Debug.Log("Generated instruction for " + componentType + ": \n\n" + instruction.instructionText);
    //        OnNewInstructionGenerated.Invoke(instruction);

    //        callback?.Invoke(instruction);

    //        yield return instruction;
    //    }

    //    internal string GetInstructionForComponentType(ComponentTypes.ComponentType componentType)
    //    {
    //        // Find the instruction for the given component type
    //        if (generatedInstructions.ContainsKey(componentType))
    //        {
    //            return generatedInstructions[componentType].instructionText;
    //        }

    //        return "No instruction found";
    //    }
}
