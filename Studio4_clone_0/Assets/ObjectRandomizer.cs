using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    [SerializeField] List<GameObject> bagObjects = new List<GameObject>();
    BagMovement bagmovement;
    bool gotInstantiated = false;
    bool canBeInstantiated;

    private void Start()
    {
        bagmovement= transform.parent.GetComponent<BagMovement>();
    }
    void Update()
    {
        if (bagmovement.currentPositionIndex == 0)
        {
            canBeInstantiated = true;
            RandomizeObjects(transform);
        }
        else
        {
            canBeInstantiated = false;
            gotInstantiated = false;
        }
    }

    public void RandomizeObjects(Transform instantiatingPosition)
    {
        if (!gotInstantiated && canBeInstantiated==true)
        {
            GameObject instantiatedObject = Instantiate(bagObjects[Random.RandomRange(0, bagObjects.Count)], instantiatingPosition.position, Quaternion.identity);
            instantiatedObject.transform.parent = transform;
            gotInstantiated = true;
        }
    }
}