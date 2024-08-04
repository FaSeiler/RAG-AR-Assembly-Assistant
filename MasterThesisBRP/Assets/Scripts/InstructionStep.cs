using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStep : MonoBehaviour
{
    // The article number of the component that this step is related to
    // This needs to be set in the Unity Editor so that the ComponentSIMATIC object can be fetched from the database
    public string componentArticleNumber; 

    public string instructionText;
    
    public ComponentSIMATIC component; // The component that this step is related to
    public List<GameObject> relatedGameObjects; // Use this to enable relevant GameObjects for this step (e.g., model targets)
    public Animator animator;

    public int animationIndex = 0; // Change this to choose a different animation in the animator

    private void Awake()
    {
        if (componentArticleNumber != null)
        {
            if (componentArticleNumber == "")
            {
                Debug.LogError("Component article number is empty for instruction step: " + gameObject.name);
            }
            else
            {
                component = ComponentDatabase.instance.GetComponentSIMATIC(componentArticleNumber);
            }
        }
    }

    public void StartAnimation()
    {
        if(animator != null)
            animator.SetBool(animationIndex.ToString(), true);
    }

    public void StopAnimation()
    {
        if (animator != null)
            animator.SetBool(animationIndex.ToString(), false);
    }

    public void OnEnable()
    {
        foreach (GameObject relatedGameObject in relatedGameObjects)
        {
            if (relatedGameObject != null)
                relatedGameObject.SetActive(true);
        }
    }

    public void OnDisable()
    {
        foreach (GameObject relatedGameObject in relatedGameObjects)
        {
            if (relatedGameObject != null)
                relatedGameObject.SetActive(false);
        }
    }
}
