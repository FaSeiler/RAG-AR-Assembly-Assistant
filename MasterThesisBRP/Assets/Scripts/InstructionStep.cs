using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStep : MonoBehaviour
{
    public string instructionText;
    public List<GameObject> components; // Use this to enable relevant components for this step (model targets)
    public Animator animator;

    public int animationIndex = 0; // Change this to choose a different animation in the animator

    public void StartAnimation()
    {
        animator.SetBool(animationIndex.ToString(), true);
    }

    public void StopAnimation()
    {
        animator.SetBool(animationIndex.ToString(), false);
    }
}
