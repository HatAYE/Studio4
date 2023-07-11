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
    [SerializeField] GameObject player;

    void Start()
    {
        serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serversocket.Bind(new IPEndPoint(IPAddress.Any, 3000)); //entrance to recieve clients
        serversocket.Listen(10);
        serversocket.Blocking = false;

    }

    private void Update()
    {
        if (!clientIsConnected)
        {
            try
            {
                client = serversocket.Accept();
                Debug.Log("Client has connected");
                clientIsConnected = true;
            }
            catch
            {
                //print("Failed to accept connection");
            }
        }

        if (clientIsConnected)
        {

                try
                {
                    //client.Receive(buffer);
                    byte[] buffer = Util.SerializeVector3(player.transform.position);
                    client.Send(buffer);
                }
                catch
                {
                }
        }
    }
}