using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    public Image healthBar;

    public float health, maxHealth = 100;
    float lerpSpeed;
    [SerializeField] float armorNum;
    public Armor myArmor;
    private GameManager gm;

    [SerializeField] Armor testArmor;
    void Start()
    {
        health = maxHealth;
        gm = FindObjectOfType<GameManager>();
    }


    private void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        lerpSpeed = 3f * Time.deltaTime;
        
        if (myArmor.armrEef == ArmorEffect.turtle)
        {
            if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Mouse0))
            {
                armorNum = myArmor.eefNumOne;
            }
            else { armorNum = myArmor.armor; }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetArmor(testArmor);
        }

        HealthBarFiller();

        if (health <= 0f)
        {
            if (myArmor.armrEef == ArmorEffect.phoenix)
            {
                maxHealth = maxHealth / 2;
                health = maxHealth;
                //make it so player can't increase max hp probably
                if (maxHealth <= 10)
                { Camera.main.GetComponent<GameManager>().OpenLossPan(); }
            }
            else { Camera.main.GetComponent<GameManager>().OpenLossPan(); }
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
            health -= (Mathf.Max(1, damagePoints - armorNum));
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
        armorNum = am.armor;
        myArmor = am;
        if (myArmor.armrEef == ArmorEffect.greed)
        {
            gm.bonusGold = true;
        }
    }
}
