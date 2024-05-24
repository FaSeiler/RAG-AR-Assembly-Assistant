using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ClientRAG : Singleton<ClientRAG>
{
    string BASE_URL = "http://192.168.0.110:5000";
    public bool isBusy;

    public void SendRequest(string request, Action<string> callback)
    {
        StartCoroutine(SendQuery(request, callback));
    }

    IEnumerator SendQuery(string question, Action<string> callback, string database = "et200sp_system_manual_en-US_en-US_stripped", bool useContext = true, int kContext = 5)
    {
        isBusy = true;
        string url = BASE_URL + "/query";
        string jsonData = "{\"question\": \"" + question + "\", \"useContext\": " + useContext.ToString().ToLower() + ", \"database\": \"" + database + "\", \"k_context\": " + kContext + "}";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                // Remove all '"' characters from the response text
                responseText = responseText.Replace("\"", "");
                Debug.Log(responseText);
                callback?.Invoke(responseText);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                callback?.Invoke(request.error);
            }
        }

        isBusy = false;
    }

    IEnumerator SayHello()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(BASE_URL + "/hello"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text); // prints "Hello Fabian"
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}