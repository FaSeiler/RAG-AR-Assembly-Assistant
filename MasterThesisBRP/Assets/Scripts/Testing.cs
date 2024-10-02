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
    private void Start()
    {
        int val1 = 1;
        int val2 = 21;
        int val3 = 11;
        int val4 = 2;



        Debug.Log("Before: " + val1 + " / After: " + TransformFromUnityToNASAScala(val1).ToString());
        Debug.Log("Before: " + val2 + " / After: " + TransformFromUnityToNASAScala(val2).ToString());
        Debug.Log("Before: " + val3 + " / After: " + TransformFromUnityToNASAScala(val3).ToString());
        Debug.Log("Before: " + val4 + " / After: " + TransformFromUnityToNASAScala(val4).ToString());
    }

    private int TransformFromUnityToNASAScala(int valueBefore)
    {
        int valueAfter = ((valueBefore - 1) * 100) / 20;
        return valueAfter;
    }
}
