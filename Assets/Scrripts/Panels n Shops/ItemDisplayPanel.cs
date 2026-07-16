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
    [SerializeField] Image strikeAreaImage;
    [SerializeField] Image formPatternImage;
    [SerializeField] List<GameObject> boxesForStyles;
    [SerializeField] GameObject WeaponSection;

    [Header("Armor display info")]
    [SerializeField] TextMeshProUGUI ItemAmorValue;
    [SerializeField] GameObject ArmorSection;

    [Header("Item diplay info")]
    [SerializeField] List<GameObject> anvilLevelIcons;
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] TextMeshProUGUI ItemDescription;
    [SerializeField] Image ItemIconSprite;

    private int itemSlotLookingAt=-1;

    public void OpenItemDescriptionPanel(Item item,int itemSlot)
    {
        panel.SetActive(true);
        itemSlotLookingAt = itemSlot;
        
        if(item is Weapon)
        {
            SetForWeapon((Weapon)item);
        }    
        if(item is Armor)
        {
            SetForArmor((Armor)item);
        }
        if(item is Curio)
        {
            SetForCurio((Curio)item);
        }


        //just keep the styles in the same order
        //I also thought getting styles from beating certain enemies or by making certain decisions is way cooler than just getitng them randomly
        //problem is you can't exactly explore in the same way, or even choose which enemies to fight
        ItemDescription.text = item.itemDescription;
        ItemName.text = item.itemName;
        ItemIconSprite.sprite = item.itemPanelIcon;
    }

    public void SetForWeapon(Weapon we)
    {
        WeaponSection.SetActive(true);
        ArmorSection.SetActive(false);

        int stylesToShow = FindObjectOfType<SenseiPanel>().getNumberOfKnownStyles();
        Debug.Log("known styles they say is " + stylesToShow);
        for (int lcv = 0; lcv < boxesForStyles.Count; lcv++)
        {
            boxesForStyles[lcv].SetActive(false);
        }


        for (int lcv = 0; lcv < stylesToShow; lcv++)
        {
            boxesForStyles[lcv].SetActive(true);
        }

        strikeAreaImage.sprite = we.DisplayStrikeAreaIcon;
        ItemDescription.text = we.itemDescription;
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

    public void DisplayStyle(Sprite stylePic)//I use this on the check boxes, ideally default to the right one in future
    {
        formPatternImage.sprite = stylePic;
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
}
