using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplayPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] GameObject panel;

    [Header("weapon display info")]
    [SerializeField] GameObject WeaponSection;
    [SerializeField] StyleDisplay syd;

    [Header("Armor display info")]
    [SerializeField] TextMeshProUGUI ItemAmorValue;
    [SerializeField] GameObject ArmorSection;

    [Header("Item diplay info")]
    [SerializeField] List<GameObject> anvilLevelIcons;
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] TextMeshProUGUI ItemDescription;
    [SerializeField] Image ItemIconSprite;

    private int itemSlotLookingAt=-1;
    private itemDisplayOpenedFrom from;
    private PickPanManager ppm;
    private Armory arm;
    

    private Weapon weRefrence;

    private void Start()
    {
        ppm = FindObjectOfType<PickPanManager>();
        arm = FindObjectOfType<Armory>();
        WeaponSection.SetActive(false);
    }

    public void OpenItemDescriptionPanel(Item item,int itemSlot, itemDisplayOpenedFrom opener)
    {
        
        itemSlotLookingAt = itemSlot;
        from = opener;
        
        if(item is Weapon)
        {
            weRefrence = (Weapon)item;
            SetForWeapon();
        }    
        if(item is Armor)
        {
            SetForArmor((Armor)item);
        }
        if(item is Curio)
        {
            SetForCurio((Curio)item);
        }
        panel.SetActive(true);//moved to not get null on style display when it enables and looks for weapon

        //just keep the styles in the same order
        //I also thought getting styles from beating certain enemies or by making certain decisions is way cooler than just getitng them randomly
        //problem is you can't exactly explore in the same way, or even choose which enemies to fight
        ItemDescription.text = item.itemDescription;
        ItemName.text = item.itemName;
        ItemIconSprite.sprite = item.itemPanelIcon;
    }

    public void SetForWeapon()
    {
        WeaponSection.SetActive(true);
        ArmorSection.SetActive(false);
        //will handle the info and ask for it just needs to be enabled
    }

    public void SetForArmor(Armor ar)
    {
        WeaponSection.SetActive(false);
        ArmorSection.SetActive(true);

        ItemAmorValue.text = ar.armorLevel[ar.itemLevel]+"";
    }

    public void SetForCurio(Curio cu)
    {
        WeaponSection.SetActive(false);
        ArmorSection.SetActive(false);

        //anything specific for curios? maybe in future?
    }

    public void CloseItemDisplayPanel()
    {
        panel.SetActive(false);
    }

    public void PlayerSelectedItem()
    {
        Debug.Log("slot should pick up number " + itemSlotLookingAt);
        FindObjectOfType<PickPanManager>().PickButton(itemSlotLookingAt);
        CloseItemDisplayPanel();
    }

    public void RightArrow()
    {
        WeaponSection.SetActive(false);
        int nextSlot = itemSlotLookingAt+1;
        if (from == itemDisplayOpenedFrom.PickPan)
        {
            ppm.inspectItem(nextSlot);//handles overflow
        }
        else if(from == itemDisplayOpenedFrom.Armory)
        {
            arm.InspectAtSlot(nextSlot);//handles overflow
        }
        
    }

    public void LeftArrow()
    {
        WeaponSection.SetActive(false);
        int nextSlot = itemSlotLookingAt-1;
        if (from==itemDisplayOpenedFrom.PickPan)
        {
            ppm.inspectItem(nextSlot);//handles overflow or underflow?
        }
        else if (from == itemDisplayOpenedFrom.Armory)
        {
            arm.InspectAtSlot(nextSlot);//handles wrap
        }
        
    }

    public Weapon getWeapon()//called by Style Display for having multiple in different places
    {
        Debug.Log("null?" + weRefrence == null);
        return weRefrence;
    }
}
public enum itemDisplayOpenedFrom { PickPan,Armory }