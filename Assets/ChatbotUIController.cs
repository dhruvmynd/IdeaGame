using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ChatbotUIController : MonoBehaviour
{
    public InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI chatText;
    public ScrollRect scrollRect; // Reference to the ScrollRect

    private FlowiseAPI flowiseApi;

    void Start()
    {
        flowiseApi = FindObjectOfType<FlowiseAPI>();
        sendButton.onClick.AddListener(SendMessage);

        // Subscribe to the response event
        if (flowiseApi != null)
        {
            flowiseApi.OnResponseReceived += HandleResponse;
        }

        // Optional: Initialize chatText with an empty string
        chatText.text = "";
    }

    private void Speak(string text)
    {
        Application.ExternalEval($"speak('{text.Replace("'", "\\'")}');");
    }


    void SendMessage()
    {
        string userMessage = inputField.text;
        if (!string.IsNullOrWhiteSpace(userMessage))
        {
            flowiseApi.SendQuestion(userMessage);
            AppendToChat($"You: {userMessage}");
            inputField.text = "";
        }
    }

    // This method will be called when the FlowiseAPI receives a response
    private void HandleResponse(string response)
    {
        AppendToChat($"Bot: {response}");
        Speak(response);
    }

    public void AppendToChat(string message)
    {
        chatText.text += $"{message}\n";
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        // Wait for end of frame so that the UI elements can update their positions
        yield return new WaitForEndOfFrame();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    void OnDestroy()
    {
        if (flowiseApi != null)
        {
            flowiseApi.OnResponseReceived -= HandleResponse;
        }
    }
}
