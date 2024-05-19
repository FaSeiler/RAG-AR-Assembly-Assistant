using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class ComponentSIMATIC
{
    public GameObject model;
    public Dictionary<string, string> dataDictionary;

    public UnityEvent<ComponentSIMATIC> OnComponentInitialized = new UnityEvent<ComponentSIMATIC>();

    public ComponentSIMATIC(string articleNumber)
    {
        Init(articleNumber);
    }

    // Get the component information from the web scraper
    private void Init(string articleNumber)
    {
        WebScraperSIMATIC.instance.StartScraping(articleNumber, OnComponentDataReceived);
        model = ModelDatabase.instance.GetModel(articleNumber);
    }

    private void OnComponentDataReceived(Dictionary<string, string> dataDictionary)
    {
        this.dataDictionary = dataDictionary;
        OnComponentInitialized.Invoke(this);
    }

    public void PrintDataDictionary()
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        foreach (KeyValuePair<string, string> kvp in dataDictionary)
        {
            stringBuilder.Append(kvp.Key + ": " + kvp.Value + "\n");
        }
    }
}
