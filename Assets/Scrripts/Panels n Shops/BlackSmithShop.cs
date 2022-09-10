using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithShop : MonoBehaviour
{
    private PlayerEquipedItemsManager _playerEquipedItems;
    private GameManager _gm;


    private int baseCost = 10;
    public Text improveWeaponText;
    public Text improveArmorText;
    public Text lootingUpgradeText;
    public bool lootingUpgradesEnabled = false;
    public int reduceCost;
    void Start()
    {
        _playerEquipedItems = GetComponent<PlayerEquipedItemsManager>();
        _gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void ImproveWeaponButton()
    {
        var itemLvl = Mathf.Clamp(_playerEquipedItems.equipedWeapon.itemLevel + 1, 0, 3);
        var cost = (baseCost * itemLvl)-reduceCost;

        if (itemLvl <= 3 && _gm.playerCoins >= cost)
        {
            _playerEquipedItems.equipedWeapon.itemLevel = Mathf.Clamp(itemLvl, 0, 3);
            _playerEquipedItems.EquipItem(_playerEquipedItems.equipedWeapon, lootingUpgradesEnabled);

            _gm.playerCoins -= cost;
            //improveWeaponCost += 10;
            SetUpgradeCostsButtonsText();
        }
    }

    public void ImproveArmorButton()
    {
        var itemLvl = Mathf.Clamp(_playerEquipedItems.equipedArmor.itemLevel + 1, 0, 3);
        var cost = (baseCost * itemLvl)-reduceCost;

        if (itemLvl <= 3 && _gm.playerCoins >= cost)
        {
            

            _playerEquipedItems.equipedArmor.itemLevel = Mathf.Clamp(itemLvl, 0, 3);
            _playerEquipedItems.EquipItem(_playerEquipedItems.equipedArmor, lootingUpgradesEnabled);
            _gm.playerCoins -= cost;
            //improveArmorCost += 10;
            SetUpgradeCostsButtonsText();
        }
    }

    public void EnableLootingUpgrades()
    {
        if (_gm.playerCoins >= 10 && lootingUpgradesEnabled == false)
        {
            lootingUpgradesEnabled = true;
            _gm.playerCoins -= 10;
        }
    }

    public void SetUpgradeCostsButtonsText()
    {
        int temp = (baseCost * Mathf.Clamp(_playerEquipedItems.equipedWeapon.itemLevel + 1,0,3))-reduceCost;
        improveWeaponText.text = "Improve Weapon " + temp + "g";

        temp = (baseCost * Mathf.Clamp(_playerEquipedItems.equipedArmor.itemLevel + 1, 0, 3))- reduceCost;
        improveArmorText.text = "Improve Armor " + temp + "g";
    }

}
