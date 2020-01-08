using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchControl : MonoBehaviour
{
    public Collider2D[] clickableObj;
    public GameObject mobSpawner;
    public GameObject gameBoard;
    bool isPicked = false;
    RaycastHit2D hit;
    RaycastHit2D release;
    Vector2 originPos;
    Vector2 mousePos;
    int layerMask;

    void Start()
    {
        layerMask = LayerMask.GetMask("Roads");
    }
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     //odczytanie pozycji myszy

        if (Input.GetMouseButtonUp(0))  //po wypuszczeniu przycisku / zakończeniu przeciągania
        {
            if (isPicked == true)
            {
                //sprawdzenie pozycji myszy przy wypuszczaniu przycisku
                release = Physics2D.Raycast(mousePos, Vector2.zero, 10, layerMask);     

                if (release.collider != null)
                {
                    //sprawdzenie czy wystarczy many
                    if (hit.collider.gameObject.GetComponent<cardStats>().manaCost <= gameBoard.GetComponent<gameMana>().mana)  
                                            {
                        //zmniejszamy ilość many
                        gameBoard.GetComponent<gameMana>().mana -= hit.collider.gameObject.GetComponent<cardStats>().manaCost;

                        //sprawdzamy czy kartę wypuszczono na planszy
                        if (hit.collider.transform)

                        //sprawdzamy linię
                        if (release.collider.name[4] == 'T')
                        {
                            mobSpawner.GetComponent<mobSpawner>().lane = global::mobSpawner.Lane.top;
                        }
                        else if (release.collider.name[4] == 'M')
                        {
                            mobSpawner.GetComponent<mobSpawner>().lane = global::mobSpawner.Lane.mid;
                        }
                        else if (release.collider.name[4] == 'B')
                        {
                            mobSpawner.GetComponent<mobSpawner>().lane = global::mobSpawner.Lane.bot;
                        }

                        //sprawdzamy stronę
                        if (mousePos.x < 2.225)
                        {
                            mobSpawner.GetComponent<mobSpawner>().side = global::mobSpawner.Side.left;
                        }
                        else
                        {
                            mobSpawner.GetComponent<mobSpawner>().side = global::mobSpawner.Side.right;
                        }

                        // ustalamy id moba
                        mobSpawner.GetComponent<mobSpawner>().mobID = hit.collider.gameObject.GetComponent<cardStats>().mobSpawnID;

                        // potwierdzamy
                        mobSpawner.GetComponent<mobSpawner>().spawnMob();

                        // niszczymy karte
                        Destroy(hit.collider.gameObject);
                    }

                }
                hit.transform.position = originPos; //jeżeli nie uzyta na linii wraca na swoje miejsce
                isPicked = false;   //zakończono przeciąganie
            }
        }

        if (Input.GetMouseButtonDown(0))    //sprawdzenie pozycji kliknietego obiektu
        {
            hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "Card")     //jeżeli kliknięto kartę
            {
                originPos = hit.collider.transform.position;
                isPicked = true;    //zaczęto przeciąganie
            }

            else if (hit.collider != null && hit.collider.tag == "Reroll"       //jeżeli klinknięto reroll
            && gameBoard.GetComponent<gameMana>().mana >= gameBoard.GetComponent<gameMana>().rerollCost)
            {
                for (int i = 0; i < 4; i++)
                {
                    Destroy(GameObject.FindGameObjectWithTag("ScriptsObject").GetComponent<cards>().cardsOnBoard[i]);
                }
                gameBoard.GetComponent<gameMana>().mana -= gameBoard.GetComponent<gameMana>().rerollCost;
            }
        }

        if (isPicked == true)   //jeżeli obiekt został kliknięty
        {
            hit.collider.transform.position = mousePos;
        }
    }
}
