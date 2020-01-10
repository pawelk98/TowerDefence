using UnityEngine;
using UnityEngine.Networking;

public class spawner : NetworkBehaviour
{
    public GameObject healthBar;
    GameObject mobsOnRoad;
    private Vector3 startTopLeft = new Vector3(-3.48f, 1.22f, 0.0f);
    private Vector3 startTopRight = new Vector3(7.97f, 1.22f, 0.0f);
    private Vector3 startMidLeft = new Vector3(-3.48f, -1.3f, 0.0f);
    private Vector3 startMidRight = new Vector3(7.97f, -1.3f, 0.0f);
    private Vector3 startBotLeft = new Vector3(-3.48f, -3.75f, 0.0f);
    private Vector3 startBotRight = new Vector3(7.97f, -3.75f, 0.0f);
    private Vector3 startHidden = new Vector3(-10.0f, 0.0f, 0.0f);

    public GameObject[] mobList;

    void Start()
    {
        mobsOnRoad = GameObject.Find("Mobs");
    }

    void Update()
    {
        
    }

    [Command]
    public void CmdSpawnMob(playerScript.Side side, playerScript.Lane lane, int mobId)
    {
        GameObject spawnedMob = Instantiate(mobList[mobId], startHidden, Quaternion.identity);
        GameObject spawnedMobHealthBar = Instantiate(healthBar, startHidden, Quaternion.identity);
        NetworkServer.Spawn(spawnedMob);
        NetworkServer.Spawn(spawnedMobHealthBar);

        RpcSetSpawnedMob(side, lane, spawnedMob, spawnedMobHealthBar);
    }

    [ClientRpc]
    void RpcSetSpawnedMob(playerScript.Side side, playerScript.Lane lane, GameObject spawnedMob, GameObject spawnedMobHealthBar)
    {
        //dodajemy pasek hp do moba
        spawnedMob.GetComponent<mobStats>().addHealthBar(spawnedMobHealthBar);

        //skalujemy utworzonego moba
        spawnedMob.GetComponent<Transform>().localScale += new Vector3(-1, -1, 0);

        //ustawiamy stronę, w którą patrzy mob
        if(side == playerScript.Side.left)
        {
            spawnedMob.GetComponent<mobStats>().mobFacingRight = true;
        }
        else
        {
            spawnedMob.GetComponent<mobStats>().mobFacingRight = false;
        }

        //ustawiamy pozycję moba i przypisujemy do odpowiedniej linii
        if (lane == playerScript.Lane.top && side == playerScript.Side.left)
        {
            spawnedMob.GetComponent<mobStats>().positionX = startTopLeft.x;
            spawnedMob.GetComponent<mobStats>().positionY = startTopLeft.y;
            mobsOnRoad.GetComponent<mobsOnRoad>().roadTopLeft.Add(spawnedMob);
        }
        else if (lane == playerScript.Lane.top && side == playerScript.Side.right)
        {
            spawnedMob.GetComponent<mobStats>().positionX = startTopRight.x;
            spawnedMob.GetComponent<mobStats>().positionY = startTopRight.y;
            mobsOnRoad.GetComponent<mobsOnRoad>().roadTopRight.Add(spawnedMob);
        }
        else if (lane == playerScript.Lane.mid && side == playerScript.Side.left)
        {
            spawnedMob.GetComponent<mobStats>().positionX = startMidLeft.x;
            spawnedMob.GetComponent<mobStats>().positionY = startMidLeft.y;
            mobsOnRoad.GetComponent<mobsOnRoad>().roadMidLeft.Add(spawnedMob);
        }
        else if (lane == playerScript.Lane.mid && side == playerScript.Side.right)
        {
            spawnedMob.GetComponent<mobStats>().positionX = startMidRight.x;
            spawnedMob.GetComponent<mobStats>().positionY = startMidRight.y;
            mobsOnRoad.GetComponent<mobsOnRoad>().roadMidRight.Add(spawnedMob);
        }
        else if (lane == playerScript.Lane.bot && side == playerScript.Side.left)
        {
            spawnedMob.GetComponent<mobStats>().positionX = startBotLeft.x;
            spawnedMob.GetComponent<mobStats>().positionY = startBotLeft.y;
            mobsOnRoad.GetComponent<mobsOnRoad>().roadBotLeft.Add(spawnedMob);
        }
        else if (lane == playerScript.Lane.bot && side == playerScript.Side.right)
        {
            spawnedMob.GetComponent<mobStats>().positionX = startBotRight.x;
            spawnedMob.GetComponent<mobStats>().positionY = startBotRight.y;
            mobsOnRoad.GetComponent<mobsOnRoad>().roadBotRight.Add(spawnedMob);
        }        
    }
}
