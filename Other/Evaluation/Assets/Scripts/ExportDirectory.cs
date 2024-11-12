using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ExportDirectory : MonoBehaviour
{
    private TMPro.TMP_InputField inputField;

    private void Start()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();
        inputField = GetComponent<TMPro.TMP_InputField>();
        inputField.text = Path.Combine(Application.persistentDataPath, "Results");
    }
}
