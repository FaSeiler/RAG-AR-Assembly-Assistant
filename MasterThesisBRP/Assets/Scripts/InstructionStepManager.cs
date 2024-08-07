using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Manages iterating through the various instruction steps
/// </summary>
public class InstructionStepManager : MonoBehaviour
{
    public InstructionStep currentInstructionStep;

    public Transform parentScanInstructions;
    public Transform parentAnimationInstructions;

    private bool isCreatingInstructionStep = false;

    private void Start()
    {
        List<ComponentSIMATIC> components = ComponentDatabase.instance.GetAllComponents();
        StartCoroutine(CreateInstructionStepsForAllComponents(components));
    }

    /// <summary>
    /// Creates instruction steps for all components in the list of componentsSIMATIC
    /// </summary>
    public IEnumerator CreateInstructionStepsForAllComponents(List<ComponentSIMATIC> componentsSIMATIC)
    {
        // If isCreatingInstructionStep is true, wait until it becomes false. Then create new instruction step for the next component
        foreach (ComponentSIMATIC componentSIMATIC in componentsSIMATIC)
        {
            yield return new WaitUntil(() => !isCreatingInstructionStep);

            StartCoroutine(CreateInstructionStepsForComponent(componentSIMATIC));
        }
    }

    public IEnumerator CreateInstructionStepsForComponent(ComponentSIMATIC componentSIMATIC)
    {
        isCreatingInstructionStep = true;

        /* First create the scan instruction step */
        CreateScanInstructionStep(componentSIMATIC);

        /* Then create the animation instruction step */
        yield return new WaitUntil(() => componentSIMATIC.assemblyInstructionInitialized); // Wait for the assemblyInstructionInitialized to become true
        CreateAssemblyInstructionStep(componentSIMATIC);

        isCreatingInstructionStep = false;
    }

    private void CreateScanInstructionStep(ComponentSIMATIC componentSIMATIC)
    {
        GameObject scanInstructionStep = new GameObject("ScanInstructionStep_" + componentSIMATIC.name); // Create a new GameObject 
        scanInstructionStep.transform.SetParent(parentScanInstructions); // Set the parent of the new GameObject to the parentScanInstructions
        InstructionStepScan instructionStepScan = scanInstructionStep.AddComponent<InstructionStepScan>(); // Add the InstructionStepScan component 
        instructionStepScan.Init(componentSIMATIC); // Initialize the InstructionStepScan component
    }

    private void CreateAssemblyInstructionStep(ComponentSIMATIC componentSIMATIC)
    {
        GameObject animationInstructionStep = new GameObject("AnimationInstructionStep_" + componentSIMATIC.name); // Create a new GameObject
        animationInstructionStep.transform.SetParent(parentAnimationInstructions); // Set the parent of the new GameObject to the parentAnimationInstructions
        InstructionStepAnimation instructionStepAnimation = animationInstructionStep.AddComponent<InstructionStepAnimation>(); // Add the InstructionStepAnimation component
        instructionStepAnimation.Init(componentSIMATIC); // Initialize the InstructionStepAnimation component
    }


























    //public int currentStepIndex = 0;
    //public ComponentSIMATIC activeComponent;

    //[Space(10)]
    //public static UnityEvent<InstructionStep> OnNewInstructionStep = new UnityEvent<InstructionStep>();

    //[Header("References")]
    //public TextMeshProUGUI instructionText;

    //public List<InstructionStep> steps = new List<InstructionStep>();

    //void Start()
    //{
    //    //LoadInstructionStepsRecursively(this.transform); // We probably have to set them manually to change the order of steps

    //    // If instruction step list was set manually in the inspector, we don't need to load them recursively
    //    foreach (var step in steps)
    //    {
    //        LoadInstructionStep(step);
    //    }

    //    DisableAllInstructionSteps();
    //    ShowStep(0);
    //}

    //private void LoadInstructionStepsRecursively(Transform parent)
    //{
    //    foreach (Transform child in parent)
    //    {
    //        // Check for InstructionStep component in the current child
    //        InstructionStep step = child.GetComponent<InstructionStep>();
    //        LoadInstructionStep(step);
    //        steps.Add(step);

    //        // Recursively check for InstructionSteps in the children of this child
    //        LoadInstructionStepsRecursively(child);
    //    }
    //}

    //private void LoadInstructionStep(InstructionStep instructionStep)
    //{
    //    // Check for InstructionStep component in the current child
    //    if (instructionStep != null)
    //    {
    //        if (!instructionStep.initialized)
    //        {
    //            instructionStep.InitInstructionStep();
    //        }

    //        instructionStep.OnInstructionUpdated.AddListener(OnInstructionStepUpdated);
    //    }
    //}

    //private void OnInstructionStepUpdated(InstructionStep updatedInstructionStep)
    //{
    //    // If an instruction step is updated, check if it is the current instruction step
    //    // If yes, update the instruction text with the updated text it received from the RAG
    //    if (updatedInstructionStep == currentInstructionStep)
    //    {
    //        UpdateInstructionText(updatedInstructionStep.instruction.instructionText);
    //    }
    //}

    //public void NextInstructionStep()
    //{
    //    // Check if there exists a next step
    //    if (currentStepIndex == steps.Count - 1)
    //    {
    //        return;
    //    }

    //    currentStepIndex++;
    //    ShowStep(currentStepIndex);
    //}

    //public void PreviousInstructionStep()
    //{
    //    // Check if there exists a previous step
    //    if (currentStepIndex == 0)
    //    {
    //        return;
    //    }

    //    currentStepIndex--;
    //    ShowStep(currentStepIndex);
    //}


    //private void ShowStep(int stepIndex)
    //{
    //    // Disable last step
    //    if (currentInstructionStep != null)
    //    {
    //        currentInstructionStep.gameObject.SetActive(false);
    //    }

    //    // Get new step
    //    currentInstructionStep = steps[stepIndex];
    //    currentStepIndex = stepIndex;
    //    // Enable new step
    //    currentInstructionStep.gameObject.SetActive(true);
    //    activeComponent = currentInstructionStep.component;

    //    // Update instruction text
    //    if (currentInstructionStep.instruction.instructionText != "")
    //    {
    //        UpdateInstructionText(currentInstructionStep.instruction.instructionText);
    //    }

    //    OnNewInstructionStep.Invoke(currentInstructionStep);
    //}

    //private void UpdateInstructionText(string newInstructionText)
    //{
    //    string richTextFormattedText = URLDetector.FormatTextMeshProForRichTextFormat(newInstructionText);
    //    instructionText.text = richTextFormattedText;
    //}

    //public void DisableAllInstructionSteps()
    //{
    //    foreach (var step in steps)
    //    {
    //        step.gameObject.SetActive(false);
    //    }
    //}
}
