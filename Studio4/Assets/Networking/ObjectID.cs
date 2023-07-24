
using UnityEngine;

public class ObjectID : MonoBehaviour
{
    public string ownerID;
    public string objectID;
    public bool BelongsToSelf()
    {
        return Client.instance.playerData.playerID == ownerID;
    }

    public void GenerateGameObjectIDToSelf()
    {
        objectID = Random.Range(0, 10).ToString();
    }
}
