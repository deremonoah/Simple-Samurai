using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithShop : MonoBehaviour
{
    private PlayerEquipedItemsManager _playerEquipedItems;
    private GameManager _gm;

    private int improveWeaponCost = 10;
    private int improveArmorCost = 10;
    public Text improveWeaponText;
    public Text improveArmorText;
    public bool lootingUpgradesEnabled = false;
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
        if (_playerEquipedItems.equipedWeapon.itemLevel + 1 <= 3 && _gm.playerCoins >= improveWeaponCost * (Mathf.Clamp(_playerEquipedItems.equipedWeapon.itemLevel + 1, 0, 3)))
        {
            _playerEquipedItems.equipedWeapon.itemLevel = Mathf.Clamp(_playerEquipedItems.equipedWeapon.itemLevel + 1, 0, 3);
            _playerEquipedItems.EquipItem(_playerEquipedItems.equipedWeapon, lootingUpgradesEnabled);

            _gm.playerCoins -= improveWeaponCost * Mathf.Clamp(_playerEquipedItems.equipedWeapon.itemLevel, 0, 3);
            //improveWeaponCost += 10;
            SetUpgradeCostsButtonsText();
        }
    }

    public void ImproveArmorButton()
    {
        if (_playerEquipedItems.equipedArmor.itemLevel + 1 <= 3 && _gm.playerCoins >= improveArmorCost * (Mathf.Clamp(_playerEquipedItems.equipedArmor.itemLevel + 1, 0, 3)))
        {
            _playerEquipedItems.equipedArmor.itemLevel = Mathf.Clamp(_playerEquipedItems.equipedArmor.itemLevel + 1, 0, 3);
            _playerEquipedItems.EquipItem(_playerEquipedItems.equipedArmor, lootingUpgradesEnabled);
            _gm.playerCoins -= improveArmorCost * Mathf.Clamp(_playerEquipedItems.equipedArmor.itemLevel, 0, 3);
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
        int temp = improveWeaponCost * (_playerEquipedItems.equipedWeapon.itemLevel + 1);
        improveWeaponText.text = "Improve Weapon " + temp + "g";

        temp = improveArmorCost * Mathf.Clamp(_playerEquipedItems.equipedArmor.itemLevel + 1, 0, 3); ;
        improveArmorText.text = "Improve Weapon " + temp + "g";
    }


}
