using UnityEngine;
using UnityEngine.Networking;

public class connectionCounter : NetworkBehaviour
{
    [SyncVar]
    public bool hasConnected;
    public bool screenChanged;
    public GameObject waitingScreen;
    void Start()
    {
        screenChanged = false;

        if(isServer)
        {
            hasConnected = false;
        }
    }

    void Update()
    {
        if(hasConnected && !screenChanged)
        {
            waitingScreen.SetActive(false);
        }
    }

}
