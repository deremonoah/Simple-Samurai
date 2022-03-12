using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enmy : MonoBehaviour
{
    //stats
    [SerializeField] float HP, maxHP, armor,damgThreash,damgMin, damgMax;
    
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
    [SerializeField] GameObject atkPrefab;
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

    private Coroutine attackRoutine;


    enum attackState 
    { 
        waiting,readying,swinging,damaging,damaged
    }

    public enum Ability
    {
        none,steal, antiarmor
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

        
         attackRoutine = StartCoroutine(attack());

        
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

    public void HealEnemy(float heal)
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
            }
        }
        if (antArm)
        {
            HP -= (deal - armor);
        }
        else
        {
            HP -= deal;
        }
        
        curState = attackState.damaged;
        StartCoroutine(Flash());
        
    }

    public void Blocked()
    {
        if(attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }

        attackRoutine = StartCoroutine(attack());
    }

    public void SetThings( GameObject str, GameObject end)
    {
        atkStart = str;
        atkEnd = end;
    }

    IEnumerator attack()
    {
        curState = attackState.waiting;
        yield return new WaitForSeconds(Random.Range(randWaitmin, randWaitmax));


        Strike();
        curState = attackState.readying;
        yield return new WaitForSeconds(readyingTimer);

        curState = attackState.swinging;
        yield return new WaitForSeconds(strikeTimer);
        playerHP.DamagePlayer(Random.Range(damgMin, damgMax));
        if (myAbility == Ability.steal)
        {
            int randRob = Random.Range(2, 4);
            GM.robPlayer(randRob);
            amountRobbed += randRob;
        }
            

        attackRoutine = StartCoroutine(attack());
    }

    private void OnValidate()
    {
        if (randWaitmax < randWaitmin)
            randWaitmax = randWaitmin;
    }

    public void Strike()
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

    IEnumerator Flash()
    {
        GetComponent<SpriteRenderer>().material = matWhite;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material = matDefault;
    }

    IEnumerator OnFire()
    {
        yield return null;
    }

    public void SetAsTarget()
    {
        HPPointer.SetActive(true);
    }


}
