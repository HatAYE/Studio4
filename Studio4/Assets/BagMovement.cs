using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BagMovement : MonoBehaviour
{
    [SerializeField] List<GameObject> bagPositions;
    [SerializeField] int speed;
    [SerializeField] GameObject rejectButton;
    int currentPositionIndex;
    Coroutine movementCoroutine;
    bool isMoving;
    
    void Start()
    {

    }

    // 1- iterate loop when button is clicked them make bag move to the position
    void Update()
    {

    }

    private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition, float moveSpeed)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            rejectButton.SetActive(false);
            yield return null;
        }
        rejectButton.SetActive(true);
        isMoving = false;
        currentPositionIndex++;


    }
    public void MoveToPosition()
    {
            /*Vector3 positionDirection = bagPositions[i].transform.position - gameObject.transform.position;
            if (gameObject.transform.position != bagPositions[i].transform.position)
                transform.position += positionDirection * speed * Time.deltaTime;*/
            if (!isMoving)
            {
                if (movementCoroutine != null)
                {
                    // Stop the previous coroutine if it was running
                    StopCoroutine(movementCoroutine);
                }
            }
        if (currentPositionIndex < bagPositions.Count)
        {
            StartMovement();
        }
    }
    private void StartMovement()
    {
        Vector3 targetPosition = bagPositions[currentPositionIndex].transform.position;
        movementCoroutine = StartCoroutine(MoveToPositionCoroutine(targetPosition, speed));
    }
}