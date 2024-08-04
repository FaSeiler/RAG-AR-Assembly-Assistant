using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class ComponentDataUI : WindowManager
{
    public ComponentSIMATIC activeComponent;
    public string activeComponentArticleNumber;

    [Space(10)]
    public GameObject activePreviewComponent;

    [Space(10)]
    public GameObject listParent;
    public GameObject keyTextEntryPrefab;
    public GameObject valueTextEntryPrefab;
    public GameObject placeholderPrefab;


    private void Start()
    {
        InstructionStepManager.OnNewInstructionStep.AddListener(OnNewInstructionStep);
        OnWindowEnabled.AddListener(OnWindowUIEnabled);
        OnWindowDisabled.AddListener(OnWindowUIDisabled);
    }

    private GameObject lastPreviewComponent;

    public void OnWindowUIEnabled()
    {
        if (activeComponent != null)
        {
            // Store the active preview component so we can enable it again when the window is disabled
            lastPreviewComponent = RotatingPreviewComponent.instance.activePreviewComponent;

            UpdateActiveComponent(activeComponent);
        }
    }

    private void OnWindowUIDisabled()
    {
        RotatingPreviewComponent.instance.SetActivePreview(lastPreviewComponent);
    }

    private void OnNewInstructionStep(InstructionStep newInstructionStep)
    {
        activeComponent = newInstructionStep.component;
    }

    public void UpdateActiveComponent(ComponentSIMATIC component)
    {
        ClearScrollableListEntries();

        if (component.webDataDictionary == null) // If the web data has not been initialized yet, wait for it to be initialized
        {
            component.OnComponentWebDataInitialized.AddListener(OnComponentWebDataInitialized);
        }
        else
        {
            UpdateWebDataText(component);
        }

        UpdatePreviewImage(component);

        activeComponent = component;
    }

    private void OnComponentWebDataInitialized(ComponentSIMATIC component)
    {
        UpdateWebDataText(component);
    }

    private void UpdateWebDataText(ComponentSIMATIC component)
    {
        foreach (KeyValuePair<string, string> kvp in component.webDataDictionary) // TODO: THIS LATER
        {
            AddKeyValuePair(kvp.Key, kvp.Value);
        }
    }

    private void UpdatePreviewImage(ComponentSIMATIC component)
    {
        if (activeComponent != null)
        {
            Destroy(activePreviewComponent);
        }

        if (component.model == null)
        {
            Debug.Log("For the component *" + component.articleNumber + "* no model was found!");
            return;
        }

        GameObject model = RotatingPreviewComponent.instance.SetActivePreview(component.model);

        activePreviewComponent = model;
    }

    public void AddKeyValuePair(string key, string value)
    {
        GameObject keyTextEntry = Instantiate(keyTextEntryPrefab, listParent.transform);
        GameObject valueTextEntry = Instantiate(valueTextEntryPrefab, listParent.transform);
        GameObject placeHolder = Instantiate(placeholderPrefab, listParent.transform);

        keyTextEntry.name = "Key_" + key;
        valueTextEntry.name = "Value_" + value;
        placeHolder.name = "PlaceHolder";

        keyTextEntry.GetComponent<TMPro.TextMeshProUGUI>().text = key + ":";
        valueTextEntry.GetComponent<TMPro.TextMeshProUGUI>().text = value;
    }

    public void ClearScrollableListEntries()
    {
        foreach (Transform child in listParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
