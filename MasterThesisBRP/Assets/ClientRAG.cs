using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ClientRAG : Singleton<ClientRAG>
{
    public string url = "http://localhost:5000/query"; // Replace with your Flask server URL
    public string request = "Give me sample questions I can ask you.";
    public bool useContext = true;
    public string database = "et200sp_system_manual_en-US_en-US_stripped";
    public int k_context = 5;
    public bool isProcessing = false;
    public string response;

    // A function that takes in a request string and sends it to the server
    public void SendRequest(string request, Action<string> callback, 
        bool useContext = true, string database = "et200sp_system_manual_en-US_en-US_stripped", int k_context = 5)
    {
        this.request = request;
        this.useContext = useContext;
        this.database = database;
        this.k_context = k_context;
        StartCoroutine(SendRAGRequest(callback));
    }

    public IEnumerator SendRAGRequest(Action<string> callback)
    {
        isProcessing = true;
        var request = new UnityWebRequest(url, "POST");
        var jsonBody = new { question = request, useContext = true, database = "et200sp_system_manual_en-US_en-US_stripped", k_context = 5 };
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonBody));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            callback("Error: " + request.error);
        }
        else
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            callback(request.downloadHandler.text);
        }

        isProcessing = false;
    }
}