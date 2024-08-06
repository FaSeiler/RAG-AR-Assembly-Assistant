using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionStepScan : InstructionStep
{
    [Header("Scanning")]
    public GameObject scanPreviewModelPrefab;
    private RotatingPreviewComponent previewComponent;

    public override void Awake()
    {
        base.Awake();

        // Find the preview component in the scene with tag "PreviewComponentScan"
        previewComponent = GameObject.FindGameObjectWithTag("PreviewComponentScan").GetComponent<RotatingPreviewComponent>();

        instruction = new Instruction
        {
            componentTypeEnum = componentType,
            instructionText = "Find the " + ComponentTypes.GetComponentTypeEnumToString(componentType) + " and scan it with the tablet.",
            imageTextures = new List<Texture2D>()
        };
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
