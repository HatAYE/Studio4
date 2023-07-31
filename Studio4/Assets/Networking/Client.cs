
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectInstantiationEvent : UnityEvent<int, Vector3, Quaternion> { }
public class Client : MonoBehaviour
{
    Socket socket;
    public static Client instance;
    public PlayerData playerData;
    public ObjectInstantiationEvent onObjectInstantiation;
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
                byte[] buffer = new byte[socket.Available];
                socket.Receive(buffer);
                BasePacket basePacket= new BasePacket().Deserialize(buffer);


                if (basePacket.packType == BasePacket.PackType.instantiate)
                {
                    InstantiatePacket ip = new InstantiatePacket().Deserialize(buffer);
                    InstantiateFromNetwork(ip);
                }

                else if (basePacket.packType == BasePacket.PackType.destroy)
                {
                    DestroyPacket dp = new DestroyPacket().Deserialize(buffer);
                    DestroyFromNetwork(dp);
                }

                else if (basePacket.packType == BasePacket.PackType.movement)
                {
                    MovementPacket mp = new MovementPacket().Deserialize(buffer);
                    ObjectID[] ID = FindObjectsOfType<ObjectID>();

                    for (int i = 0; i < ID.Length; i++)
                    {
                        if (ID[i].objectID == mp.GameObjectID)
                        {
                            ID[i].transform.position = mp.position;
                            break;
                        }
                    }
                }
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

    #region Instantiation
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
    #endregion

    #region Destruction
    public void DestroyFromNetwork(DestroyPacket dp)
    {
        // Find all objects with the ObjectID component
        ObjectID[] objectIDs = FindObjectsOfType<ObjectID>();

        // Loop through the objects to find the one with the matching GameObjectID
        foreach (ObjectID objectIDComponent in objectIDs)
        {
            if (objectIDComponent.objectID == dp.GameObjectID)
            {
                // Destroy the game object associated with the ObjectID component
                Destroy(objectIDComponent.gameObject);
                return;
            }
        }
    }
    public void DestroyLocally(string gameObjectID)
    {
        ObjectID[] objectIDs = FindObjectsOfType<ObjectID>();

        foreach (ObjectID objectIDComponent in objectIDs)
        {
            if (objectIDComponent.objectID == gameObjectID)
            {
                Destroy(objectIDComponent.gameObject);
                return;
            }
        }
    }
    #endregion
}



