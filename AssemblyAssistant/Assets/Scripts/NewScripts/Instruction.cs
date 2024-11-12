using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Instruction
{
    public ComponentTypes.ComponentType componentType; // The component type the instruction is for
    public string text; // The description (steps) of the instruction
    public List<Texture2D> images; // Relevant images for the instruction
    public List<int> pageNumbers;
    //public Animation animationClip; // Animation for the instruction
}
