using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawnManager : MonoBehaviour
{
    public static ClientSpawnManager instance;
    public List<ObjectRandomizer> registeredBags = new List<ObjectRandomizer>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ReceivePrefabIndexes(List<int> prefabIndexes)
    {
        // Iterate through each registered bag and pass the prefab index
        for (int i = 0; i < registeredBags.Count; i++)
        {
            int prefabIndex = prefabIndexes[i % prefabIndexes.Count]; // Get the current prefab index
            registeredBags[i].InstantiateItems(prefabIndex);
            Debug.Log("like a plastic bag");
        }
    }

}
