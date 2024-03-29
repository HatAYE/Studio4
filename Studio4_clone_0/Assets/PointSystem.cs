using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour
{
    private static int totalScore;
    [SerializeField] BagReset bagReset;
    [SerializeField] BagMovement bagMovement;
    [SerializeField] TextMeshProUGUI scoreText;
    List<Transform> legalItemsList = new List<Transform>();
    List<Transform> illegalItemsList = new List<Transform>();

    void CalculatePoints(int itemPointValue)
    {
        totalScore += itemPointValue;
    }

    public void ItemCheck()
    {
        ///this function will run a check on the type of items are in the bag. each item will have an index number ranging from 0 to 3. 0 being a safe item whilie 3 being high danger
        /// this function should loop items, access their points, determine how much points the player will gain or lose
        if (bagMovement.currentPositionIndex == bagMovement.bagPositions.Count)
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
                        if (child.gameObject.GetComponent<ItemType>().illegalItem == true)
                        {
                            //itemValue = child.gameObject.GetComponent<ItemType>().dangerValue;              //DANGER VALUE determines how dangerous an item is from 0 to 3
                            illegalItemsList.Add(child);
                            int itemPointValue = child.gameObject.GetComponent<ItemType>().dangerPoints;    //DANGER POINTS is the number calculated at the end of the baggage inspection
                            if (bagMovement.rejecting==false) CalculatePoints(itemPointValue);

                            //return;
                        }

                        if (child.gameObject.GetComponent<ItemType>().illegalItem == false)
                        {
                            legalItemsList.Add(child);
                            if (illegalItemsList.Count == 0 && bagMovement.rejecting==false)
                            {
                                CalculatePoints(50);
                                return;
                            }
                        }

                    }
                }
            }
        }
        else
        {
            illegalItemsList.Clear();
            legalItemsList.Clear();
        }
    }

   
    private void InteractingWithItem()
    {
        ///this function is supposed to detect whether the item clicked on is illegal or not. if player click on item and it is in fact illegal, item gets destroyed and players gains some points. if player clicks and item isn't illegal, he loses points and items remains.

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Items"));
            if (hit.collider != null && bagMovement.currentPositionIndex == bagMovement.bagPositions.Count - 1)
            {
                if (hit.collider.gameObject.GetComponent<ItemType>() != null)
                {
                    if (hit.collider.gameObject.GetComponent<ItemType>().illegalItem == true)
                    {
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
        scoreText.text = totalScore.ToString();
        if (Input.GetMouseButtonDown(0))
        {
            InteractingWithItem();
        }
    }
}
