
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;

public class Server : MonoBehaviour
{
    Socket serversocket;
    public List<Socket> clients = new List<Socket>();
    public static Server instance;
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

    private void Update()
    {
        try
        {
            clients.Add(serversocket.Accept());
            Debug.Log("Client has connected");
            if (currentPrefabIndex == -1)
            {
                // Generate a random prefab index once and send it to the connected clientfor
            for (int i = 0; i < 17; i++)
            {
                listOfSpawners.Add(spawnManager.GetRandomPrefabIndex());
            }
                //currentPrefabIndex = spawnManager.GetRandomPrefabIndex();
                string gameObjectID = Random.Range(0, 1000).ToString();
                BagInstantiatePacket bagInstantiatePacket = new BagInstantiatePacket(playerData, listOfSpawners, gameObjectID);
                byte[] packetData = bagInstantiatePacket.Serialize();
                clients[clients.Count - 1].Send(packetData);
            }
            
        }
        catch
        {
            //print("Failed to accept connection");
        }
        try
        {
            for (int i = 0; i < clients.Count; i++)
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
        }
        catch
        {

        }
    }
}
