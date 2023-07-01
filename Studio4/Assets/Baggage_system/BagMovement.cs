using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BagMovement : MonoBehaviour
{
    [SerializeField] List<GameObject> bagPositions;
    [SerializeField] int speed;
    [SerializeField] GameObject rejectButton;
    [HideInInspector] public int currentPositionIndex;
    Coroutine movementCoroutine;
    [SerializeField] bool isMoving;
    [SerializeField] bool initialMove=true;

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
            rejectButton.SetActive(false);
            yield return null;
        }
        rejectButton.SetActive(true);
        isMoving = false;
        currentPositionIndex++;
    }
    public void MoveToPosition()
    {
            if (!isMoving && currentPositionIndex < bagPositions.Count - 1)
            {
                if (movementCoroutine != null)
                {
                    // Stop the previous coroutine if it was running
                    StopCoroutine(movementCoroutine);
                }
            }
        if (currentPositionIndex < bagPositions.Count)
        {
            Vector3 targetPosition = bagPositions[currentPositionIndex].transform.position;
            movementCoroutine = StartCoroutine(MoveToPositionCoroutine(targetPosition, speed));
        }
    }

    public void MoveToLastPosition()
    {
        if (!isMoving && currentPositionIndex < 3)
        {
            if (movementCoroutine != null)
            {
                // Stop the previous coroutine if it was running
                StopCoroutine(movementCoroutine);
            }
            Vector3 targetPosition = bagPositions[bagPositions.Count - 1].transform.position;
            currentPositionIndex = bagPositions.Count - 1;
            movementCoroutine = StartCoroutine(MoveToPositionCoroutine(targetPosition, speed));
        }
    }
}
    