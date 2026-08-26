using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraStrikeArea : MonoBehaviour
{
    private Camera mc;
    private EnemysManager enmySys;
    private bool inStrikeArea = false;
    private float timer,damgMult = 1, defaultDamgMult=1, baseDamage,maxDamage;
    [SerializeField] List<int> target;
    public Weapon MyWeapon;
    [SerializeField] int WhichAreaMe;
    private StrikePoint strikePoint;
    private bool justStruck = false;

    void Start()
    {
        mc = Camera.main;
        enmySys = mc.GetComponent<EnemysManager>();
        strikePoint = FindObjectOfType<StrikePoint>();
        MyWeapon = Instantiate(MyWeapon);
        SetExtrasWeapon(MyWeapon);
        CheckTarget();
    }

    private void Update()
    {


        if (strikePoint.mostRecentX < 1.5)
        { damgMult = MyWeapon.damageMults[0]; }
        else if (strikePoint.mostRecentX >= 1.5 && strikePoint.mostRecentX < 2.1)
        { damgMult = MyWeapon.damageMults[1]; }
        else if (strikePoint.mostRecentX >= 2.1 && strikePoint.mostRecentX < 2.84)
        { damgMult = MyWeapon.damageMults[2]; }
        else if (strikePoint.mostRecentX >= 2.84 && strikePoint.mostRecentX < 3.6)
        { damgMult = MyWeapon.damageMults[3]; }
        else if (strikePoint.mostRecentX >= 3.6 && strikePoint.mostRecentX < 4.32)
        { damgMult = MyWeapon.damageMults[4]; }
        else if (strikePoint.mostRecentX >= 4.32)
        { damgMult = MyWeapon.damageMults[5]; }

        //original if numbers were a bit off (1.5,3,4 those were the lines had less variability(multipliers were 1, 8, 12, 20))
        // new correct numbers that line up with the strike are builder are as follows
        // 1.35,2.1,2.84,3.6,4.32
        //dif:.75,.74,.76,.72 I am okay with these they don't need to be perfect, it comes down to where the center is located which implies
        //as long as most of the pointer is in one area you are fine

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
                    EnemyHPBarPlacerManager.instance.DamageEnemy(Damger, target[lcv], MyWeapon.effs);
                    
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
        foreach (WeaponEffect we in MyWeapon.effs)
        {
            if (we == WeaponEffect.odachi)
            {
                target.Clear();
                target.Add(0);
                if (enmySys.aliveEnemys.Count > 2)
                {
                    target.Add(2);
                }
            }
            else if (we == WeaponEffect.FourTarget)
            {
                if (enmySys.aliveEnemys.Count == 1)
                {
                    target.Clear();
                    target.Add(0);
                }
                else if (enmySys.aliveEnemys.Count == 2)
                {
                    if (WhichAreaMe == 1)
                    {
                        target.Clear();
                        target.Add(1);
                    }
                    else
                    {
                        target.Clear();
                        target.Add(0);
                    }
                }
                else if (enmySys.aliveEnemys.Count ==3)
                {
                    target.Clear();
                    target.Add(WhichAreaMe-1);
                    //for now might change later, so the top 2 are the front most enemy, though maybe the middle 2 should be
                }
                else if (enmySys.aliveEnemys.Count == 4)
                {
                    target.Clear();
                    target.Add(WhichAreaMe);
                }
            }
            else if (we == WeaponEffect.ThreeTarget)
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
