using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class enemyStats : MonoBehaviour
{

    [Header("Stats")]
    public float HP;
    public float maxHP;
    [SerializeField] float armor, damgMin, damgMax, defendValue, defendingMin, defendingMax;
    //private enemy targetally;

    //mostly timers n stuff
    [Header("timers")]
    public float randWaitmin, randWaitmax, readyingTimer, strikeTimer, waitTimerOffset;

    private Camera _mainCam;
    private GameManager _GM;
    private PlayerHealthBar _playerHP;
    private SoundManager _soundManager;

    [Header("Position in List")]
    public int posInList;
    private Transform spotToReturnTo;
    //private Transform attackThrowMarker;
    //this is for handling the enemy moving from thier spot up to attack area for better animation

    [Header("AnimatorGORefrence")]
    public GameObject spriteChild;
    private Animator anim;
    //below the state is what handles animation timing
    protected attackState curState;

    [Header("HP Bar stuff")]
    public Image myHPBar;
    public Transform HPBarToMove;
    public Transform backUpHPBarSpot;
    //private Vector3 HPBarPosToReturnTo;
    public EnemysManager enmsSys;

    //attack projectile stuff **also fully utelizing the multiple attack and special prefabs has not been used yet

    public List<Ability> myAbilities=new List<Ability> { Ability.none};//default it to none as that cuases error
    private int amountRobbed = 0;

    public List<GameObject> currentAttacks = new List<GameObject>();

    //public Material matWhite;


    [SerializeField] int minCoin, maxCoin;

    public GameObject HPPointer;
    public List<GameObject> BowPointers;
    public Canvas myCanvas;

    public Coroutine myActionRoutine;

    [SerializeField] GameObject OnFireSprite;
    [SerializeField] GameObject StunnedSprite;
    [SerializeField] bool basicAttackDiversity;
    [SerializeField] bool longRanged;
    [SerializeField] int Aggression;
    [SerializeField] int Defensiveness;

    private float currentDefense = 0;

    public List<int> difficulty;
    public float stunTimer = 0;

    //selfheal stuff
    [SerializeField] float _healAmount;
    private bool _regening = false;
    [SerializeField] float regenTimer, regenMaxTimer;
    [SerializeField] float healThreashold;
    [SerializeField] bool aboveHealThreashold;

    [SerializeField] Transform hurtPoint;//for the particle system to hopefully work better, more hard coding lol

    //poison Variables
    public TMP_Text PoisonText;
    private Coroutine WasPoisonedRoutine;
    private float PoisonTimer;
    //might not need to be public aoe healer should remove debuffs
    public bool isPoisoned = false;

    private int regenTracker;

    private ParticleManager parM;

    //testing new block?
    private List<Transform> BlockSpots;
    private List<GameObject> BlockSets;

    private EnemyBehavior eb;
    private float AttackSpeedScaler;

    //adding deligates to actually implement the stategy pattern
    public System.Action _DelegateAction;
    public System.Action delegateAction
    {
        get
        {
            return _DelegateAction;
        }
        set
        {
            _DelegateAction = value;
        }
    }
    public bool hasPickedAction = false;

    public void SendActionUI()
    {
        delegateAction.Invoke();
    }

    public enum attackState 
    { 
        waiting,ThrowingAttack,damaged
    }

    public enum Ability
    {
        none,steal, antiarmor, heal, multiHeal, ninja, boss, sasumata,fire, blacksmith, sensei, farmWife, poison
    }

    protected virtual void Start()
    {
        _mainCam = Camera.main;
        _GM = _mainCam.GetComponent<GameManager>();
        _playerHP = _mainCam.GetComponent<PlayerHealthBar>();
        enmsSys = _mainCam.GetComponent<EnemysManager>();
        anim = spriteChild.GetComponent<Animator>();
        HP = maxHP;
        BlockSets = new List<GameObject>();
        eb = GetComponent<EnemyBehavior>();
        parM = FindObjectOfType<ParticleManager>();

        _soundManager = FindObjectOfType<SoundManager>();
        //matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        
        var temp = enmsSys.GetTrapSpawnSpots();
        BlockSpots = temp[0];

        //for moving hp bar over ui elements
        //HPBarPosToReturnTo = HPBarToMove.localPosition;
        HandleHPBarPlacement();

        //DecideNStartAction();  
        AttackSpeedScaler = SaveData.instance.getTimeScaledEnemyWaitTimeValue();
    }

    void Awake()
    {
        HPPointer.SetActive(false);
    }

    protected virtual void Update()
    {
        //Hp ifs
        if (HP<=0)
        {
            EnemyDied();
        }
        if (HP > maxHP)
        {
            HP = maxHP;
        }

        

        SetWaitTimerOffset();
        /*switch(curState)
        {
            case attackState.waiting: state = 0

        }*/
        //below is the effect of range or mob honour

        anim.SetFloat("State", (int)curState);
    }
    /*protected virtual void fillMyHP()//called in
    {
        if (myHPBar != null)
        { myHPBar.fillAmount = HP / maxHP; }
        else { Debug.Log("hp bar is null??"); }
    }*/

    public void healEnm(float heal)
    {
        HP += heal;
        parM.ShowHeal(hurtPoint, heal);
    }

    

    public void damageEnemy(float deal, List<WeaponEffect> effects)
    {

        bool antArm = false;
        
        for (int lcv =0;lcv<effects.Count;lcv++)
        {
            switch (effects[lcv])
            {
                case WeaponEffect.none:
                    break;
                case WeaponEffect.flame:
                    StartCoroutine(OnFire(deal/2));//so with base version 50 full damage will be 32 fire damage over time, likley
                    OnFireSprite.SetActive(true);
                    //add sound effect here
                    break;
                case WeaponEffect.antiarmor:
                    antArm = true;
                    //maybe different sound
                    break;
                case WeaponEffect.lifeSteal:
                    _playerHP.HealPlayer(deal / 6);
                    //get it to calculate armor aswell
                    break;
                case WeaponEffect.sasumata:
                    this.Stunned(deal);
                    break;
                case WeaponEffect.poison:
                    HP -= 1;
                    if (!isPoisoned)
                        GotPoisoned(deal);
                    else PoisonTimer -= deal / 8;
                    //for future refrence the 8 should probably be what scales
                    break;
            }
        }
        OnHitEffect(deal);
        if (antArm)
        {
            HP -= Mathf.Clamp((deal - currentDefense), 1, deal);
        }
        else
        {
            if (deal > armor)
            { HP = HP- Mathf.Clamp((deal - armor - currentDefense),1,deal); }
        }

        //play sound
        _soundManager.PlaySound("hit", deal);

        curState = attackState.damaged;
        parM.ShowDamage(hurtPoint, deal);
    }

    protected virtual void OnHitEffect(float deal)
    {
        //this is overwritten by other scripts
        //maybe in future we will give enemies a rage on a certain number of hits, but that is for future Noah ha ha
        if (_regening && deal > healThreashold)
        {
            Debug.Log("regen stoped");
            _regening = false;
            //should have a custome noise maybe bowl breaks and sprite the sumo the bowl should go flying
            StopCoroutine(myActionRoutine);
            //DecideNStartAction();
        }
    }

    public void Stunned(float num)
    {
        if(num >= 60){ stunTimer += 2; }
        else if(num >= 40){ stunTimer += 1f; }
        else if(num >=20) { stunTimer += 0.5f; }

        if (stunTimer > 5)
        {
            stunTimer = 5f;
        }
        if (stunTimer > 0)
        { StunnedSprite.SetActive(true); }
    }

    public void SetThings(int point)
    {
        //atkStarts = str;
        //atkEnd = end;
        posInList = point;
    }

    public void SetPositionRefrences(Transform mypos, Transform attackMark)
    {
        spotToReturnTo = mypos;
        //attackThrowMarker = attackMark;

    }

    public void SetPosInList(int pos)
    {
        posInList = pos;
    }

    public void SetWaitTimerOffset()
    {
        if (!longRanged)
        {
            waitTimerOffset = posInList * 1.5f;
        }
    }

   

    #region Attack Stuff
    

    public bool HasAbility(Ability abl)//mostly called in here called in EnemyHPBarPlacerManager to check if boss
    {
        for (int lcv = 0; lcv < myAbilities.Count; lcv++)
        {
            if(myAbilities[lcv]==abl)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator StunnedRoutine()
    { 
        while(stunTimer>0)
        {
            stunTimer -= Time.deltaTime;
            yield return null;
        }
        stunTimer = 0f;
        StunnedSprite.SetActive(false);

        //DecideNStartAction();
    }

    

    public void hitNow(float dmg,AttackEffect atkeef)
    {
        _playerHP.DamagePlayer(this,dmg, (int)myAbilities[0]);
        
        if (HasAbility(Ability.steal))
        {
            int randRob = Random.Range(2, 4);
            _GM.robPlayer(randRob);
            _soundManager.PlaySound("yoink");
            amountRobbed += randRob;
        }
        else if (HasAbility(Ability.blacksmith) && atkeef == AttackEffect.DamageArmor)
        {
 
            FindObjectOfType<PlayerEquipedItemsManager>().DamageItem(2);
            _soundManager.PlaySound("breakItem");

        }
        else if(atkeef == AttackEffect.confuseStyle)
        {
            //change current style
        }

    }

    #endregion


    protected virtual IEnumerator TheDefendingRoutine()
    {
        curState = attackState.waiting;

        yield return new WaitForSeconds(waitTimerOffset + randWaitmin);
        // was this yield return new WaitForSeconds(Random.Range(randWaitmin + waitTimerOffset, randWaitmax + waitTimerOffset)); 
        //gonna test it defending quicker see how it look


        //for event panel enemies being pacifist
        while (EventManager.PanelUP == true)
        {
            yield return null;
        }
        
        currentDefense = defendValue;
        spriteChild.GetComponent<SpriteRenderer>().color = FindObjectOfType<ColorManager>().defendingColor;
        //there should also be indication to the player shields over enemy hp or the strike area changes color and maybe the enemy
        //it waits between lowest and highest defend timer and defense is up during that time
        yield return new WaitForSeconds(Random.Range(defendingMin,defendingMax));

        //because we need their current deffense to be 0 while attacking
        currentDefense = 0;
        spriteChild.GetComponent<SpriteRenderer>().color = Color.white;
        //DecideNStartAction();
    }
        private void MoveUP()
    {
        //why outsource max agression?
        int rand = Random.Range(0, enmsSys.GetMaxAgression());
        if (rand <= Aggression)
        {
            var enmList = FindObjectOfType<EnemysManager>().aliveEnemys;
            if(posInList>0 && enmList.Count>1)
            {
                //move up
                var targetToSwap = enmsSys.aliveEnemys[posInList -1].gameObject;
                var targetPos = targetToSwap.transform.position;
                var myOldPos = this.gameObject.transform.position;

                this.transform.position = targetPos;
                targetToSwap.transform.position = myOldPos;
                enmsSys.aliveEnemys[posInList -1] = this;
                enmsSys.aliveEnemys[posInList] = targetToSwap.GetComponent<enemyStats>();

                enmsSys.UpdateEnmsPosRefrence();

                enmsSys.UpdateOurPointers();

                enmsSys.IncreaseAgressionRange(Aggression);
                //all of a sudden idk if i spelled agression right ah yes 2 gs
                targetToSwap.GetComponent<enemyStats>().DisablePointer();
                HandleHPBarPlacement();
            }
        }
    }

    public void IRan()
    {
        if (enmsSys.aliveEnemys.Count >= 1)
        {
            enmsSys.aliveEnemys.Remove(this);
            enmsSys.OpenTimer = 1.5f;
            enmsSys.UpdateEnmsPosRefrence();
            EnemyHPBarPlacerManager.instance.RemoveMeFromList(this);
        }
        Destroy(this.gameObject);
    }

    private void OnValidate()
    {
        if (randWaitmax < randWaitmin)
            randWaitmax = randWaitmin;
    }

    IEnumerator OnFire(float dmg)
    {
        Debug.Log("dmg sent in" + dmg);
        yield return new WaitForSeconds(0.5f);
        HP -= 2;
        dmg -= 1;
        myHPBar.fillAmount = HP / maxHP;
        int randNum = Random.Range(0, 6);
        if (dmg > randNum)//to simulate them maybe putting it out, maybe i could get behavior to pay attention to this sort of show putting themselves out if they choose
        {
            OnFireSprite.SetActive(true);
            StartCoroutine(OnFire(dmg));
        }else
        {
            OnFireSprite.SetActive(false);
        }
    }

    IEnumerator RegenRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        HP += _healAmount;
        if (HP != maxHP && regenTracker <12)
        {
            //keep sprite as healing
            Debug.Log("Regen" + HP);
            regenTracker++;
            myActionRoutine =StartCoroutine(RegenRoutine());
        }
        else
        {
            //DecideNStartAction();
        }
    }

    public void StartRegen(float healAmount)
    {
        _healAmount = healAmount;
        _regening = true;
        regenTimer = regenMaxTimer;
        myActionRoutine = StartCoroutine(RegenRoutine());
    }

    public void SetTargetPointers(List<Sprite> myPointers)
    {
        //this is called from pointer manager on individual enemies
        HPPointer.SetActive(true);
        HPPointer.GetComponent<SpriteRenderer>().sprite = myPointers[0];
        myPointers.RemoveAt(0);
        if (myPointers.Count>0)
        {
            for (int lcv = 0; lcv < myPointers.Count; lcv++)
            {
                BowPointers[lcv].SetActive(true);
                BowPointers[lcv].GetComponent<SpriteRenderer>().sprite = myPointers[lcv];
            }
            //if multiple we set 1st one then remove from list and enable a number of 2ndary pointers which are then set
        }
    }
    
    public void DisablePointer()
    {
        HPPointer.SetActive(false);
    }

    public float getCurrentHP()
    {
        return HP;
    }

    protected void HandleHPBarPlacement()
    {
        EnemyHPBarPlacerManager.instance.PlaceMyHPBar(this, posInList);
    }

    private void GotPoisoned(float Damage)
    {
        //this is to centralize where all the poison stuff except variables are
        //calculate poison timer
        PoisonTimer = (maxHP-Damage) / 8;
        WasPoisonedRoutine = StartCoroutine(PoisonedRoutine());
        //poison timer number should be set here 
    }

    public void CuredofPoison()
    {
        //stop poison routine
        //might need to call color manager to have color consistancy
        myHPBar.color = Color.red;
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
        //set poisonTimer off initial attack
        myHPBar.color = FindObjectOfType<ColorManager>().PoisonedColor;

        //PoisonText.gameObject.SetActive(true); can just have no text

        while (PoisonTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            PoisonTimer--;
            PoisonText.text = "" + (int)PoisonTimer;
        }
        //if secCount<=0
        HP = 0;
        PoisonText.text = "";
        myHPBar.color = Color.red;
    }

    public void EnemyDied()
    {
        GetComponent<EnemyBehavior>().ClearAttacksNTraps();

        _GM.PayOut(minCoin + amountRobbed, maxCoin);
        if (myAbilities[0] == Ability.poison)
        {
            FindObjectOfType<PlayerHealthBar>().CuredofPoison();
            //currently this wouldn't check if there are other enemies with the ability to poison, but worry about it later
        }
        if (BlockSets.Count > 0)
        {
            foreach (GameObject trap in BlockSets)
            {
                Destroy(trap);
            }
        }
        EnemyHPBarPlacerManager.instance.RemoveMeFromList(this);
        enmsSys.OnDied(this);
    }

    public List<float> getRandomAttackDamage()
    {
        float rand = Random.Range(damgMin, damgMax);
        return new List<float>() { rand, damgMax};
    }

    public float getRandomWaitTime()
    {
        //maybe get from save data each time if I want it to live update AttackSpeedScaler = SaveData.instance.getTimeScaledEnemyWaitTimeValue();
        float rand = Random.Range(randWaitmin, randWaitmax) + waitTimerOffset;

        Debug.Log("Time wait scaller " + AttackSpeedScaler);

        return rand*AttackSpeedScaler;
    }
}
