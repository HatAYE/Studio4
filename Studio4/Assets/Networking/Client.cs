using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    Socket socket;
    private void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000)); //end point is an ip address. port is a random number
    }

    void Update()
    {
        if (socket.Available>0)
        {
            try
            {
                byte[] buffer = new byte[256];
                socket.Receive(buffer);
                InstantiatePacket packet = new InstantiatePacket().Deserialize(buffer);
                InstantiateFromNetwork(packet.prefabName, packet.position, packet.rotation);
                
            }
            catch
            {
                Debug.Log("Client unable to connect wah wah");
            }
        }

    }
    void InstantiateFromNetwork(string prefabName, Vector3 position, Quaternion rotation)
    {
        Instantiate(Resources.Load(prefabName), position, rotation);
    }
}


