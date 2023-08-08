using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    ObjectID ID;
    [SerializeField] TextMeshProUGUI scoreText;
    bool playerMoved = false;

    void Start()
    {
        ID = GetComponent<ObjectID>();
        Client.instance.UpdateNetworkEvent += OnUpdateNetwork;
    }

    private void OnUpdateNetwork()
    {
        if (playerMoved)
        {
            Client.instance.Send(new MovementPacket(Client.instance.playerData, ID.objectID, transform.position).Serialize());
        }
    }

    void Update()
    {
        #region destroying
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position into the scene
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Items"));

            // Check if the ray hit any GameObject
            if (hit.collider != null)
            {
                ObjectID objectIDComponent = hit.collider.gameObject.GetComponent<ObjectID>();
                if (objectIDComponent != null)
                {
                    string objectID = objectIDComponent.objectID;

                    Client.instance.DestroyLocally(objectID);
                    Client.instance.Send(new DestroyPacket(Client.instance.playerData, objectID).Serialize());
                }
            }
        }
        #endregion

        #region movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * 5 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * 5 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * 5 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * 5 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            playerMoved = true;
        }
        else
        {
            playerMoved = false;
        }

        #endregion

        #region score
        if (Input.GetKeyDown(KeyCode.E))
        {
            Client.instance.CalculatePointsLocally(50);
            Client.instance.Send(new ScorePacket(Client.instance.playerData, ID.objectID, Client.totalScore).Serialize()) ;
        }
        scoreText.text = Client.totalScore.ToString();
        #endregion
    }
}
