using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace Evaluation
{
    public class Data_Questions : MonoBehaviour
    {
        [Header("Scenario")]
        public Szenario_Evaluation scenario;

        [Header("Subject")]
        public string subjectID;

        [Header("Answers")]
        public string answer_1;
        public string answer_2;

        public TMP_InputField inputField_A1;
        public TMP_InputField inputField_A2;

        public void Start()
        {
            // Subject
            subjectID = Settings_Evaluation.instance.subjectID;

            inputField_A1.text = answer_1;
            inputField_A2.text = answer_2;

            inputField_A1.onValueChanged.AddListener(delegate { answer_1 = inputField_A1.text; });
            inputField_A2.onValueChanged.AddListener(delegate { answer_2 = inputField_A2.text; });
        }

        public void ExportData()
        {
            Exporter_Questions exportData = new Exporter_Questions
            {
                subjectID = subjectID,
                szenario = scenario.szenario,

                answer_1 = answer_1,
                answer_2 = answer_2
            };

            // Serialize the data to JSON
            string jsonData = JsonUtility.ToJson(exportData, true);
            string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

            string directoryPath_Results = Path.Combine(Application.persistentDataPath, "Results");
            string directoryPath_SubjectID = Path.Combine(directoryPath_Results, subjectID.ToString());
            string directoryPath_Scenario = Path.Combine(directoryPath_SubjectID, scenario.szenario);

            // Check if the directory exists
            if (!Directory.Exists(directoryPath_Results))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath_Results);
            }
            if (!Directory.Exists(directoryPath_SubjectID))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath_SubjectID);
            }
            if (!Directory.Exists(directoryPath_Scenario))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath_Scenario);
            }

            string fileName = subjectID + "_" + scenario.szenario + "_Answers_" + timeStamp;

            string path = Path.Combine(directoryPath_Scenario, fileName + ".json");

            File.WriteAllText(path, jsonData);
            Debug.Log("Data exported to: " + path);
        }
    }
}
