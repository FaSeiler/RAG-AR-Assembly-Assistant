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
    private string jsonFilePath;
    private string imagesFolderPath;
    private string persistentJsonFilePath;
    private string persistentImagesFolderPath;

    void Start()
    {
        DebugUI.AddLog("FILEPATH: " + InstructionSerializer.instance.filePath);
        CopyDataToPersistentPath(); // Copy files from StreamingAssets to persistentDataPath on Awake
    }

    void CopyDataToPersistentPath()
    {
        // Step 1: Copy JSON file from StreamingAssets to persistentDataPath
        jsonFilePath = Path.Combine(Application.streamingAssetsPath, "instructions.json");
        persistentJsonFilePath = InstructionSerializer.instance.filePath;

        if (File.Exists(jsonFilePath))
        {
            DebugUI.AddLog("Streaming assets jsonFilePath exists: \n" + jsonFilePath);
        }
        else
        {
            DebugUI.AddLog("Streaming assets jsonFilePath NOT EXIST: \n" + jsonFilePath);
        }
        if (File.Exists(persistentJsonFilePath))
        {
            DebugUI.AddLog("Persistent json exists: \n" + persistentJsonFilePath);
        }
        else
        {
            DebugUI.AddLog("Persistent json NOT EXIST: \n" + persistentJsonFilePath);
        }

        if (File.Exists(jsonFilePath) && !File.Exists(persistentJsonFilePath))
        {
            File.Copy(jsonFilePath, persistentJsonFilePath, true); // Copy and overwrite if necessary
            //DebugUI.AddLog("Copied JSON to persistent path: " + persistentJsonFilePath);
        }
        else
        {
            //DebugUI.AddLog("JSON file not found in StreamingAssets or already copied!");
        }

        // Step 2: Copy all images from StreamingAssets/Images to persistentDataPath/Images
        imagesFolderPath = Path.Combine(Application.streamingAssetsPath, "Images");
        persistentImagesFolderPath = InstructionSerializer.instance.imageDirectory;

        if (!Directory.Exists(persistentImagesFolderPath) && !Directory.Exists(persistentImagesFolderPath))
        {
            Directory.CreateDirectory(persistentImagesFolderPath); // Create the folder if it doesn't exist
        }

        if (Directory.Exists(imagesFolderPath))
        {
            string[] imageFiles = Directory.GetFiles(imagesFolderPath, "*.png"); // Adjust for your image formats

            foreach (var imagePath in imageFiles)
            {
                string fileName = Path.GetFileName(imagePath);
                string destPath = Path.Combine(persistentImagesFolderPath, fileName);
                File.Copy(imagePath, destPath, true); // Copy and overwrite if necessary
                //DebugUI.AddLog("Copied image to persistent path: " + destPath);
            }
        }
        else
        {
            //DebugUI.AddLog("Images folder not found in StreamingAssets or already copied!");
        }
    }
}
