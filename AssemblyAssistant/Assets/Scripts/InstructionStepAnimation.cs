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

        TrackingManager.OnReferencePointChanged.AddListener(UpdatePosition);

        SetAssemblyInstruction(componentSIMATIC);
        AddAnimatingModel(componentSIMATIC);

        base.Init(componentSIMATIC);
    }

    // We can't add all animating models at the start, because we don't know the position of the first component
    // Take first reference point if we don't have the model target position yet, otherwise take the model target position
    private void AddAnimatingModel(ComponentSIMATIC componentSIMATIC)
    {
        // Update the position of the animation parent based on the reference point
        UpdatePosition();

        // Instantiate the model as a child of the position parent to not mess up animation position
        modelGO = Instantiate(componentSIMATIC.modelPrefab, transform); 
        
        animator = modelGO.AddComponent<Animator>(); // Add an animator to the model
        animator.runtimeAnimatorController = // Get the animator controller for the component type
            AnimationDatabase.instance.GetAnimatorController(componentSIMATIC.componentType);
        AnimationDatabase.instance.AttachAnimationArrows(modelGO, componentSIMATIC.componentType); // Attach animation arrows to the model

        AnimationDatabase.instance.AttachAdditionalModels(modelGO, componentSIMATIC.componentType); // Attach additional models to the model
    }

    // Update the position of the animation model based on the reference point
    private void UpdatePosition()
    {
        Transform placeTransform = TrackingManager.instance.GetTransformNextToReferenceComponent(componentSIMATIC.offsetOnRail);
        placeTransform.Rotate(-90f, 0f, 0f);
        transform.position = placeTransform.position;
        transform.rotation = placeTransform.rotation;
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
