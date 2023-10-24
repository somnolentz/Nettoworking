using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public TMP_InputField messageInput;
    public TextMeshProUGUI chatText;
    public NetworkManager networkManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(messageInput.text))
        {
            SendMessageToServer(messageInput.text);
        }
    }

    private void SendMessageToServer(string message)
    {
        if (networkManager == null)
        {
            Debug.LogError("network manager null");
            return;
        }

        networkManager.SendData(message);

        chatText.text += "\nYou: " + message;

        messageInput.text = ""; 
    }
}
