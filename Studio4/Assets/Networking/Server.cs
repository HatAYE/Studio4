
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Collections.Generic;

public class Server : MonoBehaviour
{
    Socket serversocket;
    public List<Socket> clients = new List<Socket>();

    void Start()
    {
        serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serversocket.Bind(new IPEndPoint(IPAddress.Any, 3000)); //entrance to recieve clients
        serversocket.Listen(10);
        serversocket.Blocking = false;

    }

    private void Update()
    {
            try
            {
                clients.Add(serversocket.Accept());
                Debug.Log("Client has connected");
            }
            catch
            {
                //print("Failed to accept connection");
            }
            try
            {
                for (int i = 0;i<clients.Count; i++)
                {
                    if (clients[i].Available>0)
                    {
                        byte[] buffer = new byte[clients[i].Available];
                        clients[i].Receive(buffer);
                        for (int j = 0;j < clients.Count; j++)
                        {
                            if (i == j) continue;
                            clients[j].Send(buffer);
                        }
                    }
                }
            }
            catch 
            { 
            
            }
    }
}