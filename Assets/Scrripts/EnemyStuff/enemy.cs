using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class enemy : MonoBehaviour
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
    private Transform attackThrowMarker;
    //this is for handling the enemy moving from thier spot up to attack area for better animation

    [Header("AnimatorGORefrence")]
    public GameObject spriteChild;
    private Animator anim;
    //below the state is what handles animation timing
    protected attackState curState;

    public Image myHPBar;
    public EnemysManager enmsSys;

    //attack projectile stuff **also fully utelizing the multiple attack and special prefabs has not been used yet
    public List<GameObject> atkPrefabs, specialPrefabs;
    public List<GameObject> atkStarts;
    [SerializeField] GameObject atkEnd;
    public List<Vector2> atkDirs, SpecialDirs;

    public List<Ability> myAbilities;
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

    //adding deligates to actually implement the stategy pattern
    public System.Action DelegateAction;
    public System.Action delegateAction
    {
        get
        {
            return DelegateAction;
        }
        set
        {
            DelegateAction = value;
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

        parM = FindObjectOfType<ParticleManager>();

        _soundManager = FindObjectOfType<SoundManager>();
        //matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        
        var temp = enmsSys.GetTrapSpawnSpots();
        BlockSpots = temp[0];

        DecideNStartAction();  
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
            foreach (var atk in currentAttacks)
                Destroy(atk);

            _GM.PayOut(minCoin + amountRobbed, maxCoin);
            if(myAbilities[0]==Ability.poison)
            {
                FindObjectOfType<PlayerHealthBar>().CuredofPoison();
                //currently this wouldn't check if there are other enemies with the ability to poison, but worry about it later
            }
            if(BlockSets.Count>0)
            {
                foreach (GameObject trap in BlockSets)
                {
                    Destroy(trap);
                }
            }
            enmsSys.OnDied(this);
            
            
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
        fillMyHP();
        

    }
    protected virtual void fillMyHP()
    {
        myHPBar.fillAmount = HP / maxHP;
    }

    public void healEnm(float heal)
    {
        HP += heal;
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
                    StartCoroutine(OnFire());
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
        parM.ShowDamage(spriteChild.transform, deal);
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
            DecideNStartAction();
        }
    }

    public void Blocked(AttackEffect atkeef)
    {
        if(stunTimer <= 0)
        {
            if(myActionRoutine != null)
            {
                StopCoroutine(myActionRoutine);
                DecideNStartAction();
            }
        }
        
        if (HasAbility(Ability.blacksmith) && atkeef == AttackEffect.DamageWeapon)
        {
                FindObjectOfType<PlayerEquipedItemsManager>().DamageItem(1);
                _soundManager.PlaySound("breakItem");
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

    public void SetThings( List<GameObject> str, GameObject end, int point)
    {
        atkStarts = str;
        atkEnd = end;
        posInList = point;
    }

    public void SetPositionRefrences(Transform mypos, Transform attackMark)
    {
        spotToReturnTo = mypos;
        attackThrowMarker = attackMark;

    }

    public void SetPosInList(int pos)
    {
        posInList = pos;
    }

    public void SetWaitTimerOffset()
    {
        if (!longRanged)
        {
            if (posInList > 0)
            {
                waitTimerOffset = 1.5f;
                if (posInList == 3)
                {
                    waitTimerOffset = 2.5f;
                }
            }
            else
            {
                waitTimerOffset = 0;
            }
        }
    }

    protected virtual void DecideNStartAction()
    {

        //maybe move up should be a coroutine, so I can have it only happen at the end of frames
        MoveUP();

        /*if(myActionRoutine != null)
        {
            hasStarted = true;
        }*/

        if(stunTimer>0)
        {
            myActionRoutine = StartCoroutine(StunnedRoutine());
            hasPickedAction = true;
        }
        if(!hasPickedAction)
        {
                //speccial abiliy routines
                if (HasAbility(Ability.steal) && amountRobbed > 5)
                {
                //myActionRoutine = StartCoroutine(RunRoutine());
                    delegateAction = BeginRunUI;
                    hasPickedAction = true;
                }
                else if (HasAbility(Ability.sasumata))
                {
                    //like 50% of the time sasumata special other half regular attack
                    int rand = Random.Range(0, 2);
                    if (rand == 1)
                    {
                        myActionRoutine = StartCoroutine(SasumataRoutine());
                        hasPickedAction = true;
                    }
                }
                else if(HasAbility(Ability.fire))
            {
                //testing basically a new trap or maybe chaning it to a block, maybe the traps should only go 
                //off if you stop there? so players have more agency and they would need a lot of stuff covered
                int rand = Random.Range(0, 10);
                //50% for test
                if(rand<3)
                {
                    DelegateAction = PlaceTrap;
                    //make fire trap
                    //need info from enemy trap
                    hasPickedAction = true;
                }
                //removed blocking for the time being
            }
        }

        
            //we are chaning the attack routine back into the action routine
            //and making the deligate decide what they are doing,
            //so for children of this we just need to overwrite this
            //int rand = Random.Range(1, 11);
            if (!hasPickedAction)
            {
                delegateAction = AttackUI;
                hasPickedAction = true;
            }
        myActionRoutine = StartCoroutine(TheActionRoutine());
            
            /*else
            { myActionRoutine = StartCoroutine(TheDefendingRoutine()); }*/
        
    }

    #region Attack Stuff
    protected virtual IEnumerator TheActionRoutine()
    {
        curState = attackState.waiting;
        


        yield return new WaitForSeconds(Random.Range(randWaitmin + waitTimerOffset, randWaitmax+ waitTimerOffset));

        //for event panel enemies being pacifist
        while (EventManager.PanelUP == true)
        {
            yield return null;
        }
        //the showing also turns on the animation to throw the attack
        StartCoroutine(moveToShowAttack());

        //can we wait for another coroutine to be done? google says yes, below line seems simple enough
        yield return moveToShowAttack();
        //yield return new WaitForSeconds(readyingTimer);
        //the readying timer seems a bit useless now could remove more than likley as it is the same
        
        curState = attackState.waiting;
        
        yield return new WaitForSeconds(strikeTimer);
        Debug.Log("got to the end of action routine: "+gameObject.name);
        hasPickedAction = false;
        DecideNStartAction();
    }

    public IEnumerator moveToShowAttack()
    {
        //this function has the enemy move up to the front of the strike area then attack
        var sPos=spriteChild.transform.position;
        float t = 0;
        
        while (t<1)
        {
            sPos = spriteChild.transform.position;
            t = t + Time.deltaTime* 4f;
            spriteChild.transform.position = Vector3.Lerp(sPos, attackThrowMarker.position, t);
            yield return null;
        }
        
        spriteChild.transform.position = attackThrowMarker.position;
        //play animation
        curState = attackState.ThrowingAttack;
        //wait for the right amount time for the animation to finish hopefully
        yield return new WaitForSeconds(0.5f);

        //moving back to their spot
        t = 0;

        while (t < 1)
        {
            sPos = spriteChild.transform.position;
            t = t + Time.deltaTime;
            spriteChild.transform.position = Vector3.Lerp(sPos, spotToReturnTo.position, t);
            yield return null;
        }
        spriteChild.transform.position = spotToReturnTo.position;
    }

    private bool HasAbility(Ability abl)
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

    public IEnumerator SasumataRoutine()
    {
        curState = attackState.waiting;
        yield return new WaitForSeconds(Random.Range(randWaitmin + waitTimerOffset, randWaitmax + waitTimerOffset));

        curState = attackState.ThrowingAttack;
        SpecialUI();
        
        yield return new WaitForSeconds(readyingTimer);
        curState = attackState.waiting;

        yield return new WaitForSeconds(strikeTimer);

        DecideNStartAction();
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

        DecideNStartAction();
    }

    public void AttackUI()
    {
        var dir = Random.Range(0, atkDirs.Count);
        int randAttack = Random.Range(0, atkPrefabs.Count);
        float dmg = Random.Range(damgMin, damgMax);
        //atkDirs[0]= standard, [1] = top atk spawn, [2] bottom, [3]Reverse start

        GameObject atk = Instantiate(atkPrefabs[randAttack], atkStarts[0].transform.position, atkStarts[0].transform.rotation);

        atk.GetComponent<EnmAtKArea>().SetDamage(dmg, damgMax);

        if (atkDirs[dir].y == 0)
        {
            if (basicAttackDiversity)
            {
                int rand = Random.Range(0, 3);
                atk.transform.position = atkStarts[rand].transform.position;
            }
            else
            { atk.transform.position = atkStarts[0].transform.position; }
        }
        else if (atkDirs[dir].y == -0.5)
        {
            atk.transform.position = atkStarts[1].transform.position;
        }
        else if (atkDirs[dir].y == 0.5)
        {
            atk.transform.position = atkStarts[2].transform.position;
        }
        /*else if (atkDirs[dir].x == 0)
        {
            atk.transform.position = atkStarts[1].transform.position;
            atk.transform.Rotate(0f, 0f, 90f, Space.Self);
        //this was an attempt to make it go down but its too off center for it to work
        }*/
        else
        {
            atk.transform.position = atkStarts[3].transform.position;
        }


        atk.GetComponent<EnmAtKArea>().Setstuff(this, atkEnd.transform, atkDirs[dir]);
        var newList = new List<GameObject>();
        if (currentAttacks.Count > 0)
            foreach (var swing in currentAttacks)
                if (swing != null)
                    newList.Add(swing);

        newList.Add(atk);
        currentAttacks = newList;

    }

    public void SpecialUI()
    {
        var dir = Random.Range(0, SpecialDirs.Count);
        int randSpecial = Random.Range(0, specialPrefabs.Count);
        float dmg = Random.Range(damgMin, damgMax);

        GameObject special = Instantiate(specialPrefabs[randSpecial], atkStarts[0].transform.position, atkStarts[0].transform.rotation);
        

        // set damage if any    special.GetComponent<EnmAtKArea>().SetDamage(dmg, damgMax);

        if (SpecialDirs[dir].y == 0)
        {
            if (basicAttackDiversity)
            {
                int rand = Random.Range(0, 3);
                special.transform.position = atkStarts[rand].transform.position;
            }
            else
            { special.transform.position = atkStarts[0].transform.position; }
        }
        else if (SpecialDirs[dir].y == -0.5)
        {
            special.transform.position = atkStarts[1].transform.position;
        }
        else if (SpecialDirs[dir].y == 0.5)
        {
            special.transform.position = atkStarts[2].transform.position;
        }

        special.GetComponent<SasumataUIScript>().Setstuff(this, atkEnd.transform, SpecialDirs[dir]);
        var newList = new List<GameObject>();
        if (currentAttacks.Count > 0)
            foreach (var swing in currentAttacks)
                if (swing != null)
                    newList.Add(swing);

        newList.Add(special);
        currentAttacks = newList;
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
    
    private void PlaceTrap()
    {
        //currently places sumo blocks and fire dude trap
        int rand = Random.Range(0, BlockSpots.Count + 2);
        rand = Mathf.Clamp(rand - 2, 0, BlockSpots.Count);
        //look up better way of weighting outcomes of randomness
        var trap=Instantiate(specialPrefabs[0], BlockSpots[rand].position, transform.rotation);
        BlockSets.Add(trap);
    }


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
        DecideNStartAction();
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
                enmsSys.aliveEnemys[posInList] = targetToSwap.GetComponent<enemy>();

                enmsSys.UpdateEnmsPosRefrence();

                enmsSys.UpdateOurPointers();

                enmsSys.IncreaseAgressionRange(Aggression);
                //all of a sudden idk if i spelled agression right ah yes 2 gs
                targetToSwap.GetComponent<enemy>().DisablePointer();
            }
        }
    }

    public void BeginRunUI()
    {
        GameObject run = Instantiate(specialPrefabs[0], atkStarts[3].transform.position, atkStarts[3].transform.rotation);
        run.GetComponent<EnmAtKArea>().Setstuff(this, atkStarts[0].transform, SpecialDirs[0]);
        currentAttacks.Add(run);
    }

    public void IRan()
    {
        if (enmsSys.aliveEnemys.Count >= 1)
        {
            enmsSys.aliveEnemys.Remove(this);
            enmsSys.OpenTimer = 1.5f;
            enmsSys.UpdateEnmsPosRefrence();
        }
        Destroy(this.gameObject);
    }

    private void OnValidate()
    {
        if (randWaitmax < randWaitmin)
            randWaitmax = randWaitmin;
    }


    

    IEnumerator OnFire()
    {
        yield return new WaitForSeconds(0.5f);
        HP -= 1;
        int randNum = Random.Range(0, 10);
        if (randNum < 8)
        {
            OnFireSprite.SetActive(true);
            StartCoroutine(OnFire());
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
            DecideNStartAction();
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
        //healthBar.color =  Color.black;
        //would like to change that to purple
        //PoisonText.gameObject.SetActive(true); can just have no text

        while (PoisonTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            PoisonTimer--;
            PoisonText.text = "" + (int)PoisonTimer;
        }
        //if secCount<=0
        HP = 0;
    }
}
