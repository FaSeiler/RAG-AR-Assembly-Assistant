using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old_Implementation
{
    [System.Serializable]
public struct Instruction
{
    public ComponentTypes.ComponentType componentTypeEnum;
    public string instructionText;
    public List<Texture2D> imageTextures;
    public List<int> page_numbers;
}
}