using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDataUI : MonoBehaviour
{
    public GameObject listParent;
    public GameObject keyTextEntryPrefab;
    public GameObject valueTextEntryPrefab;
    public GameObject placeholderPrefab;

    public RotatingPreviewComponent activePreviewComponentParent;
    public GameObject activePreviewComponent;

    public ComponentSIMATIC activeComponent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextComponent();
        }
    }

    public void LoadNextComponent()
    {
        int nextIndex = ComponentDatabase.instance.components.IndexOf(activeComponent) + 1;

        if (nextIndex >= ComponentDatabase.instance.components.Count)
        {
            nextIndex = 0;
        }

        UpdateActiveComponent(ComponentDatabase.instance.components[nextIndex]);
    }

    public void UpdateActiveComponent(ComponentSIMATIC component)
    {
        ClearScrollableListEntries();

        foreach (KeyValuePair<string, string> kvp in component.dataDictionary)
        {
            AddKeyValuePair(kvp.Key, kvp.Value);
        }

        UpdatePreviewImage(component);

        activeComponent = component;
    }

    private void UpdatePreviewImage(ComponentSIMATIC component)
    {
        if (activeComponent != null)
        {
            Destroy(activePreviewComponent);
        }

        GameObject previewComponent = Instantiate(component.model);

        activePreviewComponentParent.UpdatePreviewComponent(previewComponent);

        activePreviewComponent = previewComponent;
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
