using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("Ui refrences")]
    public Image healthBarFill;
    [SerializeField] Transform HPScaling;
    [SerializeField] Transform HPForMoving;
    public Text HpNumbers;
    public bool inCombat;

    [Header("UI refrences for armor gauge")]
    [SerializeField] Image armorBarFill;
    [SerializeField] Transform ArmorScaling;
    private float maxArmor;
    private float currentArmor;
    private float currentRegen;

    [Header("Refrences for position")]
    [SerializeField] Transform inCombatSpot;
    [SerializeField] Transform inShopSpot;

    [Header("Numbers")]
    [SerializeField] float health, maxHealth = 100;
    //private float bonusHealth = 0;
    private bool hadBonusHP;
    float lerpSpeed;
    [SerializeField] float armorValue;
    public Armor equipedArmor;
    private GameManager _gm;

    [SerializeField] Armor testArmor;
    private SoundManager _soundManager;
    private PlayerDefense _playerDefense;
    [SerializeField] Curio _myCurio;

    [SerializeField] GameObject DefensesUIParent;

    [SerializeField] GameObject PlayerOnFireSprite;
    [SerializeField] GameObject angrySymbol;

    [SerializeField] private float timeAngrySymbolIsOnScreen = 2f;

    [Header("OnFire test")]
    [SerializeField] float FireLeft;
    [SerializeField] int RollCount;

    private Vector3 startingScale;
    private ColorManager colman;

    //poison stuff added
    public Text PoisonText;
    private Coroutine WasPoisonedRoutine;
    private bool isPoisoned;
    private int PoisonTimer = 20;

    private StrikePoint _strikePoint;
    private bool PlayerDead;

    void Start()
    {
        health = maxHealth;
        _gm = FindObjectOfType<GameManager>();
        equipedArmor = Instantiate(equipedArmor);
        testArmor = Instantiate(testArmor);
        _soundManager = FindObjectOfType<SoundManager>();
        _playerDefense = FindObjectOfType<PlayerDefense>();
        _myCurio = FindObjectOfType<PlayerEquipedItemsManager>().equipedCurio;
        angrySymbol.SetActive(false);
        colman = FindObjectOfType<ColorManager>();

        startingScale = angrySymbol.transform.localScale;
        PoisonText.text = "";
        isPoisoned = false;

        _strikePoint = FindObjectOfType<StrikePoint>();

        //seting armor
        SetArmor(equipedArmor);
        setHPBarSize();
    }

    private void OnEnable()
    {
        bool inc=StrikeArea.PlayerOn;
        HPIsInCombat(inc);
    }


    private void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0f && !PlayerDead)
        {
            if (equipedArmor.armrEef == ArmorEffect.phoenix)
            {
                maxHealth = maxHealth / 2;
                health = maxHealth;
                //make it so player can't increase max hp probably
                if (maxHealth <= 10)
                { PlayerDied(); }
            }
            else { PlayerDied(); }
        }

        if (_myCurio != null)
        {
            if (_myCurio.curiEef == CurioEffect.healOnGo && health <= maxHealth / 2)
            {
                health += _myCurio.CurioNum;
                FindObjectOfType<PlayerEquipedItemsManager>().ClearConsumable();
                _myCurio = null;
            }
        }

        lerpSpeed = 2f * Time.deltaTime;

        if (equipedArmor.armrEef == ArmorEffect.turtle)
        {
            if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Mouse0))
            {
                armorValue = equipedArmor.effectNumberOneLevel[equipedArmor.itemLevel];
            }
            else { armorValue = equipedArmor.armorLevel[equipedArmor.itemLevel]; }
        }

        

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetArmor(testArmor);
        }
