using UnityEngine;
using UnityEngine.Networking;

public class playerScript : NetworkBehaviour
{
    public enum Side { left, right };
    public enum Lane { top, mid, bot }

    GameObject win;
    GameObject lose;

    [SyncVar]
    public Side playerSide;

    GameObject leftCastle;
    GameObject rightCastle;
    public Vector3 posOnScreen = new Vector3(2.13f, -0.32f, 0);
    public Vector3 posOutOfScreen = new Vector3(1f, 8.98f, 0);

    void Start()
    {
        //ustawienie strony hosta na lewo a clienta na prawo
        if (isLocalPlayer)
        {
            leftCastle = GameObject.Find("GameCastleLeft");
            rightCastle = GameObject.Find("GameCastleRight");

            win = GameObject.Find("Wygrana");
            lose = GameObject.Find("Przegrana");

            win.SetActive(false);
            lose.SetActive(false);

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
                    setEndGameScreen(false);
                }
                else
                {
                    setEndGameScreen(true);
                }
            }
            else if (rightCastle.GetComponent<castle>().health <= 0)
            {
                if (playerSide == Side.left)
                {
                    setEndGameScreen(true);

                }
                else
                {
                    setEndGameScreen(false);
                }
            }
        }
    }

    void setEndGameScreen(bool gameWon)
    {
        if (gameWon)
        {
            win.SetActive(true);
            win.transform.position = posOnScreen;
        }
        else
        {
            lose.SetActive(true);
            lose.transform.position = posOnScreen;
        }
    }
}
