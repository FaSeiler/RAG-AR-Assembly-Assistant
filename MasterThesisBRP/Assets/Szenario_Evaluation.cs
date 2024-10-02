using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Szenario_Evaluation : MonoBehaviour
{
    public string szenario;
    public TMP_Text szenarioText;

    public void SetScenario(string szenario)
    {
        this.szenario = szenario;
        szenarioText.text = szenario;
    }
}
