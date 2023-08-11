using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;


public class Client : MonoBehaviour
{
    Socket socket;
    public static Client instance;
    public PlayerData playerData;
    public static int totalScore;

    public delegate void UpdateNetwork();
    public UpdateNetwork UpdateNetworkEvent;

    const float tickRate = (1000.0f / 15.0f) / 1000.0f;
    float timer;

    //private int _clientCurrentPositionIndex;
    /*public int clientCurrentPositionIndex
    {
        get { return _clientCurrentPositionIndex; }
        set
        {
            if (_clientCurrentPositionIndex != value)
            {
                _clientCurrentPositionIndex = value;
                GetPositionIdexLocally(_clientCurrentPositionIndex); // Call this whenever the clientCurrentPositionIndex changes
            }
        }
    }*/
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
        string id = Random.Range(0,100).ToString();
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
        timer += Time.deltaTime;

        if(timer >= tickRate)
        {
            //UpdateNetworkEvent();
            timer = 0;
        }

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
                    Debug.Log("network destroy");

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
                else if (basePacket.packType == BasePacket.PackType.score)
                {
                    ScorePacket sp = new ScorePacket().Deserialize(buffer);
                    totalScore = sp.gameScore;
                }

                else if (basePacket.packType == BasePacket.PackType.indexInstantiate)
                {
                    BagInstantiatePacket receivedPacket = new BagInstantiatePacket().Deserialize(buffer);
                    List<int> prefabIndexes = receivedPacket.prefabIndex;
                    ClientSpawnManager.instance.ReceivePrefabIndexes(prefabIndexes); // Use ReceivePrefabIndexes method with the list
                    Debug.Log("do you ever feel");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
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

    /*public void GetPositionIdexLocally(int currentPositionIndex)
    {
        BagInstantiatePacket bagInstantiatePacket = new BagInstantiatePacket(playerData,currentPositionIndex, null);
        byte[] packetData = bagInstantiatePacket.Serialize();
        Send(packetData);
    }*/
    #endregion

    #region Destruction
    public void DestroyFromNetwork(DestroyPacket dp)
    {
        ObjectID[] objectIDs = FindObjectsOfType<ObjectID>();

        foreach (ObjectID objectIDComponent in objectIDs)
        {
            if (objectIDComponent.objectID == dp.GameObjectID)
            {
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

    #region Score and points

    public void CalculatePointsFromNetwork(ScorePacket dp)
    {
        totalScore += dp.gameScore;
    }

    public void CalculatePointsLocally(int amount)
    {
        totalScore += amount;
    }


    #endregion
}



