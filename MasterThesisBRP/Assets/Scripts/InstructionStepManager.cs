using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages iterating through the various instruction steps
/// </summary>
public class InstructionStepManager : Singleton<InstructionStepManager>
{
    [Header("Dynamic Properties")]
    public InstructionStep currentInstructionStep;
    public int currentInstructionStepIndex = 0;
    public int totalInstructionStepCount = 0;
    public List<InstructionStep> createdInstructionSteps = new List<InstructionStep>();

    [Header("Static Properties")]
    public Transform parentScanInstructions;
    public Transform parentAnimationInstructions;
    public InstructionStepUIManager instructionStepUIManager;

    [Header("Debugging")]
    public bool isCreatingInstructionStep = false;
    public bool createdFirstInstructionStep = false;


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
        CreateAndSetScanInstructionStep(componentSIMATIC);

        /* Set the currentInstructionStep to the first instruction step */
        if (currentInstructionStep == null)
        {
            currentInstructionStepIndex = 0;
            totalInstructionStepCount = createdInstructionSteps.Count;

            SetCurrentInstructionStep();

            createdFirstInstructionStep = true;
        }

        /* Then create the animation instruction step */
        yield return new WaitUntil(() => componentSIMATIC.assemblyInstructionInitialized); // Wait for the assemblyInstructionInitialized to become true
        CreateAndSetAssemblyInstructionStep(componentSIMATIC);
        UpdateEnabledInstructionStep();

        isCreatingInstructionStep = false;
    }

    private void CreateAndSetScanInstructionStep(ComponentSIMATIC componentSIMATIC)
    {
        GameObject scanInstructionStepGO = new GameObject("ScanInstructionStep_" + componentSIMATIC.name); // Create a new GameObject 
        scanInstructionStepGO.transform.SetParent(parentScanInstructions); // Set the parent of the new GameObject to the parentScanInstructions
        InstructionStepScan instructionStepScan = scanInstructionStepGO.AddComponent<InstructionStepScan>(); // Add the InstructionStepScan component 
        instructionStepScan.Init(componentSIMATIC); // Initialize the InstructionStepScan component

        createdInstructionSteps.Add(instructionStepScan);
        totalInstructionStepCount = createdInstructionSteps.Count;
    }

    private void CreateAndSetAssemblyInstructionStep(ComponentSIMATIC componentSIMATIC)
    {
        GameObject animationInstructionStepGO = new GameObject("AnimationInstructionStep_" + componentSIMATIC.name); // Create a new GameObject
        animationInstructionStepGO.transform.SetParent(parentAnimationInstructions); // Set the parent of the new GameObject to the parentAnimationInstructions
        InstructionStepAnimation instructionStepAnimation = animationInstructionStepGO.AddComponent<InstructionStepAnimation>(); // Add the InstructionStepAnimation component
        instructionStepAnimation.Init(componentSIMATIC); // Initialize the InstructionStepAnimation component

        createdInstructionSteps.Add(instructionStepAnimation);
        totalInstructionStepCount = createdInstructionSteps.Count;
    }

    /// <summary>
    /// Sets the currentInstructionStep to the instruction step at the currentInstructionStepIndex
    /// </summary>
    private void SetCurrentInstructionStep()
    {
        currentInstructionStep = createdInstructionSteps[currentInstructionStepIndex];
        instructionStepUIManager.UpdateInstructionUI(currentInstructionStep, currentInstructionStepIndex + 1, totalInstructionStepCount);

        // Enable currentInstructionStep and disable all others
        UpdateEnabledInstructionStep();
    }

    /// <summary>
    /// Enable currentInstructionStep and disable all others
    /// </summary>
    private void UpdateEnabledInstructionStep()
    {
        foreach (InstructionStep instructionStep in createdInstructionSteps)
        {
            if (instructionStep == currentInstructionStep)
            {
                instructionStep.gameObject.SetActive(true);
                
                // Workaround: If the instructionStep is of type InstructionStepScan, show the scan preview
                if (instructionStep is InstructionStepScan)
                {
                    InstructionStepScan instructionStepScan = (InstructionStepScan)instructionStep;
                    
                    // Update the model target for the object to scan
                    TrackingManager.instance.UpdateActiveModelTarget(instructionStepScan.component.modelTargetBehaviour);

                    // Show the scan preview
                    StartCoroutine(instructionStepScan.ShowScanPreview());
                }
            }
            else
            {
                instructionStep.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Sets the currentInstructionStep to the next instruction step
    /// </summary>
    public void NextInstructionStep()
    {
        if (currentInstructionStepIndex == createdInstructionSteps.Count - 1)
        {
            return;
        }

#if !UNITY_EDITOR
        // If we are at index 1 we are setting the reference point to the first component
        // NOTE: We can't use other components as reference points, since they can't be tracked using model tracking
        // as soon as they are installed on the rail
        // Therefore we have to remember the position of the first component and use it as a reference point for all other components
        if (currentInstructionStepIndex == 1)
        {
            if (TrackingManager.instance.modelTargets[0].TargetStatus.Status != Vuforia.Status.TRACKED)
            {
                // We don't want to continue to the next instruction step if the model target is not tracked
                // This ensures that the reference point for all further components is accurate
                WarningUI.instance.ShowWarning("The component needs to be scanned to continue!");

                return;
            }
            else
            {
                // Log in the first component, override the transform where it is instantiated
                TrackingManager.instance.CreateLoggedInComponent(
                    currentInstructionStep.component, TrackingManager.instance.modelTargets[0].transform);
            }
            
        }
        // We are moving from an assembly instruction step to the next scan instruction step
        // Therefore we need to log in the position of the component, as it has been installed at a fixed position
        else if (currentInstructionStep is InstructionStepAnimation)
        {
            // Log in all other components. We don't need to override the transform, as they are instantiated in reference to the first component
            TrackingManager.instance.CreateLoggedInComponent(currentInstructionStep.component);
        }
#endif

        currentInstructionStepIndex++;
        WarningUI.instance.HideWarning();

        SetCurrentInstructionStep();
    }

    /// <summary>
    /// Sets the currentInstructionStep to the previous instruction step
    /// </summary>
    public void PreviousInstructionStep()
    {
        if (currentInstructionStepIndex == 0)
        {
            return;
        }

        currentInstructionStepIndex--;
        WarningUI.instance.HideWarning();

        SetCurrentInstructionStep();
    }
}
