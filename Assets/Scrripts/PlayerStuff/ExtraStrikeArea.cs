using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraStrikeArea : MonoBehaviour
{
    private Camera mc;
    private GameManager GM;
    private EnemysManager enmySys;
    private SoundManager SoundMng;
    private StrikeArea mainArea;
    private bool inStrikeArea = false;
    private float timer,damgMult = 1, defaultDamgMult=1, baseDamage,maxDamage;
    [SerializeField] List<int> target;
    public Weapon MyWeapon;
    [SerializeField] int WhichAreaMe;
    private StrikePoint strikePoint;
    private bool justStruck = false;

    void Awake()
    {
        mainArea = FindObjectOfType<StrikeArea>();
        mc = Camera.main;
        GM = mc.GetComponent<GameManager>();
        enmySys = mc.GetComponent<EnemysManager>();
        strikePoint = FindObjectOfType<StrikePoint>();
        SoundMng = FindObjectOfType<SoundManager>();
        MyWeapon = Instantiate(MyWeapon);
        SetExtrasWeapon(MyWeapon);
        CheckTarget();
    }

    private void Update()
    {

        

        if (strikePoint.mostRecentX < 1.5)
        { damgMult = 1; }
        else if (strikePoint.mostRecentX >= 1.5 && strikePoint.mostRecentX < 3)
        { damgMult = 8; }
        else if (strikePoint.mostRecentX >= 3 && strikePoint.mostRecentX < 4)
        { damgMult = 12; }
        else if (strikePoint.mostRecentX >= 4)
        { damgMult = 20; }

        if (StrikeArea.PlayerOn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timer = 0;
                damgMult = defaultDamgMult;
                timer = 0;

            }

            CheckTarget();

            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && inStrikeArea && !justStruck)
            {
                float Damger = Mathf.Clamp((strikePoint.mostRecentX * damgMult) + baseDamage, 0, maxDamage);

                for (int lcv = 0; lcv < target.Count; lcv++)
                {
                    enmySys.DamageEnemy(Damger, target[lcv], MyWeapon.effs);
                    SoundMng.PlaySound("hit");
                }
                justStruck = true;
                timer = 0.1f;
            }
        }
        if (timer < 0 && justStruck)
        {
            justStruck = false;
        }
        else { timer -= Time.deltaTime; }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
            inStrikeArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inStrikeArea = false;
    }

    private void CheckTarget()
    {
        //reformat to case statement thing maybe

        if (MyWeapon.effs[0] == WeaponEffect.odachi)
        {
            target.Clear();
            target.Add(0);
            if (enmySys.aliveEnemys.Count > 2)
            {
                target.Add(1);
            }else
            {
                target.Add(2);
            }
        }
        else if (MyWeapon.effs[0] == WeaponEffect.bow)
        {
            if (enmySys.aliveEnemys.Count == 1)
            {
                target.Clear();
                target.Add(0);
            }
            else if (enmySys.aliveEnemys.Count == 2)
            {
                if (WhichAreaMe == 2)
                {
                    target.Clear();
                    target.Add(0);
                }
                else
                {
                    target.Clear();
                    target.Add(1);
                }
            }
            else if (enmySys.aliveEnemys.Count == 3)
            {
                if (WhichAreaMe == 3)
                {
                    target.Clear();
                    target.Add(0);

                }
                else
                {
                    target.Clear();
                    target.Add(WhichAreaMe);

                }
            }
            else if (enmySys.aliveEnemys.Count == 4)
            {
                target.Clear();
                target.Add(WhichAreaMe);

            }
        }
    }

    public void SetExtrasWeapon(Weapon wee)
    {
        MyWeapon = wee;
        baseDamage = wee.baseDamageLevel[wee.itemLevel];
        maxDamage = wee.maxDamageLevel[wee.itemLevel];
    }
}
