using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelingUp : MonoBehaviour
{
    float levelTimer= 360;
    [SerializeField] float requiredPointsToPass;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;

    void Start()
    {
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelTimer > 0) levelTimer -= 1 * .007f;

        timeText.text = levelTimer.ToString("F0");

        if (levelTimer <= 0)
        {
            if (Client.totalScore < requiredPointsToPass) loseScreen.SetActive(true);
            else winScreen.SetActive(true);
        }

    }
}
