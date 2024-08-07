using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstructionStep : MonoBehaviour
{
    public Instruction instruction;

    [Header("Debugging")]
    protected bool initialized = false;

    public virtual void Init(ComponentSIMATIC componentSIMATIC)
    {
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











    //public bool useGeneratedInstruction = false;
    //// The article number of the component that this step is related to
    //// This needs to be set in the Unity Editor so that the ComponentSIMATIC object can be fetched from the database
    //public string componentArticleNumber;
    //public Instruction instruction;
    //public ComponentSIMATIC component; // The SIMATIC component that this step is about
    //public ComponentTypes.ComponentType componentType; // The type of the component that this step is about
    //public List<GameObject> relatedGameObjects; // Use this to enable relevant GameObjects for this step (e.g., model targets)

    //public bool initialized = false;

    //public UnityEvent<InstructionStep> OnInstructionUpdated = new UnityEvent<InstructionStep>();

    //public virtual void Awake()
    //{
    //}

    //public virtual void Start()
    //{
    //    //InitInstructionStep();
    //}

    //public void InitInstructionStep()
    //{
    //    if (componentArticleNumber != null)
    //    {
    //        if (componentArticleNumber == "")
    //        {
    //            Debug.LogError("Component article number is empty for instruction step: " + gameObject.name);
    //        }
    //        else
    //        {
    //            component = ComponentDatabase.instance.GetComponentSIMATIC(componentArticleNumber);
    //        }

    //        initialized = true;
    //    }
    //}

    //public virtual void OnEnable()
    //{
    //    if (useGeneratedInstruction)
    //    {
    //        // Check if the instruction for this component type has already been generated
    //        // If it has, use it otherwise wait for the instruction to be generated
    //        if (InstructionGenerator.instance.generatedAssemblyInstructions.TryGetValue(componentType, out instruction))
    //        {
    //            OnInstructionUpdated.Invoke(this);
    //        }
    //        else
    //        {
    //            InstructionGenerator.OnNewAssemblyInstructionGenerated.AddListener(OnNewInstructionGenerated);
    //        }

    //    }

    //    foreach (GameObject relatedGameObject in relatedGameObjects)
    //    {
    //        if (relatedGameObject != null)
    //            relatedGameObject.SetActive(true);
    //    }
    //}

    //private void OnNewInstructionGenerated(Instruction newInstruction)
    //{
    //    if (newInstruction.componentTypeEnum == this.componentType)
    //    {
    //        this.instruction = newInstruction;
    //        OnInstructionUpdated.Invoke(this);
    //    }
    //}

    //public virtual void OnDisable()
    //{
    //    foreach (GameObject relatedGameObject in relatedGameObjects)
    //    {
    //        if (relatedGameObject != null)
    //            relatedGameObject.SetActive(false);
    //    }
    //}
}
