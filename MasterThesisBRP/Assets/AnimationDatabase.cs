using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationDatabase : Singleton<AnimationDatabase>
{
    public ArrowSpawner arrowSpawner;

    [Serializable]
    public struct ComponentAnimationData
    {
        public ComponentTypes.ComponentType componentType;
        public AnimatorController animatorController;
        public List<ArrowSpawner.ArrowData> arrowDatas; // This specifies where the arrows needed in the animation are spawned
    }

    public List<ComponentAnimationData> componentAnimationDatas = new List<ComponentAnimationData>();

    public AnimatorController GetAnimatorController(ComponentTypes.ComponentType componentType)
    {
        foreach (ComponentAnimationData componentAnimationData in componentAnimationDatas)
        {
            if (componentAnimationData.componentType == componentType)
            {
                return componentAnimationData.animatorController;
            }
        }

        return null;
    }

    public void AttachAnimationArrows(GameObject parentGO, ComponentTypes.ComponentType componentType)
    {
        foreach (ComponentAnimationData componentAnimationData in componentAnimationDatas)
        {
            if (componentAnimationData.componentType == componentType)
            {
                foreach (ArrowSpawner.ArrowData arrowData in componentAnimationData.arrowDatas)
                {
                    arrowSpawner.AddArrowAtPosition(arrowData, parentGO.transform);
                }
            }
        }
    }
}
