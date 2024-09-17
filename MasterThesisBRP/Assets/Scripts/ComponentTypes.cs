using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentTypes : MonoBehaviour
{
    [Serializable]
    public enum ComponentType
    {
        None,
        CPU_InterfaceModule,
        BaseUnitForIOModules,
        ServerModule,
        BusAdapterForCPU_InterfaceModule,
        MemoryCardForCPU,
        IOModulesAndBUCovers,
        PowerSupply
    }

    // We need to get the exact string for the component type to increase chances of finding the component with the RAG
    public static Dictionary<ComponentType, string> componentTypeEnumStringDictionary = new Dictionary<ComponentType, string>()
    {
        { ComponentType.None, "None" },
        { ComponentType.CPU_InterfaceModule, "CPU/interface module" },
        { ComponentType.BaseUnitForIOModules, "BaseUnit for I/O modules" },
        { ComponentType.ServerModule, "Server Module" },
        { ComponentType.BusAdapterForCPU_InterfaceModule, "BusAdapter for the CPU/interface module" },
        { ComponentType.MemoryCardForCPU, "Memory Card for the CPU" },
        { ComponentType.IOModulesAndBUCovers, "I/O modules and BU covers" },
        { ComponentType.PowerSupply, "Power Supply" }
    };

    [SerializeField]
    private List<string> componentTypes = new List<string>(); // This is just to show the list in the inspector

    private void Awake()
    {
        componentTypes = GetAllComponentTypeStrings();
    }

    public static List<string> GetAllComponentTypeStrings()
    {
        List<string> componentTypeStrings = new List<string>();

        foreach (ComponentType componentType in System.Enum.GetValues(typeof(ComponentType)))
        {
            componentTypeStrings.Add(GetComponentTypeEnumToString(componentType));
        }

        return componentTypeStrings;
    }

    public static List<ComponentType> GetAllComponentTypeEnums()
    {
        return System.Enum.GetValues(typeof(ComponentType)).Cast<ComponentType>().ToList();
    }

    public static string GetComponentTypeEnumToString(ComponentType componentType)
    {
        return componentTypeEnumStringDictionary[componentType];
    }

    public static ComponentType GetComponentTypeStringToEnum(string componentTypeString)
    {
        // Find the key in the dictionary that has the value of componentTypeString
        return componentTypeEnumStringDictionary.FirstOrDefault(x => x.Value == componentTypeString).Key;
    }
}
