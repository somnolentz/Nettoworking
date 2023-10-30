using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public TMP_InputField messageInput;
    public TextMeshProUGUI chatText;

    private void Start()
    {
        NetworkManager.instance.DataRecievedEvent += OnDataRecieved;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(messageInput.text))
        {
            SendMessageToServer(messageInput.text);
        }
    }

    private void OnDataRecieved(string data)
    {
        chatText.text += "\n" + data;
    }

    private void SendMessageToServer(string message)
    {
      
        NetworkManager.instance.SendData(message);

        chatText.text += "\nYou: " + message;

        messageInput.text = ""; 
    }

}
