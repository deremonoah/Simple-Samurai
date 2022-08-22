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
    [SerializeField] float damgMult=1;
    [SerializeField] float defaultDamgMult;
    [SerializeField] List<int> target;

    [SerializeField] float timer = 0;
    [SerializeField]bool timering=false;
    [SerializeField] GameObject bottomOdachi;
    [SerializeField] List<GameObject> BowAreas;
    public List<Sprite> BowPointers;

    public SoundManager SoundMng;

    public GameObject strikePointObj;
    private StrikePoint strikePoint;
    public bool justStruck;

    //myStrikeAreaSprite.sprite = the sprite you want from weapon
    private SpriteRenderer myStrikeAreaSprite;
    public Weapon equipedWeapon;
    public Weapon Test;
    void Start()
    {
        GM = mc.GetComponent<GameManager>();
        enmySys = mc.GetComponent<EnemysSystem>();
        myStrikeAreaSprite = GetComponent<SpriteRenderer>();
        SoundMng = FindObjectOfType<SoundManager>();
        strikePoint = strikePointObj.GetComponent<StrikePoint>();
        justStruck = false;
    }

    
    void Update()
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
        

        if (PlayerOn)
        {

            if ((Input.GetKeyUp(KeyCode.Space)|| Input.GetKeyUp(KeyCode.Mouse0)) && indere && !justStruck)
            {
                float Damger = baseDamg + Mathf.Clamp(strikePoint.mostRecentX * damgMult, 0, maxDamg);
                
                for (int lcv = 0; lcv < target.Count; lcv++)
                {
                    Debug.Log(Damger +"  damgMult: "+damgMult + "  most recentX: "+strikePoint.mostRecentX);
                    enmySys.DamageEnemy(Damger, target[lcv], equipedWeapon.effs);
                    SoundMng.PlaySound("hit");
                    justStruck = true;
                    timer = 0.1f;
                    if (Damger >= 15f && equipedWeapon.effs[0] == WeaponEffect.greed)
                    {
                        if(Damger >=25)
                        { GM.PayOut(2); }
                        else { GM.PayOut(1); }
                    }
                }


            }
        }

        if (timer<0 && justStruck)
        {
            justStruck = false;
        }
        else { timer -= Time.deltaTime; }
        
          if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(Test);
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

            if (wee.effs[lcv] == WeaponEffect.greed)
            {
                strikePointObj.GetComponent<StrikePoint>().SetBoundsSmaller();
            }else
            {
                strikePointObj.GetComponent<StrikePoint>().SetBoundsRegular();
            }
        }
    }

    public static void SwitchPlayerOn(bool tf)
    {
        PlayerOn = tf;
    }
}
