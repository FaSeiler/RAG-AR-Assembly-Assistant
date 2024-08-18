using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ClientRAG : Singleton<ClientRAG>
{
    public string BASE_URL = "http://127.0.0.1:5000";
    public bool isBusy = false;
    //string query = "What are the steps for installing/mounting a BaseUnit? Include the page_numbers but no introductory sentences.";
    string pdfFileName = "et200sp_system_manual_en-US_en-US_stripped.pdf";


    public Transform imageContainer; // Reference to the container for the images

    public void SendRequest(string query, Action<ResponseData> callback)
    {
        StartCoroutine(SendQuery(query, pdfFileName, callback));
    }

    IEnumerator SendQuery(string query, string pdfFileName, Action<ResponseData> callback)
    {
        isBusy = true;
        string url = BASE_URL + "/query";

        Dictionary<string, string> payload = new Dictionary<string, string>
        {
            { "query", query },
            { "pdf_file_name", pdfFileName }
        };
        string jsonData = JsonConvert.SerializeObject(payload);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                //Debug.Log(jsonResponse);

                ResponseData responseData = ProcessResponse(jsonResponse);
                callback?.Invoke(responseData);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                callback?.Invoke(null);
            }
        }

        isBusy = false;
    }

    [Serializable]
    public class ResponseData
    {
        public string text;
        public List<int> page_numbers;
        public Dictionary<string, string> encoded_images;
        public List<Texture2D> decoded_images = null;
    }

    private ResponseData ProcessResponse(string jsonResponse) 
    {

        ResponseData responseData = JsonUtility.FromJson<ResponseData>(jsonResponse);
        Dictionary<string, object> tmp = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
        responseData.encoded_images = JsonConvert.DeserializeObject<Dictionary<string, string>>(tmp["encoded_images"].ToString());

        // Printing the response data
        //Debug.Log(responseData.text);
        //Debug.Log(responseData.page_numbers[0]);
        //foreach (var image in responseData.encoded_images)
        //{
        //    Debug.Log($"Key: {image.Key}, Value: {image.Value}");
        //}


        foreach (var imageEntry in responseData.encoded_images)
        {
            string base64Image = imageEntry.Value;
            Texture2D texture = Base64ToTexture2D(base64Image);

            if (texture != null)
            {
                responseData.decoded_images.Add(texture);
            }
        }

       //Remove all '"' characters from the response text
       responseData.text = responseData.text.Replace("\"", "");
       return responseData;
    }

    public GameObject rawImagePrefab;

    private Texture2D Base64ToTexture2D(string base64)
    {
        byte[] imageBytes = System.Convert.FromBase64String(base64);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageBytes))
        {
            return texture;
        }
        else
        {
            Debug.LogError("Failed to load texture from base64 string");
            return null;
        }
    }
}