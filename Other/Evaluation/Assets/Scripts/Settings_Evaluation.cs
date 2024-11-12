using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings_Evaluation : Singleton<Settings_Evaluation>
{
    public string subjectID; // Subject ID

    public TMP_InputField inputField_ID;
    public ToggleButton firstToggle; // Which scenario first

    public Szenario_Evaluation szenario1;
    public Szenario_Evaluation szenario2;

    public Button finishButton;


    public void Start()
    {
        subjectID = "-1";

        inputField_ID.onValueChanged.AddListener(delegate { subjectID = inputField_ID.text; CheckIfAllFilledOut(); });
        firstToggle.StartEvent.AddListener(delegate { SetScenario(firstToggle.isToggled); });
        firstToggle.ToggledEvent.AddListener(delegate { SetScenario(firstToggle.isToggled); });

        SetScenario(true);

        CheckIfAllFilledOut();
    }

    private void CheckIfAllFilledOut()
    {
        if (subjectID != "-1")
        {
            finishButton.interactable = true;
        }
        else
        {
            finishButton.interactable = false;
        }
    }

    public void SetScenario(bool val)
    {
        if (val)
        {
            szenario1.SetScenario("PDF Szenario");
            szenario2.SetScenario("Assistant Szenario");
        }
        else
        {
            szenario1.SetScenario("Assistant Szenario");
            szenario2.SetScenario("PDF Szenario");
        }

        
    }
}
