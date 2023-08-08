using UnityEngine;

public class ServerSpawnManager : MonoBehaviour
{
    public int GetRandomPrefabIndex()
    {
        return Random.Range(0, 12);
    }
}
