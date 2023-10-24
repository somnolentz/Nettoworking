using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public Socket socket;

    private void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
            Debug.Log("welcome!");
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    public void SendData(string message)
    {
        try
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            socket.Send(messageBytes);
            Debug.Log("Sent message: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending message: " + e.Message);
        }
    }

    public string ReceiveData()
    {
        try
        {
            byte[] buffer = new byte[1024];
            int bytesRead = socket.Receive(buffer);
            string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("Received data: " + data);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving data: " + e.Message);
            return null;
        }
    }
}
