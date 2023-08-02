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
           
                mainStrikeArea.RecieveBuff((int)mybuff);
                //im realizing that I should be able to have get rid of the ifs

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
