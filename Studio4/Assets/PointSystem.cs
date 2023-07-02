using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour
{
    int scorePoints;
    [SerializeField] BagReset bagReset;
    [SerializeField] BagMovement bagMovement;
    [SerializeField] TextMeshProUGUI scoreText;

    void CalculatePoints(int itemPointValue)
    {
        scorePoints += itemPointValue;
    }

    public void ItemCheck()
    {
        ///this function will run a check on the type of items are in the bag. each item will have an index number ranging from 0 to 3. 0 being a safe item whilie 3 being high danger
        /// this function should loop items, access their points, determine how much points the player will gain or lose
        if (bagMovement.currentPositionIndex == 3)
        {
            for (int i = 0; i < bagReset.objectRandomizer.Count; i++)
            {
                int itemValue;
                Transform bagObject = bagReset.objectRandomizer[i].transform;
                if (bagObject.childCount > 0)
                {
                    Transform child = bagObject.transform.GetChild(0);
                    if (child.gameObject.GetComponent<ItemType>() != null)
                    {
                        itemValue = child.gameObject.GetComponent<ItemType>().dangerValue;          //DANGER VALUE determines how dangerous an item is from 0 to 3
                        int itemPointValue = child.gameObject.GetComponent<ItemType>().dangerPoints;    //DANGER POINTS is the number calculated at the end of the baggage inspection
                        CalculatePoints(itemPointValue);
                    }
                }
            }
        }

    }
    private void InteractingWithItem()
    {
        ///this function is supposed to detect whether the item clicked on is illegal or not. if player click on item and it is in fact illegal, item gets destroyed and players gains some points. if player clicks and item isn't illegal, he loses points and items remains.

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Items"));
        if (hit.collider != null && bagMovement.currentPositionIndex==2)
        {
            if (hit.collider.gameObject.GetComponent<ItemType>() != null)
            {
                if (hit.collider.gameObject.GetComponent<ItemType>().illegalItem == true)
                {
                    CalculatePoints(20);
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.GetComponent<ItemType>().illegalItem == false)
                {
                    CalculatePoints(-50);
                }
            }
        }

    }
    void Update()
    {
        scoreText.text = scorePoints.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            InteractingWithItem();
        }
    }
}
