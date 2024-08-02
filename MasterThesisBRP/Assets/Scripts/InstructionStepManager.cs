using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionStepManager : MonoBehaviour
{
    public TextMeshProUGUI instructionText;

    public List<InstructionStep> steps;

    public int currentStepIndex = 0;
    public InstructionStep currentInstructionStep;

    void Start()
    {
        DisableAllInstructionSteps();
        ShowStep(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextInstructionStep();
        }
    }

    //public void InitInstructionTextPanel()
    //{
    //    instructionText.gameObject.SetActive(true);
    //}

    public void NextInstructionStep()
    {
        // Check if there exists a next step
        if (currentStepIndex == steps.Count - 1)
        {
            return;
        }

        currentStepIndex++;
        ShowStep(currentStepIndex);
    }

    public void PreviousInstructionStep() 
    {         
        // Check if there exists a previous step
        if (currentStepIndex == 0)
        {
            return;
        }

        currentStepIndex--;
        ShowStep(currentStepIndex);
    }


    private void ShowStep(int stepIndex)
    {
        // Disable last step
        if (currentInstructionStep != null)
        {
            currentInstructionStep.gameObject.SetActive(false);
        }

        // Get new step
        currentInstructionStep = steps[stepIndex];
        currentStepIndex = stepIndex;
        // Enable new step
        currentInstructionStep.gameObject.SetActive(true);

        instructionText.text = currentInstructionStep.instructionText;
        currentInstructionStep.StartAnimation();
    }

    public void DisableAllInstructionSteps()
    {
        foreach (var step in steps)
        {
            step.gameObject.SetActive(false);
        }
    }
}
