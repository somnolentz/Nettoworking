using UnityEngine;
using TMPro;
using NetworkingLibrary; 
using System.Text;

public class ChatManager : MonoBehaviour
{
    public TMP_InputField messageInput;
    public TextMeshProUGUI chatText;

    private void Start()
    {
        NetworkManager.instance.RecievedMessageEvent += OnDataRecieved;
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
       chatText.text +="\n" + data;
    }

    private void SendMessageToServer(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        chatText.text += "\nYou: " + message;
        message = NetworkManager.instance.username + ": " + message;
        MessagePacket messagePacket = new MessagePacket(message);
        NetworkManager.instance.SendData(messagePacket.Serialize());

        messageInput.text = "";
        Canvas.ForceUpdateCanvases();
        chatText.rectTransform.sizeDelta = new Vector2(chatText.rectTransform.sizeDelta.x, chatText.preferredHeight);
    }
}
