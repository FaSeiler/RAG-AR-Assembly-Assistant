using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Szenario_Evaluation : MonoBehaviour
{
    public string scenario;
    public TMP_Text szenarioText;

    public void SetScenario(string szenario)
    {
        this.scenario = szenario;
        szenarioText.text = szenario;
    }
}
