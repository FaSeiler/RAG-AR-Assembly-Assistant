using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableInstruction
{
    public ComponentTypes.ComponentType componentType;
    public string text;
    public List<string> imagePaths; // Paths to the saved image files
    public List<int> pageNumbers;
    //public string animationClipName;
}