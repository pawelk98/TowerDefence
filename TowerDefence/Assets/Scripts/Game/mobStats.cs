using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobStats : MonoBehaviour
{
    public GameObject healthBar;
    public Vector2 healthBarOffset;
    public enum state { stand, walk, attack, death };

    public Animator animator;
    public int damage;
    public float health;
    public float speed;

    public state mobState;
    public bool mobFacingRight;
    public float attackRange;
    public bool freezeMob;

    public float positionYCorrection;

    [Header("Automatic")]
    public bool allyInFrontWalking;
    public float attackDuration;
    public float deathDuration;
    public float behindMobRange;
    public float positionX;
    public float positionY;
    public float realPositionY;
    public float destinationPositionX;

    private float attackDurationCount = 0.0f;
    private float deathDurationCount = 0.0f;

    private bool hasHealthBar = false;
    private GameObject createdHealthBar;
    private float maxHealth;





    // Start is called before the first frame update
    void Start()
    {
        
        attackDurationCount = 0.0f;
        deathDurationCount = 0.0f;
        // if(mobFacingRight) destinationPositionX = 20;
        // else destinationPositionX = -20;

        //gameObject.GetComponent<RectTransform>().pivot = new Vector2(gameObject.GetComponent<Renderer>().bounds.size.x / 2, gameObject.GetComponent<Renderer>().bounds.size.y);

        // Ustawienie dlugosci umierania i ataku
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name.Contains("attack"))
            {
                attackDuration = ac.animationClips[i].length;
            }
            if (ac.animationClips[i].name.Contains("death"))
            {
                deathDuration = ac.animationClips[i].length;
            }
        }

        if (mobFacingRight)
            createdHealthBar = Instantiate(healthBar, new Vector2(positionX + healthBarOffset.x, positionY + healthBarOffset.y), Quaternion.identity);
        else
            createdHealthBar = Instantiate(healthBar, new Vector2(positionX - healthBarOffset.x, positionY + healthBarOffset.y), Quaternion.identity);
        maxHealth = health;
    }


    void setHealthBar()
    {
        if (mobFacingRight)
            createdHealthBar.transform.position = new Vector2(positionX + healthBarOffset.x, positionY + healthBarOffset.y);
        else
            createdHealthBar.transform.position = new Vector2(positionX - healthBarOffset.x, positionY + healthBarOffset.y);

        Vector2 healthBarScale;

        if (health > 0)
            healthBarScale = new Vector2(health / maxHealth, 1);
        else
            healthBarScale = new Vector2(0, 1);

        createdHealthBar.transform.Find("Bar").localScale = healthBarScale;
    }

    // Update is called once per frame
    void Update()
    {
        setHealthBar();

        // Jesli mob ma sie ruszyć do tyłu to sie nie rusza
        if (mobFacingRight && destinationPositionX < positionX)
        {
            destinationPositionX = positionX;
        }
        else if (!mobFacingRight && destinationPositionX > positionX)
        {
            destinationPositionX = positionX;
        }

        // Jesli mob jest zamrozony to sie nigdzie nie rusza
        if (freezeMob)
        {
            destinationPositionX = positionX;
        }



        // Ustawia pozycje y o polowe wyzej niz wysokosc sprajta czyli na nogi
        realPositionY = positionY + (gameObject.GetComponent<Renderer>().bounds.size.y/* * gameObject.GetComponent<Transform>().localScale.y */) / 2 + positionYCorrection;

        // Ustawia odleglosc stania za sojusznikiem
        behindMobRange = gameObject.GetComponent<Renderer>().bounds.size.x / 2;

        // Ustawiamy kierunek patrzenia sie moba
        if (mobFacingRight)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        //Chodzenie moba
        if(mobState == state.walk)
        {
            if (mobFacingRight)
            {
                positionX += 1 * speed * Time.deltaTime;
                if (positionX > destinationPositionX) positionX = destinationPositionX;

                gameObject.GetComponent<Transform>().position = new Vector3(positionX, realPositionY, 0.0f);
            }
            else
            {
                positionX -= 1 * speed * Time.deltaTime;
                if (positionX < destinationPositionX) positionX = destinationPositionX;

                gameObject.GetComponent<Transform>().position = new Vector3(positionX, realPositionY, 0.0f);
            }
        }
        



        // Zerujemy czasy ataku i umierania
        // Jest to juz nizej i w tym miejscu nie działa nie wiem czemu
        /*if (mobState != state.attack)
            attackDurationCount = 0.0f;
        if (mobState != state.death)
            deathDurationCount = 0.0f;*/


        // Liczymy jak dlugo mob atakuje albo umiera
        if (mobState == state.attack)
        {
            attackDurationCount += Time.deltaTime;
        }

        if (mobState == state.death)
        {
            deathDurationCount += Time.deltaTime;
        }

        // Jesli wystarczajaco dlugo atakuje to zmienia sie na stanie w miejscu a jak wystarczajaco dlugo umiera to sie niszczy
        if (attackDurationCount >= attackDuration)
        {
            mobState = state.stand;
            attackDurationCount = 0.0f;
        }

        if (deathDurationCount >= deathDuration)
        {
            deathDurationCount = 0.0f;
            Destroy(this.gameObject);
            Destroy(createdHealthBar);
        }

        // Jeśli mob nie jest na docelowej pozycji to zmieniamy animacje na chodzenie a jak jest to na stanie
        if (mobState == state.stand && destinationPositionX != positionX)
        {
            mobState = state.walk;
        }
        else if (mobState == state.walk && destinationPositionX == positionX && allyInFrontWalking == false)
        {
            mobState = state.stand;
        }

        // Jesli ma malo zycia to umiera
        if (health <= 0)
        {
            mobState = state.death;
        }


        // Ustawiamy animacje wzgledem stanu w jakim sie znajduje mob
        if (animator != null)
        {
            switch (mobState)
            {
                case state.stand:
                    animator.SetBool("stand", true);
                    animator.SetBool("walk", false);
                    animator.SetBool("attack", false);
                    animator.SetBool("death", false);
                    break;
                case state.walk:
                    animator.SetBool("stand", false);
                    animator.SetBool("walk", true);
                    animator.SetBool("attack", false);
                    animator.SetBool("death", false);
                    break;
                case state.attack:
                    animator.SetBool("stand", false);
                    animator.SetBool("walk", false);
                    animator.SetBool("attack", true);
                    animator.SetBool("death", false);
                    break;
                case state.death:
                    animator.SetBool("stand", false);
                    animator.SetBool("walk", false);
                    animator.SetBool("attack", false);
                    animator.SetBool("death", true);
                    break;

            }
        }
    }
}
