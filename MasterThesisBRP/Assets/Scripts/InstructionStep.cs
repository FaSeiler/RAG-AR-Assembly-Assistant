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
        if(animator != null)
            animator.SetBool(animationIndex.ToString(), true);
    }

    public void StopAnimation()
    {
        if (animator != null)
            animator.SetBool(animationIndex.ToString(), false);
    }

    public void OnEnable()
    {
        foreach (GameObject component in components)
        {
            component.SetActive(true);
        }
    }

    public void OnDisable()
    {
        foreach (GameObject component in components)
        {
            component.SetActive(false);
        }
    }
}
