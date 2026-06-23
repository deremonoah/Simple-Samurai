using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem angrySymbol;
    [SerializeField] ParticleSystem buffUp;
    [SerializeField] GameObject smolBlood;//.1
    [SerializeField] GameObject bigBlood;//1
    [SerializeField] GameObject blockSparks;
    [SerializeField] Transform PlayerImageForBlood;
    Quaternion playerRotation = Quaternion.Euler(new Vector3(-56.23f, -270.179f, 90));
    //blood flying
    //coins?
    private float raging = 0f;
    private float lowerEmmisionTimer = 0;

    public static ParticleManager instance;

    private void Awake()
    {
        if(instance !=null & instance != this)
        {
            Debug.LogError("we have 2 particle managers");
        }
        else
        {
            instance = this;
            //no don't destroy on load as reloading the scene is how we restart the game, then there would be 2 in the scene
        }
    }

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
            par=Instantiate(smolBlood, pos.position, smolBlood.transform.rotation);
            return;
        }
        //instatiate blood spray or burst
        par=Instantiate(bigBlood, pos.position, bigBlood.transform.rotation);
        //parent it to follow for the animation
    }

    public void ShowPayerDamage(float dmg)
    {
        var par = gameObject;
        
        if (dmg <= 26)
        {
            par = Instantiate(smolBlood, PlayerImageForBlood.position, playerRotation);
            return;
        }
        //instatiate blood spray or burst
        par = Instantiate(bigBlood, PlayerImageForBlood.position, playerRotation);
    }

    public void BlockedHere(Vector2 pos,float enmDmg)//maybe get player damage potential too
    {
        //Transform pos = FindObjectOfType<StrikePoint>().gameObject.transform;//works for now

        var par = Instantiate(blockSparks, pos, transform.rotation);
        
        //set particles to emit in refrence to the damage
    }
}
