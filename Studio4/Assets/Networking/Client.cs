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
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }
    public void StartClient()
    {
        try
        {
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000)); //end point is an ip address. port is a random number
            byte[] buffer = new byte[256];
            socket.Receive(buffer);
            Debug.Log(Encoding.ASCII.GetString(buffer)); //this code is used to recieve data   
            socket.Send(Encoding.ASCII.GetBytes("client says hamood"));
        }
        catch
        {
            Debug.Log("Client unable to connect wah wah");
        }


    }
}


