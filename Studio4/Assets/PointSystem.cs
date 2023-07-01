using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    int scorePoints;
    void GainPoints(int pointWeight)
    {
        scorePoints += 1 * pointWeight;
    }

    void LosePoints(int pointWeight)
    {
        scorePoints -= 1 * pointWeight;
    }

    void ItemCheck()
    {
        ///this function will run a check on the type of items are in the bag
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
