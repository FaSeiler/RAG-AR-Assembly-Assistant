using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class Feedback_Evaluation : MonoBehaviour
{
    public string subjectID;
    public string improvements;
    public string overallExperience;
    public string workedWellAndNot;

    public TMP_InputField inputField_improvements;
    public TMP_InputField inputField_overallExperience;
    public TMP_InputField inputField_workedWellAndNot;

    public void Start()
    {
        // Subject
        subjectID = Settings_Evaluation.instance.subjectID;

        improvements = "";
        overallExperience = "";
        workedWellAndNot = "";

        inputField_improvements.onValueChanged.AddListener(delegate { improvements = inputField_improvements.text; });
        inputField_overallExperience.onValueChanged.AddListener(delegate { overallExperience = inputField_overallExperience.text; });
        inputField_workedWellAndNot.onValueChanged.AddListener(delegate { workedWellAndNot = inputField_workedWellAndNot.text; });
    }

    public void ExportDataCSV()
    {
        // Create a string for the CSV header
        string header = "SubjectID,Improvements,Overall_Experience,Worked_Well_And_Not";

        // Create a string for the CSV data
        string csvData = $"{subjectID},{improvements},{overallExperience},{workedWellAndNot}";

        string timeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

        string directoryPath_Results = Path.Combine(Application.persistentDataPath, "Results");
        string directoryPath_SubjectID = Path.Combine(directoryPath_Results, subjectID.ToString());

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

        string fileName = subjectID + "_Feedback_" + timeStamp;

        string path = Path.Combine(directoryPath_SubjectID, fileName + ".csv");

        // Write the header and data to the CSV file
        File.WriteAllText(path, header + "\n" + csvData);
        Debug.Log("Data exported to: " + path);
    }
}
