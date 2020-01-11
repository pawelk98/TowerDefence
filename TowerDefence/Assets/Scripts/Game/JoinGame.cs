using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System;

public class JoinGame : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    private NetworkManager networkManager;
    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matches == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();

    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        status.text = "Loading";

    }

    void OnMatchList(bool succeess,string extendedInfo ,List<MatchInfoSnapshot> matches)
    {
        status.text = "";
        if(matches == null)
        {
            status.text = "couldn't get room list.";
            return;
        }

        ClearRoomList();
        foreach(MatchInfoSnapshot match in matches)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if(_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }
            roomList.Add(_roomListItemGO);
        }

        if (roomList.Count == 0)
        {
            status.text = "No rooms at the moment";
        }
    }

    private void ClearRoomList()
    {
       for(int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "",0,0,networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Joining...";
    }


    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            StartCoroutine(SetupLobbySceneButtons());
        }

    }



    public IEnumerator SetupLobbySceneButtons()
    {

        yield return new WaitForSeconds(0.3f);
        GameObject.Find("RefreshButton").GetComponent<Button>().onClick.AddListener(RefreshRoomList);
    }
}

