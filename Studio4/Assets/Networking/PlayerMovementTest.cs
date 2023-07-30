using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    ObjectID ID;
    void Start()
    {
        ID = GetComponent<ObjectID>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position into the scene
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Items"));

            // Check if the ray hit any GameObject
            if (hit.collider != null)
            {
                // Check if the hit GameObject has the ObjectID component
                ObjectID objectIDComponent = hit.collider.gameObject.GetComponent<ObjectID>();
                if (objectIDComponent != null)
                {
                    // Get the objectID of the clicked GameObject
                    string objectID = objectIDComponent.objectID;

                    // Destroy the GameObject locally and notify the server to destroy it as well
                    Client.instance.DestroyLocally(objectID);
                    Client.instance.Send(new DestroyPacket(Client.instance.playerData, objectID).Serialize());
                }
            }
        }


        if (Input.GetKey(KeyCode.W)) transform.position += Vector3.forward * 5 * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) transform.position += Vector3.back * 5 * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) transform.position += Vector3.right * 5 * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) transform.position += Vector3.left * 5 * Time.deltaTime;
        
        Client.instance.Send(new MovementPacket( Client.instance.playerData, ID.objectID, transform.position).Serialize());

    }
}
