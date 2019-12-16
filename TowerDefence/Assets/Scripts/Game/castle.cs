using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castle : MonoBehaviour
{
    public GameObject healthBar;
    public float health = 1000;
    private float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    void setHealthBar()
    {
        Vector2 healthBarScale;

        if (health > 0)
            healthBarScale = new Vector2(health / maxHealth, 1);
        else
            healthBarScale = new Vector2(0, 1);

        healthBar.transform.Find("Bar").localScale = healthBarScale;
    }

    // Update is called once per frame
    void Update()
    {
        setHealthBar();
    }
}
