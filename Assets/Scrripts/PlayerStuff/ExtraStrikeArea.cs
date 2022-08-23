using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraStrikeArea : MonoBehaviour
{
    private Camera mc;
    private GameManager GM;
    private EnemysSystem enmySys;
    private SoundManager SoundMng;
    private StrikeArea mainArea;
    private bool indere,timering = false;
    private float timer,damgMult, defaultDamgMult, baseDamg,maxDamg;
    [SerializeField] List<int> target;
    public Weapon MyWeapon;
    [SerializeField] int WhichAreaMe;
    private StrikePoint strikePoint;

    void Awake()
    {
        mainArea = FindObjectOfType<StrikeArea>();
        mc = Camera.main;
        GM = mc.GetComponent<GameManager>();
        enmySys = mc.GetComponent<EnemysSystem>();
        strikePoint = FindObjectOfType<StrikePoint>();
        SoundMng = FindObjectOfType<SoundManager>();
        SetExtrasWeapon(MyWeapon);
        CheckTarget();
    }

    private void Update()
    {

        if (timering)
        {
            timer += Time.deltaTime;
        }

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
                timering = true;
                timer = 0;
                damgMult = defaultDamgMult;
                timer = 0;

            }

            CheckTarget();

            if (Input.GetKeyUp(KeyCode.Space) && indere)
            {
                float Damger = Mathf.Clamp(baseDamg + (timer * damgMult), 0, maxDamg);
                
                for (int lcv = 0; lcv < target.Count; lcv++)
                {
                    enmySys.DamageEnemy(Damger, target[lcv], MyWeapon.effs);
                    SoundMng.PlaySound("hit");
                }

                timer = 0;
                timering = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
            indere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        indere = false;
    }

    private void CheckTarget()
    {
        //reformat to case statement thing maybe

        if (MyWeapon.effs[0] == WeaponEffect.odachi)
        {

        }
        else if (MyWeapon.effs[0] == WeaponEffect.bow)
        {
            if (enmySys.enms.Count == 1)
            {
                target.Clear();
                target.Add(0);
            }
            else if (enmySys.enms.Count == 2)
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
            else if (enmySys.enms.Count == 3)
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
            else if (enmySys.enms.Count == 4)
            {
                target.Clear();
                target.Add(WhichAreaMe);

            }
        }
    }

    private void SetExtrasWeapon(Weapon wee)
    {
        baseDamg = wee.baseDamg;
        maxDamg = wee.maxDamg;
    }
}
