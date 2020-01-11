using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class GameManager : NetworkManager
{
   
    //Rzeczy do LobbyMenu

    public static GameManager Instance { set; get; }

    private void Start()
    {

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void HostGameButton()
    {
        StartupHost();
    }

    public void GoBackToMainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    //NetworkManager 


    public void StartupHost()
    {
        NetworkServer.Reset();
        SetPort();
        singleton.StartHost();
        
    }

    public void JoinGame()
    {
        SetIpAddress();
        SetPort();
        singleton.StartClient();
    }

    private void SetIpAddress()
    {
        string IpAddress = GameObject.Find("HostInput").transform.Find("Text").GetComponent<Text>().text;
       singleton.networkAddress = IpAddress;
    }

    public void SetPort()
    {
        singleton.networkPort= 7777;
    }
    private void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            StartCoroutine(SetupLobbySceneButtons());
        }
        else if(level ==2)
        {
            SetupOtherSceneButtons();
        }
    }

    void SetupOtherSceneButtons()
    {

        GameObject.Find("DcButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("DcButton").GetComponent<Button>().onClick.AddListener(singleton.StopHost);

    }

    public IEnumerator SetupLobbySceneButtons()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject.Find("HostButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("HostButton").GetComponent<Button>().onClick.AddListener(HostGameButton);

      
        GameObject.Find("ConnectToGameButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ConnectToGameButton").GetComponent<Button>().onClick.AddListener(JoinGame);
       
      
        GameObject.Find("BackToMainMenuButton").GetComponent<Button>().onClick.AddListener(GoBackToMainMenuButton);
       
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("Succesfully Conected to the server");
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
      
        Debug.Log("A client connected to the server: " + conn);
    }
   

}
