using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Demographic_Evaluation : Singleton<Demographic_Evaluation>
{
    public string subjectID;
    public int age;
    public string gender;
    public string education;
    public int experienceAR;
    public int experienceAI;
    public int experienceSIMATIC;

    public TMP_InputField inputField_age;
    public TMP_Dropdown dropdown_gender;
    public TMP_Dropdown dropdown_education;
    public Slider slider_experienceAR;
    public Slider slider_experienceAI;
    public Slider slider_experienceSIMATIC;

    public Button finishButton;

    public void Start()
    {
        subjectID = Settings_Evaluation.instance.subjectID;
        age = -1;
        experienceAR = -1;
        experienceAI = -1;
        experienceSIMATIC = -1;
        gender = dropdown_gender.options[dropdown_gender.value].text;
        education = dropdown_education.options[dropdown_education.value].text;

        inputField_age.onValueChanged.AddListener(delegate { try { age = int.Parse(inputField_age.text); CheckIfAllFilledOut(); } catch (System.Exception) { } });
        dropdown_education.onValueChanged.AddListener(delegate { education = dropdown_education.options[dropdown_education.value].text; CheckIfAllFilledOut(); });
        slider_experienceAR.onValueChanged.AddListener(delegate { experienceAR = (int)slider_experienceAR.value; CheckIfAllFilledOut(); });
        slider_experienceAI.onValueChanged.AddListener(delegate { experienceAI = (int)slider_experienceAI.value; CheckIfAllFilledOut(); });
        slider_experienceSIMATIC.onValueChanged.AddListener(delegate { experienceSIMATIC = (int)slider_experienceSIMATIC.value; CheckIfAllFilledOut(); });

        try
        {

        }
        catch (System.Exception)
        {

            throw;
        }

        CheckIfAllFilledOut();
    }

    public void ExportDataCSV()
    {
        // Create a string for the CSV header
        string header = "SubjectID,Age,Gender,Education,Experience_AR,Experience_AI,Experience_SIMATIC";

        // Create a string for the CSV data
        string csvData = $"{subjectID},{age},{gender},{education},{experienceAR},{experienceAI},{experienceSIMATIC}";

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

        string fileName = subjectID + "_Demographic_" + timeStamp;

        string path = Path.Combine(directoryPath_SubjectID, fileName + ".csv");

        // Write the header and data to the CSV file
        File.WriteAllText(path, header + "\n" + csvData);
        Debug.Log("Demographic exported to: " + path);
    }


    public void CheckIfAllFilledOut()
    {
        if (subjectID != "-1" && 
            age != -1 && 
            gender != "-1" &&
            education != "-1" &&
            experienceAR != -1 &&
            experienceAI != -1 &&
            experienceSIMATIC != -1)
        {
            finishButton.interactable = true;
        }
        else
        {
            finishButton.interactable = false;
        }
    }

}
