using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobsOnRoad : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> roadTopLeft = new List<GameObject>();
    public List<GameObject> roadMidLeft = new List<GameObject>();
    public List<GameObject> roadBotLeft = new List<GameObject>();
    public List<GameObject> roadTopRight = new List<GameObject>();
    public List<GameObject> roadMidRight = new List<GameObject>();
    public List<GameObject> roadBotRight = new List<GameObject>();

    public GameObject castleLeft;
    public GameObject castleRight;

    private float leftMaxDestination = -3.48f;
    private float rightMaxDestination = 7.97f;

    void Start()
    {

    }

    void deleteDead(List<GameObject> road)
    {
        for (int i = 0; i < road.Count; i++)
        {
            if (road[i] == null)
            {
                road.RemoveAt(i);
                i--;
            }
        }
    }

    void fixDistanceBehindCastle(List<GameObject> road)
    {
        for (int i = 0; i < road.Count; i++)
        {
            if (road[i].GetComponent<mobStats>().destinationPositionX < leftMaxDestination)
            {
                road[i].GetComponent<mobStats>().destinationPositionX = leftMaxDestination;
            }
            else if (road[i].GetComponent<mobStats>().destinationPositionX > rightMaxDestination)
            {
                road[i].GetComponent<mobStats>().destinationPositionX = rightMaxDestination;
            }
           
        }
    }

    void dontBlockMovement(List<GameObject> road)
    {
        for (int i = 0; i < road.Count; i++)
        {
            if (i == 0)
            {
                road[0].GetComponent<mobStats>().allyInFrontWalking = false;
            }
            else
            {
                if (road[i - 1].GetComponent<mobStats>().mobState == mobStats.state.walk)
                {
                    road[i].GetComponent<mobStats>().allyInFrontWalking = true;
                }
                else
                {
                    road[i].GetComponent<mobStats>().allyInFrontWalking = false;
                }
            }
        }
    }

    void destinationPositionCastle(List<GameObject> road, bool facingRight)
    {
        for (int i = 0; i < road.Count; i++)
        {
            if (facingRight)
            {
                road[i].GetComponent<mobStats>().destinationPositionX = rightMaxDestination - road[i].GetComponent<mobStats>().attackRange;
            }
            else if (!facingRight)
            {
                road[i].GetComponent<mobStats>().destinationPositionX = leftMaxDestination + road[i].GetComponent<mobStats>().attackRange;
            }
            road[i].GetComponent<mobStats>().freezeMob = false;
        }
    }

    void destinationPositionBehindAlly(List<GameObject> road, bool facingRight)
    {
        for (int i = road.Count - 1; i > 0; i--)
        {
            if (facingRight)
            {
                road[i].GetComponent<mobStats>().destinationPositionX = road[i - 1].GetComponent<mobStats>().positionX - road[i].GetComponent<mobStats>().behindMobRange;
            }
            else if (!facingRight)
            {
                road[i].GetComponent<mobStats>().destinationPositionX = road[i - 1].GetComponent<mobStats>().positionX + road[i].GetComponent<mobStats>().behindMobRange;
            }
        }
    }

    void destinationPositionEnemies(List<GameObject> roadLeft, List<GameObject> roadRight)
    {
        if (roadLeft.Count > 0 && roadRight.Count > 0)
        {
            roadLeft[0].GetComponent<mobStats>().destinationPositionX = roadRight[0].GetComponent<mobStats>().positionX - roadLeft[0].GetComponent<mobStats>().attackRange;
            roadRight[0].GetComponent<mobStats>().destinationPositionX = roadLeft[0].GetComponent<mobStats>().positionX + roadRight[0].GetComponent<mobStats>().attackRange;
        }

    }

    bool isApproximately(float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    void attack(List<GameObject> roadLeft, List<GameObject> roadRight)
    {
        // Bicie sie mobow
        if (roadLeft.Count > 0 && roadRight.Count > 0)
        {
            if (roadLeft[0].GetComponent<mobStats>().freezeMob == false
                        && roadLeft[0].GetComponent<mobStats>().destinationPositionX <= roadLeft[0].GetComponent<mobStats>().positionX + 0.001f
                        && roadLeft[0].GetComponent<mobStats>().mobState == mobStats.state.stand)
            {
                roadLeft[0].GetComponent<mobStats>().mobState = mobStats.state.attack;
                roadRight[0].GetComponent<mobStats>().health -= roadLeft[0].GetComponent<mobStats>().damage;
            }

            if (roadRight[0].GetComponent<mobStats>().freezeMob == false
                && roadRight[0].GetComponent<mobStats>().destinationPositionX >= roadRight[0].GetComponent<mobStats>().positionX - 0.001f
                && roadRight[0].GetComponent<mobStats>().mobState == mobStats.state.stand)
            {
                roadRight[0].GetComponent<mobStats>().mobState = mobStats.state.attack;
                roadLeft[0].GetComponent<mobStats>().health -= roadRight[0].GetComponent<mobStats>().damage;
            }
        }
        // Bicie zamku
        else if (roadLeft.Count > 0)
        {
            if (roadLeft[0].GetComponent<mobStats>().freezeMob == false
                        && roadLeft[0].GetComponent<mobStats>().destinationPositionX <= roadLeft[0].GetComponent<mobStats>().positionX + 0.001f
                        && roadLeft[0].GetComponent<mobStats>().mobState == mobStats.state.stand)
            {
                roadLeft[0].GetComponent<mobStats>().mobState = mobStats.state.attack;
                castleRight.GetComponent<castle>().health -= roadLeft[0].GetComponent<mobStats>().damage;
            }
        }
        else if (roadRight.Count > 0)
        {
            if (roadRight[0].GetComponent<mobStats>().freezeMob == false
                && roadRight[0].GetComponent<mobStats>().destinationPositionX >= roadRight[0].GetComponent<mobStats>().positionX - 0.001f
                && roadRight[0].GetComponent<mobStats>().mobState == mobStats.state.stand)
            {
                roadRight[0].GetComponent<mobStats>().mobState = mobStats.state.attack;
                castleLeft.GetComponent<castle>().health -= roadRight[0].GetComponent<mobStats>().damage;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        deleteDead(roadTopLeft);
        deleteDead(roadTopRight);
        deleteDead(roadMidLeft);
        deleteDead(roadMidRight);
        deleteDead(roadBotLeft);
        deleteDead(roadBotRight);


        destinationPositionCastle(roadTopLeft, true);
        destinationPositionCastle(roadTopRight, false);
        destinationPositionCastle(roadMidLeft, true);
        destinationPositionCastle(roadMidRight, false);
        destinationPositionCastle(roadBotLeft, true);
        destinationPositionCastle(roadBotRight, false);

        destinationPositionBehindAlly(roadTopLeft, true);
        destinationPositionBehindAlly(roadTopRight, false);
        destinationPositionBehindAlly(roadMidLeft, true);
        destinationPositionBehindAlly(roadMidRight, false);
        destinationPositionBehindAlly(roadBotLeft, true);
        destinationPositionBehindAlly(roadBotRight, false);

        destinationPositionEnemies(roadTopLeft, roadTopRight);
        destinationPositionEnemies(roadMidLeft, roadMidRight);
        destinationPositionEnemies(roadBotLeft, roadBotRight);

        attack(roadTopLeft, roadTopRight);
        attack(roadMidLeft, roadMidRight);
        attack(roadBotLeft, roadBotRight);

        dontBlockMovement(roadTopLeft);
        dontBlockMovement(roadTopRight);
        dontBlockMovement(roadMidLeft);
        dontBlockMovement(roadMidRight);
        dontBlockMovement(roadBotLeft);
        dontBlockMovement(roadBotRight);




        //Uwaga: To powinno byc na koncu
        fixDistanceBehindCastle(roadTopLeft);
        fixDistanceBehindCastle(roadTopRight);
        fixDistanceBehindCastle(roadMidLeft);
        fixDistanceBehindCastle(roadMidRight);
        fixDistanceBehindCastle(roadBotLeft);
        fixDistanceBehindCastle(roadBotRight);
        //Uwaga: Koniec
    }
}
