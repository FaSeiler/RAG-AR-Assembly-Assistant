using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;

public class Testing : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            InstructionSerializer.instance.RemoveInstruction(ComponentTypes.ComponentType.ServerModule);
            Debug.Log("REMOVED INSTRUCTION FOR SERVER MODULE");

        }
    }
}
