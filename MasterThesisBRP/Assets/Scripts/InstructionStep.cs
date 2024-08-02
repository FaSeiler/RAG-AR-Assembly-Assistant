using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStep : MonoBehaviour
{
    public string instructionText;
    public List<GameObject> components;
    public Animator animator;

    public int animationIndex = 0; // Change this to choose a different animation in the animator

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        StartAnimation();
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        StopAnimation();
    //    }
    //}

    public void StartAnimation()
    {
        animator.SetBool(animationIndex.ToString(), true);
    }

    public void StopAnimation()
    {
        animator.SetBool(animationIndex.ToString(), false);
    }
}
