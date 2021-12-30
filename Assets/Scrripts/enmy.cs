using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enmy : MonoBehaviour
{
    //stats
    [SerializeField] float HP, maxHP, armor,damgThreash,damgMin, damgMax;
    

    //mostly timers n stuff
    [SerializeField] float waitTimer, waitTimerMax,randWaitmin, randWaitmax, readyingTimer, readyingTimerMax, strikeTimer;

    public Camera mainCam;
    private HealthBar playerHP;
    
    
    //animation stuff
    private Animator anim;
    
    public Image myHPBar;
    [SerializeField] EnmyHPsSystem enmyhpsystem;
    [SerializeField] EnemysSystem enmsSys;
    [SerializeField] int myPos;

    //attack projectile stuff
    [SerializeField] GameObject atkPrefab;
    [SerializeField] GameObject atkStart;
    [SerializeField] GameObject atkEnd;
    attackState curState;

    //in refrance to position in enemySystem's prefab list
    [SerializeField] int myType;
    enum attackState 
    { 
        waiting,readying,swinging,damaging,damaged

    }

    void Start()
    {
        mainCam = Camera.main;
        playerHP = mainCam.GetComponent<HealthBar>();
        enmyhpsystem = mainCam.GetComponent<EnmyHPsSystem>();
        enmsSys = mainCam.GetComponent<EnemysSystem>();
        myPos = enmsSys.GetPos();
        myHPBar = enmyhpsystem.GetMyHPBar(myPos);

        anim = GetComponent<Animator>();
        HP = maxHP;
        waitTimer = waitTimerMax + Random.Range(randWaitmin, randWaitmax);
        
        
        
        anim = GetComponent<Animator>();

        StartCoroutine(attack());
    }

    
    void Update()
    {
        //Hp ifs
        if (HP<=0)
        {
            enmsSys.Died(myPos);
            Destroy(myHPBar);
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

    public void damgEnemy(float deal)
    {
        HP -= deal - armor;
        curState = attackState.damaged;
        
    }
    public void SetPos(int temppos)
    {
        myPos = temppos;
    }
    public void Blocked()
    {
        
        StopAllCoroutines();
        StartCoroutine(attack());
    }

    public void SetThings(GameObject prefb, GameObject str, GameObject end)
    {
        atkPrefab = prefb;
        atkStart = str;
        atkEnd = end;
    }

    IEnumerator attack()
    {
        curState = attackState.waiting;
        yield return new WaitForSeconds(waitTimer);

        
        GameObject atk = Instantiate(atkPrefab, atkStart.transform.position, atkStart.transform.rotation);
        atk.GetComponent<EnmAtKArea>().Setstuff(this, atkEnd.transform);
        curState = attackState.readying;
        yield return new WaitForSeconds(readyingTimer);

        curState = attackState.swinging;
        yield return new WaitForSeconds(strikeTimer);
        playerHP.DamagePlayer(Random.Range(damgMin, damgMax));

        StartCoroutine(attack());
    }

    public int getType()
    {
        return myType;
    }
}
