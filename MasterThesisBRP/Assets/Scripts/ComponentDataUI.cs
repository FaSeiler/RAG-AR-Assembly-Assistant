using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Old_Implementation
{
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
        public TextMeshProUGUI openOnIndustryMallText;

        private RotatingPreviewComponent previewComponent;

        private void Awake()
        {
            previewComponent = GameObject.FindGameObjectWithTag("PreviewComponentModelData").GetComponent<RotatingPreviewComponent>();
        }

        //private void Start()
        //{
        //    InstructionStepManager.OnNewInstructionStep.AddListener(OnNewInstructionStep);
        //}

        //private void OnNewInstructionStep(InstructionStep newInstructionStep)
        //{
        //    activeComponent = newInstructionStep.component;

        //    if (activeComponent != null)
        //    {
        //        UpdateActiveComponent(activeComponent);
        //    }
        //}

        //public void UpdateActiveComponent(ComponentSIMATIC component)
        //{
        //    ClearScrollableListEntries();

        //    string industryMallLink = "https://mall.industry.siemens.com/mall/de/WW/Catalog/Product/" + component.articleNumber;
        //    openOnIndustryMallText.text = $"<link=\"{industryMallLink}\">Open on Industry Mall</link>";

        //    if (component.webDataDictionary == null) // If the web data has not been initialized yet, wait for it to be initialized
        //    {
        //        component.OnComponentWebDataInitialized.AddListener(OnComponentWebDataInitialized);
        //    }
        //    else
        //    {
        //        UpdateWebDataText(component);
        //    }

        //    UpdatePreviewImage(component);

        //    activeComponent = component;
        //}

        //private void OnComponentWebDataInitialized(ComponentSIMATIC component)
        //{
        //    UpdateWebDataText(component);
        //}

        //private void UpdateWebDataText(ComponentSIMATIC component)
        //{
        //    foreach (KeyValuePair<string, string> kvp in component.webDataDictionary) // TODO: THIS LATER
        //    {
        //        AddKeyValuePair(kvp.Key, kvp.Value);
        //    }
        //}

        //private void UpdatePreviewImage(ComponentSIMATIC component)
        //{
        //    if (activeComponent != null)
        //    {
        //        Destroy(activePreviewComponent);
        //    }

        //    if (component.model == null)
        //    {
        //        Debug.Log("For the component *" + component.articleNumber + "* no model was found!");
        //        return;
        //    }

        //    GameObject model = previewComponent.SetActivePreview(component.model);

        //    activePreviewComponent = model;
        //}

        //public void AddKeyValuePair(string key, string value)
        //{
        //    GameObject keyTextEntry = Instantiate(keyTextEntryPrefab, listParent.transform);
        //    GameObject valueTextEntry = Instantiate(valueTextEntryPrefab, listParent.transform);
        //    GameObject placeHolder = Instantiate(placeholderPrefab, listParent.transform);

        //    keyTextEntry.name = "Key_" + key;
        //    valueTextEntry.name = "Value_" + value;
        //    placeHolder.name = "PlaceHolder";

        //    keyTextEntry.GetComponent<TMPro.TextMeshProUGUI>().text = key + ":";
        //    valueTextEntry.GetComponent<TMPro.TextMeshProUGUI>().text = value;
        //}

        //public void ClearScrollableListEntries()
        //{
        //    foreach (Transform child in listParent.transform)
        //    {
        //        Destroy(child.gameObject);
        //    }
        //}
    }
}
