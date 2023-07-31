using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkingServerUI : MonoBehaviour
{
    [SerializeField] Server server;
    [SerializeField] TextMeshProUGUI clientConnectionText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clientConnectionText.text= server.clients.Count + "clients connected";
    }
}
