using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStepAnimation : InstructionStep
{
    public Animator animator;
    public int animationIndex = 0; // Change this to choose a different animation in the animator

    public override void OnEnable()
    {
        base.OnEnable();
        StartAnimation();
    }

    public void StartAnimation()
    {
        if (animator != null)
            animator.SetBool(animationIndex.ToString(), true);
    }

    public void StopAnimation()
    {
        if (animator != null)
            animator.SetBool(animationIndex.ToString(), false);
    }
}
