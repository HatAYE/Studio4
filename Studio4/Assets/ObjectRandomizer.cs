using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour
{
    [SerializeField] List<GameObject> bagObjects= new List<GameObject>();
    bool gotInstantiated= false;

    void Update()
    {
        if (!gotInstantiated)
        {
            GameObject instantiatedObject= Instantiate(bagObjects[Random.RandomRange(0, bagObjects.Count)], transform.position, Quaternion.identity);
            instantiatedObject.transform.parent = transform;
            gotInstantiated = true;
        }


    }
}
