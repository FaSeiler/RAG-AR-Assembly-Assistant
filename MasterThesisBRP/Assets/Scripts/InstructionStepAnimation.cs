using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStepAnimation : InstructionStep
{
    public Animator animator;
    public int animationIndex = 0; // Change this to choose a different animation in the animator

    public override void Init(ComponentSIMATIC componentSIMATIC)
    {
        SetAssemblyInstruction(componentSIMATIC);
        AddAnimator();

        base.Init(componentSIMATIC);
    }

    /// <summary>
    /// Sets the assembly instruction of the componentSIMATIC to the instruction variable
    /// </summary>
    private void SetAssemblyInstruction(ComponentSIMATIC componentSIMATIC)
    {
        instruction = componentSIMATIC.assemblyInstruction;
    }

    /// <summary>
    /// Adds an Animator component to the GameObject
    /// </summary>
    private void AddAnimator()
    {
        gameObject.AddComponent<Animator>();
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
