using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<string> objectIDS = new List<string>();
    List<int> listOfSpawners = new List<int>();

    void Start()
    {
        Server.instance.ClientConnectedEvent += OnClientConnected;
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
    }

    public int GetRandomPrefabIndex()
    {
        return Random.Range(0, 11);
    }
}