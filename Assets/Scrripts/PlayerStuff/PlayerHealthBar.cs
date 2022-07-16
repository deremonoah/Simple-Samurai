using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    public Image healthBar;

    public float health, maxHealth = 100;
    float lerpSpeed;
    [SerializeField] float armor;

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

        if (health <= 0f)
        {
            Camera.main.GetComponent<GameManager>().OpenLossPan();
        }
    }


    void HealthBarFiller()
    {
        //healthBar.fillAmount = health / Mathf.Lerp(healthBar.fillAmount, health / maxHealth,lerpSpeed);
        healthBar.fillAmount = health / maxHealth;
    }

    public void DamagePlayer(float damagePoints, int ability)
    {
        //add enum stuff for effects damage could have
        if (ability == 2)
        {
            health -= (Mathf.Max(1,damagePoints));
        }else
        {
            health -= (Mathf.Max(1, damagePoints - armor));
        }
    }

    public void HealPlayer(float healingPoints)
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
