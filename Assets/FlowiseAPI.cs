//author : Dhruv Adhia

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FlowiseAPI : MonoBehaviour
{
    private readonly string url = "https://flowise.thecela.com/api/v1/prediction/587e29be-52c5-471d-985c-42838b115776";
    private readonly string token = "nviwvK7rhEeXPY05uGH8XRq2pgvSgC801joAZ92P4ng="; 
    public delegate void ResponseReceived(string response);
    public event ResponseReceived OnResponseReceived;

    [System.Serializable]
    public class ApiResponse
    {
        public string text;
        // Add other fields if necessary
    }

    // Public method to send a question to the API
    public void SendQuestion(string question)
    {
        string jsonPayload = "{\"question\": \"" + question + "\"}";
        StartCoroutine(QueryFlowise(jsonPayload));
    }

    IEnumerator QueryFlowise(string json)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + webRequest.error);
                OnResponseReceived?.Invoke("Error: " + webRequest.error);
            }
            else
            {
                var responseText = webRequest.downloadHandler.text;
                ApiResponse response = JsonUtility.FromJson<ApiResponse>(responseText);
                Debug.Log("Response: " + response.text);
                OnResponseReceived?.Invoke(response.text);
            }
        }
    }
}
