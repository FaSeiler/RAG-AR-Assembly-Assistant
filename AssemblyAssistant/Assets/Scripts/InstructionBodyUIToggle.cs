using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionBodyUIToggle : MonoBehaviour
{
    // Find InstructionStepUIManager in the scene 
    private InstructionStepUIManager instructionStepUIManager;

    private void Start()
    {
        // Find the InstructionStepUIManager in the scene
        instructionStepUIManager = FindFirstObjectByType<InstructionStepUIManager>();
    }

    public void EnableInstructionBody()
    {
        instructionStepUIManager.EnableInstructionBody();
    }

    public void DisableInstructionBody()
    {
        instructionStepUIManager.DisableInstructionBody();
    }
}
