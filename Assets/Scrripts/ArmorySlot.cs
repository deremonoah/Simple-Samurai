using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorySlot : MonoBehaviour
{
    public int slotNum;
    public Image imageToSet;

    public void ItemAtSlot()//used for equiping, and discarding, in future maybe selling
    {
        var armory = FindObjectOfType<Armory>();
        if(slotNum < armory.stockPile.Count)
        {
            armory.ThisSlot(slotNum);
        }
        else { Debug.LogError("slot number pressed is great than stockpile count"); }
        
    }

    public void InspectSlotItem()
    {
        var armory = FindObjectOfType<Armory>();
        armory.InspectAtSlot(slotNum);
    }

}
