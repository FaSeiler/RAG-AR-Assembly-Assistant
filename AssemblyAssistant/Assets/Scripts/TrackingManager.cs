using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class TrackingManager : Singleton<TrackingManager> //DefaultObserverEventHandler
{
    public ImageTargetBehaviour imageTarget;
    public List<ModelTargetBehaviour> modelTargets = new List<ModelTargetBehaviour>();

    [Space(10)]
    public ModelTargetBehaviour activeModelTarget;
    public Transform referenceTransformInit; // Static reference transform for first component (stays unchanged)
    private Transform referenceTransform; // Reference transform that is update throughout the assembly process

    public struct LoggedInComponent
    {
        public ComponentSIMATIC componentSIMATIC;
        public GameObject loggedInComponentVisualizer;
    }

    public GameObject parentLoggedInComponents; // This is just to group all logged in component visualizer GO in the scene
    public Material loggedInComponentMaterial;
    public List<LoggedInComponent> loggedInComponents = new List<LoggedInComponent>();
    public static UnityEvent OnReferencePointChanged = new UnityEvent();

    private void Start()
    {
        referenceTransform = referenceTransformInit;

        imageTarget.OnTargetStatusChanged += OnImageTargetStatusChanged;

        foreach (ModelTargetBehaviour modelTarget in modelTargets)
        {
            modelTarget.OnTargetStatusChanged += OnModelTargetStatusChanged;
        }
    }

    /// <summary>
    /// Use the position of the first component tracked as a model target as a reference for all further animations
    /// Attach the reference point to the image marker for stable tracking after the model target is lost
    /// We know all further components will be placed next to the first component (x-axis)
    /// 
    /// If overrideReferenceTransform is set, use this transform as the reference point
    /// Else use the transform of the first component
    /// </summary>
    public void CreateLoggedInComponent(ComponentSIMATIC componentSIMATIC, Transform overrideReferenceTransform = null)
    {
        if (overrideReferenceTransform == null)
        {
            // This is not the reference component, so we need to calculate the position of the component relative to the reference component
            if (referenceTransform != null)
            {
                overrideReferenceTransform = GetTransformNextToReferenceComponent(componentSIMATIC.offsetOnRail);
                overrideReferenceTransform.Rotate(-90f, 0f, 0f); // Correct the rotation of the model
            }
            else
            {
                Debug.LogError("The first component is not logged in!");
            }
        }

        GameObject loggedInComponentVisualizer = InstantiateLoggedInVisualizer(componentSIMATIC, overrideReferenceTransform);
        Debug.Log("Logged in component: " + loggedInComponentVisualizer.name);
    }

    public GameObject InstantiateLoggedInVisualizer(ComponentSIMATIC componentSIMATIC, Transform transformAtInstantiate)
    {
        GameObject loggedInComponentVisualizer;

        // Check if this component was logged in before
        foreach (LoggedInComponent loggedInComponent in loggedInComponents)
        {
            if (loggedInComponent.componentSIMATIC == componentSIMATIC)
            {
                // Was logged in before: Use the existing visualizer and update its position and rotation
                loggedInComponentVisualizer = loggedInComponent.loggedInComponentVisualizer;
                loggedInComponentVisualizer.transform.position = transformAtInstantiate.transform.position;
                loggedInComponentVisualizer.transform.rotation = transformAtInstantiate.transform.rotation;

                return loggedInComponentVisualizer;
            }
        }

        // Instantiate the model with the transform of the model target
        loggedInComponentVisualizer = Instantiate(componentSIMATIC.modelPrefab, transformAtInstantiate);
        loggedInComponentVisualizer.transform.Rotate(90f, 0f, 0f); // Correct the rotation of the model
        loggedInComponentVisualizer.name = componentSIMATIC.componentName + "_LoggedInComponentVisualizer";
        componentSIMATIC.SetMaterials(loggedInComponentVisualizer, loggedInComponentMaterial); // Green material
        
        // Attach the reference point to the image marker for stable tracking after the model target is lost
        loggedInComponentVisualizer.transform.parent = parentLoggedInComponents.transform;


        // Add the component to the list of logged in components
        LoggedInComponent newloggedInComponent = new LoggedInComponent
        {
            componentSIMATIC = componentSIMATIC,
            loggedInComponentVisualizer = loggedInComponentVisualizer
        };

        loggedInComponents.Add(newloggedInComponent);

        // If this is the first component, set the reference point
        if (loggedInComponents.Count == 1)
        {
            UpdateReferenceTransform(loggedInComponentVisualizer.transform);
        }

        return loggedInComponentVisualizer;
    }

    public void UpdateReferenceTransform(Transform newReferenceTransform)
    {
        referenceTransform = newReferenceTransform;
        UpdateLoggedInComponents();
        OnReferencePointChanged.Invoke();
    }

    // If the reference point is updated, update all logged in components
    private void UpdateLoggedInComponents()
    {
        foreach (LoggedInComponent loggedInComponent in loggedInComponents)
        {
            Transform updatedTransform = GetTransformNextToReferenceComponent(loggedInComponent.componentSIMATIC.offsetOnRail);
            loggedInComponent.loggedInComponentVisualizer.transform.position = updatedTransform.position;
            loggedInComponent.loggedInComponentVisualizer.transform.rotation = updatedTransform.rotation;
        }
    }

    public void RemoveLoggedInComponent(ComponentSIMATIC componentSIMATIC)
    {
        foreach (LoggedInComponent loggedInComponent in loggedInComponents)
        {
            if (loggedInComponent.componentSIMATIC == componentSIMATIC)
            {
                Destroy(loggedInComponent.loggedInComponentVisualizer);
                loggedInComponents.Remove(loggedInComponent);

                if (loggedInComponents.Count == 0)
                {
                    // If there are no more logged in components, reset the reference point to the initial reference point (in scene)
                    referenceTransform = referenceTransformInit;
                }
                break;
            }
        }
    }

    /// <summary>
    /// Returns a transform next to the reference component with the given offset
    /// </summary>
    public Transform GetTransformNextToReferenceComponent(Vector3 offsetOnRail)
    {
        // Adjust the axis of the offset to fix the orientation of the model
        Vector3 adjustedOffset = new Vector3(
            -offsetOnRail.x, offsetOnRail.z, -offsetOnRail.y);

        // Apply the adjusted offset relative to the reference object's rotation and position
        Vector3 worldOffset = referenceTransform.position + referenceTransform.rotation * adjustedOffset;

        GameObject place = new GameObject();
        Transform placeTransform = place.transform;
        Destroy(place);

        // Set the position of the "place" object
        placeTransform.position = worldOffset;

        // Set the rotation of the "place" object to match the "reference" object's rotation
        placeTransform.rotation = referenceTransform.rotation;
        
        return placeTransform;
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

    public void ShowLoggedInComponents()
    {
        parentLoggedInComponents.SetActive(true);
    }

    public void HideLoggedInComponents()
    {
        parentLoggedInComponents.SetActive(false);
    }
}
