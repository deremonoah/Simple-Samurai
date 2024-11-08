using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem angrySymbol;
    [SerializeField] ParticleSystem buffUp;
    [SerializeField] GameObject smolBlood;
    [SerializeField] GameObject bigBlood;
    //blood flying
    //coins?
    private float raging = 0f;
    private float lowerEmmisionTimer = 0;

    private void Update()
    {
        if(raging>0)
        {
            raging -= Time.deltaTime;
            angrySymbol.startSize -= (Time.deltaTime *0.15f);         
        }
        
        if(lowerEmmisionTimer<=0)
        {
            angrySymbol.emissionRate -= 1;
            lowerEmmisionTimer = 0.11f;
            Color temp = new Color(1f,1f,1f,angrySymbol.startColor.a-(angrySymbol.emissionRate*0.01f));
            //angrySymbol.startColor = new ParticleSystem.MinMaxGradient(temp);
        }
        else { lowerEmmisionTimer -= Time.deltaTime; }

    }

    public void Revenge()
    {
        if (!angrySymbol.isPlaying)
        { angrySymbol.Play(); }
        raging = 2f;
        angrySymbol.startSize = 0.2f;
        angrySymbol.emissionRate = 15;
        lowerEmmisionTimer = 0.11f;
    }

    public void BuffPointer(string type)
    {
        if(type == "speed")
        {

        }
        if(type == "damage")
        {
            //not sure if I want this one here
        }
    }

    public void ShowDamage(Transform pos,float dmg)
    {
        var par=gameObject;
        if(dmg<=26)
        {
            par=Instantiate(smolBlood, pos, pos);
            par.transform.parent = pos;
            return;
        }
        //instatiate blood spray or burst
        par=Instantiate(bigBlood, pos, pos);
        //parent it to follow for the animation
        par.transform.parent = pos;
    }
}
