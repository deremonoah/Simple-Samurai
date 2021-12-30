using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    HealthBar myhp;
    EnergyBar myeng;
    EnemyHP myenemy;
    

    // Start is called before the first frame update
    void Start()
    {
        myhp = GetComponent<HealthBar>();
        myeng = GetComponent<EnergyBar>();
        //myenemy= GetComponent<EnemyHP>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //myenemy.DamageEnemy(myeng.getEnergyDamage());
            //Debug.Log("space");
            
        }
        
        
    }
}
