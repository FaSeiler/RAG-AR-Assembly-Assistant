using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ClientRAG : MonoBehaviour
{
    private string url = "http://127.0.0.1:5000/query";

    private void Start()
    {
        SendQuery("What is the birthday of Fabian Seiler?");
    }

    public void SendQuery(string queryText)
    {
        StartCoroutine(PostRequest(queryText));
    }

    private IEnumerator PostRequest(string queryText)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes("{\"query_text\": \"" + queryText + "\"}");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Received: " + request.downloadHandler.text);
        }
    }
}
