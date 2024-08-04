using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InstructionStepManager : MonoBehaviour
{
    public InstructionStep currentInstructionStep;
    public int currentStepIndex = 0;
    public ComponentSIMATIC activeComponent;

    [Space(10)]
    public static UnityEvent<InstructionStep> OnNewInstructionStep = new UnityEvent<InstructionStep>();

    [Header("References")]
    public TextMeshProUGUI instructionText;

    private List<InstructionStep> steps = new List<InstructionStep>();

    void Start()
    {
        LoadAllInstructionSteps();
        DisableAllInstructionSteps();
        ShowStep(0);
    }

    private void LoadAllInstructionSteps()
    {
        // Find all InstructionStep objects that are children of this object and add them to the list
        foreach (Transform child in transform)
        {
            InstructionStep step = child.GetComponent<InstructionStep>();
            if (step != null)
            {
                steps.Add(step);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextInstructionStep();
        }
    }

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
        activeComponent = currentInstructionStep.component;

        instructionText.text = currentInstructionStep.instructionText;

        OnNewInstructionStep.Invoke(currentInstructionStep);
    }

    public void DisableAllInstructionSteps()
    {
        foreach (var step in steps)
        {
            step.gameObject.SetActive(false);
        }
    }
}
