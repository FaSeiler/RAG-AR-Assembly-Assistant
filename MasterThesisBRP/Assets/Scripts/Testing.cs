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

public class Testing : MonoBehaviour
{


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Animator anim = GetComponent<Animator>();

            anim.SetBool("0", false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Animator anim = GetComponent<Animator>();

            anim.SetBool("0", true);
        }
    }

}
