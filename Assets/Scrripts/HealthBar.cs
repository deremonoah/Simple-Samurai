using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image healthBar;

    public float health, maxHealth = 100;
    float lerpSpeed;
    [SerializeField] float armor;

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

    public void DamagePlayer(float damagePoints )
    {
        //add enum stuff for effects damage could have
        if (health > 0)
        {
            health -= (Mathf.Max(1,damagePoints-armor));
        }
    }

    public void Heal(float healingPoints)
    {
        if (health < maxHealth)
        {
            health += healingPoints;
        }
    }

    public void SetArmor(Armor am)
    {
        Debug.Log($"armor is: {armor:0}");
        armor = am.armor;
        Debug.Log($"armor is: {armor}");
    }
}
