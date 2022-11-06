using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    
    //stats
    public float HP, maxHP;
    [SerializeField] float armor,damgMin, damgMax, healMin, healMax;
    public enemy targetally;

    //mostly timers n stuff
    [SerializeField] float randWaitmin, randWaitmax, readyingTimer, strikeTimer;
    [SerializeField] float waitTimerOffset;

    private Camera _mainCam;
    private GameManager _GM;
    private PlayerHealthBar _playerHP;
    [SerializeField] int posInList;

    //animation stuff
    private Animator anim;
    
    public Image myHPBar;
    public EnemysManager enmsSys;

    //attack projectile stuff **also fully utelizing the multiple attack and special prefabs has not been used yet
    public List<GameObject> atkPrefabs, specialPrefabs;
    [SerializeField] List<GameObject> atkStarts;
    [SerializeField] GameObject atkEnd;
    [SerializeField] List<Vector2> atkDirs,SpecialDirs;
    attackState curState;
    public List<Ability> myAbilities;
    private int amountRobbed = 0;

    private List<GameObject> currentAttacks = new List<GameObject>();

    public Material matWhite;
    private Material matDefault;

    [SerializeField] int minCoin, maxCoin;

    public GameObject HPPointer;
    public List<GameObject> BowPointers;
    public Canvas myCanvas;

    public Coroutine myActionRoutine;

    [SerializeField] GameObject OnFireSprite;
    [SerializeField] bool basicAttackDiversity;

    enum attackState 
    { 
        waiting,readying,swinging,damaging,damaged
    }

    public enum Ability
    {
        none,steal, antiarmor, heal, multiHeal, ninja, boss
    }

    protected virtual void Start()
    {
        _mainCam = Camera.main;
        _GM = _mainCam.GetComponent<GameManager>();
        _playerHP = _mainCam.GetComponent<PlayerHealthBar>();
        enmsSys = _mainCam.GetComponent<EnemysManager>();
        anim = GetComponent<Animator>();
        HP = maxHP;
        anim = GetComponent<Animator>();
        //matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        
        matDefault = GetComponent<SpriteRenderer>().material;

        if (myAbilities[0] == Ability.heal)
        {
            myActionRoutine = StartCoroutine(healEnmRoutine());
        } else
        {
            myActionRoutine = StartCoroutine(TheAttackRoutine());
        }
         

            
    }

    void Awake()
    {
        HPPointer.SetActive(false);
        
    }

    void Update()
    {
        //Hp ifs
        if (HP<=0)
        {
            foreach (var atk in currentAttacks)
                Destroy(atk);
            enmsSys.OnDied(this);
            _GM.PayOut(minCoin + amountRobbed, maxCoin);
            Destroy(this.gameObject);
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

    

    public void damgEnemy(float deal, List<WeaponEffect> effects)
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
                    break;
                case WeaponEffect.antiarmor:
                    antArm = true;
                    break;
                case WeaponEffect.lifeSteal:
                    _playerHP.HealPlayer(deal / 6);
                    //get it to calculate armor aswell
                    break;

            }
        }
        if (antArm)
        {
            HP -= deal;
        }
        else
        {
            if (deal > armor)
            { HP = HP- (deal - armor); }
        }
        
        curState = attackState.damaged;
        StartCoroutine(Flash());
        
    }

    public void Blocked()
    {
        if(myActionRoutine != null)
        {
            StopCoroutine(myActionRoutine);
        }

        StartMyRoutine();
    }

    public void SetThings( List<GameObject> str, GameObject end, int point)
    {
        atkStarts = str;
        atkEnd = end;
        posInList = point;
    }

    public void SetPosInList(int pos)
    {
        posInList = pos;
    }

    public void SetWaitTimerOffset()
    {
        if (posInList > 0)
        {
            waitTimerOffset = 1.5f;
            if (posInList == 3)
            {
                waitTimerOffset = 2.5f;
            }
        }else
        {
            waitTimerOffset = 0;
        }
    }

    protected virtual void StartMyRoutine()
    {
        if (myAbilities[0] == Ability.heal)
        {
            myActionRoutine = StartCoroutine(healEnmRoutine());
        }
        else if (myAbilities[0] == Ability.steal && amountRobbed > 5)
        {
            myActionRoutine = StartCoroutine(RunRoutine());
        }
        else
        {
            myActionRoutine = StartCoroutine(TheAttackRoutine());
        }
        
    }

    #region Attack Stuff
    protected virtual IEnumerator TheAttackRoutine()
    {
        curState = attackState.waiting;
        yield return new WaitForSeconds(Random.Range(randWaitmin + waitTimerOffset, randWaitmax+ waitTimerOffset));


        AttackUI();
        curState = attackState.readying;
        yield return new WaitForSeconds(readyingTimer);

        curState = attackState.swinging;
        yield return new WaitForSeconds(strikeTimer);

        StartMyRoutine();


    }

    public void AttackUI()
    {
        var dir = Random.Range(0, atkDirs.Count);
        int randAttack = Random.Range(0, atkPrefabs.Count);
        //atkDirs[0]= standard, [1] = top atk spawn, [2] bottom, [3]Reverse start

        GameObject atk = Instantiate(atkPrefabs[randAttack], atkStarts[0].transform.position, atkStarts[0].transform.rotation);

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

    public void hitNow()
    {
        _playerHP.DamagePlayer(Random.Range(damgMin, damgMax), (int)myAbilities[0]);
        
        if (myAbilities[0] == Ability.steal)
        {
            int randRob = Random.Range(2, 4);
            _GM.robPlayer(randRob);
            amountRobbed += randRob;
        }
    }
    #endregion

    IEnumerator healEnmRoutine()
    {
        curState = attackState.waiting;
         bool hitIf = false;
         targetally = new enemy();
        

        yield return new WaitForSeconds(Random.Range(randWaitmin + waitTimerOffset, randWaitmax + waitTimerOffset));

        foreach (enemy i in enmsSys.aliveEnemys)
        {
            if (i.HP < i.maxHP)
            {
                targetally = i;
                hitIf = true;
                break;
            }
        }


        if (hitIf)
        {
            HealingUI();
            curState = attackState.readying;
            yield return new WaitForSeconds(readyingTimer);

            curState = attackState.swinging;
            yield return new WaitForSeconds(strikeTimer);


            //soundMRef.PlaySound("heal"); make a heal sound

            myActionRoutine = StartCoroutine(healEnmRoutine());
        }
        else
        {
            myActionRoutine = StartCoroutine(TheAttackRoutine());
        }
        

    }

    public void healAllyNow()
    {
        targetally.healEnm(Random.Range(healMin, healMax));
    }

    IEnumerator RunRoutine()
    {
        yield return new WaitForSeconds(Random.Range(randWaitmin, randWaitmax));
        //which then has to when hit stop this routine and if not it just destroys the enm thief clone
        GameObject run = Instantiate(specialPrefabs[0], atkStarts[3].transform.position, atkStarts[3].transform.rotation);
        run.GetComponent<EnmAtKArea>().Setstuff(this, atkStarts[0].transform, SpecialDirs[0]);
        currentAttacks.Add(run);
        yield return new WaitForSeconds(2);
    }

    public void IRan()
    {
        if (enmsSys.aliveEnemys.Count >= 1)
        {
            enmsSys.aliveEnemys.Remove(this);
            enmsSys.OpenTimer = 1.5f;
            enmsSys.UpdateEnmsPos();
        }
        Destroy(this.gameObject);
    }

    private void OnValidate()
    {
        if (randWaitmax < randWaitmin)
            randWaitmax = randWaitmin;
    }

    

    public void HealingUI()
    {
        GameObject heal = Instantiate(specialPrefabs[0], atkStarts[3].transform.position, atkStarts[3].transform.rotation);
        //var dir = Random.Range(0, healDirs.Count);
        heal.GetComponent<EnmAtKArea>().Setstuff(this, atkStarts[0].transform,SpecialDirs[0]);
        var newList = new List<GameObject>();
        if (currentAttacks.Count > 0)
            foreach (var swing in currentAttacks)
                if (swing != null)
                    newList.Add(swing);

        newList.Add(heal);
        currentAttacks = newList;
    }

    IEnumerator Flash()
    {
        GetComponent<SpriteRenderer>().material = matWhite;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material = matDefault;
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

    public void SetTargetPointer(Sprite img)
    {
        HPPointer.SetActive(true);
        HPPointer.GetComponent<SpriteRenderer>().sprite = img;
    }
 
}
