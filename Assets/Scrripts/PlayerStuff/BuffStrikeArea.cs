using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStrikeArea : MonoBehaviour
{
    public enum Buff { swapEnemy, swapWeapon, speedUp, damageUp,}
    [SerializeField] Buff mybuff;
    private StrikeArea mainStrikeArea;
    private PlayerEquipedItemsManager playerEquips;
    void Start()
    {
        mainStrikeArea = FindObjectOfType<StrikeArea>();
        playerEquips = FindObjectOfType<PlayerEquipedItemsManager>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //set an in buff area variable to true (maybe returns effect)
        if(other.name == "strike point")
        {
            if (mybuff == Buff.swapWeapon && playerEquips.twoWeapons)
            {
                //might want too change the player equips to on strike areas but whatever for now
                mainStrikeArea.RecieveBuff((int)mybuff);
            }
            //rest of the ifs or this is where we have ones that inherate form this and use strategy pattern
            //the base for the buffs would be a buff class that would have a refrence to the players i think and a buff method
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
             mainStrikeArea.RecieveBuff(-1);
        }
    }
}
