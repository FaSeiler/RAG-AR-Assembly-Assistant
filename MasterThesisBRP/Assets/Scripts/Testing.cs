using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;

public class Testing : MonoBehaviour
{
    public Transform imageContainer; // Reference to the container for the images

    private void Start()
    {
        string query = "What are the steps for installing/mounting a BaseUnit? Include the page_numbers but no introductory sentences.";
        string pdfFileName = "et200sp_system_manual_en-US_en-US_stripped.pdf";
        StartCoroutine(SendQuery(query, pdfFileName));
    }

    private IEnumerator SendQuery(string query, string pdfFileName)
    {
        string url = "http://localhost:5000/query";
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
                Debug.Log("Response from server:");
                string responseText = request.downloadHandler.text;
                Debug.Log(responseText);
                ProcessResponse(responseText);
            }
            else
            {
                Debug.LogError($"Failed to get a response. Status code: {request.responseCode}");
                Debug.LogError(request.error);
            }
        }
    }

    private void ProcessResponse(string jsonResponse)
    {
        var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
        var encodedImages = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseData["encoded_images"].ToString());

        foreach (var imageEntry in encodedImages)
        {
            string base64Image = imageEntry.Value;
            Texture2D texture = Base64ToTexture2D(base64Image);

            if (texture != null)
            {
                GameObject rawImageGO = Instantiate(rawImagePrefab, imageContainer);

                RawImage rawImage = rawImageGO.GetComponent<RawImage>();
                rawImage.texture = texture;
                rawImage.SetNativeSize();
            }
        }
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
