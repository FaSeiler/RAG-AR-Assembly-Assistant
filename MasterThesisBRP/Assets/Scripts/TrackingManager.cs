using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingManager : MonoBehaviour //DefaultObserverEventHandler
{
    public ImageTargetBehaviour imageTarget;
    public List<ModelTargetBehaviour> modelTargets = new List<ModelTargetBehaviour>();

    private void Start()
    {
        imageTarget.OnTargetStatusChanged += OnTargetStatusChanged;

        foreach (ModelTargetBehaviour modelTarget in modelTargets)
        {
            modelTarget.OnTargetStatusChanged += OnTargetStatusChanged;
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
