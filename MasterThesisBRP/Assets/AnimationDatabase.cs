using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationDatabase : Singleton<AnimationDatabase>
{
    [Serializable]
    public struct ComponentAnimatorController
    {
        public ComponentTypes.ComponentType componentType;
        public AnimatorController animatorController;
    }

    public List<ComponentAnimatorController> componentAnimators = new List<ComponentAnimatorController>();

    public AnimatorController GetAnimatorController(ComponentTypes.ComponentType componentType)
    {
        foreach (ComponentAnimatorController componentAnimator in componentAnimators)
        {
            if (componentAnimator.componentType == componentType)
            {
                return componentAnimator.animatorController;
            }
        }

        return null;
    }
}
