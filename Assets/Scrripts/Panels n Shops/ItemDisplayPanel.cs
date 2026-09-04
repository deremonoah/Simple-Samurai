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
    
    private Reward rewardToInspect;
    //private Weapon weRefrence;
    //private int lookingAtStyleID;

    private void Start()
    {
        ppm = FindObjectOfType<PickPanManager>();
        arm = FindObjectOfType<Armory>();
        WeaponSection.SetActive(false);
    }

    public void OpenItemDescriptionPanel(Reward reward,int itemSlot, itemDisplayOpenedFrom opener)
    {
        
        itemSlotLookingAt = itemSlot;
        from = opener;
        rewardToInspect = reward;


        if (reward is Weapon)
        {
            //weRefrence = (Weapon)reward;
            SetForWeapon();
        }    
        else if(reward is Armor)
        {
            SetForArmor((Armor)reward);
        }
        else if(reward is Curio)
        {
            SetForCurio((Curio)reward);//keeping because might be different with item level?
        }
        else if(reward is StyleReward)
        {
            
            StyleReward sty = (StyleReward)reward;
            SetForStyle((int)sty.styleToLearn);
        }
        else if(reward is ShopReward)//now more learning rewards as they are in the loot list, but idk if I want to rename yet
        {
            SetForSimple();
        }
        panel.SetActive(true);//moved to not get null on style display when it enables and looks for weapon

        //just keep the styles in the same order
        //I also thought getting styles from beating certain enemies or by making certain decisions is way cooler than just getitng them randomly
        //problem is you can't exactly explore in the same way, or even choose which enemies to fight
        ItemDescription.text = reward.Description;
        ItemName.text = reward.Name;
        ItemIconSprite.sprite = reward.PanelIcon;
    }

    private void SetForWeapon()
    {
        WeaponSection.SetActive(true);
        ArmorSection.SetActive(false);
        //lookingAtStyleID = -1;
        //will handle the info and ask for it just needs to be enabled
    }

    private void SetForArmor(Armor ar)
    {
        WeaponSection.SetActive(false);
        ArmorSection.SetActive(true);
        //lookingAtStyleID = -1;

        ItemAmorValue.text = ar.armorLevel[ar.itemLevel]+"";
    }

    private void SetForCurio(Curio cu)
    {
        WeaponSection.SetActive(false);
        ArmorSection.SetActive(false);
        //lookingAtStyleID = -1;
        //anything specific for curios? maybe in future?
    }

    private void SetForSimple()
    {
        WeaponSection.SetActive(false);
        ArmorSection.SetActive(false);
        //lookingAtStyleID = -1;
    }

    private void SetForStyle(int style)
    {
        WeaponSection.SetActive(true);
        ArmorSection.SetActive(false);

        //tell display for weapon, an extra style
        //lookingAtStyleID = style;
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

    /*public Weapon getWeapon()//called by Style Display for having multiple in different places
    {
        Debug.Log("null?" + weRefrence == null);
        return weRefrence;
    }

    public int getStyle()//this and above variables are set when the item is selected
    {
        return lookingAtStyleID;
    }*/

    public Reward getRewardInspecting()
    {
        return rewardToInspect;
    }
}
public enum itemDisplayOpenedFrom { PickPan,Armory }