using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Armory : MonoBehaviour
{
    [Header("Main stats")]
    public List<Item> stockPile;
    [SerializeField] List<ArmorySlot> InventorySlotsActive=new();
    [SerializeField] List<ArmorySlot> InventorySlotsInActive=new();
    [Header("ui for panel")]
    [SerializeField] GameObject ArmoryPanel;
    [SerializeField] GameObject ArmoryButton;
    [SerializeField] TextMeshProUGUI helperText;
    [SerializeField] ArmorySlot OverFlowSlot;
    
    [Header("upgrade slots info")]
    [SerializeField] TextMeshProUGUI upgradeText;
    [SerializeField] int IncrementToIncreaseCostBy;
    [SerializeField] int StartingCost;
    [SerializeField] GameObject increaseSlotButton;

    private int currentUpgradeCost;
    [Header("Colors for inventorySlots")]
    [SerializeField] Color equipingColor;
    [SerializeField] Color discardingColor;

    private GameManager gm;
    private PlayerEquipedItemsManager equipManager;
    private bool Equiping=true;//use this to know if we are equiping or deleting an item

    void Start()
    {
        //this is to make sure any scriptable objects in the list are clones not the original
        equipManager = FindObjectOfType<PlayerEquipedItemsManager>();
        gm = GetComponent<GameManager>();
        currentUpgradeCost = StartingCost;
    }

    public void OpenArmorPanel()
    {
        ArmoryPanel.SetActive(true);
        LoadArmoryPanel();
        if(Equiping)
        { changeInventroyBackColor(equipingColor); }
        else { changeInventroyBackColor(discardingColor); }
    }

    public void CloseArmoryPanel()
    {
        if(Equiping)
        {
            ArmoryPanel.SetActive(false);
        }
    }

    public void AddItemToArmory(Item item)
    {
        stockPile.Add(item);
        ArmoryButton.SetActive(true);//if you add an item to the stock pile this should be active but not until then

        //if you picked up more items than your armory can store
        if(stockPile.Count>InventorySlotsActive.Count)
        {
            Equiping = false;
            OpenArmorPanel();
            helperText.text = "click to discard";
            helperText.color = discardingColor;
            //maybe need an option to delete currently equiped items
            //can have no armor and no curio, but always need a weapon? or can you throw hands?
        }
    }

    public void LoadArmoryPanel()
    {
        for (int lcv = 0; lcv < InventorySlotsActive.Count; lcv++)
        {
            if (lcv < stockPile.Count)
            { 
                InventorySlotsActive[lcv].imageToSet.sprite = stockPile[lcv].itemPanelIcon;
                InventorySlotsActive[lcv].imageToSet.color = new Color(1, 1, 1, 1);//make sure alpha is 100%
            }
            else
            {
                InventorySlotsActive[lcv].imageToSet.color = new Color(1, 1, 1, 0);//make sure alpha is 0% to not display white square
            }
        }
        if(!Equiping)
        {
            OverFlowSlot.gameObject.SetActive(true);
            OverFlowSlot.GetComponent<Image>().color = discardingColor;
            OverFlowSlot.imageToSet.sprite = stockPile[stockPile.Count - 1].itemPanelIcon;
            OverFlowSlot.slotNum = stockPile.Count - 1;//it will always be the last one as it unEquips
        }
        else
        {
            OverFlowSlot.gameObject.SetActive(false);
        }
    }

    public void IncreaseItemSlot()
    {
        if(gm.playerCoins>=currentUpgradeCost &&InventorySlotsInActive.Count>0)
        {
            gm.playerCoins -= currentUpgradeCost;
            currentUpgradeCost += IncrementToIncreaseCostBy;//currently thinking increase by the same cost each time, rn am thinking 1

            InventorySlotsActive.Add(InventorySlotsInActive[0]);
            InventorySlotsInActive[0].gameObject.SetActive(true);
            InventorySlotsInActive.RemoveAt(0);

            upgradeText.text = "+1 slot " + currentUpgradeCost + "g";

            if(!Equiping)
            {
                ResolveOverFlowStockPile();
            }
            if(InventorySlotsInActive.Count<1)
            {
                increaseSlotButton.SetActive(false);
            }
            LoadArmoryPanel();
        }
    }

    public void ThisSlot(int slot)
    {
        if(Equiping)
        {
            if (slot < stockPile.Count)
            {
                equipManager.EquipItem(stockPile[slot], false);
                stockPile.RemoveAt(slot);
                LoadArmoryPanel();//to update the images to the correct items
            }
        }
        else
        {
            stockPile.RemoveAt(slot);
            ResolveOverFlowStockPile();
        }
    }

    private void ResolveOverFlowStockPile()
    {
        //check if we have enough slots for the whole stock pile
        if (stockPile.Count <= InventorySlotsActive.Count)
        {
            Equiping = true;
            changeInventroyBackColor(equipingColor);
            helperText.text = "click to equip";
            helperText.color = Color.black;
        }
        LoadArmoryPanel();
    }

    private void changeInventroyBackColor(Color col)
    {
        foreach(ArmorySlot slot in InventorySlotsActive)
        {
            GameObject back = slot.gameObject;
            back.GetComponent<Image>().color = col;
        }
    }
}
