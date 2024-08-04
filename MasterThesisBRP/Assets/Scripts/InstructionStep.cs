using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstructionStep : MonoBehaviour
{
    // The article number of the component that this step is related to
    // This needs to be set in the Unity Editor so that the ComponentSIMATIC object can be fetched from the database
    public string componentArticleNumber; 

    public string instructionText;
    public ComponentSIMATIC component; // The SIMATIC component that this step is about
    public List<GameObject> relatedGameObjects; // Use this to enable relevant GameObjects for this step (e.g., model targets)

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

    public virtual void OnEnable()
    {
        foreach (GameObject relatedGameObject in relatedGameObjects)
        {
            if (relatedGameObject != null)
                relatedGameObject.SetActive(true);
        }
    }

    public virtual void OnDisable()
    {
        foreach (GameObject relatedGameObject in relatedGameObjects)
        {
            if (relatedGameObject != null)
                relatedGameObject.SetActive(false);
        }
    }
}
