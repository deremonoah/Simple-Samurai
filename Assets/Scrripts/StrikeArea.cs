using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeArea : MonoBehaviour
{
    private EnemysSystem enmySys;
    public Camera mc;
    [SerializeField] bool indere;
    [SerializeField] float maxDamg = 70;
    [SerializeField] float baseDamg;
    [SerializeField] float damgMult;
    [SerializeField] float defaultDamgMult;
    [SerializeField] int target;

    [SerializeField] float timer = 0;
    [SerializeField]bool timering=false;
    [SerializeField] GameObject bottomOdachi;
    [SerializeField] List<GameObject> BowAreas;

    public GameObject strikePointObj;

    //myStrikeAreaSprite.sprite = the sprite you want from weapon
    private SpriteRenderer myStrikeAreaSprite;
    public Weapon equipedWeapon;
    public Weapon Test;
    void Start()
    {
        enmySys = mc.GetComponent<EnemysSystem>();
        myStrikeAreaSprite = GetComponent<SpriteRenderer>();
        bottomOdachi.SetActive(false);
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


            if (Input.GetKeyDown(KeyCode.Space))
            {
                timering = true;
                timer = 0;
                damgMult = defaultDamgMult;
                timer = 0;

            }
        

        if (Input.GetKeyUp(KeyCode.Space) && indere)
        {
            float Damger = Mathf.Clamp(baseDamg + (timer * damgMult),0,maxDamg);
            
            enmySys.DamageEnemy(Damger,target,equipedWeapon.effs);
            
            timer = 0;
            timering = false;
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

        Debug.Log("in equip");
        for (int lcv = 0; lcv<wee.effs.Count; lcv++)
        {
            if (wee.effs[lcv] == Effect.odachi)
            {
                bottomOdachi.SetActive(true);
                Debug.Log("hit if");
            }
            else
            {
                bottomOdachi.SetActive(false);
            }
            if (wee.effs[lcv] == Effect.bow)
            {
                TurnBow(true);
            }
            else
            {
                TurnBow(false);
            }
        }
    }

    
}
