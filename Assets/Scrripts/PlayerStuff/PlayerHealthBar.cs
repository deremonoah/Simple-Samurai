using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    public Image healthBar;

    public float health, maxHealth = 100;
    float lerpSpeed;
    [SerializeField] float armorValue;
    public Armor myArmor;
    private GameManager _gm;

    [SerializeField] Armor testArmor;
    private SoundManager _soundManager;
    private PlayerDefense _playerDefense;
    [SerializeField] Curio _myCurio;

    void Start()
    {
        health = maxHealth;
        _gm = FindObjectOfType<GameManager>();
        myArmor = Instantiate(myArmor);
        testArmor = Instantiate(testArmor);
        _soundManager = FindObjectOfType<SoundManager>();
        _playerDefense = FindObjectOfType<PlayerDefense>();
        _myCurio = FindObjectOfType<PlayerEquipedItemsManager>().equipedCurio;
    }


    private void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0f)
        {
            if (myArmor.armrEef == ArmorEffect.phoenix)
            {
                maxHealth = maxHealth / 2;
                health = maxHealth;
                //make it so player can't increase max hp probably
                if (maxHealth <= 10)
                { _gm.OpenLossPan(); }
            }
            else { _gm.OpenLossPan(); }
        }
        
        if (_myCurio != null)
        {
            if (_myCurio.curiEef == CurioEffect.healOnGo && health <= maxHealth / 2)
            {
                health += _myCurio.CurioNum;
                FindObjectOfType<PlayerEquipedItemsManager>().ClearConsumable();
            }
        }
        


        lerpSpeed = 2f * Time.deltaTime;
        
        if (myArmor.armrEef == ArmorEffect.turtle)
        {
            if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Mouse0))
            {
                armorValue = myArmor.effectNumberOneLevel[myArmor.itemLevel];
            }
            else { armorValue = myArmor.armorLevel[myArmor.itemLevel]; }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetArmor(testArmor);
        }
#endif

        HealthBarFiller();

        
    }


    void HealthBarFiller()
    {
        //healthBar.fillAmount = health / Mathf.Lerp(healthBar.fillAmount, health / maxHealth,lerpSpeed);
        healthBar.fillAmount = health / maxHealth;
    }

    public void DamagePlayer(enemy enmy,float damagePoints, int ability)
    {
        //add enum stuff for effects damage could have
        if (_playerDefense.isDefended() && enmy != null)
        {
            //this is where I would check which player defense they have so ima make that script
            _playerDefense.DefendPlayer(enmy);
        }
        else
        {
            if (ability == 2)
            {
                health -= (Mathf.Max(1, damagePoints));
            }
            else
            {
                health -= (Mathf.Max(1, damagePoints - armorValue));
            }
            //this is also where I could add throns type armor well I still would need to check if enemy is null again
            _soundManager.PlaySound("hit");
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
        armorValue = am.armorLevel[am.itemLevel];
        myArmor = am;

    }
    public void SetCurio(Curio cur)
    {
        _myCurio = cur;
    }
}
