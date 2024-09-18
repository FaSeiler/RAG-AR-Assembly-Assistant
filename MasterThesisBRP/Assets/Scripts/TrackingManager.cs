using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingManager : Singleton<TrackingManager> //DefaultObserverEventHandler
{
    public ImageTargetBehaviour imageTarget;
    public List<ModelTargetBehaviour> modelTargets = new List<ModelTargetBehaviour>();
    public ModelTargetBehaviour activeModelTarget;


    private void Start()
    {
        imageTarget.OnTargetStatusChanged += OnTargetStatusChanged;

        foreach (ModelTargetBehaviour modelTarget in modelTargets)
        {
            modelTarget.OnTargetStatusChanged += OnTargetStatusChanged;
        }
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

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        Debug.Log("Target " + behaviour.name + " Status Changed: " + targetStatus.Status.ToString());
    }

    public void OnTrackableFound(GameObject trackableGameObject)
    {

    }

    //protected override void OnTrackingFound()
    //{
    //    Debug.Log("Target Found");

    //    //Hide the scan preview for the current instruction step

    //    InstructionStepScan currentScanStep = (InstructionStepScan)InstructionStepManager.instance.currentInstructionStep;
    //    currentScanStep.HideScanPreview();

    //    base.OnTrackingFound();
    //}

    //protected override void OnTrackingLost()
    //{
    //    Debug.Log("Target Lost");

    //    //Show the scan preview for the current instruction step

    //    InstructionStepScan currentScanStep = (InstructionStepScan)InstructionStepManager.instance.currentInstructionStep;
    //    currentScanStep.ShowScanPreview();

    //    base.OnTrackingLost();
    //}
}
