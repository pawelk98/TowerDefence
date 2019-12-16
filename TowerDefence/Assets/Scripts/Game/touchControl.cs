using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchControl : MonoBehaviour
{
    public Collider2D[] clickableObj;
    public GameObject mobSpawner;
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
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))  //po wypuszczeniu przycisku
        {
            if (isPicked == true)
            {
                release = Physics2D.Raycast(mousePos, Vector2.zero, 10, layerMask);
                if (release.collider != null)
                {
                    // Debug.Log("Spawn mob on: " + release.collider.gameObject.name);
                    //TUTAJ SPAWNOWANKO MOBKA
                    if (hit.collider.gameObject.GetComponent<cardStats>().manaCost <= GameObject.FindGameObjectWithTag("gameBoard").GetComponent<gameMana>().mana)
                    // jesli jest wystarczajaca mana
                    {
                        // odejmujemy mane
                        GameObject.FindGameObjectWithTag("gameBoard").GetComponent<gameMana>().mana -= hit.collider.gameObject.GetComponent<cardStats>().manaCost;
                        // ustawiamy linie
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

                        // ustalamy strone
                        if (hit.collider.transform.position.x < 0)
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
                isPicked = false;
            }
        }

        if (Input.GetMouseButtonDown(0))    //sprawdzenie pozycji kliknietego obiektu
        {
            hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Card")
            {
                originPos = hit.collider.transform.position;
                isPicked = true;
            }
            else if (hit.collider != null && hit.collider.tag == "Reroll"
            && GameObject.FindGameObjectWithTag("gameBoard").GetComponent<gameMana>().mana >= 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    Destroy(GameObject.FindGameObjectWithTag("ScriptsObject").GetComponent<cards>().cardsOnBoard[i]);
                }
                GameObject.FindGameObjectWithTag("gameBoard").GetComponent<gameMana>().mana -= 5;
            }
        }

        if (isPicked == true)   //jeżeli obiekt został kliknięty
        {
            hit.collider.transform.position = mousePos;
        }
    }
}
