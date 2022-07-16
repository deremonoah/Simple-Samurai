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
    [SerializeField] float WhichAreaMe;
    void Awake()
    {
        mainArea = FindObjectOfType<StrikeArea>();
        mc = Camera.main;
        GM = mc.GetComponent<GameManager>();
        enmySys = mc.GetComponent<EnemysSystem>();
        SoundMng = FindObjectOfType<SoundManager>();
        SetExtrasWeapon(MyWeapon);
        Debug.Log("am awake");
        CheckTarget();
    }

    private void Update()
    {

        if (timering)
        {
            timer += Time.deltaTime;
        }

        if (timer > 2.5 && timer < 4.4)
        { damgMult = 12; }
        else if (timer > 4.5)
        { damgMult = 20; }

        if (StrikeArea.PlayerOn)
        {
            Debug.Log("player on");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timering = true;
                timer = 0;
                damgMult = defaultDamgMult;
                timer = 0;

            }


            if (Input.GetKeyUp(KeyCode.Space) && indere)
            {
                Debug.Log("up and in dere");
                float Damger = Mathf.Clamp(baseDamg + (timer * damgMult), 0, maxDamg);
                CheckTarget();
                for (int lcv = 0; lcv < target.Count; lcv++)
                {
                    
                    Debug.Log("should hit");
                    enmySys.DamageEnemy(Damger, target[lcv], MyWeapon.effs);
                    SoundMng.PlaySound("hit");
                    if (Damger >= 20f && MyWeapon.effs[0] == WeaponEffect.greed)
                    {
                        GM.PayOut(1);
                        if (Damger >= 25)
                        { GM.PayOut(2); }
                        else { GM.PayOut(1); }
                    }
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
        if (enmySys.enms.Count == 1)
        {
            target.Clear();
            target.Add(0);
        }
        else if (enmySys.enms.Count == 2)
        {
            if (WhichAreaMe == 1)
            {
                target.Clear();
                target.Add(0);
            }else
            {
                target.Clear();
                target.Add(1);
            }
        }else if (enmySys.enms.Count == 3)
        {
            if (WhichAreaMe == 3)
            {
                target.Clear();
                target.Add(0);
            }else
            {
                target.Clear();
                target.Add((int)WhichAreaMe);
            }
        }else if (enmySys.enms.Count == 4)
        {
            target.Clear();
            target.Add((int)WhichAreaMe);
        }
    }

    private void SetExtrasWeapon(Weapon wee)
    {
        baseDamg = wee.baseDamg;
        maxDamg = wee.maxDamg;
    }
}
