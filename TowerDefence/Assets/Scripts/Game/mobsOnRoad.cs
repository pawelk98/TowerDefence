using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobsOnRoad : MonoBehaviour
{

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

    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator delayDamage(GameObject target, GameObject attacker)
    {
        if (attacker != null)
        {
            int damage = attacker.GetComponent<mobStats>().damage;
            float time = attacker.GetComponent<mobStats>().attackDuration;

            yield return new WaitForSeconds(time);

            if (target != null)
                target.GetComponent<mobStats>().health -= damage;
        }
    }

    IEnumerator delayDamageCastle(GameObject target, GameObject attacker)
    {
        if (attacker != null)
        {
            int damage = attacker.GetComponent<mobStats>().damage;
            float time = attacker.GetComponent<mobStats>().attackDuration;
            yield return new WaitForSeconds(time);

            if (target != null)
                target.GetComponent<castle>().health -= damage;
        }
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
                StartCoroutine(delayDamage(roadRight[0], roadLeft[0]));
            }

            if (roadRight[0].GetComponent<mobStats>().freezeMob == false
                && roadRight[0].GetComponent<mobStats>().destinationPositionX >= roadRight[0].GetComponent<mobStats>().positionX - 0.001f
                && roadRight[0].GetComponent<mobStats>().mobState == mobStats.state.stand)
            {
                roadRight[0].GetComponent<mobStats>().mobState = mobStats.state.attack;
                StartCoroutine(delayDamage(roadLeft[0], roadRight[0]));
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
                StartCoroutine(delayDamageCastle(castleRight, roadLeft[0]));
            }
        }
        else if (roadRight.Count > 0)
        {
            if (roadRight[0].GetComponent<mobStats>().freezeMob == false
                && roadRight[0].GetComponent<mobStats>().destinationPositionX >= roadRight[0].GetComponent<mobStats>().positionX - 0.001f
                && roadRight[0].GetComponent<mobStats>().mobState == mobStats.state.stand)
            {
                roadRight[0].GetComponent<mobStats>().mobState = mobStats.state.attack;
                StartCoroutine(delayDamageCastle(castleLeft, roadRight[0]));
            }
        }


    }

    void attackBehindMob(List<GameObject> roadLeft, List<GameObject> roadRight)
    {
        // Z lewej strony bije moba
        if (roadLeft.Count >= 2 && roadRight.Count >= 1)
        {
            for (int i = 1; i < roadLeft.Count; i++)
            {
                if (roadLeft[i].GetComponent<mobStats>().freezeMob == true) continue;
                if (roadLeft[i].GetComponent<mobStats>().positionX + roadLeft[i].GetComponent<mobStats>().attackRange >= roadRight[0].GetComponent<mobStats>().positionX &&
                roadLeft[i].GetComponent<mobStats>().mobState == mobStats.state.stand)
                {
                    roadLeft[i].GetComponent<mobStats>().mobState = mobStats.state.attack;
                    StartCoroutine(delayDamage(roadRight[0], roadLeft[i]));
                }
            }
        }
        // z lewej strony bije zamek
        else if (roadLeft.Count >= 2 && roadRight.Count == 0)
        {
            for (int i = 1; i < roadLeft.Count; i++)
            {
                if (roadLeft[i].GetComponent<mobStats>().freezeMob == true) continue;
                if (roadLeft[i].GetComponent<mobStats>().positionX + roadLeft[i].GetComponent<mobStats>().attackRange >= rightMaxDestination &&
                roadLeft[i].GetComponent<mobStats>().mobState == mobStats.state.stand)
                {
                    roadLeft[i].GetComponent<mobStats>().mobState = mobStats.state.attack;
                    StartCoroutine(delayDamageCastle(castleRight, roadLeft[i]));
                }
            }
        }
        // Z prawej strony bije moba
        if (roadRight.Count >= 2 && roadLeft.Count >= 1)
        {
            for (int i = 1; i < roadRight.Count; i++)
            {
                if (roadRight[i].GetComponent<mobStats>().freezeMob == true) continue;
                if (roadRight[i].GetComponent<mobStats>().positionX - roadRight[i].GetComponent<mobStats>().attackRange <= roadLeft[0].GetComponent<mobStats>().positionX &&
                roadRight[i].GetComponent<mobStats>().mobState == mobStats.state.stand)
                {
                    roadRight[i].GetComponent<mobStats>().mobState = mobStats.state.attack;
                    StartCoroutine(delayDamage(roadLeft[0], roadRight[i]));
                }
            }
        }
        // z prawej strony bije zamek
        else if (roadRight.Count >= 2 && roadLeft.Count == 0)
        {
            for (int i = 1; i < roadRight.Count; i++)
            {
                if (roadRight[i].GetComponent<mobStats>().freezeMob == true) continue;
                if (roadRight[i].GetComponent<mobStats>().positionX - roadRight[i].GetComponent<mobStats>().attackRange <= leftMaxDestination &&
                roadRight[i].GetComponent<mobStats>().mobState == mobStats.state.stand)
                {
                    roadRight[i].GetComponent<mobStats>().mobState = mobStats.state.attack;
                    StartCoroutine(delayDamageCastle(castleLeft, roadRight[i]));
                }
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

        attackBehindMob(roadTopLeft, roadTopRight);
        attackBehindMob(roadMidLeft, roadMidRight);
        attackBehindMob(roadBotLeft, roadBotRight);

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
