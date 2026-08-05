using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorySlot : MonoBehaviour
{
    public Image imageToSet;

    public void EquipItemAtSlot(int slotCount)
    {
        var armory = FindObjectOfType<Armory>();
        if(slotCount < armory.stockPile.Count)
        {
            armory.EquipThisSlot(slotCount);
        }
        
    }

}
