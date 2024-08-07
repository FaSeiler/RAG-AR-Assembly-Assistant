using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Old_Implementation
{
    [System.Serializable]
    public class ComponentSIMATIC
    {
        //public string articleNumber;
        //public GameObject model;
        //public Dictionary<string, string> webDataDictionary; // Data about the component scraped from the web

        //public UnityEvent<ComponentSIMATIC> OnComponentWebDataInitialized = new UnityEvent<ComponentSIMATIC>();

        //public ComponentSIMATIC(string articleNumber)
        //{
        //    InitData(articleNumber);
        //}

        //// Init model and article number and get the component information from the web scraper
        //private void InitData(string articleNumber)
        //{
        //    WebScraperSIMATIC.instance.StartScraping(articleNumber, OnComponentDataReceived);
        //    model = ModelDatabase.instance.GetModel(articleNumber);
        //    this.articleNumber = articleNumber;
        //}

        //private void OnComponentDataReceived(Dictionary<string, string> dataDictionary)
        //{
        //    this.webDataDictionary = dataDictionary;
        //    OnComponentWebDataInitialized.Invoke(this);
        //}

        //public void PrintWebDataDictionary()
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    foreach (KeyValuePair<string, string> kvp in webDataDictionary)
        //    {
        //        stringBuilder.Append(kvp.Key + ": " + kvp.Value + "\n");
        //    }
        //}
    }
}
