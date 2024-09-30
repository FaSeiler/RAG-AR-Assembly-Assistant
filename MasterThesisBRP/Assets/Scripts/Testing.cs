using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;

public class Testing : MonoBehaviour
{
    public GameObject parent;
    public float alpha;

    private void Start()
    {
        SetMaterialsAlpha(parent, alpha);
    }

    public void SetMaterials(GameObject parent, Material newMaterial)
    {
        // Get all MeshRenderer components in the parent object and its children
        MeshRenderer[] meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();

        // Loop through each MeshRenderer and set its materials to the new material
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // Create a new array of materials with the same length as the original
            Material[] newMaterials = new Material[meshRenderer.materials.Length];

            // Assign the new material to each slot in the new array
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = newMaterial;
            }

            // Set the MeshRenderer's materials to the new array
            meshRenderer.materials = newMaterials;
        }
    }

    // NOTE: This method does not work properly if on Android.
    public void SetMaterialsAlpha(GameObject parent, float v)
    {
        // Get all MeshRenderer components in the parent object and its children
        MeshRenderer[] meshRenderers = parent.GetComponentsInChildren<MeshRenderer>();

        // Loop through each MeshRenderer and update its materials
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // Get the materials array
            Material[] materials = meshRenderer.materials;

            // Loop through each material and modify it
            for (int i = 0; i < materials.Length; i++)
            {
                // Copy the original material
                Material copiedMaterial = new Material(materials[i]);

                // Set the rendering mode to transparent
                copiedMaterial.SetFloat("_Mode", 3); // 3 corresponds to Transparent in Unity's Standard Shader
                copiedMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                copiedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                copiedMaterial.SetInt("_ZWrite", 0);
                copiedMaterial.DisableKeyword("_ALPHATEST_ON");
                copiedMaterial.EnableKeyword("_ALPHABLEND_ON");
                copiedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                copiedMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                // Modify the alpha value
                Color color = copiedMaterial.color;
                color.a = v;
                copiedMaterial.color = color;

                // Assign the modified material back to the array
                materials[i] = copiedMaterial;
            }

            // Reassign the modified materials array back to the MeshRenderer
            meshRenderer.materials = materials;
        }
    }
}
