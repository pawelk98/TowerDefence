using UnityEngine;
using UnityEngine.Networking;

public class connectionCounter : NetworkBehaviour
{
    [SyncVar]
    public bool hasConnected;
    void Start()
    {
        if(isServer)
        {
            hasConnected = false;
        }
    }

}
