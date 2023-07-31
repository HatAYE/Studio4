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
        bagmovement= transform.parent.GetComponent<BagMovement>();
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
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            int prefabIndex = Random.Range(0, bagObjects.Count);
            Client.instance.onObjectInstantiation?.Invoke(prefabIndex, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }*/
        if (bagmovement.currentPositionIndex == 0)
        {
            canBeInstantiated = true;            
            int prefabIndex = Random.Range(0, bagObjects.Count);
            Client.instance.onObjectInstantiation?.Invoke(prefabIndex, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Call the event to trigger the object instantiation on both clients
        }
        else
        {
            canBeInstantiated = false;
            gotInstantiated = false;
        }
    }
    public void InstantiateLocallyAndSendPacket(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        if (!gotInstantiated && canBeInstantiated == true)
        {
            string prefabName = bagObjects[prefabIndex];
            GameObject instantiatedObject = Client.instance.InstantiateLocally(prefabName, position, rotation);// Send an instantiate packet to the server, which will relay it to all clients
            InstantiatePacket packet = new InstantiatePacket(Client.instance.playerData, prefabIndex, instantiatedObject.GetComponent<ObjectID>().objectID, prefabName, position, rotation);
            instantiatedObject.transform.parent = transform;
            gotInstantiated = true;
            Client.instance.Send(packet.Serialize());
        }
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