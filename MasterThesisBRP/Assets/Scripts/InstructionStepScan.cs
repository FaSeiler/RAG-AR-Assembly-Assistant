using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstructionStepScan : InstructionStep
{
    public GameObject scanModelTargetPreviewGO; // The instance of the model target preview GameObject

    private RotatingPreviewComponent previewComponent;

    public override void Init(ComponentSIMATIC componentSIMATIC)
    {
        SetPreviewComponent();
        SetPreviewModel(componentSIMATIC);
        SetScanInstruction(componentSIMATIC); // Find the scan instruction in the list of instructions of the componentSIMATIC and set it to the instruction variable

        base.Init(componentSIMATIC);
    }

    private void SetPreviewModel(ComponentSIMATIC componentSIMATIC)
    {
        scanModelTargetPreviewGO = componentSIMATIC.modelTargetPreviewGO;
        scanModelTargetPreviewGO.transform.SetParent(transform);
    }

    /// <summary>
    /// Sets the scan instruction of the componentSIMATIC to the instruction variable
    /// </summary>
    private void SetScanInstruction(ComponentSIMATIC componentSIMATIC)
    {
        instruction = componentSIMATIC.scanInstruction;
    }

    public override void Start()
    {
        base.Start();

        SetPreviewComponent();
    }

    /// <summary>
    /// Finds the preview component in the scene with tag "PreviewComponentScan" and sets it to the previewComponent variable
    /// </summary>
    private void SetPreviewComponent()
    {
        previewComponent = GameObject.FindGameObjectWithTag("PreviewComponentScan").GetComponent<RotatingPreviewComponent>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(ShowScanPreview());
    }

    public override void OnDisable()
    {
        base.OnDisable();

        RemoveScanPreview();
    }

    public IEnumerator ShowScanPreview()
    {
        yield return new WaitUntil(() => initialized);

        previewComponent.SetActivePreview(scanModelTargetPreviewGO);
    }

    public void RemoveScanPreview()
    {
        previewComponent.RemoveActivePreview();
    }
}
