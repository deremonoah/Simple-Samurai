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
    [SerializeField] GameObject atkPrefab, specialPrefab;
    [SerializeField] List<GameObject> atkStarts;
    [SerializeField] GameObject atkEnd;
    [SerializeField] List<Vector2> atkDirs,SpecialDirs;
    attackState curState;
    public Ability myAbility;
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

    public void SetThings( List<GameObject> str, GameObject end)
    {
        atkStarts = str;
        
        atkEnd = end;
    }

    public void StartMyRoutine()
    {
        if (myAbility == Ability.heal)
        {
            myattackRoutine = StartCoroutine(healEnmRoutine());
        }
        else if (myAbility == Ability.steal && amountRobbed > 5)
        {
            myattackRoutine = StartCoroutine(RunRoutine());
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

        StartMyRoutine();


    }

    public void hitNow()
    {
        playerHP.DamagePlayer(Random.Range(damgMin, damgMax), (int)myAbility);
        soundMRef.PlaySound("hit");
        if (myAbility == Ability.steal)
        {
            int randRob = Random.Range(2, 4);
            GM.robPlayer(randRob);
            amountRobbed += randRob;
        }
    }

    IEnumerator healEnmRoutine()
    {
        curState = attackState.waiting;
         bool hitIf = false;
        enmy targetally = new enmy();
        

        yield return new WaitForSeconds(Random.Range(randWaitmin, randWaitmax));

        foreach (enmy i in enmsSys.enms)
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

            targetally.Healenm(Random.Range(healMin, healMax));
            //soundMRef.PlaySound("heal"); make a heal sound

            myattackRoutine = StartCoroutine(healEnmRoutine());
        }
        else
        {
            myattackRoutine = StartCoroutine(TheattackRoutine());
        }
        

    }

    IEnumerator RunRoutine()
    {
        yield return new WaitForSeconds(1);
        //create run away ui 
        //which then has to when hit stop this routine and if not it just destroys the enm thief clone
        GameObject run = Instantiate(specialPrefab, atkStarts[3].transform.position, atkStarts[3].transform.rotation);
        run.GetComponent<EnmAtKArea>().Setstuff(this, atkStarts[0].transform, SpecialDirs[0]);
        yield return new WaitForSeconds(2);
    }

    public void IRan()
    {
        if (enmsSys.enms.Count == 1)
        {
            enmsSys.enms.Remove(this);
            enmsSys.OpenTimer = 1.5f; 
        }
        Destroy(this.gameObject);
    }

    private void OnValidate()
    {
        if (randWaitmax < randWaitmin)
            randWaitmax = randWaitmin;
    }

    public void StrikeUI()
    {
        var dir = Random.Range(0, atkDirs.Count);
        //atkDirs[0]= standard, [1] = top atk spawn, [2] bottom, [3]Reverse start

        GameObject atk = Instantiate(atkPrefab, atkStarts[0].transform.position, atkStarts[0].transform.rotation);

        if (atkDirs[dir].y == 0)
        {
            atk.transform.position= atkStarts[0].transform.position;

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
        if (curAtks.Count > 0)
            foreach (var swing in curAtks)
                if (swing != null)
                    newList.Add(swing);

        newList.Add(atk);
        curAtks = newList;

    }

    public void HealingUI()
    {
        GameObject heal = Instantiate(specialPrefab, atkStarts[3].transform.position, atkStarts[3].transform.rotation);
        //var dir = Random.Range(0, healDirs.Count);
        heal.GetComponent<EnmAtKArea>().Setstuff(this, atkStarts[0].transform,SpecialDirs[0]);
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

    public void SetTargetPointer(Sprite img)
    {
        HPPointer.SetActive(true);
        HPPointer.GetComponent<SpriteRenderer>().sprite = img;
    }


}
