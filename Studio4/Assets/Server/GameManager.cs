using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    List<string> objectIDS = new List<string>();
    List<int> listOfSpawners = new List<int>();
    bool newListSent = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Server.instance.ClientConnectedEvent += OnClientConnected;
        Server.instance.ReceivedPacketEvent += onRecieve;
    }

    void Update()
    {

    }
    void OnClientConnected(int totalClients)
    {
        if (totalClients >= 2)
        {
            SendInstantiationPackets();
            
        }
    }
    void onRecieve(byte[] buffer)
    {Debug.Log(buffer[0]);
        if (buffer.Length == 1 && buffer[0] == 255)
        {
            newListSent = false;
            
            SendInstantiationPackets();
        }
    }
    void SendInstantiationPackets()
    {
        listOfSpawners.Clear();
        objectIDS.Clear();

        for (int k = 0; k < 17; k++)
        {
            listOfSpawners.Add(GetRandomPrefabIndex());
            objectIDS.Add(Random.Range(1, 1000).ToString());
        }

        BagInstantiatePacket bagInstantiatePacket = new BagInstantiatePacket(
                Server.instance.playerData,
            listOfSpawners,
            objectIDS,
            Random.Range(1, 1000).ToString());

        Server.instance.SendToAllClients(bagInstantiatePacket.Serialize());
        newListSent = true;
    }

    public int GetRandomPrefabIndex()
    {
        return Random.Range(0, 11);
    }
}