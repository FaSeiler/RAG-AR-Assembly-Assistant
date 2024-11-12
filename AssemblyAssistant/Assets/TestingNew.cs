using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingNew : MonoBehaviour
{
    private InstructionStepAnimation instructionStepAnimation;

    private void Start()
    {
        instructionStepAnimation = GetComponent<InstructionStepAnimation>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            instructionStepAnimation.StartAnimation();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            instructionStepAnimation.StopAnimation();
        }
    }
}
