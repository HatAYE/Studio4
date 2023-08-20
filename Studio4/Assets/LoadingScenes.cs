using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScenes : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject connectionText;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ConnecttoServer()
    {
        Client.instance.Connect();
        
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
    }

    private void Start()
    {
        startScreen.SetActive(true);
        connectionText.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Client.instance.socket.Available > 0)
        {
            connectionText.SetActive(true);
        }
    }
}
