using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject[] cameraPositions;
    [SerializeField] int speed;
    IEnumerator MoveCam(Vector2 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

    }
    public void MoveCameraToTheRight()
    {
        StartCoroutine(MoveCam(cameraPositions[1].transform.position));
    }

    public void MoveCameraToTheLeft()
    {
        StartCoroutine(MoveCam(cameraPositions[0].transform.position));
    }
}
