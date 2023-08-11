
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Collections.Generic;


public class Server : MonoBehaviour
{
    Socket serversocket;
    public List<Socket> clients = new List<Socket>();
    ServerSpawnManager spawnManager;
    int currentPrefabIndex = -1;
    PlayerData playerData;
    [SerializeField] List<int> listOfSpawners = new List<int>();


    void Start()
    {
        serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serversocket.Bind(new IPEndPoint(IPAddress.Any, 3000)); //entrance to recieve clients
        serversocket.Listen(10);
        serversocket.Blocking = false;

        spawnManager = FindObjectOfType<ServerSpawnManager>();
        playerData = new PlayerData("1", "server");

        InvokeRepeating("IsAlive", 1, 1);
    }

    void Update()
    {
        try
        {
            clients.Add(serversocket.Accept());
            Debug.Log("Client has connected");
            // Generate a random prefab index once and send it to the connected clientfor
            for (int i = 0; i < 17; i++)
            {
                listOfSpawners.Add(spawnManager.GetRandomPrefabIndex());
            }
            string gameObjectID = Random.Range(0, 1000).ToString();
            BagInstantiatePacket bagInstantiatePacket = new BagInstantiatePacket(playerData, listOfSpawners, gameObjectID);
            byte[] packetData = bagInstantiatePacket.Serialize();
            clients[clients.Count - 1].Send(packetData);
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

        void IsAlive()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                try
                {
                    clients[i].Send(new byte[1]);
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

