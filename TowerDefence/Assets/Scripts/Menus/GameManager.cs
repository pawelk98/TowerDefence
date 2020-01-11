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
    private uint roomSize = 4;
    //Rzeczy do LobbyMenu
    private NetworkManager manager;
    private string roomname;
    
    public static GameManager Instance { set; get; }
 
    private void Start()
    {
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

 
    /*private void OnLevelWasLoaded(int level)
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
   */

}
