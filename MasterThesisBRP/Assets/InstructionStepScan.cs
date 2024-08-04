using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStepScan : InstructionStep
{
    [Header("Scanning")]
    public GameObject scanPreviewModelPrefab;
    public RotatingPreviewComponent previewComponent;

    public override void Awake()
    {
        base.Awake();
        // Find the preview component in the scene with tag "PreviewComponentScan"
        previewComponent = GameObject.FindGameObjectWithTag("PreviewComponentScan").GetComponent<RotatingPreviewComponent>();
    }

    public override void Start()
    {
        base.Start();
    }

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
        previewComponent.SetActivePreview(scanPreviewModelPrefab);
    }

    public void HideScanPreview()
    {
        previewComponent.RemoveActivePreview();

    }
}
