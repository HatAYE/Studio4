
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Collections.Generic;

public class Server : MonoBehaviour
{
    public delegate void ClientConnected(int totalClients);
    public ClientConnected ClientConnectedEvent;

    public delegate void ReceivedPacket(byte[] buffer);
    public ReceivedPacket ReceivedPacketEvent;

    Socket serversocket;
    public List<Socket> clients = new List<Socket>();

    public PlayerData playerData;
    public static byte HEARTBEAT { get { return 176; } }

    public static Server instance;

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
        serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serversocket.Bind(new IPEndPoint(IPAddress.Any, 3000)); //entrance to recieve clients
        serversocket.Listen(10);
        serversocket.Blocking = false;

        playerData = new PlayerData("1", "server");
        InvokeRepeating("IsAlive", 1, 1);
    }

    void Update()
    {
        try
        {
            clients.Add(serversocket.Accept());
            Debug.Log("Client has connected");

            if (ClientConnectedEvent != null)
                ClientConnectedEvent(clients.Count);
        }
        catch (SocketException e)
        {
            if (e.SocketErrorCode != SocketError.WouldBlock)
            {
                Debug.Log(e);
            }
        }

        for (int i = 0; i < clients.Count; i++)
        {
            try
            {
                if (clients[i].Available > 0)
                {
                    byte[] buffer = new byte[clients[i].Available];
                    clients[i].Receive(buffer);

                    if(ReceivedPacketEvent != null)
                        ReceivedPacketEvent(buffer);

                    for (int j = 0; j < clients.Count; j++)
                    {
                        if (i == j) continue;
                        clients[j].Send(buffer);
                    }
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    Debug.Log(e);
                }
            }
        }
    }

    public void SendToAllClients(byte[] buffer)
    {
        //Debug.LogError($"Sending {buffer.Length}");

        for (int i = 0; i < clients.Count; i++)
        {
            clients[i].Send(buffer);
        }
    }

    void IsAlive()
    {
        for (int i = 0; i < clients.Count; i++)
        {
            try
            {
                clients[i].Send(new byte[1] { HEARTBEAT });
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.WouldBlock)
                {
                    if (e.SocketErrorCode == SocketError.ConnectionAborted || e.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        Debug.Log("CLient Disconnected..");
                        clients[i].Close();
                        clients.RemoveAt(i);
                    }
                    else
                        Debug.Log(e);
                }
            }
        }
    }
}