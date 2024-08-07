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
}
