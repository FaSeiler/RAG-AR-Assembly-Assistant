using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.EventSystems;

public class Testing : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("Start generating instruction for BaseUnit");
        StartCoroutine(InstructionGenerator.instance.GenerateInstructionCoroutine(ComponentTypes.ComponentType.BaseUnitForIOModules, (instruction) =>
        {
            Debug.Log("Instruction generated for BaseUnit: " + instruction.instructionText);
        }));
    }
}
