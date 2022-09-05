using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipedItemsManager : MonoBehaviour
{
    public Weapon equipedWeapon;
    public Armor equipedArmor;
    public Curio equipedCurio;

    public Image weaponIcon;
    public Image armorIcon;
    public Image curioIcon;

    public List<GameObject> weaponUpgradeIcons;
    public List<GameObject> armorUpgradeIcons;

    private StrikeArea _mainStrikeArea;
    private PlayerHealthBar _playerHP;
    private StrikePoint _strikePointer;

    private GameManager _gm;

    private void Start()
    {
        _mainStrikeArea = FindObjectOfType<StrikeArea>();
        _playerHP = FindObjectOfType<PlayerHealthBar>();
        _strikePointer = FindObjectOfType<StrikePoint>();

        _gm = GetComponent<GameManager>();

        equipedWeapon = Instantiate(equipedWeapon);
        equipedArmor = Instantiate(equipedArmor);
    }

    public void EquipItem(Item item, bool lootingUpgradesEnabled)
    {
        if (item.GetType() == typeof(Weapon))
        {
            if (lootingUpgradesEnabled && item.name == _mainStrikeArea.equipedWeapon.name)
            {
                _mainStrikeArea.equipedWeapon.itemLevel = Mathf.Clamp(_mainStrikeArea.equipedWeapon.itemLevel + 1, 0, 3);
            }
            equipedWeapon = (Weapon)item;
            _mainStrikeArea.SetWeapon(item as Weapon);
            weaponIcon.sprite = item.itemPanelIcon;

           

        }
        if (item.GetType() == typeof(Armor))
        {
            if (lootingUpgradesEnabled && item.name == _playerHP.myArmor.name)
            {
                _playerHP.myArmor.itemLevel = Mathf.Clamp(_playerHP.myArmor.itemLevel + 1, 0, 3);
            }
            equipedArmor = (Armor)item; 
            _playerHP.SetArmor(item as Armor);
            armorIcon.sprite = item.itemPanelIcon;

        }
        if (item.GetType() == typeof(Curio))
        {
            //equip Curio to HP and strike Point and Strike area
            equipedCurio = (Curio)item;
            curioIcon.sprite = item.itemPanelIcon;
            ResolveCurioEffect(item as Curio);
        }

        UpdateItemUpgrades();
    }

    private void ResolveCurioEffect(Curio cur)
    {
        switch (cur.curiEef)
        {
            case CurioEffect.quick:
                //this gets called twice and Im not sure why
                _strikePointer.speed = 6;
                Debug.Log("Pointer speed: "+_strikePointer.speed);
                curioIcon.sprite = cur.itemPanelIcon;
                break;
            case CurioEffect.greed:
                _gm.ReducePrice(equipedCurio.CurioNum);
                break;
        }


    }
    private void UpdateItemUpgrades()
    {
        //reseting them so if new item there are none
        for (int lcv = 0; lcv < weaponUpgradeIcons.Count; lcv++)
        {
            weaponUpgradeIcons[lcv].SetActive(false);
        }

        for (int lcv = 0; lcv < armorUpgradeIcons.Count; lcv++)
        {
            armorUpgradeIcons[lcv].SetActive(false);
        }


        //Getting the correct number of anvils
        for (int lcv = 0; lcv < equipedWeapon.itemLevel; lcv++)
        {
            weaponUpgradeIcons[lcv].SetActive(true);
        }

        for (int lcv = 0; lcv < equipedArmor.itemLevel; lcv++)
        {
            armorUpgradeIcons[lcv].SetActive(true);
        }
    }
}
