using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingManager : Singleton<TrackingManager> //DefaultObserverEventHandler
{
    public ImageTargetBehaviour imageTarget;
    public List<ModelTargetBehaviour> modelTargets = new List<ModelTargetBehaviour>();

    [Space(10)]
    public ModelTargetBehaviour activeModelTarget;

    private void Start()
    {
        imageTarget.OnTargetStatusChanged += OnImageTargetStatusChanged;

        foreach (ModelTargetBehaviour modelTarget in modelTargets)
        {
            modelTarget.OnTargetStatusChanged += OnModelTargetStatusChanged;
        }
    }

    public GameObject loggedInFirstComponent;
    public Material loggedInComponentMaterial;
    /// <summary>
    /// Use the position of the first component tracked as a model target as a reference for all further animations
    /// Attach the reference point to the image marker for stable tracking after the model target is lost
    /// We know all further components will be placed next to the first component (x-axis)
    /// </summary>
    public void SetFirstComponentReferencePoint(ModelTargetBehaviour modelTargetFirstComponent)
    {
        ComponentSIMATIC cpuComponentSIMATIC = ComponentDatabase.instance.GetComponentByType(ComponentTypes.ComponentType.CPU_InterfaceModule);
        loggedInFirstComponent = Instantiate(cpuComponentSIMATIC.modelPrefab, modelTargetFirstComponent.transform);
        // Attach the reference point to the image marker for stable tracking after the model target is lost
        loggedInFirstComponent.transform.parent = imageTarget.gameObject.transform;
        loggedInFirstComponent.gameObject.name = "LoggedInFirstComponent_REFERENCE";
        loggedInFirstComponent.transform.Rotate(90f, 0f, 0f);
        cpuComponentSIMATIC.SetMaterials(loggedInFirstComponent, loggedInComponentMaterial);

        Debug.Log("Logged in First Component");
    }

    public void UpdateActiveModelTarget(ModelTargetBehaviour modelTarget)
    {
        DisableAllModelTargets();

        modelTarget.gameObject.SetActive(true);

        activeModelTarget = modelTarget;
    }

    public void DisableAllModelTargets()
    {
        foreach (ModelTargetBehaviour modelTarget in modelTargets)
        {
            modelTarget.gameObject.SetActive(false);
        }
    }

    private void OnImageTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        Debug.Log("Target " + behaviour.name + " Status Changed: " + targetStatus.Status.ToString());
    }

    private void OnModelTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        Debug.Log("Target " + behaviour.name + " Status Changed: " + targetStatus.Status.ToString());

        /*
        Status.TRACKED: Good tracking
        Status.EXTENDED_TRACKED: Out of view but still tracked
        Status.LIMITED: Tracked but only with low accuracy
        Status.NO_POSE: Not tracked
         */

        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED) // Good tracking
        {
            //Hide the scan preview for the current instruction step
            InstructionStepScan currentScanStep = (InstructionStepScan)InstructionStepManager.instance.currentInstructionStep;
            currentScanStep.HideScanPreview();
            // We are tracking the model target, so we can move on to the assembly step
            InstructionStepManager.instance.NextInstructionStep();
        }
        else if (targetStatus.Status == Status.NO_POSE || targetStatus.Status == Status.LIMITED) // Not tracked
        {
            //Show the scan preview for the current instruction step
            InstructionStepScan currentScanStep = (InstructionStepScan)InstructionStepManager.instance.currentInstructionStep;
            currentScanStep.ShowScanPreview();
            
            // TODO: Check if this is the correct behavior
            // We lost tracking of the model target, so we need to go back to the scan step 
            //InstructionStepManager.instance.PreviousInstructionStep();
        }
    }
}
