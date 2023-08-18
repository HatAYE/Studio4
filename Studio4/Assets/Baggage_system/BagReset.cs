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
    bool sendPacket;
    const float tickRate = (1000.0f / 15.0f) / 1000.0f;
    float timer;

    [SerializeField] float timerToRespawn;
    float maxTimerToRespawn=2;
    void Start()
    {
        bagMovement = GetComponent<BagMovement>();
        ID=GetComponent<ObjectID>();
    }
    void Update()
    {
        StartCoroutine(ActivateReset());
        if (sendPacket)
        {
            ResetPacket resetPacket = new ResetPacket(Client.instance.playerData, ID.objectID);
            byte[] packetData = resetPacket.Serialize();
            //Client.instance.Send(packetData);
            sendPacket = false;
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
            timerToRespawn += Time.deltaTime;
            if (timerToRespawn>= maxTimerToRespawn)
            {
                
                timerToRespawn = 0;
            }
            
            //sendPacket= true;
        }
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
        bagMovement.currentPositionIndex = 0;
        parent.position = startPoint.transform.position;
    }
}

