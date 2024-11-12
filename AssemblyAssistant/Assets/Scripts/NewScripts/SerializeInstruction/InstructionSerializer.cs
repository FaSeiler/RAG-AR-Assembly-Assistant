using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InstructionSerializer : Singleton<InstructionSerializer>
{
    public Dictionary<ComponentTypes.ComponentType, Instruction> loadedAssemblyInstructions =
        new Dictionary<ComponentTypes.ComponentType, Instruction>();
    public string filePath;
    public string imageDirectory;

    protected override void Awake()
    {
        base.Awake();

        InitializeFilePathsAndDirectories();

        Debug.Log(filePath);
    }

    public void InitializeFilePathsAndDirectories()
    {
        filePath = Path.Combine(Application.persistentDataPath, "instructions.json");
        imageDirectory = Path.Combine(Application.persistentDataPath, "Images");
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }
    }

    // Returns the path to the saved instructions
    public string SaveInstructions(Dictionary<ComponentTypes.ComponentType, Instruction> instructionsDictionary)
    {
        return SaveInstructions(instructionsDictionary, filePath, imageDirectory);
    }

    public Dictionary<ComponentTypes.ComponentType, Instruction> LoadInstructions()
    {
        return LoadInstructions(filePath);
    }

    // Returns the path to the saved instructions
    public string SaveInstructions(Dictionary<ComponentTypes.ComponentType, Instruction> instructions, string filePath, string imageDirectory)
    {
        Dictionary<ComponentTypes.ComponentType, SerializableInstruction> serializableDict = new Dictionary<ComponentTypes.ComponentType, SerializableInstruction>();

        foreach (var kvp in instructions)
        {
            serializableDict[kvp.Key] = ConvertToSerializable(kvp.Value, imageDirectory);
        }

        string json = JsonConvert.SerializeObject(serializableDict, Formatting.Indented);
        File.WriteAllText(filePath, json);

        return filePath;
    }

    public Dictionary<ComponentTypes.ComponentType, Instruction> LoadInstructions(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new Dictionary<ComponentTypes.ComponentType, Instruction>();
        }

        string json = File.ReadAllText(filePath);
        Dictionary<ComponentTypes.ComponentType, SerializableInstruction> serializableDict = JsonConvert.DeserializeObject<Dictionary<ComponentTypes.ComponentType, SerializableInstruction>>(json);
        Dictionary<ComponentTypes.ComponentType, Instruction> instructions = new Dictionary<ComponentTypes.ComponentType, Instruction>();

        foreach (var kvp in serializableDict)
        {
            instructions[kvp.Key] = ConvertFromSerializable(kvp.Value);
        }

        Debug.Log("Loaded " + instructions.Count + " instructions from filepath: " + filePath);

        return instructions;
    }

    public SerializableInstruction ConvertToSerializable(Instruction instruction, string imageDirectory)
    {
        SerializableInstruction serializableInstruction = new SerializableInstruction
        {
            componentType = instruction.componentType,
            text = instruction.text,
            imagePaths = new List<string>(),
            pageNumbers = instruction.pageNumbers,
        };

        if (instruction.images != null)
        {
            for (int i = 0; i < instruction.images.Count; i++)
            {
                Texture2D image = instruction.images[i];
                string imagePath = Path.Combine(imageDirectory, instruction.componentType.ToString() + "_" + i + ".png");
                File.WriteAllBytes(imagePath, image.EncodeToPNG());
                serializableInstruction.imagePaths.Add(imagePath);
            }
        }

        return serializableInstruction;
    }

    public Instruction ConvertFromSerializable(SerializableInstruction serializableInstruction)
    {
        Instruction instruction = new Instruction
        {
            componentType = serializableInstruction.componentType,
            text = serializableInstruction.text,
            images = new List<Texture2D>(),
            pageNumbers = serializableInstruction.pageNumbers,
            //animationClip = serializableInstruction.animationClipName != null ? Resources.Load<Animation>(serializableInstruction.animationClipName) : null
        };

        foreach (var path in serializableInstruction.imagePaths)
        {
            byte[] imageData = File.ReadAllBytes(path);
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(imageData);
            instruction.images.Add(image);
        }

        return instruction;
    }

    public void DeleteAllSerializedInstructions()
    {
        // If filepath exists, delete it
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // If imageDirectory exists, delete it
        if (Directory.Exists(imageDirectory))
        {
            Directory.Delete(imageDirectory, true);
        }

        // Recreate the imageDirectory
        InitializeFilePathsAndDirectories();
    }

    // Remove one instruction from the saved instructions based on the component type
    // Make sure when this is called no other instruction is being loaded or generated
    public void RemoveInstruction(ComponentTypes.ComponentType componentType)
    {
        // Read the existing JSON file
        string json = File.ReadAllText(filePath);

        // Deserialize the JSON into a dictionary
        Dictionary<ComponentTypes.ComponentType, SerializableInstruction> instructions =
            JsonConvert.DeserializeObject<Dictionary<ComponentTypes.ComponentType, SerializableInstruction>>(json);

        // Remove the entry if it exists
        if (instructions.ContainsKey(componentType))
        {
            instructions.Remove(componentType);

            // Serialize the updated dictionary back to JSON
            string updatedJson = JsonConvert.SerializeObject(instructions, Formatting.Indented);

            // Write the updated JSON back to the file
            File.WriteAllText(filePath, updatedJson);
        }
    }
}
