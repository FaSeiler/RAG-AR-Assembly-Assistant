using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Demographic_Evaluation : MonoBehaviour
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

        inputField_age.onValueChanged.AddListener(delegate { age = int.Parse(inputField_age.text); CheckIfAllFilledOut(); });
        dropdown_education.onValueChanged.AddListener(delegate { education = dropdown_education.options[dropdown_education.value].text; CheckIfAllFilledOut(); });
        slider_experienceAR.onValueChanged.AddListener(delegate { experienceAR = (int)slider_experienceAR.value; CheckIfAllFilledOut(); });
        slider_experienceAI.onValueChanged.AddListener(delegate { experienceAI = (int)slider_experienceAI.value; CheckIfAllFilledOut(); });
        slider_experienceSIMATIC.onValueChanged.AddListener(delegate { experienceSIMATIC = (int)slider_experienceSIMATIC.value; CheckIfAllFilledOut(); });

        CheckIfAllFilledOut();
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
