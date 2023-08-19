using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    [SerializeField] List<string> bagObjects = new List<string>();
    public BagMovement bagmovement;
    [SerializeField] bool gotInstantiated = false;
    [SerializeField] GameObject parentObject;
    ObjectID ID;
    float ZAxis = -0.40f;
    private void Start()
    {
        bagmovement= transform.parent.GetComponent<BagMovement>();
        ID=GetComponent<ObjectID>();
    }
    private void Update()
    {
        gotInstantiated = false;
    }

    public void InstantiateItems(int prefabIndexes, string objectID)
    {
        GameObject prefabToInstantiate = Resources.Load<GameObject>(bagObjects[prefabIndexes]);
        if (transform.childCount == 0)
        {
            GameObject instantiatedObject = Instantiate(prefabToInstantiate, new Vector3(transform.position.x, transform.position.y, ZAxis), Quaternion.identity);
            ObjectID objectIDComponent = instantiatedObject.GetComponent<ObjectID>();
            if (objectIDComponent != null)
            {
                objectIDComponent.ownerID = Client.instance.playerData.playerID;
                objectIDComponent.objectID = objectID;
            }

            instantiatedObject.transform.parent = parentObject.transform;
            gotInstantiated = true;
        }

    }

}