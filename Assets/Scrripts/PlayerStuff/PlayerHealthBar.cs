using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    public Image healthBar;

    public float health, maxHealth = 100;
    private float bonusHealth=0;
    private bool hadBonusHP;
    float lerpSpeed;
    [SerializeField] float armorValue;
    public Armor myArmor;
    private GameManager _gm;

    [SerializeField] Armor testArmor;
    private SoundManager _soundManager;
    private PlayerDefense _playerDefense;
    [SerializeField] Curio _myCurio;

    [SerializeField] GameObject HpBarUI;
    [SerializeField] GameObject HpBarBackground;
    [SerializeField] GameObject DefensesUIParent;

    [SerializeField] GameObject PlayerOnFireSprite;
    [SerializeField] GameObject angrySymbol;

    [SerializeField] private float timeAngrySymbolIsOnScreen = 2f;
    private Vector3 startingScale;
    private ColorManager colman;

    //poison stuff added
    public Text PoisonText;
    private Coroutine WasPoisonedRoutine;
    private bool isPoisoned;
    private int PoisonTimer = 20;

    void Start()
    {
        health = maxHealth;
        _gm = FindObjectOfType<GameManager>();
        myArmor = Instantiate(myArmor);
        testArmor = Instantiate(testArmor);
        _soundManager = FindObjectOfType<SoundManager>();
        _playerDefense = FindObjectOfType<PlayerDefense>();
        _myCurio = FindObjectOfType<PlayerEquipedItemsManager>().equipedCurio;
        angrySymbol.SetActive(false);
        colman = FindObjectOfType<ColorManager>();

        startingScale = angrySymbol.transform.localScale;
        PoisonText.text = "";
        isPoisoned = false;
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
        
        //testing
        if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(RevengeRoutine());
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
            _playerDefense.DefendPlayer(enmy,damagePoints);
        }
        else
        {
            if (ability == 2)
            {
                //anti armor
                health -= (Mathf.Max(1, damagePoints));
            } else if(ability ==8)
            {
                //fire ability
                health -= (Mathf.Max(1, damagePoints));
                StartCoroutine(OnFire());
            } else if(ability == 12)
            {
                //poison
                if (isPoisoned)
                    {
                    PoisonTimer -= 3;
                    }
                else
                { WasPoisonedRoutine = StartCoroutine(PoisonedRoutine()); }
                
            }
            else
            {
                //regular attack
                health -= (Mathf.Max(1, damagePoints - armorValue));
                //Debug.Log("max: " + Mathf.Max(1, damagePoints - armorValue));
            }
            //this is also where I could add throns type armor well I still would need to check if enemy is null again
            _soundManager.PlaySound("hit");
            StartCoroutine(RevengeRoutine());
        }
    }

    public void DamagePlayerNoRevenge(float damagePoints, int ability)
    {
        if (ability == 2)
        {
            health -= (Mathf.Max(1, damagePoints));
        }
        else if (ability == 8)
        {
            health -= (Mathf.Max(1, damagePoints));
            StartCoroutine(OnFire());
        }
        else
        {
            health -= (Mathf.Max(1, damagePoints - armorValue));
            //Debug.Log("No Revenge max: " + Mathf.Max(1, damagePoints - armorValue));
        }

        _soundManager.PlaySound("hit");
    }


    IEnumerator RevengeRoutine()
    {
        angrySymbol.SetActive(true);
        float revengeTimer = 0;
        FindObjectOfType<ParticleManager>().Revenge();
        Vector3 startingScale = angrySymbol.transform.localScale;
        FindObjectOfType<StrikeArea>().RevengeBuff();

        while (revengeTimer <= timeAngrySymbolIsOnScreen)
        {
            revengeTimer += Time.deltaTime;
            angrySymbol.transform.localScale =
                Vector3.Lerp(startingScale, Vector3.zero, revengeTimer / timeAngrySymbolIsOnScreen);
            yield return null;
        }
        angrySymbol.SetActive(false);
        angrySymbol.transform.localScale = startingScale;
    }

    IEnumerator OnFire()
    {
        yield return new WaitForSeconds(0.5f);
        //we might need to add an if checking a immunity to fire
        health -= 1;
        int randNum = Random.Range(0, 10);
        if (randNum < 8)
        {
            PlayerOnFireSprite.SetActive(true);
            StartCoroutine(OnFire());
        }
        else
        {
            PlayerOnFireSprite.SetActive(false);
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
        if(_myCurio.curiEef == CurioEffect.XtHealth && !hadBonusHP)
        {
            IncreaseMaxHPBy(_myCurio.CurioNum);
            health += _myCurio.CurioNum;
            hadBonusHP = true;
        }
        else if(_myCurio.curiEef != CurioEffect.XtHealth && hadBonusHP)
        {
            hadBonusHP = false;
            //currently the extra health will be 50 but in future i will have to make it dynamic
            ReduceMaxHP(50);
        }
    }

    public void IncreaseMaxHPBy(float Xhealth)
    {
        maxHealth += Xhealth;
        //I need to increase the size of hp bar and background then I also need to move the defenses over
        //the increase should also be proportional. there are 4 levels so probably 4 ifs or a switch statement so maybe take in level
        
    }
    public void ReduceMaxHP(float lessHP)
    {
        maxHealth -= lessHP;
    }

    public void CuredofPoison()
    {
        //stop poison routine
        //might need to call color manager to have color consistancy
        healthBar.color = Color.red;
        PoisonText.text = "";
        StopCoroutine(WasPoisonedRoutine);
        //restore poisonTimer
        PoisonTimer = 20;
        isPoisoned = false;
    }

    IEnumerator PoisonedRoutine()
    {
        isPoisoned = true;
        yield return new WaitForSeconds(.3f);
        PoisonTimer = 20;
        healthBar.color = colman.PoisonedColor;
        //healthBar.color =  Color.black;
        //would like to change that to purple
        //PoisonText.gameObject.SetActive(true); can just have no text

        while (PoisonTimer>0)
        {
            yield return new WaitForSeconds(1f);
            PoisonTimer--;
            PoisonText.text = ""+PoisonTimer;
        }
        //if secCount<=0
        health = 0;
    }
}
