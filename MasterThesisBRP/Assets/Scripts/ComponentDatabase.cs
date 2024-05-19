using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ComponentDatabase : Singleton<ComponentDatabase>
{
    public List<ComponentSIMATIC> components = new List<ComponentSIMATIC>();

    public List<string> articleNumbersToLoad = new List<string>(new string[]
    {
        "6ES7193-6BP00-0BA0",
        "6ES7193-6BP00-0DA0",
        "6ES7193-6BP20-0BA0",
        "6ES7512-1DM03-0AB0",
        "6ES7134-6GB00-0BA1",
        "6ES7134-6HB00-0CA1",
        "6ES7134-6GD01-0BA1",
        "6ES7193-6AR00-0AA0",
        "6EP7133-6AE00-0BN0",
        "6ES7511-1UL03-0AB0",
        "6ES7954-8LC02-0AA0",
        "6ES7590-1AF30-0AA0",
        "6ES7193-6MR00-0BA0"
    });

    private void Start()
    {
        foreach (string articleNumber in articleNumbersToLoad)
        {
            AddNewComponent(articleNumber);
        }
    }

    private void AddNewComponent(string articleNumber)
    {
        ComponentSIMATIC newComponent = new ComponentSIMATIC(articleNumber);
        newComponent.OnComponentInitialized.AddListener(OnComponentInitialized);
    }

    private void OnComponentInitialized(ComponentSIMATIC initializedComponent)
    {
        components.Add(initializedComponent);
        initializedComponent.PrintDataDictionary();
    }
}
