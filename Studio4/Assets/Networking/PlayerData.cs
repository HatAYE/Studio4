using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string playerID {  get; private set; }
    public string playerName { get; private set;}
    public PlayerData(string playerID, string playerName)
    {
        this.playerID = playerID;
        this.playerName = playerName;
    }

}
