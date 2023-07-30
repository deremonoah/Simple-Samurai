using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipedItemsManager : MonoBehaviour
{
    public Weapon equipedWeapon;
    public Weapon PrimaryWeapon;
    public Weapon SecondaryWeapon;

    public Armor equipedArmor;
    public Curio equipedCurio;

    public Image PrimaryweaponIcon;
    public Image SecondaryWeaponIcon;
    [SerializeField] GameObject SecondaryWeaponUI;

    public Image armorIcon;
    public Image curioIcon;

    public List<GameObject> weaponUpgradeIcons;
    public List<GameObject> armorUpgradeIcons;

    private StrikeArea _mainStrikeArea;
    private PlayerHealthBar _playerHP;
    private StrikePoint _strikePointer;

    private GameManager _gm;

    [SerializeField] List<ExtraStrikeArea> extraStrikeAreas;
    public bool twoWeapons;

    private void Start()
    {
        _mainStrikeArea = FindObjectOfType<StrikeArea>();
        _playerHP = FindObjectOfType<PlayerHealthBar>();
        _strikePointer = FindObjectOfType<StrikePoint>();

        _gm = GetComponent<GameManager>();

        equipedWeapon = Instantiate(equipedWeapon);
        PrimaryWeapon = equipedWeapon;
        SecondaryWeapon = Instantiate(SecondaryWeapon);
        equipedArmor = Instantiate(equipedArmor);
    }

    public void EquipItem(Item item, bool lootingUpgradesEnabled)
    {
        if (item.GetType() == typeof(Weapon))
        {
            if (lootingUpgradesEnabled && item.name == _mainStrikeArea.equipedWeapon.name)
            {
                _mainStrikeArea.equipedWeapon.itemLevel = Mathf.Clamp(_mainStrikeArea.equipedWeapon.itemLevel + 1, 0, 3);
                foreach (ExtraStrikeArea ex in extraStrikeAreas)
                {
                    ex.SetExtrasWeapon(_mainStrikeArea.equipedWeapon);
                }
            }
            equipedWeapon = (Weapon)item;
            _mainStrikeArea.SetWeapon(item as Weapon);
            PrimaryweaponIcon.sprite = item.itemPanelIcon;
            //we will  have to update this to if unlocked and no secondary add it there or stock pile
           

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
            _playerHP.SetCurio(equipedCurio);
        }

        UpdateItemUpgrades();
    }

    private void ResolveCurioEffect(Curio cur)
    {
        switch (cur.curiEef)
        {
            case CurioEffect.quick:
                //this gets called twice and Im not sure why
                _strikePointer.bonusSpeed = 1;
                Debug.Log("Pointer speed: "+_strikePointer.baseSpeed);
                curioIcon.sprite = cur.itemPanelIcon;
                break;
            case CurioEffect.greed:
                _gm.ReducePrice(equipedCurio.CurioNum);
                break;
        }
        if (equipedCurio.curiEef != CurioEffect.greed) { _gm.ReducePrice(0); }
        if (equipedCurio.curiEef != CurioEffect.quick) { _strikePointer.bonusSpeed = 0; }

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
    public void ClearConsumable()
    {
        equipedCurio = null;
        curioIcon.sprite = Resources.Load<Sprite>("Blank");
    }

    public void DamageItem(int kind)
    {
        //weapon
        if(kind == 1)
        {
            equipedWeapon.itemLevel = Mathf.Clamp(equipedWeapon.itemLevel - 1, 0, 3);
            EquipItem(equipedWeapon, false);
        }
        //armor
        else if(kind == 2)
        {
            equipedArmor.itemLevel = Mathf.Clamp(equipedArmor.itemLevel - 1, 0, 3);
            EquipItem(equipedArmor, false);
        }
    }

    public void UnlockTwoWeapons()
    {
        twoWeapons = true;
    }
}
