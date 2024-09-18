using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStepAnimation : InstructionStep
{
    public Animator animator;
    public int animationIndex = 0; // Change this to choose a different animation in the animator

    private GameObject modelGO;

    public override void Init(ComponentSIMATIC componentSIMATIC)
    {
        SetAssemblyInstruction(componentSIMATIC);
        AddAnimatingModel(componentSIMATIC);
        AddArrows(componentSIMATIC);

        base.Init(componentSIMATIC);
    }

    private void AddAnimatingModel(ComponentSIMATIC componentSIMATIC)
    {
        transform.position = componentSIMATIC.positionAnimationModel; // The parent of the animation defines the world position
        //transform.rotation = Quaternion.Euler(90, 0, 0);
        modelGO = Instantiate(componentSIMATIC.modelPrefab, transform); // Instantiate the model as a child of the position parent
        animator = modelGO.AddComponent<Animator>(); // Add an animator to the model
        animator.runtimeAnimatorController = // Get the animator controller for the component type
            AnimationDatabase.instance.GetAnimatorController(componentSIMATIC.componentType); 
        AnimationDatabase.instance.AttachAnimationArrows(modelGO, componentSIMATIC.componentType); // Attach animation arrows to the model
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
