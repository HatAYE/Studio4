using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Client;

public class BagMovement : MonoBehaviour
{
    public List<GameObject> bagPositions;
    [SerializeField] int speed;
    [SerializeField] GameObject rejectButton;
    [SerializeField] GameObject acceptButton;
    [SerializeField] PointSystem pointSystem;
    public int currentPositionIndex;
    public bool isMoving;
    bool initialMove = true;
    public bool rejecting;
    ObjectID ID;

    const float tickRate = (1000.0f / 15.0f) / 1000.0f;
    float timer;
    private void Start()
    {
        ID = GetComponent<ObjectID>();
        instance.UpdateNetworkEvent += UpdateMovement;
    }

    void UpdateMovement(Vector3 pos, int posIndex, string eventObjectID)
    {
        timer += Time.deltaTime;

        if (timer >= tickRate)
        {
            if (ID.objectID == eventObjectID)
            {
                //Vector3 newPosition = new Vector3(pos.x, pos.y, transform.position.z);
                transform.position = pos;
                currentPositionIndex = posIndex;
                timer = 0;
            }
        }
        
    }
    private void OnDestroy()
    {
        instance.UpdateNetworkEvent -= UpdateMovement;
    }
    void SendPacket()
    {
        if (instance != null)
        {
            instance.Send(new MovementPacket(instance.playerData, transform.position, currentPositionIndex, ID.objectID).Serialize());
        }
    }
    private void Update()
    {
        if (currentPositionIndex < bagPositions.Count && initialMove && currentPositionIndex == 0)
        {
            initialMove = false;
            MoveToPosition();
        }
        else if (currentPositionIndex>0)
            initialMove = true;
    }

    private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition, float moveSpeed)
    {
        ///this coroutine is responsible for moving the object and deactiving the buttons. it takes a position and speed.
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            SendPacket();
            if (rejectButton != null)
            rejectButton.SetActive(false);
            acceptButton.SetActive(false);
            yield return null;
        }
        if (rejectButton != null)
            rejectButton.SetActive(true);
        acceptButton.SetActive(true);
        isMoving = false;
        currentPositionIndex++;
        pointSystem.ItemCheck();
        rejecting = false;
    }
    public void MoveToPosition()
    {
        if (currentPositionIndex < bagPositions.Count)
        {
            Vector3 targetPosition = bagPositions[currentPositionIndex].transform.position;

            rejecting = true;
            StartCoroutine(MoveToPositionCoroutine(targetPosition, speed));
            
        }
        
    }

    public void MoveToLastPosition()
    {
        if (!isMoving && currentPositionIndex < 3)
        {
            Vector3 targetPosition = bagPositions[bagPositions.Count - 1].transform.position;

            currentPositionIndex = bagPositions.Count - 1;
            StartCoroutine(MoveToPositionCoroutine(targetPosition, speed));
        }
    }
}
    