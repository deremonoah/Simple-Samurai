using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image healthBar;

    public float health, maxHealth = 100;
    float lerpSpeed;
    float armor;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }


    private void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        lerpSpeed = 3f * Time.deltaTime;
        
        HealthBarFiller();

    }


    void HealthBarFiller()
    {
        //healthBar.fillAmount = health / Mathf.Lerp(healthBar.fillAmount, health / maxHealth,lerpSpeed);
        healthBar.fillAmount = health / maxHealth;
    }

    public void DamagePlayer(float damagePoints)
    {
        if (health > 0)
        {
            health -= (damagePoints-armor);
        }
    }

    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
        {
            health += healingPoints;
        }
    }

}
