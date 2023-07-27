using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    [SerializeField] List<string> bagObjects = new List<string>();
    BagMovement bagmovement;
    bool gotInstantiated = false;
    bool canBeInstantiated;
    private void Start()
    {
        //bagmovement= transform.parent.GetComponent<BagMovement>();
        if (Client.instance != null)
        {
            Client.instance.onObjectInstantiation.AddListener(InstantiateLocallyAndSendPacket);
        }
    }
    private void OnDestroy()
    {
        if (Client.instance != null)
        {
            Client.instance.onObjectInstantiation.RemoveListener(InstantiateLocallyAndSendPacket);
        }
    }
    void Update()
    {
        /*if (bagmovement.currentPositionIndex == 0)
        {
            canBeInstantiated = true;
            RandomizeObjects(transform);
        }
        else
        {
            canBeInstantiated = false;
            gotInstantiated = false;
        }*/
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Get a random prefab index
            int prefabIndex = Random.Range(0, bagObjects.Count);

            // Call the event to trigger the object instantiation on both clients
            Client.instance.onObjectInstantiation?.Invoke(prefabIndex, new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0), Quaternion.identity);
        }
    }
    public void InstantiateLocallyAndSendPacket(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        // Instantiate the object locally on this client
        string prefabName = bagObjects[prefabIndex];

        // Send an instantiate packet to the server, which will relay it to all clients
        GameObject instantiatedObject = Client.instance.InstantiateLocally(prefabName, position, rotation);
        InstantiatePacket packet = new InstantiatePacket(Client.instance.playerData, prefabIndex, instantiatedObject.GetComponent<ObjectID>().objectID, prefabName, position, rotation);
        Client.instance.Send(packet.Serialize());
    }
    /*public void RandomizeObjects(Transform instantiatingPosition)
    {
        if (!gotInstantiated && canBeInstantiated==true)
        {
            int prefabIndex = Random.Range(0, bagObjects.Count);
            string bag = bagObjects[prefabIndex];
            GameObject instantiatedObject = Client.instance.InstantiateLocally(bag, instantiatingPosition.position, Quaternion.identity);
            instantiatedObject.transform.parent = transform;
            gotInstantiated = true;
            Client.instance.Send(new InstantiatePacket(Client.instance.playerData, prefabIndex, instantiatedObject.GetComponent<ObjectID>().objectID, bag, instantiatingPosition.position, Quaternion.identity).Serialize());
            
        }
    }*/

}