using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{

    public Image energyBar;

    float energy, maxEnergy = 50;
    public float eSpeed;
    float lerpSpeed;
    float energyDamg;
    public float damgMultplr;
    

    void Start()
    {
        energy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (energy > 20 && energy< 39)
        {
            damgMultplr = 1.8f;
        } else if(damgMultplr>40)
        { damgMultplr = 3; }
        
        
        if (energy < maxEnergy)
        {
            energy += eSpeed * Time.deltaTime ;
            energyDamg = damgMultplr * energy;
        }

        lerpSpeed = 3f * Time.deltaTime;
        
        energyFillBar();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            energy = 0;
            //Debug.Log(damgMultplr+"  "+attackCounter);
            damgMultplr = 1.2f;
        }

    }

    void energyFillBar()
    {
        //energyBar.fillAmount= energy / Mathf.Lerp(energyBar.fillAmount, energy / maxEnergy, lerpSpeed);
        energyBar.fillAmount = energy / maxEnergy;
    }

    public float getEnergyDamage()
    {
        return energyDamg;
    }

}
