using UnityEngine;
using UnityEngine.Networking;

public class touchControl : NetworkBehaviour
{
    GameObject mobSpawner;
    GameObject gameBoard;
    GameObject scriptsObject;
    bool isPicked;
    int layerMaskRoads, layerMaskCards;
    RaycastHit2D hit, release;
    Vector2 mousePos, originPos;
    playerScript.Side side;
    playerScript.Lane lane;
    int mobID;



    void Start()
    {
        scriptsObject = GameObject.Find("ScriptObject");

        //sprawdzamy czy to odpowiedni gracz
        if (isLocalPlayer)
        {
            isPicked = false;

            layerMaskRoads = LayerMask.GetMask("Roads");
            layerMaskCards = LayerMask.GetMask("Cards");

            gameBoard = GameObject.Find("GameBoard");

            if(!isServer)
            {
                CmdSetConnected();
            }
        }
    }


    [Command]
    void CmdSetConnected()
    {
        scriptsObject.GetComponent<connectionCounter>().hasConnected = true;
    }

    void Update()
    {
        //sprawdzamy czy to odpowiedni gracz
        if (!isLocalPlayer)
        {
            return;
        }


        //odczytuję pozycję myszy
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //wciśnięto przycisk myszy
        if (Input.GetMouseButtonDown(0))
        {
            //sprawdzamy czy klient już się połączył
            if (scriptsObject.GetComponent<connectionCounter>().hasConnected)
            {
                //sprawdzenie na co kliknięto
                hit = Physics2D.Raycast(mousePos, Vector2.zero, 10, layerMaskCards);

                //jeżeli kliknięto na kartę
                //zapamiętanie początkowej pozycji (do cofnięcia po nieudanym przeciąganiu)
                //ustawienie flagi, że zaczęto przeciąganie

                if (hit.collider != null && hit.collider.tag == "Card")
                {
                    originPos = hit.collider.transform.position;
                    isPicked = true;
                }
                //jeżeli kliknięto na reroll oraz wystarczy many na jego użycie
                //usuwamy karty ze stołu i zmniejszamy ilość many
                else if (hit.collider != null && hit.collider.tag == "Reroll" &&
                gameBoard.GetComponent<gameMana>().mana >= gameBoard.GetComponent<gameMana>().rerollCost)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Destroy(scriptsObject.GetComponent<cards>().cardsOnBoard[i]);
                    }

                    gameBoard.GetComponent<gameMana>().mana -= gameBoard.GetComponent<gameMana>().rerollCost;
                }
            }
        }


        //trwa przeciąganie, więc przesuwamy przeciągany obiekt do pozycji myszy
        if (isPicked == true)
        {
            hit.collider.transform.position = mousePos;
        }


        //zwolniono przycisk myszy
        if (Input.GetMouseButtonUp(0))
        {
            //jeżeli przeciągano kartę
            if (isPicked == true)
            {
                //sprawdzenie na co została upuszczona
                release = Physics2D.Raycast(mousePos, Vector2.zero, 10, layerMaskRoads);

                if (release.collider != null)
                {
                    //sprawdzenie czy wystarczy many na użycie karty
                    //zmniejszamy ilość many
                    if (hit.collider.gameObject.GetComponent<cardStats>().manaCost <= gameBoard.GetComponent<gameMana>().mana)
                    {
                        gameBoard.GetComponent<gameMana>().mana -= hit.collider.gameObject.GetComponent<cardStats>().manaCost;

                        //sprawdzamy czy kartę wypuszczono na którejś linii
                        switch (release.collider.name[4])
                        {
                            case 'T':
                                lane = playerScript.Lane.top;
                                break;

                            case 'M':
                                lane = playerScript.Lane.mid;
                                break;

                            case 'B':
                                lane = playerScript.Lane.bot;
                                break;
                        }

                        //bierzemy id moba który zostanie zespawnowany z karty
                        //spawnujemy i niszczymy kartę
                        mobID = hit.collider.gameObject.GetComponent<cardStats>().mobSpawnID;
                        side = GetComponent<playerScript>().playerSide;

                        //spawnujemy moba
                        GetComponent<spawner>().CmdSpawnMob(side, lane, mobID);

                        Destroy(hit.collider.gameObject);
                    }
                    //jeżeli za mało many, karta wraca na swoje miejsce
                    else
                    {
                        hit.transform.position = originPos;
                    }

                }
                //jeżeli karta nie została upuszczona na linię wraca na swoje miejsce
                else
                {
                    hit.transform.position = originPos;
                }

                //zakończono przeciąganie
                isPicked = false;
            }
        }
    }
}
