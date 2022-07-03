using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enmy : MonoBehaviour
{
    //stats
    public float HP;
    [SerializeField] float maxHP, armor,damgMin, damgMax, healMin, healMax;
    
    //mostly timers n stuff
    [SerializeField] float randWaitmin, randWaitmax, readyingTimer, strikeTimer;

    private Camera mainCam;
    private GameManager GM;
    private PlayerHealthBar playerHP;
    
    
    //animation stuff
    private Animator anim;
    
    public Image myHPBar;
    [SerializeField] EnemysSystem enmsSys;

    //attack projectile stuff
    [SerializeField] GameObject atkPrefab, healPrefab;
    [SerializeField] GameObject atkStart;
    [SerializeField] GameObject atkEnd;
    attackState curState;
    [SerializeField] Ability myAbility;
    private int amountRobbed = 0;

    private List<GameObject> curAtks = new List<GameObject>();

    public Material matWhite;
    private Material matDefault;

    [SerializeField] int minCoin, maxCoin;

    public GameObject HPPointer;
    public Canvas myCanvas;

    private Coroutine myattackRoutine;

    private SoundManager soundMRef;
    enum attackState 
    { 
        waiting,readying,swinging,damaging,damaged
    }

    public enum Ability
    {
        none,steal, antiarmor, heal, multiHeal
    }

    void Start()
    {
        mainCam = Camera.main;
        GM = mainCam.GetComponent<GameManager>();
        playerHP = mainCam.GetComponent<PlayerHealthBar>();
        enmsSys = mainCam.GetComponent<EnemysSystem>();
        anim = GetComponent<Animator>();
        HP = maxHP;
        anim = GetComponent<Animator>();
        //matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        
        matDefault = GetComponent<SpriteRenderer>().material;

        if (myAbility == Ability.heal)
        {
            myattackRoutine = StartCoroutine(healEnmRoutine());
        } else
        {
            myattackRoutine = StartCoroutine(TheattackRoutine());
        }
         

        soundMRef = FindObjectOfType<SoundManager>();    
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
            foreach (var atk in curAtks)
                Destroy(atk);
            enmsSys.Died(this);
            GM.PayOut(Random.Range(minCoin, maxCoin)+amountRobbed);
            Destroy(this.gameObject);
        }
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        /*switch(curState)
        {
            case attackState.waiting: state = 0

        }*/

        anim.SetFloat("State", (int)curState);
        fillMyHP();
        

    }
    void fillMyHP()
    {
        myHPBar.fillAmount = HP / maxHP;
    }

    public void Healenm(float heal)
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
                    break;
                case WeaponEffect.antiarmor:
                    antArm = true;
                    break;
                case WeaponEffect.lifeSteal:
                    playerHP.HealPlayer(deal / 3);
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
            { HP -= (deal - armor); }
        }
        
        curState = attackState.damaged;
        StartCoroutine(Flash());
        
    }

    public void Blocked()
    {
        if(myattackRoutine != null)
        {
            StopCoroutine(myattackRoutine);
        }

        StartMyRoutine();
    }

    public void SetThings( GameObject str, GameObject end)
    {
        atkStart = str;
        atkEnd = end;
    }

    public void StartMyRoutine()
    {
        if (myAbility == Ability.heal)
        {
            myattackRoutine = StartCoroutine(healEnmRoutine());
        }
        else
        {
            myattackRoutine = StartCoroutine(TheattackRoutine());
        }
    }

    IEnumerator TheattackRoutine()
    {
        curState = attackState.waiting;
        yield return new WaitForSeconds(Random.Range(randWaitmin, randWaitmax));


        StrikeUI();
        curState = attackState.readying;
        yield return new WaitForSeconds(readyingTimer);

        curState = attackState.swinging;
        yield return new WaitForSeconds(strikeTimer);
        playerHP.DamagePlayer(Random.Range(damgMin, damgMax));
        soundMRef.PlaySound("hit");
        if (myAbility == Ability.steal)
        {
            int randRob = Random.Range(2, 4);
            GM.robPlayer(randRob);
            amountRobbed += randRob;
        }

        StartMyRoutine();


    }

    IEnumerator healEnmRoutine()
    {
        curState = attackState.waiting;
         bool hitIf = false;
        enmy targetally = new enmy();
        

        yield return new WaitForSeconds(Random.Range(randWaitmin, randWaitmax));

        foreach (enmy i in enmsSys.enms)
        {
            Debug.Log("in foreach");
            Debug.Log("hp: " + i.HP + "   max hap: " + i.maxHP);
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

            targetally.Healenm(Random.Range(healMin, healMax));
            //soundMRef.PlaySound("heal"); make a heal sound


            
        }
        StartMyRoutine();

    }

    private void OnValidate()
    {
        if (randWaitmax < randWaitmin)
            randWaitmax = randWaitmin;
    }

    public void StrikeUI()
    {
        GameObject atk = Instantiate(atkPrefab, atkStart.transform.position, atkStart.transform.rotation);
        atk.GetComponent<EnmAtKArea>().Setstuff(this, atkEnd.transform);
        var newList = new List<GameObject>();
        if (curAtks.Count > 0)
            foreach (var swing in curAtks)
                if (swing != null)
                    newList.Add(swing);

        newList.Add(atk);
        curAtks = newList;
    }

    public void HealingUI()
    {
        GameObject heal = Instantiate(healPrefab, atkStart.transform.position, atkStart.transform.rotation);
        heal.GetComponent<EnmAtKArea>().Setstuff(this, atkEnd.transform);
        var newList = new List<GameObject>();
        if (curAtks.Count > 0)
            foreach (var swing in curAtks)
                if (swing != null)
                    newList.Add(swing);

        newList.Add(heal);
        curAtks = newList;
    }

    IEnumerator Flash()
    {
        GetComponent<SpriteRenderer>().material = matWhite;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material = matDefault;
    }

    IEnumerator OnFire()
    {
        yield return new WaitForSeconds(1f);
        HP -= 1;
        int randNum = Random.Range(0, 4);
        if (randNum < 3)
        {
            StartCoroutine(OnFire());
        }
    }

    public void SetAsTarget()
    {
        HPPointer.SetActive(true);
    }


}
