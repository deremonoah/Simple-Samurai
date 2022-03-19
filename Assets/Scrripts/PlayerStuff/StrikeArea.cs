using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeArea : MonoBehaviour
{
    private EnemysSystem enmySys;
    public Camera mc;
    private GameManager GM;
    public static bool PlayerOn = true;
    [SerializeField] bool indere;
    [SerializeField] float maxDamg = 70;
    [SerializeField] float baseDamg;
    [SerializeField] float damgMult;
    [SerializeField] float defaultDamgMult;
    [SerializeField] List<int> target;

    [SerializeField] float timer = 0;
    [SerializeField]bool timering=false;
    [SerializeField] GameObject bottomOdachi;
    [SerializeField] List<GameObject> BowAreas;

    public SoundManager SoundMng;

    public GameObject strikePointObj;

    //myStrikeAreaSprite.sprite = the sprite you want from weapon
    private SpriteRenderer myStrikeAreaSprite;
    public Weapon equipedWeapon;
    public Weapon Test;
    void Start()
    {
        GM = mc.GetComponent<GameManager>();
        enmySys = mc.GetComponent<EnemysSystem>();
        myStrikeAreaSprite = GetComponent<SpriteRenderer>();
        Debug.Log("started");
    }

    
    void Update()
    {
        if (timering)
        {
            timer += Time.deltaTime;
        }

        if (timer > 2.5 && timer <4.4)
        { damgMult = 12; }
        else if (timer > 4.5)
        { damgMult = 20; }

        if (PlayerOn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timering = true;
                timer = 0;
                damgMult = defaultDamgMult;
                timer = 0;

            }


            if (Input.GetKeyUp(KeyCode.Space) && indere)
            {
                float Damger = Mathf.Clamp(baseDamg + (timer * damgMult), 0, maxDamg);
                for (int lcv = 0; lcv < target.Count; lcv++)
                {
                    SoundMng.PlaySound("hit");
                    enmySys.DamageEnemy(Damger, target[lcv], equipedWeapon.effs);
                    if (Damger >= 25f && equipedWeapon.effs[0] == WeaponEffect.greed)
                    {
                        GM.PayOut(1);
                    }
                }

                timer = 0;
                timering = false;
            }
        }

        
        
          if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(Test);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetComponent<PolygonCollider2D>().isTrigger = true;
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

    private void TurnBow(bool iss)
    {
        for (int lcv = 0; lcv < BowAreas.Count; lcv++)
        {
            BowAreas[lcv].SetActive(iss);
        }
    }

    public void SetWeapon(Weapon wee)
    {
        baseDamg = wee.baseDamg;
        maxDamg = wee.maxDamg;
        myStrikeAreaSprite.sprite = wee.myStrikeArea;
        strikePointObj.GetComponent<StrikePoint>().ChangeStrikeSprite(wee.strikePointer);
        //get help figureing out how to refresh spritet colider or why it didnt work the old way that you deleted 
        var colld = GetComponent<PolygonCollider2D>();
        DestroyImmediate(colld);
        colld = gameObject.AddComponent<PolygonCollider2D>();
        colld.isTrigger = true;

        equipedWeapon = wee;
        for (int lcv = 0; lcv<wee.effs.Count; lcv++)
        {
            Debug.Log("lcv: "+lcv);
            Debug.Log("count: " + wee.effs.Count);

            if (wee.effs[lcv] == WeaponEffect.odachi)
            {
                bottomOdachi.SetActive(true);
                target.Add(1);
            }
            else
            {
                bottomOdachi.SetActive(false);
                if (target.Count > 1)
                {
                    target.RemoveAt(1);
                }
            }

            if (wee.effs[lcv] == WeaponEffect.bow)
            {
                TurnBow(true);
            }
            else
            {
                TurnBow(false);
            }
        }
    }

    public static void SwitchPlayerOn(bool tf)
    {
        PlayerOn = tf;
    }
}
