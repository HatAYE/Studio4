using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    [SerializeField] List<string> bagObjects = new List<string>();
    BagMovement bagmovement;
    [SerializeField] bool gotInstantiated = false;
    [SerializeField] bool canBeInstantiated;
    private void Start()
    {
        //bagmovement= transform.parent.GetComponent<BagMovement>();
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
        /*if (Input.GetKey(KeyCode.Q))
        {
            string bag = bagObjects[Random.Range(0, bagObjects.Count)];
            GameObject instantiatedObject = Client.instance.InstantiateLocally(bag, new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0), Quaternion.identity);
            Client.instance.Send(new InstantiatePacket(Client.instance.playerData, instantiatedObject.GetComponent<ObjectID>().objectID, bag, new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0), Quaternion.identity).Serialize());

        }*/
    }

    /*public void RandomizeObjects(Transform instantiatingPosition)
    {
        if (!gotInstantiated && canBeInstantiated==true)
        {
            string bag = bagObjects[Random.Range(0, bagObjects.Count)];
            GameObject instantiatedObject = Client.instance.InstantiateLocally(bag, instantiatingPosition.position, Quaternion.identity);
            instantiatedObject.transform.parent = transform;
            gotInstantiated = true;
            Client.instance.Send(new InstantiatePacket(Client.instance.playerData, instantiatedObject.GetComponent<ObjectID>().objectID, bag, instantiatingPosition.position, Quaternion.identity).Serialize());
            
        }
    }*/

}