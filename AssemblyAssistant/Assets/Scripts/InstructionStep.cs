using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class InstructionStep : MonoBehaviour
{
    public Instruction instruction;
    public ComponentSIMATIC component; // The SIMATIC component that this step is about

    [Header("Debugging")]
    protected bool initialized = false;

    public virtual void Init(ComponentSIMATIC componentSIMATIC)
    {
        component = componentSIMATIC;
        initialized = true;
    }

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {

    }

    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }
}
