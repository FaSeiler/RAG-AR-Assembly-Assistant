using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadInstructionButton : MonoBehaviour
{
    public GameObject reloadPopupWindowPrefab;

    private ComponentTypes.ComponentType currentComponentType;

    private void Start()
    {
        // If the current instruction step is a scan step, hide the reload button
        if (InstructionStepManager.instance.currentInstructionStep is InstructionStepScan)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                // Get the current component type
                currentComponentType = InstructionStepManager.instance.currentInstructionStep.component.componentType;

                // Find the ConfirmationDialogUI
                ConfirmationDialogUI confirmationDialogUI = FindFirstObjectByType<ConfirmationDialogUI>();

                string warningMessage = "Are you sure you want to regenerate the instructions for component type: \r\n\r\n<align=\"center\"><b>" +
                    currentComponentType.ToString() + "</b>";

                confirmationDialogUI.ShowConfirmationDialog(warningMessage,
                    // Pass the function to remove the instruction as the confirmation action
                    () => ReloadCurrentInstruction()
                );
            });
        }
    }

    public void ReloadCurrentInstruction()
    {
        InstructionSerializer.instance.RemoveInstruction(currentComponentType);
        int activeSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(activeSceneIndex); // Reload the scene
    }
}