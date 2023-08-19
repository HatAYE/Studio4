using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BagReset : MonoBehaviour
{
    public List<ObjectRandomizer> objectRandomizer;
    [SerializeField] GameObject resetPoint;
    [SerializeField] GameObject startPoint;
    BagMovement bagMovement;
    ObjectID ID;
    const float tickRate = (1000.0f / 15.0f) / 1000.0f;
    float timer;
    bool canReInstantiate;
    void Start()
    {
        bagMovement = GetComponent<BagMovement>();
        ID=GetComponent<ObjectID>();
        
    }
    void Update()
    {
        StartCoroutine(ActivateReset());
        if (canReInstantiate)
        {
            StartCoroutine(RequestInstantation());
            canReInstantiate = false;
        }
        
    }

    
    IEnumerator ActivateReset()
    {
        if (bagMovement.currentPositionIndex >= bagMovement.bagPositions.Count)
        {
            yield return new WaitForSeconds(1);
            resetPoint.SetActive(true);
        }
        else resetPoint.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == resetPoint)
        {
            timer += Time.deltaTime;

            if (timer >= tickRate)
            {
                StartCoroutine(DestroyGameobjects(transform));
                timer = 0;
            }
            
        }
    }
    IEnumerator RequestInstantation()
    {
        yield return new WaitForSeconds(0.5f);
        Client.instance.Send(new byte[] { 255 });
    }
    IEnumerator DestroyGameobjects(Transform parent)
    {
        for (int i = 0; i < objectRandomizer.Count; i++)
        {
            Transform bagObject = objectRandomizer[i].transform;
            if (bagObject.childCount > 0)
            {
                Transform child = parent.transform.GetChild(i).GetChild(0);
                ObjectID objectID = child.GetComponent<ObjectID>();
                if (objectID != null)
                {
                    Client.instance.DestroyLocally(objectID.objectID);
                    Client.instance.Send(new DestroyPacket (Client.instance.playerData, objectID.objectID).Serialize());
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
        canReInstantiate = true;
        yield return new WaitForSeconds(1);
        bagMovement.currentPositionIndex = 0;
        parent.position = startPoint.transform.position;
    }
}

