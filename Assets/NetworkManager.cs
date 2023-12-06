using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;
using NetworkingLibrary;

public class NetworkManager : MonoBehaviour
{
    public Socket socket;
    bool connected = false;

    public delegate void ConnectedToServer();
    public ConnectedToServer ConnectedToServerEvent;

    delegate void NetworkTick();
    NetworkTick NetworkTickEvent;

    public delegate void RecievedMessage(string message);
    public RecievedMessage RecievedMessageEvent;

    float timer;
    const float totalTicksPerSecond = 24.0f;
    float ms = 1.0f / totalTicksPerSecond;

    public string username;

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

    public void Connect(string ipadress, string username)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            socket.Connect(new IPEndPoint(IPAddress.Parse(ipadress), 3000));
            this.username = username;
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

    public void SendData(byte[] data)
    {
        try
        {
            socket.Send(data);
        }
        catch (SocketException e)
        {
            Debug.LogError("Error sending message: " + e.Message);
        }
    }

    public void ReceiveData()
    {
        try
        {
            if(socket.Available > 0)
            {
                byte[] buffer = new byte[socket.Available];
                socket.Receive(buffer);
                int index = 0;
                while( index < buffer.Length)
                {
                    BasePacket basepacket = new BasePacket().Deserialize(buffer, index);
                    switch (basepacket.packetType)
                    {
                        case BasePacket.PacketType.Instantiation:
                            InstantiationPacket instantiationPacket = new InstantiationPacket().Deserialize(buffer, index);

                            GameObject hamper = Resources.Load(instantiationPacket.prefabName) as GameObject;
                            Instantiate(hamper, instantiationPacket.position, Quaternion.identity);
                            break;

                        case BasePacket.PacketType.Message:
                            MessagePacket msgPacket = new MessagePacket().Deserialize(buffer, index);
                            RecievedMessageEvent.Invoke(msgPacket.Message);
                            break;
                    }
                    index += basepacket.packetSize;
                }
            }
        }
        catch (SocketException ex)
        {
            Debug.LogException(ex);
        }
    }
}
