using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;

public class NetworkManager : MonoBehaviour
{
    public Socket socket;
    bool connected = false;

    public delegate void ConnectedToServer();
    public ConnectedToServer ConnectedToServerEvent;

    delegate void NetworkTick();
    NetworkTick NetworkTickEvent;

    public delegate void DataRecieved(string data);
    public DataRecieved DataRecievedEvent;

    float timer;
    const float totalTicksPerSecond = 24.0f;
    float ms = 1.0f / totalTicksPerSecond;

    public static NetworkManager instance;

    private void OnEnable()
    {
        NetworkTickEvent += OnNetworkTick;
    }

    private void OnDestroy()
    {
        NetworkTickEvent -= OnNetworkTick;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
       
    }

    void Update()
    {
        if(!connected)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= ms)
        {
            if (NetworkTickEvent != null)
                NetworkTickEvent();

            timer = 0;
        }
    }

    void OnNetworkTick()
    {
        ReceiveData();
    }

    public void Connect(string ipadress)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            socket.Connect(new IPEndPoint(IPAddress.Parse(ipadress), 3000));
            socket.Blocking = false;
            connected = true;

            if (ConnectedToServerEvent != null)
                ConnectedToServerEvent();

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
        catch (SocketException e)
        {
            Debug.LogError("Error sending message: " + e.Message);
        }
    }

    public string ReceiveData()
    {
        try
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[socket.Available];
                int bytesRead = socket.Receive(buffer);
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (DataRecievedEvent != null)
                {
                    DataRecievedEvent(data);
                    Debug.LogError("Received data: " + data);
                }

                return data;
            }
        }
        catch (SocketException e)
        {
            Debug.LogError("Error receiving data: " + e.Message);
            return null;
        }

        return "";
    }
}
