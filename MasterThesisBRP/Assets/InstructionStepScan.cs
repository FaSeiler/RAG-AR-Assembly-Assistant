using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStepScan : InstructionStep
{
    [Header("Scanning")]
    public GameObject scanPreviewModelPrefab;

    public override void OnEnable()
    {
        base.OnEnable();

        ShowScanPreview();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        HideScanPreview();
    }

    public void ShowScanPreview()
    {
        RotatingPreviewComponent.instance.SetActivePreview(scanPreviewModelPrefab);
    }

    public void HideScanPreview()
    {
        RotatingPreviewComponent.instance.RemoveActivePreview();

    }
}
