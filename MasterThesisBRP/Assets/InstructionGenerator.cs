using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstructionGenerator : Singleton<InstructionGenerator>
{
    public bool generateNewInstructions = false;

    public string instructionTemplate = "How do I mount/install the {0}? Answer in short concise steps! Dont' add any other additional information.";

    public Dictionary<ComponentTypes.ComponentType, Instruction> generatedInstructions = new Dictionary<ComponentTypes.ComponentType, Instruction>();

    public static UnityEvent<Instruction> OnNewInstructionGenerated = new UnityEvent<Instruction>();

    private void Start()
    {
        if (generateNewInstructions)
        {
            StartCoroutine(GenerateAllInstructionsCoroutine());
        }
    }

    public IEnumerator GenerateAllInstructionsCoroutine()
    {
        foreach (ComponentTypes.ComponentType componentType in ComponentTypes.GetAllComponentTypeEnums())
        {
            // Warte, bis die Anweisung für den aktuellen componentType generiert wurde
            yield return StartCoroutine(GenerateInstructionCoroutine(componentType));
        }
    }

    public IEnumerator GenerateInstructionCoroutine(ComponentTypes.ComponentType componentType, Action<Instruction> callback = null)
    {
        Debug.Log("Generating instruction for " + componentType);

        Instruction instruction = new Instruction();
        instruction.componentTypeEnum = componentType;

        string query = string.Format(instructionTemplate, componentType);

        bool isRequestCompleted = false;

        ClientRAG.instance.SendRequest(query, (responseText, imageTextures) =>
        {
            instruction.instructionText = responseText;
            instruction.imageTextures = imageTextures;
            isRequestCompleted = true;
        });

        // Wait until the request is completed
        while (!isRequestCompleted)
        {
            yield return null; // Wait for the next frame
        }

        // Add the generated instruction to the dictionary
        generatedInstructions.Add(componentType, instruction);

        Debug.Log("Generated instruction for " + componentType + ": \n\n" + instruction.instructionText);
        OnNewInstructionGenerated.Invoke(instruction);

        callback?.Invoke(instruction);

        yield return instruction;
    }

    internal string GetInstructionForComponentType(ComponentTypes.ComponentType componentType)
    {
        // Find the instruction for the given component type
        if (generatedInstructions.ContainsKey(componentType))
        {
            return generatedInstructions[componentType].instructionText;
        }

        return "No instruction found";
    }
}
