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

    void Start()
    {
        enmys = mc.GetComponent<EnemysSystem>();
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
            Debug.Log(Damger);
            enmys.DamageEnemy(Damger,target);
            
            timer = 0;
            timering = false;
        }
        
        //targeting ifs for keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            target = 0;
            pointer.transform.position = hpspots[0].transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            target = 1;
            pointer.transform.position = hpspots[1].transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            target = 2;
            pointer.transform.position = hpspots[2].transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            target = 3;
            pointer.transform.position = hpspots[3].transform.position;
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

    
}
