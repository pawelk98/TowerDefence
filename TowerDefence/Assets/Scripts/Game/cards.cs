using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cards : MonoBehaviour
{
    public Vector2[] cardPos =
    {
        new Vector2(-8.39f, 2.12f),
        new Vector2(-6.44f, 2.12f),
        new Vector2(-8.39f, 0.02f),
        new Vector2(-6.44f, 0.02f),
    };  //pozycja kart na boardzie
    public float cardScale = 0.28f; //skala kart na boardzie
    public GameObject[] card;   //tablica kart
    public GameObject[] cardsOnBoard = new GameObject[4];
    int[] cardChance; //tablica szans kart
    int cardChanceSum = 0;  //suma szans wszystkich kart
    bool[] isSlotEmpty = { true, true, true, true };    //czy w danym miejscu leży karta

    void Start()
    {
        cardChance = new int[card.Length];    //wsadzenie szans kart do tablicy
        for (int i = 0; i < card.Length; i++)
        {
            cardChance[i] = 11 - card[i].GetComponent<cardStats>().manaCost;
            cardChanceSum += cardChance[i];
        }
    }

    void Update()
    {
        for(int i = 0; i < 4; i++)
        {
            if(cardsOnBoard[i] == null)
            {
                isSlotEmpty[i] = true;
            }
        }

        if (card.Length > 0)
        {
            for (int i = 0; i < 4; i++) //sprawdzanie czy są wszystkie karty na stole
            {
                if (isSlotEmpty[i])
                {
                    nextCard(i);
                }
            }
        }
    }

    void nextCard(int pos)  //włożenie karty na brakujące miejsce
    {
        cardsOnBoard[pos] = Instantiate(card[randCard()], cardPos[pos], Quaternion.identity);
        cardsOnBoard[pos].transform.localScale = new Vector3(cardScale, cardScale, cardScale);
        isSlotEmpty[pos] = false;
    }

    int randCard()  //generowanie indeksu wylosowanej karty
    {
        int searchSum = cardChance[0];
        int result = 0;
        int rand = Random.Range(0, cardChanceSum);

        while (searchSum < rand && result < card.Length-1)
        {
            searchSum += cardChance[result];
            result++;
        }

        return result;
    }
}