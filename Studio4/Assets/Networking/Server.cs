using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEditor.PackageManager;
using UnityEngine;

public class Server : MonoBehaviour
{
    Socket serversocket;
    Socket client = null;
    bool clientIsConnected = false;
    byte[] buffer;

    public void StartingServer()
    {
        serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serversocket.Bind(new IPEndPoint(IPAddress.Any, 3000)); //entrance to recieve clients
        serversocket.Listen(10);
        serversocket.Blocking = false;

        buffer = new byte[256];
    }

    private void Update()
    {
        if (!clientIsConnected)
        {
            try
            {
                client = serversocket.Accept();
                clientIsConnected = true;
                Debug.Log("Client has connected");
            }
            catch 
            {
                //print("Failed to accept connection");
            }
        }

        if (clientIsConnected)
        {
            client.Send(Encoding.ASCII.GetBytes("Hello mammamia I'm the server")); //THERES AN ERROR HERE

            if (client.Available > 0)
            {
                try
                {
                    client.Receive(buffer);
                    Debug.Log(Encoding.ASCII.GetString(buffer));
                }
                catch 
                { 
                }
            }
        }
    }
}