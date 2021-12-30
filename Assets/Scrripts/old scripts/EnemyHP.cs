using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public Image enemyHPBar;
    float enemyHP, enemyMaxHp = 100;
    float lerpSpeed;
    HealthBar playerHP;
    
    
    public float atkBaseTimer;
    

    //animation stuff
    public GameObject enmGuy;
    private basicEnmAnim enmanim;

    [SerializeField] float State;

    [SerializeField] float waitTimer;
    //waitTimer max is a random number

    [SerializeField] float readyingTimer, readyingTimerMax;
    
    [SerializeField] float strikeTimer,strikeTimerMax;

    [SerializeField] bool vanurable = false;

    [SerializeField] float damgThreash;
    void Start()
    {
        playerHP = GetComponent<HealthBar>();
        enemyHP = enemyMaxHp;
        waitTimer = atkBaseTimer + Random.Range(1f, 2f);
        readyingTimer = readyingTimerMax;
        strikeTimer = strikeTimerMax;

        enmanim = enmGuy.GetComponent<basicEnmAnim>();
    }

    // Update is called once per frame
    void Update()
    {

        //Hp ifs
        if (enemyHP <= 0)
        {
            //respawn stuff
            waitTimer = Random.Range(1f, 2f);
            NewEnemy();
        }
        if (enemyHP > enemyMaxHp)
        {
            enemyHP = enemyMaxHp;
        }

        //attacking ifs
        if (waitTimer>0)
        {
            waitTimer -= Time.deltaTime;
            State = 0;
        }else
        {
            //playerHP.DamagePlayer(Random.Range(10f,20f));
            //waitTimer = atkBaseTimer+Random.Range(1f,3f);
            if (readyingTimer > 0)
            {
                vanurable = true;
                readyingTimer -= Time.deltaTime;
                State = 1;
            }else
            {
                if (strikeTimer > 0)
                {
                    State = 2;
                    strikeTimer -= Time.deltaTime;
                }
                else 
                {
                    playerHP.DamagePlayer(Random.Range(10f, 20f));
                    waitTimer = Random.Range(1f, 2f);
                    readyingTimer = readyingTimerMax;
                    strikeTimer = strikeTimerMax;
                    vanurable = false;
                }
            }
        }
        
        lerpSpeed = 3f * Time.deltaTime;


        enmanim.setState(State);
        fillEnemyBar();
    }

    void fillEnemyBar()
    {
        //enemyHPBar.fillAmount = enemyHP / Mathf.Lerp(enemyHPBar.fillAmount, enemyHP / enemyMaxHp, lerpSpeed);
        enemyHPBar.fillAmount = enemyHP / enemyMaxHp;
    }

    public void DamageEnemy(float deal)
    {
        enemyHP -= deal;
        if (vanurable && deal >damgThreash)
        {
            waitTimer = Random.Range(2f, 3f);
            readyingTimer = readyingTimerMax;
            strikeTimer = strikeTimerMax;
            vanurable = false;
            enmanim.setState(5f);
        }
    }

    public void HealEnemy(float heal)
    {
        enemyHP += heal;
    }

    public void NewEnemy()
    {
        enemyHP = enemyMaxHp;
    }

}
