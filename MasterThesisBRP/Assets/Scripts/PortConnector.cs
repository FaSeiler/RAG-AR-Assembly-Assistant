using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortConnector : MonoBehaviour
{
    public ComponentSIMATIC componentSIMATIC_A;
    public ComponentSIMATIC componentSIMATIC_B;
    public GameObject cablePrefab;

    public string portNameA = "1";
    public string portNameB = "L+";

    public GameObject cableInstance;

    public Port connectedPortA;
    public Port connectedPortB;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConnectPorts(portNameA, portNameB);
        }
    }

    private void ConnectPorts(string portNameA, string portNameB)
    {
        connectedPortA = componentSIMATIC_A.GetPortByName(portNameA);
        connectedPortB = componentSIMATIC_B.GetPortByName(portNameB);

        if (connectedPortA == null || connectedPortB == null)
        {
            Debug.Log("Error connecting ports. Port doesn't exist!");
            return;
        }

        if (cableInstance != null)
        {
            Destroy(cableInstance);
        }

        // Parent of the new cable
        cableInstance = new GameObject($"Cable - {portNameA} - {portNameB}");

        // Port-To-Port
        GameObject cablePortToPortGameObject = Instantiate(cablePrefab, cableInstance.transform);
        cablePortToPortGameObject.name = $"Port-To-Port";
        ProceduralCable cablePortToPort = cablePortToPortGameObject.GetComponent<ProceduralCable>();
        cablePortToPort.Connect(connectedPortA.outerTransform, connectedPortB.outerTransform);

        // Port-Inner-To-Outer-A
        GameObject cableInnerToOuterGameObjectA = Instantiate(cablePrefab, cableInstance.transform);
        cableInnerToOuterGameObjectA.name = $"Inner-To-Outer-A";
        ProceduralCable cableInnerToOuterA = cableInnerToOuterGameObjectA.GetComponent<ProceduralCable>();
        cableInnerToOuterA.Connect(connectedPortA.innerTransform, connectedPortA.outerTransform);

        // Port-Inner-To-Outer-B
        GameObject cableInnerToOuterGameObjectB = Instantiate(cablePrefab, cableInstance.transform);
        cableInnerToOuterGameObjectB.name = $"Inner-To-Outer-B";
        ProceduralCable cableInnerToOuterB = cableInnerToOuterGameObjectB.GetComponent<ProceduralCable>();
        cableInnerToOuterB.Connect(connectedPortB.innerTransform, connectedPortB.outerTransform);
    }
}
