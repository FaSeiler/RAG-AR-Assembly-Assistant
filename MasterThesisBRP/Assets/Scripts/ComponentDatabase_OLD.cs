using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old_Implementation
{
    public class ComponentDatabase_OLD : Singleton<ComponentDatabase>
    {
        //public List<string> articleNumbersToLoad = new List<string>(new string[]
        //{
        //"6ES7193-6BP00-0BA0",
        //"6ES7193-6BP00-0DA0",
        //"6ES7193-6BP20-0BA0",
        //"6ES7512-1DM03-0AB0",
        //"6ES7134-6GB00-0BA1",
        //"6ES7134-6FB00-0BA1",
        //"6ES7134-6GD01-0BA1",
        //"6ES7193-6AR00-0AA0",
        //"6EP7133-6AE00-0BN0",
        //"6ES7511-1UL03-0AB0",
        //"6ES7954-8LC02-0AA0",
        //"6ES7193-6MR00-0BA0",
        //"6ES7590-1AF30-0AA0"
        //});

        //public Dictionary<string, ComponentSIMATIC> components = new Dictionary<string, ComponentSIMATIC>();

        //public bool allComponentsInititalized = false;

        //protected override void Awake()
        //{
        //    base.Awake();

        //    foreach (string articleNumber in articleNumbersToLoad)
        //    {
        //        AddNewComponent(articleNumber);
        //    }
        //    allComponentsInititalized = true;
        //}

        //private void AddNewComponent(string articleNumber)
        //{
        //    ComponentSIMATIC newComponent = new ComponentSIMATIC(articleNumber);
        //    components.Add(articleNumber, newComponent);
        //}

        //public ComponentSIMATIC GetComponentSIMATIC(string articleNumber)
        //{
        //    return components[articleNumber];
        //}
    }
}
