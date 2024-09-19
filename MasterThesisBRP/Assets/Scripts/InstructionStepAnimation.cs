using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStepAnimation : InstructionStep
{
    public Animator animator;
    public int animationIndex = 0; // Change this to choose a different animation in the animator

    private GameObject modelGO;
    private ComponentSIMATIC componentSIMATIC;

    public override void Init(ComponentSIMATIC componentSIMATIC)
    {
        this.componentSIMATIC = componentSIMATIC;
        
        //TrackingManager.OnFirstComponentReferencePointChanged.AddListener(UpdatePosition);

        SetAssemblyInstruction(componentSIMATIC);
        AddAnimatingModel(componentSIMATIC);
        AddArrows(componentSIMATIC);

        base.Init(componentSIMATIC);
    }

    //private void UpdatePosition(Transform newTransform)
    //{
    //    transform.rotation = newTransform.rotation;
    //    transform.position = newTransform.position + componentSIMATIC.offsetOnRail;
    //}

    // We can't add all animating models at the start, because we don't know the position of the first component
    // Take first reference point if we don't have the model target position yet, otherwise take the model target position
    private void AddAnimatingModel(ComponentSIMATIC componentSIMATIC)
    {
        // position is always first plus offset.
        //transform.position = componentSIMATIC.offsetOnRail; // The parent of the animation defines the world position
        //UpdatePosition();

        transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.position = TrackingManager.instance.positionFirstComponentOnRail + componentSIMATIC.offsetOnRail;

        // Instantiate the model as a child of the position parent to not mess up animation position
        modelGO = Instantiate(componentSIMATIC.modelPrefab, transform); 
        
        animator = modelGO.AddComponent<Animator>(); // Add an animator to the model
        animator.runtimeAnimatorController = // Get the animator controller for the component type
            AnimationDatabase.instance.GetAnimatorController(componentSIMATIC.componentType); 
        AnimationDatabase.instance.AttachAnimationArrows(modelGO, componentSIMATIC.componentType); // Attach animation arrows to the model
    }

    public void UpdatePosition()
    {
        //if (componentSIMATIC == null)
        //{
        //    return;
        //}

        //// Take first reference point if we don't have the model target position yet, otherwise take the model target position
        //if (TrackingManager.instance.loggedInFirstComponent != null)
        //{
        //    transform.position = TrackingManager.instance.loggedInFirstComponent.transform.position + componentSIMATIC.offsetOnRail;
        //}
        //else
        //{
        //    transform.position = TrackingManager.instance.positionFirstComponentOnRail + componentSIMATIC.offsetOnRail;
        //}
    }

    private void AddArrows(ComponentSIMATIC componentSIMATIC)
    {
        // Set arrows based on the animation
    }

    /// <summary>
    /// Sets the assembly instruction of the componentSIMATIC to the instruction variable
    /// </summary>
    private void SetAssemblyInstruction(ComponentSIMATIC componentSIMATIC)
    {
        instruction = componentSIMATIC.assemblyInstruction;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        UpdatePosition();
        StartAnimation();
    }

    public void StartAnimation()
    {
        if (!initialized)
            return;
        
        animator.SetBool(animationIndex.ToString(), true);
    }

    public void StopAnimation()
    {
        if (!initialized)
            return;

        animator.SetBool(animationIndex.ToString(), false);
    }
}
