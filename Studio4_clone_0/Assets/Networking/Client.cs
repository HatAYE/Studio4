
using System.Net.Sockets;
using System.Net;
using UnityEngine;


public class Client : MonoBehaviour
{
    Socket socket;
    public static Client instance;
    public PlayerData playerData;

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
    private void Start()
    {
        string id = Random.Range(0, 100).ToString();
        playerData = new PlayerData(id, $"player{id}");
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000));
        socket.Blocking = false;
    }
    public void Connect()
    {
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
                //InstantiatePacket packet = new InstantiatePacket().Deserialize(buffer);
                //InstantiateFromNetwork(packet.prefabName, packet.position, packet.rotation);
                BasePacket basePacket= new BasePacket().Deserialize(buffer);
                if (basePacket.packType == BasePacket.PackType.instantiate)
                {
                    InstantiatePacket ip = new InstantiatePacket().Deserialize(buffer);
                    InstantiateFromNetwork(ip);
                }

                /*else if (bp.packType == BasePacket.PackType.Movement)
                {
                    MovementPacket mp = new MovementPacket().Desrialize(buffer);
                    NetworkComponent[] ncs = FindObjectsOfType<NetworkComponent>();

                    for (int i = 0; i < ncs.Length; i++)
                    {
                        if (ncs[i].GameObjectID == mp.GameObjectID)
                        {
                            ncs[i].transform.position = mp.position;
                            break;
                        }
                    }
                }*/
            }
            catch
            {
                Debug.Log("Client unable to connect wah wah");
            }
        }

    }

    public void Send(byte[] buffer)
    {
        socket.Send(buffer);
    }
    public static GameObject InstantiateFromNetwork(InstantiatePacket IP)
    {
        GameObject gameObject= Instantiate(Resources.Load(IP.prefabName), IP.position, IP.rotation) as GameObject;
        ObjectID objectID= gameObject.GetComponent<ObjectID>();
        objectID.ownerID = IP.player.playerID;
        objectID.objectID = IP.GameObjectID;

        return gameObject;
        
    }

    public GameObject InstantiateLocally(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject gameObject = (GameObject)Instantiate(Resources.Load(prefabName), position, rotation);

        ObjectID objectComponent = gameObject.GetComponent<ObjectID>();
        objectComponent.ownerID = playerData.playerID;
        objectComponent.GenerateGameObjectIDToSelf();

        return gameObject;
    }
}



