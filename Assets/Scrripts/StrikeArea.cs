using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikeArea : MonoBehaviour
{
    private EnemysSystem enmys;
    public Camera mc;
    [SerializeField] bool indere;
    [SerializeField] float maxDamg = 70;
    [SerializeField] float baseDamg;
    [SerializeField] float damgMult;
    [SerializeField] float defaultDamgMult;
    [SerializeField] int target;

    [SerializeField] float timer = 0;
    [SerializeField]bool timering=false;

    public GameObject[] hpspots;
    public Image pointer;

    public GameObject strikePointObj;

    //myStrikeAreaSprite.sprite = the sprite you want from weapon
    private SpriteRenderer myStrikeAreaSprite;
    public Weapon equipedWeapon;
    public Weapon kanaboTest;
    void Start()
    {
        enmys = mc.GetComponent<EnemysSystem>();
        myStrikeAreaSprite = GetComponent<SpriteRenderer>();
        
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
            
            enmys.DamageEnemy(Damger,target);
            
            timer = 0;
            timering = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(kanaboTest);
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

    public void SetWeapon(Weapon wee)
    {
        baseDamg = wee.baseDamg;
        maxDamg = wee.maxDamg;
        myStrikeAreaSprite.sprite = wee.myStrikeArea;
        strikePointObj.GetComponent<SpriteRenderer>().sprite = wee.strikePointer;
        //get help figureing out how to refresh spritet colider or why it didnt work the old way that you deleted 
        
    }
}
