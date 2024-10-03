using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewModelTextureSwitch : Singleton<PreviewModelTextureSwitch>
{
    public bool useTexturedPreview = false;

    public void SwitchPreviewModelTexture()
    {
        if (InstructionStepManager.instance.currentInstructionStep is InstructionStepScan)
        {
            useTexturedPreview = !useTexturedPreview;

            InstructionStepScan currentInstructionStepScan = (InstructionStepScan)InstructionStepManager.instance.currentInstructionStep;
            currentInstructionStepScan.UpdateScanPreview();
        }
    }
}
