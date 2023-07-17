using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeArea : MonoBehaviour
{
    private EnemysManager _enemySystem;
    public Camera mc;
    private GameManager GM;
    public static bool PlayerOn = true;
    [SerializeField] bool inStrikeArea;
    [SerializeField] float maxDamage;
    [SerializeField] float baseDamage;
    [SerializeField] float damgMult=2;
    [SerializeField] float defaultDamageMult;
    [SerializeField] List<int> targetEnemy;

    private float _JustStruckTimer = 0;
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
    public Weapon PrimaryWeapon;
    public Weapon SecondaryWeapon;
    private bool dualWielding = false;
    public Weapon TestWeapon;
    void Start()
    {
        GM = mc.GetComponent<GameManager>();
        _enemySystem = mc.GetComponent<EnemysManager>();
        myStrikeAreaSprite = GetComponent<SpriteRenderer>();
        SoundMng = FindObjectOfType<SoundManager>();
        strikePoint = strikePointObj.GetComponent<StrikePoint>();
        justStruck = false;
        equipedWeapon = Instantiate(equipedWeapon);
        TestWeapon = Instantiate(TestWeapon);

        //2 weapon system
        PrimaryWeapon = equipedWeapon;
        SecondaryWeapon = Instantiate(SecondaryWeapon);
    }

    
    void Update()
    {

        if (strikePoint.mostRecentX < 1.5)
        { damgMult = 2; }
        else if (strikePoint.mostRecentX >= 1.5 && strikePoint.mostRecentX < 3)
        { damgMult = 8; }
        else if (strikePoint.mostRecentX >= 3 && strikePoint.mostRecentX < 4)
        { damgMult = 12; }
        else if (strikePoint.mostRecentX >= 4.5)
        { damgMult = 18; }


        if (PlayerOn)
        {

            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && inStrikeArea && !justStruck)
            {
                float Damger = Mathf.Clamp((strikePoint.mostRecentX * damgMult) + baseDamage, 0, maxDamage);

                for (int lcv = 0; lcv < targetEnemy.Count; lcv++)
                {
                    Debug.Log(Damger +"  damgMult: "+damgMult + "  most recentX: "+strikePoint.mostRecentX);
                    _enemySystem.DamageEnemy(Damger, targetEnemy[lcv], equipedWeapon.effs);
                    SoundMng.PlaySound("hit", Damger);
                    justStruck = true;
                    PlayerOn = false;
                    _JustStruckTimer = 0.1f;
                    //Debug.Log("Enemy: "+targetEnemy[lcv] + "   damage: " + Damger);


                    if (Damger >= 20f && equipedWeapon.effs[0] == WeaponEffect.greed)
                    {
                        if (Damger >= 30)
                        { GM.PayOut(2, 3); }
                        else { GM.PayOut(1, 2); }
                    }
                }
            }

            //all that needs be done is a curio that allows dual weilding or "weapon swapping" or an unlocked player ability
            //current issue is that the transitions feels super jank like its just super abrupt and if you swing fast it basically flashes
            //the idea was you could make a concious decision about what weapon you would want when but that doesn't really happen
            //when I was playing it was still just get that pointer as far as it will go and do some damage!
            if((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && dualWielding)
            {
                if (equipedWeapon == PrimaryWeapon)
                {
                    if (SecondaryWeapon != null)
                    { SetWeapon(SecondaryWeapon); }
                }
                else
                {
                    SetWeapon(PrimaryWeapon);
                }
            }



        }

        if (_JustStruckTimer<0 && justStruck)
        {
            justStruck = false;
            PlayerOn = true;
        }
        else { _JustStruckTimer -= Time.deltaTime; }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(TestWeapon);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            equipedWeapon.itemLevel += 1;
            baseDamage = equipedWeapon.baseDamageLevel[equipedWeapon.itemLevel];
            maxDamage = equipedWeapon.maxDamageLevel[equipedWeapon.itemLevel];
        }
#endif

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

    private void TurnBow(bool iss,Weapon wee)
    {
        for (int lcv = 0; lcv < BowAreas.Count; lcv++)
        {
            BowAreas[lcv].SetActive(iss);
            if (iss)
            { BowAreas[lcv].GetComponent<ExtraStrikeArea>().SetExtrasWeapon(wee); }
        }
    }

    public void SetWeapon(Weapon wee)
    {
        baseDamage = wee.baseDamageLevel[wee.itemLevel];
        maxDamage = wee.maxDamageLevel[wee.itemLevel];
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
                if (targetEnemy.Count > 1)
                {
                    targetEnemy.RemoveAt(1);
                }
                targetEnemy.Add(1);
                bottomOdachi.GetComponent<ExtraStrikeArea>().SetExtrasWeapon(wee);
            }
            else
            {
                bottomOdachi.SetActive(false);
                if (targetEnemy.Count > 1)
                {
                    targetEnemy.RemoveAt(1);
                }
            }

            if (wee.effs[lcv] == WeaponEffect.bow)
            {
                TurnBow(true,wee);
            }
            else
            {
                TurnBow(false,wee);
            }

            if (wee.effs[lcv] == WeaponEffect.greed)
            {
                //strikePointObj.GetComponent<StrikePoint>().SetBoundsSmaller();
            }else
            {
                //strikePointObj.GetComponent<StrikePoint>().SetBoundsRegular();
            }
        }
    }

    public static void SwitchPlayerOn(bool tf)
    {
        PlayerOn = tf;
    }

    public void BeingBlocked(bool isblocked)
    {
        inStrikeArea = !isblocked;
    }
}
