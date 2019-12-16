using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobSpawner : MonoBehaviour
{
    public enum Lane {top, mid, bot};
    public enum Side {right, left};


    public Lane lane;
    public Side side;
    public int mobID; 
    public bool confirm;

    public GameObject[] mobList;


    private Vector3 startTopLeft = new Vector3(-4.21f, 1.22f, 0.0f);
    private Vector3 startTopRight = new Vector3(9.0f, 1.22f, 0.0f);
    private Vector3 startMidLeft = new Vector3(-4.21f, -1.3f, 0.0f);
    private Vector3 startMidRight = new Vector3(9.0f, -1.3f, 0.0f);
    private Vector3 startBotLeft = new Vector3(-4.21f, -3.75f, 0.0f);
    private Vector3 startBotRight = new Vector3(9.0f, -3.75f, 0.0f);
    private Vector3 startHidden = new Vector3(-10.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(confirm)
        {
            spawnMob();
        }
    }

    public void spawnMob()
    {
        confirm = true;
        if (confirm)
        {
            confirm = false;


            // Tworzenie moba
            GameObject createdMob = Instantiate(mobList[mobID]);
            createdMob.GetComponent<Transform>().localScale += new Vector3(-1,-1,0);

            // Strona w jaka patrzy mob
            if (side == Side.left)
            {
                createdMob.GetComponent<mobStats>().mobFacingRight = true;
            }
            else if (side == Side.right)
            {
                createdMob.GetComponent<mobStats>().mobFacingRight = false;
            }

            // Chowanie moba
            createdMob.GetComponent<Transform>().position = new Vector3(startHidden.x, startHidden.y, startHidden.z);


            if (lane == Lane.top && side == Side.left)
            {
                createdMob.GetComponent<mobStats>().positionX = startTopLeft.x;
                createdMob.GetComponent<mobStats>().positionY = startTopLeft.y;
                gameObject.GetComponent<mobsOnRoad>().roadTopLeft.Add(createdMob);
            }
            else if (lane == Lane.top && side == Side.right)
            {
                createdMob.GetComponent<mobStats>().positionX = startTopRight.x;
                createdMob.GetComponent<mobStats>().positionY = startTopRight.y;
                gameObject.GetComponent<mobsOnRoad>().roadTopRight.Add(createdMob);
            }
            else if (lane == Lane.mid && side == Side.left)
            {
                createdMob.GetComponent<mobStats>().positionX = startMidLeft.x;
                createdMob.GetComponent<mobStats>().positionY = startMidLeft.y;
                gameObject.GetComponent<mobsOnRoad>().roadMidLeft.Add(createdMob);
            }
            else if (lane == Lane.mid && side == Side.right)
            {
                createdMob.GetComponent<mobStats>().positionX = startMidRight.x;
                createdMob.GetComponent<mobStats>().positionY = startMidRight.y;
                gameObject.GetComponent<mobsOnRoad>().roadMidRight.Add(createdMob);
            }
            else if (lane == Lane.bot && side == Side.left)
            {
                createdMob.GetComponent<mobStats>().positionX = startBotLeft.x;
                createdMob.GetComponent<mobStats>().positionY = startBotLeft.y;
                gameObject.GetComponent<mobsOnRoad>().roadBotLeft.Add(createdMob);
            }
            else if (lane == Lane.bot && side == Side.right)
            {
                createdMob.GetComponent<mobStats>().positionX = startBotRight.x;
                createdMob.GetComponent<mobStats>().positionY = startBotRight.y;
                gameObject.GetComponent<mobsOnRoad>().roadBotRight.Add(createdMob);
            }


            createdMob = null;
        }
    }

}
