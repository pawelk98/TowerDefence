using UnityEngine;
using UnityEngine.Networking;

public class playerScript : NetworkBehaviour
{
    public enum Side { left, right };
    public enum Lane { top, mid, bot }

    [SyncVar]
    public Side playerSide;

    GameObject leftCastle;
    GameObject rightCastle;

    void Start()
    {
        //ustawienie strony hosta na lewo a clienta na prawo
        if (isLocalPlayer)
        {
            leftCastle = GameObject.Find("GameCastleLeft");
            rightCastle = GameObject.Find("GameCastleRight");

            playerSide = Side.right;

            if (isServer)
            {
                playerSide = Side.left;
            }
        }

    }

    void Update()
    {
        if (isLocalPlayer)
        {
            //sprawdzenie czy ktoś wygrał
            if (leftCastle.GetComponent<castle>().health <= 0)
            {
                if (playerSide == Side.left)
                {
                    //EKRAN PRZEGRANEJ
                }
                else
                {
                    //EKRAN WYGRANEJ
                }
            }
            else if (rightCastle.GetComponent<castle>().health <= 0)
            {
                if (playerSide == Side.right)
                {
                    //EKRAN WYGRANEJ
                }
                else
                {
                    //EKRAN PRZEGRANEJ
                }
            }
        }
    }
}
