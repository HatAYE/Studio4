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

    public delegate void UpdateNetwork(Vector3 pos, int posIndex);
    public UpdateNetwork UpdateNetworkEvent;



    const float tickRate = (1000.0f / 15.0f) / 1000.0f;
    float timer;

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
        /*timer += Time.deltaTime;

        if (timer >= tickRate)
        {
            UpdateNetworkEvent();
            timer = 0;
        }*/

        if (socket.Available > 0)
        {
            try
            {
                byte[] buffer = new byte[socket.Available];
                socket.Receive(buffer);

                /*
                int start = -1;
                int end = -1;

                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == 83 &&
                        buffer[i + 1] == 84 &&
                        buffer[i + 2] == 65 &&
                        buffer[i + 3] == 82 &&
                        buffer[i + 4] == 84)
                    {
                        start = i + 4;
                        Debug.LogError($"start {start}");
                    }
                    else if (buffer[i] == 69 &&
                        buffer[i + 1] == 78 &&
                        buffer[i + 2] == 68)
                    {
                        end = i + 2;
                        Debug.LogError($"end {end}");
                    }
                }*/

                /*Debug.LogError("A---------");
                string bfuferr = "";

                for (int i = 0; i < buffer.Length; i++)
                {
                    bfuferr += " " + buffer[i].ToString();
                }
                Debug.LogError(bfuferr);
                Debug.LogError("B---------");*/

                if (buffer[0] == Server.HEARTBEAT)
                    return;

                BasePacket basePacket = new BasePacket().Deserialize(buffer);

                /*if (basePacket.packType == BasePacket.PackType.instantiate)
                {
                    InstantiatePacket ip = new InstantiatePacket().Deserialize(buffer);
                    InstantiateFromNetwork(ip);
                }*/
                if (basePacket.packType == BasePacket.PackType.destroy)
                {
                    DestroyPacket dp = new DestroyPacket().Deserialize(buffer);
                    DestroyFromNetwork(dp);
                    Debug.Log("network destroy");

                }
                else if (basePacket.packType == BasePacket.PackType.movement)
                {
                    MovementPacket mp = new MovementPacket().Deserialize(buffer);
                    foreach(ObjectID id in FindObjectsOfType<ObjectID>())
                    {
                        if (id.objectID== mp.GameObjectID)
                        {
                            id.transform.position = mp.position;

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
                    Debug.LogError("indexInstantiate start");
                    BagInstantiatePacket receivedPacket = new BagInstantiatePacket().Deserialize(buffer);
                    List<int> prefabIndexes = receivedPacket.prefabIndex;
                    List<string> objectIDS = receivedPacket.objectIDs;
                    ClientSpawnManager.instance.ReceivePrefabIndexes(prefabIndexes, objectIDS); // Use ReceivePrefabIndexes method with the list
                    Debug.LogError("indexInstantiate end");
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

    /*#region Instantiation
    public static GameObject InstantiateFromNetwork(InstantiatePacket IP)
    {
        GameObject gameObject = Instantiate(Resources.Load(IP.prefabName), IP.position, IP.rotation) as GameObject;
        ObjectID objectID = gameObject.GetComponent<ObjectID>();
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
    #endregion*/

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