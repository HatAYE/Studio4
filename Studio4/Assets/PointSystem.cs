using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour
{
    [SerializeField] int scorePoints;
    [SerializeField] BagReset bagReset;
    [SerializeField] BagMovement bagMovement;
    [SerializeField] int itemPointValue;
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {

    }
    void GainPoints()
    {
        scorePoints += itemPointValue;
    }

    void LosePoints()
    {
        scorePoints -= 1 * itemPointValue;
    }

    public void ItemCheck()
    {
        ///this function will run a check on the type of items are in the bag. each item will have an index number ranging from 0 to 3. 0 being a safe item whilie 3 being high danger
        /// this function should loop items, access their points, determine how much points the player will gain or lose
        if (bagMovement.currentPositionIndex == 3)
        {
            for (int i = 0; i < bagReset.objectRandomizer.Count; i++)
            {
                Debug.Log(bagReset.objectRandomizer[i]);
                int itemValue;
                Transform child = bagReset.objectRandomizer[i].transform.GetChild(0);

                if (child.gameObject.GetComponent<ItemType>() != null)
                {
                    itemValue = child.gameObject.GetComponent<ItemType>().dangerValue;
                    itemPointValue = child.gameObject.GetComponent<ItemType>().dangerPoints;
                    if (itemValue > 0)
                    {
                        Debug.Log("dangerous items were found");
                        //LosePoints();
                    }
                    if (itemValue == 0)
                    {
                        Debug.Log("items were safe");
                        //GainPoints();
                    }
                    GainPoints();
                    Debug.Log("items value" + itemValue);
                }
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("points:" + scorePoints);
        scoreText.text = scorePoints.ToString();

    }
}
