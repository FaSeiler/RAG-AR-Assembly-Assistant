using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ComponentDataUI : WindowManager
{
    public string activeComponentArticleNumber;

    [Space(10)]
    public GameObject activePreviewComponent;

    [Space(10)]
    public GameObject listParent;
    public GameObject keyTextEntryPrefab;
    public GameObject valueTextEntryPrefab;
    public GameObject placeholderPrefab;
    public TextMeshProUGUI openOnIndustryMallText;

    private RotatingPreviewComponent previewComponent;

    private void Awake()
    {
        OnWindowEnabled.AddListener(OnWindowUIEnabled);

        previewComponent = GameObject.FindGameObjectWithTag("PreviewComponentModelData").GetComponent<RotatingPreviewComponent>();
    }

    private void OnWindowUIEnabled()
    {
        StartCoroutine(UpdateActiveComponent());
    }

    public IEnumerator UpdateActiveComponent()
    {
        ClearScrollableListEntries();

        yield return new WaitUntil(() => InstructionStepManager.instance.createdFirstInstructionStep);

        ComponentSIMATIC componentSIMATIC = InstructionStepManager.instance.currentInstructionStep.component;

        string industryMallLink = "https://mall.industry.siemens.com/mall/de/WW/Catalog/Product/" + componentSIMATIC.articleNumber;
        openOnIndustryMallText.text = $"<link=\"{industryMallLink}\">Open on Industry Mall</link>";

        yield return new WaitUntil(() => componentSIMATIC.propertiesInitialized);

        UpdatePropertiesText(componentSIMATIC);
        UpdatePreviewImage(componentSIMATIC);
    }

    //private void OnComponentWebDataInitialized(ComponentSIMATIC component)
    //{
    //    UpdatePropertiesText(component);
    //}

    private void UpdatePreviewImage(ComponentSIMATIC componentSIMATIC)
    {
        if (activePreviewComponent != null)
        {
            Destroy(activePreviewComponent);
        }

        GameObject previewComponentModel = previewComponent.SetActivePreview(componentSIMATIC.modelPrefab);

        activePreviewComponent = previewComponentModel;
    }

    private void UpdatePropertiesText(ComponentSIMATIC component)
    {
        foreach (KeyValuePair<string, string> kvp in component.properties)
        {
            AddKeyValuePair(kvp.Key, kvp.Value);
        }
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
