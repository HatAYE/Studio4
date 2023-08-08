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

    void Start()
    {
        /*ObjectRandomizer[] bags = FindObjectsOfType<ObjectRandomizer>();
        foreach (var bag in bags)
        {
            RegisterBag(bag);
        }*/
    }

    public void RegisterBag(ObjectRandomizer randomizer)
    {
        registeredBags.Add(randomizer);
    }

    public void UnregisterBag(ObjectRandomizer randomizer)
    {
        registeredBags.Remove(randomizer);
    }

    // Method to receive the prefab index from the client and call the InstantiateItems method in each registered bag.
    public void ReceivePrefabIndex(int prefabIndex)
    {
        foreach (var bag in registeredBags)
        {
            bag.InstantiateItems(prefabIndex);
            Debug.Log("like a plastic bag");
        }
    }
}
