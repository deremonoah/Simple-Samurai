using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem angrySymbol;
    [SerializeField] ParticleSystem buffUp;
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
}
