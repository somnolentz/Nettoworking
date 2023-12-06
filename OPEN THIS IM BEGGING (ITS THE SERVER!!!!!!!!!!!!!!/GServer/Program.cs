using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace NetworkExperiment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int hampersSpawned = 0;
            Vector3 offsetpos = Vector3.zero;
            Console.Title = "Game Server";

            Socket queueSocket = new Socket(
               AddressFamily.InterNetwork,
               SocketType.Stream,
               ProtocolType.Tcp);

            queueSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            queueSocket.Listen(10);
            queueSocket.Blocking = false;

            Console.WriteLine("Server is listening for incoming connections...");

            List<Socket> clients = new List<Socket>();

            while (true)
            {
               
                if (clients.Count == 2 && hampersSpawned < 2)
                {
                    Vector3 pos = new Vector3(1, 0, 1);
                    InstantiationPacket packet = new InstantiationPacket("HAMPER", pos + offsetpos, Quaternion.identity);
                    for (int i = 0; i < clients.Count; i++)
                    {
                        hampersSpawned++;
                        byte[] data = packet.Serialize();
                        clients[i].Send(data);
                        Console.WriteLine("HAMPER SUMMONED");  
                    }
                }

                try
                {
                    clients.Add(queueSocket.Accept());
                    Console.WriteLine("Connection established with client: ");

                }
                catch (SocketException clientException)
                {
                    if (clientException.SocketErrorCode != SocketError.WouldBlock)
                    {
                        Console.WriteLine("Error sending data to the client: " + clientException.Message);
                    }
                }

                for (int i = 0; i < clients.Count; i++)
                {
                    if (clients[i].Available > 0)
                    {
                        try
                        {
                            byte[] buffer = new byte[clients[i].Available];
                            clients[i].Receive(buffer);
                            for (int j = 0; j < clients.Count; j++)
                            {
                                if (i == j)
                                    continue;

                                try
                                {
                                    clients[j].Send(buffer);
                                }
                                catch (SocketException clientException)
                                {
                                    if (clientException.SocketErrorCode != SocketError.WouldBlock)
                                    {
                                        Console.WriteLine("Error sending data to the client: " + clientException.Message);
                                    }
                                }
                            }
                        }
                        catch (SocketException clientException)
                        {
                            if (clientException.SocketErrorCode != SocketError.WouldBlock)
                            {
                                Console.WriteLine("Error sending data to the client: " + clientException.Message);
                            }
                        }
                    }
                }
            }
        }
    }
}