#endif
        CheckForChangingEffects();
        HealthBarFiller();
        ArmorBarFiller();

    }

    private void PlayerDied()
    {
        PlayerDead = true;
        _gm.OpenLossPan();
    }

    void HealthBarFiller()
    {
        
        //healthBar.fillAmount = health / Mathf.Lerp(healthBar.fillAmount, health / maxHealth,lerpSpeed);
        healthBarFill.fillAmount = health / maxHealth;
        if (!inCombat)
        { 
            HpNumbers.text = (int)health + "/" + maxHealth;
            PoisonText.text = "";
        }
        else { HpNumbers.text = ""; }

    }

    void ArmorBarFiller()
    {
        armorBarFill.fillAmount = currentArmor / maxArmor;
        currentArmor += Time.deltaTime * currentRegen; //normal regen
        if(currentArmor>maxArmor)
        {
            currentArmor = maxArmor;
        }
    }

    public void HPIsInCombat(bool oo)
    {
        inCombat = oo;
        if(inCombat)
        {
            //move to combat spot and rotate
            HPForMoving.position = inCombatSpot.position;
            HPForMoving.rotation = Quaternion.Euler(0, 0, 90);
            HpNumbers.text = "";
        }
        else
        {
            //set rotation & move to non combat spot
            HPForMoving.position = inShopSpot.position;
            HPForMoving.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void DamagePlayer(enemyStats enmy,float damagePoints, int ability)
    {
        //add enum stuff for effects damage could have
        if(inCombat)
        {
            if (_playerDefense.isDefended() && enmy != null)
            {
                //this is where I would check which player defense they have so ima make that script
                _playerDefense.DefendPlayer(enmy, damagePoints);
            }
            else
            {
                if (ability == 2)
                {
                    //anti armor
                    float resolveDmg = Mathf.Max(1, damagePoints);
                    health -= resolveDmg;
                    ParticleManager.instance.ShowPayerDamage(resolveDmg);
                    currentArmor = 0;

                }
                else if (ability == 8)
                {
                    //fire ability
                    health -= (Mathf.Max(1, damagePoints));
                    StartCoroutine(OnFire(damagePoints));
                }
                else if (ability == 12)
                {
                    //poison
                    if (isPoisoned)
                    {
                        PoisonTimer -= 3;//TODO: make poison reduced amount varable based on damage
                    }
                    else
                    { WasPoisonedRoutine = StartCoroutine(PoisonedRoutine()); }

                }
                else
                {
                    //regular attack
                    float resolveDmg = Mathf.Max(0, damagePoints - currentArmor);
                    currentArmor = Mathf.Clamp(currentArmor - damagePoints, 0, 100000);
                    health -= resolveDmg;
                    ParticleManager.instance.ShowPayerDamage(resolveDmg);
                    //Debug.Log("max: " + Mathf.Max(1, damagePoints - armorValue));
                }
                //this is also where I could add throns type armor well I still would need to check if enemy is null again
                _soundManager.PlaySound("hit");
                //StartCoroutine(RevengeRoutine());mihgt add back as an ability
            }
        }
    }

    public void DamagePlayerNoRevenge(float damagePoints, int ability)
    {
        if (ability == 2)//anti armor
        {
            health -= (Mathf.Max(1, damagePoints));
        }
        else if (ability == 8)//on fire
        {
            health -= (Mathf.Max(1, damagePoints - armorValue));
            StartCoroutine(OnFire(damagePoints));
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

    IEnumerator OnFire(float dmg)
    {
        FireLeft += Mathf.Clamp(4f + dmg,6,1000);//right now fire guy does 10-30 damage, I want the less damage the longer lasting fire, cause otherwise just seems like hit big do the most
        //+= so if hit again it can stack
        PlayerOnFireSprite.SetActive(true);
        //we might need to add an if checking a immunity to fire
        while (RollCount< FireLeft)
        {
            
            health -= Time.deltaTime*2;//idk man if I want it to burn them 1 damage a second doesn't seem noticable rn
            FireLeft -= Time.deltaTime;
            if(Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.Mouse0))
            {
                RollCount++;
            }
            yield return null;
        }
       
        PlayerOnFireSprite.SetActive(false);
        
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
        currentArmor = am.armorLevel[am.itemLevel];
        maxArmor = currentArmor;
        equipedArmor = am;
        currentRegen = equipedArmor.armorRegenPerSecond[equipedArmor.itemLevel];

        SetArmorBarSize();
    }
    public void SetCurio(Curio cur)
    {
        _myCurio = cur;
        if(_myCurio.curiEef == CurioEffect.XtHealth && !hadBonusHP)
        {
            IncreaseMaxHPBy(_myCurio.CurioNum);
            hadBonusHP = true;
        }
        else if(_myCurio.curiEef != CurioEffect.XtHealth && hadBonusHP)
        {
            hadBonusHP = false;
            //currently the extra health will be 50 but in future i will have to make it dynamic
            ReduceMaxHP(50);
        }
        setHPBarSize();
    }

    public void IncreaseMaxHPBy(float Xhealth)
    {
        maxHealth += Xhealth;
        health = Mathf.Clamp(health + Xhealth, 1, maxHealth);
        //I need to increase the size of hp bar and background then I also need to move the defenses over
        //the increase should also be proportional. there are 4 levels so probably 4 ifs or a switch statement so maybe take in level
        setHPBarSize();
    }
    public void ReduceMaxHP(float lessHP)
    {
        maxHealth -= lessHP;
        health = Mathf.Clamp(health - lessHP, 1, maxHealth);
        setHPBarSize();
    }

    public void CuredofPoison()
    {
        //stop poison routine
        //might need to call color manager to have color consistancy
        healthBarFill.color = Color.red;
        PoisonText.text = "";
        if (WasPoisonedRoutine != null)
        { StopCoroutine(WasPoisonedRoutine); }
        //restore poisonTimer
        PoisonTimer = 20;
        isPoisoned = false;
    }

    IEnumerator PoisonedRoutine()
    {
        isPoisoned = true;
        yield return new WaitForSeconds(.3f);
        PoisonTimer = 20;
        healthBarFill.color = colman.PoisonedColor;
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

    public void CurePlayerStatusEffects()
    {
        StopAllCoroutines();
        PlayerOnFireSprite.SetActive(false);
        //both fire and poison are coroutines
    }

    private void setHPBarSize()
    {
        HPScaling.localScale = new Vector3(1, 1, 1);
        HPScaling.localScale = new Vector3(maxHealth / 150, 1, 1);
    }

    private void SetArmorBarSize()
    {
        ArmorScaling.localScale = new Vector3(1, 1, 1);
        ArmorScaling.localScale = new Vector3(maxArmor / 150, 1, 1);
    }

    public float getHealth()
    {
        return health;
    }

    public void ResetArmorAfterCombat()
    {
        currentArmor = maxArmor;
    }

    private void CheckForChangingEffects()
    {
        if(equipedArmor.armrEef==ArmorEffect.turtle)
        {
            //check pointer if it is out or back
            if(equipedArmor.armrEef == ArmorEffect.turtle)
            {
                if (_strikePoint.AreAttacking())
                {
                    currentRegen = equipedArmor.armorRegenPerSecond[equipedArmor.itemLevel]/4;
                }
                else
                {
                    currentRegen = equipedArmor.armorRegenPerSecond[equipedArmor.itemLevel];
                }
            }
            
            
        }
    }
}
