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
    public Transform cpuTransform;
    public Transform otherTransform;

    public Vector3 offset;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculatePosition();
        }
    }

    private void CalculatePosition()
    {
        offset = otherTransform.position - cpuTransform.position;
        Debug.Log(offset);
    }
}
