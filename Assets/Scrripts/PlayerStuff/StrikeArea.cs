using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeArea : MonoBehaviour
{
    private EnemysManager _enemySystem;
    public Camera mc;
    private GameManager GM;
    private PlayerEquipedItemsManager _myItemsManager;
    public static bool PlayerOn = true;
    [SerializeField] bool inStrikeArea;
    //buff area refers to what buff it gives that it got from what ever buff area
    [SerializeField] int inBuffArea;
    [SerializeField] int currentBuff;

    [SerializeField] float maxDamage;
    [SerializeField] float baseDamage;
    [SerializeField] float damgMult=2;
    [SerializeField] float defaultDamageMult;
    [SerializeField] List<int> targetEnemy;

    private float _JustStruckTimer = 0;
    [SerializeField] GameObject bottomOdachi;
    [SerializeField] List<GameObject> BowAreas;
    public List<Sprite> BowPointers;

    [SerializeField] List<GameObject> shurikenAreas;
    //will need shuriken pointers and that functionality working eventually

    public SoundManager SoundMng;

    public GameObject strikePointObj;
    private StrikePoint _mystrikePoint;
    public bool justStruck;

    //myStrikeAreaSprite.sprite = the sprite you want from weapon
    private SpriteRenderer myStrikeAreaSprite;
    public Weapon equipedWeapon;

    private float revengeTimer;

    public Weapon TestWeapon;
    void Start()
    {
        GM = mc.GetComponent<GameManager>();
        _enemySystem = mc.GetComponent<EnemysManager>();
        _myItemsManager = FindObjectOfType<PlayerEquipedItemsManager>();
        myStrikeAreaSprite = GetComponent<SpriteRenderer>();
        SoundMng = FindObjectOfType<SoundManager>();
        _mystrikePoint = strikePointObj.GetComponent<StrikePoint>();
        justStruck = false;
        SetWeapon(_myItemsManager.PrimaryWeapon);
        TestWeapon = Instantiate(TestWeapon);
        inBuffArea = -1;
    }

    
    void Update()
    {

        if (_mystrikePoint.mostRecentX < 1.5)
        { damgMult = equipedWeapon.damageMults[0]; }
        else if(_mystrikePoint.mostRecentX >= 1.5 && _mystrikePoint.mostRecentX < 2.1)
        { damgMult = equipedWeapon.damageMults[1]; }
        else if (_mystrikePoint.mostRecentX >= 2.1 && _mystrikePoint.mostRecentX < 2.84)
        { damgMult = equipedWeapon.damageMults[2]; }
        else if (_mystrikePoint.mostRecentX >= 2.84 && _mystrikePoint.mostRecentX < 3.6)
        { damgMult = equipedWeapon.damageMults[3]; }
        else if (_mystrikePoint.mostRecentX >= 3.6 && _mystrikePoint.mostRecentX < 4.32)
        { damgMult = equipedWeapon.damageMults[4]; }
        else if (_mystrikePoint.mostRecentX >= 4.32)
        { damgMult = equipedWeapon.damageMults[5]; }

        if (PlayerOn)
        {
            //for buff areas
            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && inBuffArea>=0)
            {
                //resolve buff effect because this is when player releases over the area
                if(inBuffArea == 1 && _myItemsManager.twoWeapons)
                {
                    SwapWeapon();
                    inBuffArea = -1;
                }
                if(inBuffArea == 0)
                {
                    _enemySystem.CycleEnemyList();
                    inBuffArea = -1;
                }
                if(inBuffArea == 2)
                {
                    //speed buff
                    _mystrikePoint.bonusSpeed = 3;
                    
                    inBuffArea = -1;
                }
                if(inBuffArea == 3)
                {
                    //damage up
                    currentBuff = inBuffArea;
                    inBuffArea = -1;
                }
                //this needs to be at the end to reset the buff
                
            }

            //for attacking in strike area
               else if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && inStrikeArea && !justStruck)
            {
                float Damger = Mathf.Clamp((_mystrikePoint.mostRecentX * damgMult) + baseDamage, 0, maxDamage);

                if(currentBuff == 3)
                {
                    //testing double damage
                    Debug.Log("in if");
                    Damger += Damger;
                    currentBuff = -1;
                }


                for (int lcv = 0; lcv < targetEnemy.Count; lcv++)
                {
                    //revenge calculation below
                    Damger +=revengeTimer*30;
                    Debug.Log(Damger + "  damgMult: " + damgMult + "  most recentX: " + _mystrikePoint.mostRecentX);
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
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
            {
                //after player releases they lose the buff
                currentBuff = -1;
            }
            if(revengeTimer>0)
            {
                revengeTimer -= Time.deltaTime;
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

    private void turnMultiStrikeAreas(bool iss, Weapon wee)
    {

        if(iss ==false)
        {
            //turns all of these off
            bottomOdachi.SetActive(false);
            if (targetEnemy.Count > 1)
            {
                targetEnemy.RemoveAt(1);
            }

            //bow
            for (int bowlcv = 0; bowlcv < BowAreas.Count; bowlcv++)
            {
                BowAreas[bowlcv].SetActive(false);
                if (iss)
                { BowAreas[bowlcv].GetComponent<ExtraStrikeArea>().SetExtrasWeapon(wee); }
            }

            //shuriken
            for (int shurlcv = 0; shurlcv < shurikenAreas.Count; shurlcv++)
            {
                shurikenAreas[shurlcv].SetActive(false);
            }

            //dont need to check ifs
            return;
        }

        for (int lcv = 0; lcv < wee.effs.Count; lcv++)
        {

            //odachi
            if (wee.effs[0] == WeaponEffect.odachi)
            {
                bottomOdachi.SetActive(true);
                if (targetEnemy.Count > 1)
                {
                    targetEnemy.RemoveAt(1);
                }
                targetEnemy.Add(1);
                bottomOdachi.GetComponent<ExtraStrikeArea>().SetExtrasWeapon(wee);
            }
            
            //bow
            if (wee.effs[lcv] == WeaponEffect.bow)
            {
                for (int bowlcv = 0; bowlcv < BowAreas.Count; bowlcv++)
                {
                    BowAreas[bowlcv].SetActive(iss);
                    if (iss)
                    { BowAreas[bowlcv].GetComponent<ExtraStrikeArea>().SetExtrasWeapon(wee); }
                }
            }

            //shuriken
            if (wee.effs[lcv] == WeaponEffect.shuriken)
            {
                for(int shurlcv=0;shurlcv<shurikenAreas.Count;shurlcv++)
                {
                    shurikenAreas[shurlcv].SetActive(true);
                    //shurikenAreas[shurlcv].GetComponent<ExtraStrikeArea>().SetExtrasWeapon(wee); shouldnt need
                }
            }

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
            //probably should just add a multistrike weapon effect to simplify this if yes will do
            if (wee.effs[lcv] == WeaponEffect.multiTarget)
            {
                turnMultiStrikeAreas(true,wee);
            }
            else
            {
                turnMultiStrikeAreas(false,wee);
            }
        }
    }

    public void SwapWeapon()
    {
        if (equipedWeapon == _myItemsManager.SecondaryWeapon)
        {
            
             SetWeapon(_myItemsManager.PrimaryWeapon); 
        }
        else
        {
            if(_myItemsManager.SecondaryWeapon != null)
            SetWeapon(_myItemsManager.SecondaryWeapon);
        }
        //swapped set up for checking if primary matched and secondary in else because first run would always be the wrong one
    }

    public static void SwitchPlayerOn(bool tf)
    {
        PlayerOn = tf;
    }

    public void BeingBlocked(bool isblocked)
    {
        inStrikeArea = !isblocked;
    }

    public void RecieveBuff(int buff)
    {
        //public enum Buff { swapEnemy, swapWeapon, speedUp, damageUp, }
        //this is so when a buffed pointer doesn't lose it passing through another buff area and it will be fine after its use because it will be reset
        
        inBuffArea = buff;
    }

    public void RevengeBuff()
    {
        revengeTimer = 2f;
    }
    
}
