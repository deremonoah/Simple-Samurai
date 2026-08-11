using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrap : MonoBehaviour
{
    [SerializeField] protected bool armed=true;
    [SerializeField] protected int numberOUses=1;//times it can be used until it breaks
    [SerializeField] protected int currentUses;//incrememnts up
    [SerializeField] protected GameObject ui;
    public Sprite displaySprite;

    public virtual void DefendPlayer(float damage, enemyStats enemy)
    {
        //interact with the damage
        //maybe interact with the enemy
        currentUses++;
        if (currentUses >= numberOUses)
        {
            armed = false;
            ui.SetActive(false);
        }
    }

    public virtual void ReArmTrap()
    {
        armed = true;
        ui.SetActive(true);
        currentUses=numberOUses;
    }

    public bool isArmed()
    {
        return armed;
    }
}
