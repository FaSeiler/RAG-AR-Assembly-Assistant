using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SUS
{
    public class Data_SUS : MonoBehaviour
    {
        [Header("Scenario")]
        public Szenario_Evaluation scenario;

        [Header("Demographic")]
        public Demographic_Evaluation demographic;

        [Header("Subject")]
        public string subjectID;

        [Header("SUS Score")]
        public float sus_score;

        [Header("Values")]
        public int value_1;
        public int value_2;
        public int value_3;
        public int value_4;
        public int value_5;
        public int value_6;
        public int value_7;
        public int value_8;
        public int value_9;
        public int value_10;

        [Header("Value Sliders")]
        public Slider slider_1;
        public Slider slider_2;
        public Slider slider_3;
        public Slider slider_4;
        public Slider slider_5;
        public Slider slider_6;
        public Slider slider_7;
        public Slider slider_8;
        public Slider slider_9;
        public Slider slider_10;

        [Header("Finish Button")]
        public Button finishButton;

        public void Start()
        {
            // Subject
            subjectID = Settings_Evaluation.instance.subjectID;

            // Values
            value_1 = -1;
            value_2 = -1;
            value_3 = -1;
            value_4 = -1;
            value_5 = -1;
            value_6 = -1;  
            value_7 = -1;
            value_8 = -1;
            value_9 = -1;
            value_10 = -1;

            slider_1.onValueChanged.AddListener(delegate { value_1 = (int)slider_1.value; CheckIfAllFilledOut(); });
            slider_2.onValueChanged.AddListener(delegate { value_2 = (int)slider_2.value; CheckIfAllFilledOut(); });
            slider_3.onValueChanged.AddListener(delegate { value_3 = (int)slider_3.value; CheckIfAllFilledOut(); });
            slider_4.onValueChanged.AddListener(delegate { value_4 = (int)slider_4.value; CheckIfAllFilledOut(); });
            slider_5.onValueChanged.AddListener(delegate { value_5 = (int)slider_5.value; CheckIfAllFilledOut(); });
            slider_6.onValueChanged.AddListener(delegate { value_6 = (int)slider_6.value; CheckIfAllFilledOut(); });
            slider_7.onValueChanged.AddListener(delegate { value_7 = (int)slider_7.value; CheckIfAllFilledOut(); });
            slider_8.onValueChanged.AddListener(delegate { value_8 = (int)slider_8.value; CheckIfAllFilledOut(); });
            slider_9.onValueChanged.AddListener(delegate { value_9 = (int)slider_9.value; CheckIfAllFilledOut(); });
            slider_10.onValueChanged.AddListener(delegate { value_10 = (int)slider_10.value; CheckIfAllFilledOut(); });
            
            CheckIfAllFilledOut();
        }

        private void CheckIfAllFilledOut()
        {
            if (subjectID != "-1" &&
                value_1 != -1 &&
                value_2 != -1 &&
                value_3 != -1 &&
                value_4 != -1 &&
                value_5 != -1 &&
                value_6 != -1 &&
                value_7 != -1 &&
                value_8 != -1 &&
                value_9 != -1 &&
                value_10 != -1)
            {
                finishButton.interactable = true;

            }
            else
            {
                finishButton.interactable = false;
            }
        }

        public void ExportDataJSON()
        {
            CalculateSUSScore();

            Exporter_SUS exportData = new Exporter_SUS
            {
                subjectID = subjectID,
                scenario = scenario.scenario,

                sus_score = sus_score,

                value_1 = value_1,
                value_2 = value_2,
                value_3 = value_3,
                value_4 = value_4,
                value_5 = value_5,
                value_6 = value_6,
                value_7 = value_7,
                value_8 = value_8,
                value_9 = value_9,
                value_10 = value_10
            };

            // Serialize the data to JSON
            string jsonData = JsonUtility.ToJson(exportData, true);
            string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

            string directoryPath_Results = Path.Combine(Application.persistentDataPath, "Results");
            string directoryPath_SubjectID = Path.Combine(directoryPath_Results, subjectID.ToString());
            string directoryPath_Scenario = Path.Combine(directoryPath_SubjectID, scenario.scenario);

            // Check if the directory exists
            if(!Directory.Exists(directoryPath_Results))
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

            string fileName = subjectID + "_" + scenario.scenario + "_SUS_" + timeStamp;

            string path = Path.Combine(directoryPath_Scenario, fileName + ".json");

            File.WriteAllText(path, jsonData);
            Debug.Log("Data exported to: " + path);
        }

        public void ExportDataCSV()
        {
            CalculateSUSScore();

            // Create a string for the CSV header
            string header = "SubjectID,Scenario,Age,Gender,Education,Experience_AR,Experience_AI,Experience_SIMATIC,SUS_Score,Value_1,Value_2,Value_3,Value_4,Value_5,Value_6,Value_7,Value_8,Value_9,Value_10";

            // Create a string for the CSV data
            string csvData = $"{subjectID},{scenario.scenario},{demographic.age},{demographic.gender},{demographic.education},{demographic.experienceAR},{demographic.experienceAI},{demographic.experienceSIMATIC},{sus_score},{value_1},{value_2},{value_3},{value_4},{value_5},{value_6},{value_7},{value_8},{value_9},{value_10}";

            string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

            string directoryPath_Results = Path.Combine(Application.persistentDataPath, "Results");
            string directoryPath_SubjectID = Path.Combine(directoryPath_Results, subjectID.ToString());
            string directoryPath_Scenario = Path.Combine(directoryPath_SubjectID, scenario.scenario);

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

            string fileName = subjectID + "_" + scenario.scenario + "_SUS_" + timeStamp;

            string path = Path.Combine(directoryPath_Scenario, fileName + ".csv");

            // Write the header and data to the CSV file
            File.WriteAllText(path, header + "\n" + csvData);
            Debug.Log("Data exported to: " + path);
        }


        private void CalculateSUSScore()
        {
            int X = (value_1 + value_3 + value_5 + value_7 + value_9) - 5;
            int Y = 25 - (value_2 + value_4 + value_6 + value_8 + value_10);
            sus_score = 2.5f * (X + Y);
        }
    }
}
