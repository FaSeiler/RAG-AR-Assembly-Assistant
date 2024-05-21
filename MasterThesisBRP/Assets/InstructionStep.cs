using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStep : MonoBehaviour
{
    public string instructionText;
    public List<GameObject> components;
    public Animator animator;
    public int stepNumber;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartAnimation();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StopAnimation();
        }
    }

    public void StartAnimation()
    {
        animator.SetBool("IsPlaying", true);
    }

    public void StopAnimation()
    {
        animator.SetBool("IsPlaying", false);
    }

    public void StartAnimation(string parameterName)
    {
        // Assuming parameterName is a bool parameter
        animator.SetBool(parameterName, true);
    }

    public void StopAnimation(string parameterName)
    {
        // Assuming parameterName is a bool parameter
        animator.SetBool(parameterName, false);
    }
}
