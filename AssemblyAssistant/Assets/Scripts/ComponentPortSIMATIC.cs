using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentPortSIMATIC : MonoBehaviour
{
    public string componentName;
    public string articleNumber;

    public GameObject portReferencePoints_Inner;
    public GameObject portReferencePoints_Outer;
    public List<Port> ports = new List<Port>();

    private void Start()
    {
        InitPortReferencePoints();
    }

    private void InitPortReferencePoints()
    {
        // init the list of ports. Every port has an inner and an outer reference point with the same name under portReferencePoint_Inner and portReferencePoint_Outer
        foreach (Transform innerTransform in portReferencePoints_Inner.transform)
        {
            Transform outerTransform = portReferencePoints_Outer.transform.Find(innerTransform.name);
            if (outerTransform != null)
            {
                Port port = new Port();
                port.portName = innerTransform.name;
                port.innerTransform = innerTransform;
                port.outerTransform = outerTransform;
                ports.Add(port);
            }
        }
    }

    public Port GetPortByName(string portName)
    {
        foreach (Port port in ports)
        {
            if (port.portName == portName)
            {
                return port;
            }
        }

        return null;
    }
}
