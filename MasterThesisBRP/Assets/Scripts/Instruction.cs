using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Instruction
{
    public ComponentTypes.ComponentType componentTypeEnum;
    public string instructionText;
    public List<Texture2D> imageTextures;
}