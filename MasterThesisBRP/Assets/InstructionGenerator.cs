using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class InstructionGenerator : Singleton<InstructionGenerator>
{
    public string instructionTemplate = "How do I mount/install the {0}? Answer in short concise steps! Dont' add any other additional information.";

    public List<string> componentTypes = new List<string>
    {
        "CPU/interface module",
        "BaseUnit for I/O modules",
        "Server Module",
        "BusAdapter for the CPU/interface module",
        "Memory Card for the CPU",
        "I/O modules",
        "SIMATIC ET 200eco",
        "SIMATIC ET 200M",
        "SIMATIC ET 200S"
    };

    public struct Instruction
    {
        public string componentType;
        public string instruction;
        public List<Texture2D> imageTextures;
    }

    public Instruction GetInstruction(string componentType)
    {
        Instruction instruction = new Instruction();
        instruction.componentType = componentType;

        string query = string.Format(instructionTemplate, componentType);


        ClientRAG.instance.SendRequest(query, (responseText, imageTextures) =>
        {
            instruction.instruction = responseText;
            instruction.imageTextures = imageTextures;
        });

        return instruction;
    }
}
