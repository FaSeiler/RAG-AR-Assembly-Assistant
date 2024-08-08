using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstructionGenerator : Singleton<InstructionGenerator>
{
    public bool loadInstructions = false; // If true, load the instructions from the PersistentDataPath otherwise generate new instructions

    public Dictionary<ComponentTypes.ComponentType, Instruction> generatedAssemblyInstructions = // For each component type, store the generated instruction
        new Dictionary<ComponentTypes.ComponentType, Instruction>();
    public UnityEvent<Instruction> OnNewAssemblyInstructionGeneratedOrLoaded = new UnityEvent<Instruction>();
    public string instructionTemplateAssembly =  // Template for RAG query on how the instruction should be generated
        "How do I mount/install the {0}? Answer in short concise steps! Dont' add any other additional information.";
    public static string instructionTemplateScan = // Template for the scan instruction
        "Point the tablet camera at the component to scan it.";

    public int generatedInstructionCount = 0;

    protected void Start() 
    {
        // We have to load and generate the instructions in start to ensure all componentsSIMATIC are subscribed to the new instructionGenerated event
        StartCoroutine(LoadOrGenerateInstructions());
    }

    /// <summary>
    /// Check if all instructions are already generated and stored, otherwise generate them
    /// </summary>
    private IEnumerator LoadOrGenerateInstructions()
    {
        generatedAssemblyInstructions = InstructionSerializer.instance.LoadInstructions();
        generatedInstructionCount = generatedAssemblyInstructions.Count;

        // Check if all instructions are already in the generatedInstructions dictionary
        foreach (ComponentSIMATIC componentSIMATIC in ComponentDatabase.instance.GetAllComponents())
        {
            if (generatedAssemblyInstructions.ContainsKey(componentSIMATIC.componentType))
            {
                OnNewAssemblyInstructionGeneratedOrLoaded.Invoke(generatedAssemblyInstructions[componentSIMATIC.componentType]);
                Debug.Log("Loaded instruction for " + componentSIMATIC.componentType + " from file.");
            }
            else
            {
                // Wait until the instruction for the current componentType has been generated
                yield return StartCoroutine(GenerateAssemblyInstructionCoroutine(componentSIMATIC));
            }
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
        generatedInstructionCount = generatedAssemblyInstructions.Count;
        InstructionSerializer.instance.SaveInstructions(generatedAssemblyInstructions); // Save the generated instruction

        OnNewAssemblyInstructionGeneratedOrLoaded.Invoke(instruction);
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
}
