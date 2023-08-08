using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    ClientSpawnManager clientManager;
    [SerializeField] List<string> bagObjects = new List<string>();
    public BagMovement bagmovement;
    bool gotInstantiated = false;
    bool canBeInstantiated;
    [SerializeField] GameObject parentObject;
    private void Start()
    {
        bagmovement= transform.parent.GetComponent<BagMovement>();
        //clientManager = FindObjectOfType<ClientSpawnManager>();
        //clientManager.RegisterBag(this);
    }

    private void OnDestroy()
    {
        clientManager.UnregisterBag(this);
    }

    public void InstantiateItems(int prefabIndex)
    {
        if (bagmovement.currentPositionIndex == 0)
        {
            if (!gotInstantiated && canBeInstantiated == true)
            {
                canBeInstantiated = true;
                GameObject prefabToInstantiate = Resources.Load<GameObject>(bagObjects[prefabIndex]);
                GameObject instantiatedObject = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
                instantiatedObject.transform.parent = parentObject.transform;
            }
        }
        else
        {
            canBeInstantiated = false;
            gotInstantiated = false;
        }

    }

}