using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    ClientSpawnManager clientManager;
    [SerializeField] List<string> bagObjects = new List<string>();
    public BagMovement bagmovement;
    [SerializeField] bool gotInstantiated = false;
    //[SerializeField] bool canBeInstantiated;
    [SerializeField] GameObject parentObject;
    private void Start()
    {
        bagmovement= transform.parent.GetComponent<BagMovement>();
        //clientManager = FindObjectOfType<ClientSpawnManager>();
        //clientManager.RegisterBag(this);
    }
    private void Update()
    {
        if (bagmovement.currentPositionIndex == 0)
        {
            if (!gotInstantiated)
            {
                //ClientSpawnManager.instance.ReInstantiateItems();
                gotInstantiated = true;
            }
        }
        else
            gotInstantiated = false;
    }

    public void InstantiateItems(int prefabIndexes)
    {
            GameObject prefabToInstantiate = Resources.Load<GameObject>(bagObjects[prefabIndexes]);
            GameObject instantiatedObject = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
            instantiatedObject.transform.parent = parentObject.transform;
            gotInstantiated = true;
    }

}