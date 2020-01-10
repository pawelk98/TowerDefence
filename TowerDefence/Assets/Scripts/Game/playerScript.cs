using UnityEngine;
using UnityEngine.Networking;

public class playerScript : NetworkBehaviour
{
    public enum Side { left, right };
    public enum Lane { top, mid, bot }

    [SyncVar]
    public Side playerSide;

    void Start()
    {
        //ustawienie strony hosta na lewo a clienta na prawo
        if (isLocalPlayer)
        {
            playerSide = Side.right;

            if (isServer)
            {
                playerSide = Side.left;
            }
        }

    }
}
