using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDatabase : Singleton<ComponentDatabase>
{
    public List<ComponentSIMATIC> components = new List<ComponentSIMATIC>();

    public List<ComponentSIMATIC> GetAllComponents()
    {
        return components;
    }

    public ComponentSIMATIC GetComponentByType(ComponentTypes.ComponentType type)
    {
        foreach (ComponentSIMATIC component in components)
        {
            if (component.componentType == type)
            {
                return component;
            }
        }

        return null;
    }
}
