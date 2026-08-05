using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Armory : MonoBehaviour
{
    public List<Item> stockPile;
    [SerializeField] List<ArmorySlot> InventorySlotsActive=new();
    [SerializeField] List<ArmorySlot> InventorySlotsInActive=new();
    [SerializeField] GameObject ArmoryPanel;
    private PlayerEquipedItemsManager equipManager;
    [Header("upgrade slots info")]
    [SerializeField] TextMeshProUGUI upgradeText;
    private int currentUpgradeCost;
    [SerializeField] int IncrementToIncreaseCostBy;

    private GameManager gm;
    void Start()
    {
        //this is to make sure any scriptable objects in the list are clones not the original
        equipManager = FindObjectOfType<PlayerEquipedItemsManager>();
        gm = GetComponent<GameManager>();
    }

    public void OpenArmorPanel()
    {
        ArmoryPanel.SetActive(true);
        LoadArmoryPanel();
    }

    public void AddItemToArmory(Item item)
    {
        stockPile.Add(item);
        //check if enough items or quality items to give buff
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
        }
    }

    public void EquipThisSlot(int slot)
    {
        if (slot < stockPile.Count)
        {
            FindObjectOfType<PlayerEquipedItemsManager>().EquipItem(stockPile[slot], false);
            stockPile.RemoveAt(slot);
            LoadArmoryPanel();//to update the images to the correct items
        }
    }
}
