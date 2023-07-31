
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Collections.Generic;

public class Server : MonoBehaviour
{
    Socket serversocket;
    public List<Socket> clients = new List<Socket>();
    public static Server instance;
    void Start()
    {
        serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serversocket.Bind(new IPEndPoint(IPAddress.Any, 3000)); //entrance to recieve clients
        serversocket.Listen(10);
        serversocket.Blocking = false;

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

                        BasePacket basePacket = new BasePacket().Deserialize(buffer);
                        if (basePacket.packType == BasePacket.PackType.instantiate)
                        {
                            InstantiatePacket ip = new InstantiatePacket().Deserialize(buffer);
                            InstantiateFromNetwork(ip);

                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
    public static GameObject InstantiateFromNetwork(InstantiatePacket IP)
    {
        GameObject gameObject = Instantiate(Resources.Load(IP.prefabName), IP.position, IP.rotation) as GameObject;
        ObjectID objectID = gameObject.GetComponent<ObjectID>();
        objectID.ownerID = IP.player.playerID;
        objectID.objectID = IP.GameObjectID;

        return gameObject;
    }
    public void Send(byte[] buffer)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            clients[i].Send(buffer);
        }
    }
}
