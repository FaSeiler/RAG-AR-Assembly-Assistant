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
using System.IO;

public class Testing : MonoBehaviour
{
    // Reference to the "reference" object
    public GameObject reference;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.U))
    //    {
    //        TrackingManager.instance.UpdateReferenceTransform(reference.transform);
           
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        foreach (InstructionStep instructionStep in InstructionStepManager.instance.createdInstructionSteps)
    //        {
    //            if (instructionStep is InstructionStepAnimation)
    //            {
    //                InstructionStepAnimation instructionStepAnimation = (InstructionStepAnimation)instructionStep;
    //                instructionStepAnimation.StartAnimation();
    //            }
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        foreach (InstructionStep instructionStep in InstructionStepManager.instance.createdInstructionSteps)
    //        {
    //            if (instructionStep is InstructionStepAnimation)
    //            {
    //                InstructionStepAnimation instructionStepAnimation = (InstructionStepAnimation)instructionStep;
    //                instructionStepAnimation.StopAnimation();
    //            }
    //        }
    //    }
    //}
}
