using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using System;

public class GameManager : NetworkManager
{

    [SerializeField]
    private uint roomSize = 2;
    private int connectionCounter;
    public static bool isConnected = false;
    //Rzeczy do LobbyMenu
    private NetworkManager manager;
    private string roomname;
    
    public static GameManager Instance { set; get; }
 
    private void Start()
    {
        isConnected = false;
        connectionCounter = 0;
        manager = NetworkManager.singleton;
        if (manager.matchMaker == null)
        {
            manager.StartMatchMaker();
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void GoBackToMainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    public void CreateRoom()
    {
        roomname = GameObject.Find("RoomNameInput").transform.Find("Text").GetComponent<Text>().text;
        if (roomname != "" && roomname != null)
        {
        Debug.Log("Creating room: " + roomname + " with room for " + roomSize + "players.");
            manager.matchMaker.CreateMatch(roomname,roomSize,true, "", "", "",0,0,manager.OnMatchCreate);
        }

        
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            StartCoroutine(SetupLobbySceneButtons());
        }
      
    }

   

    public IEnumerator SetupLobbySceneButtons()
    {
        
           yield return new WaitForSeconds(0.3f);
        GameObject.Find("BackToMainMenuButton").GetComponent<Button>().onClick.AddListener(GoBackToMainMenuButton);
        GameObject.Find("CreateRoom").GetComponent<Button>().onClick.AddListener(CreateRoom);
    }
   
    public void GoToGame()
    {
        manager.ServerChangeScene("game");
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        connectionCounter++;
        if (connectionCounter == 2)
        {
            isConnected = true;
            manager.ServerChangeScene("game");
        }
    }
}
