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
    private bool twoWeapons=false;

    [Header("Item recieved variables")]
    [SerializeField] GameObject itemAnimPrefab;//this is the item that is spawned and moved or the icon
    [SerializeField] Transform parentForUIAnim;
    [SerializeField] float showSpeed;

    private void Start()
    {
        _mainStrikeArea = FindObjectOfType<StrikeArea>();
        _playerHP = FindObjectOfType<PlayerHealthBar>();
        _strikePointer = FindObjectOfType<StrikePoint>();

        _gm = GetComponent<GameManager>();

        equipedWeapon = Instantiate(equipedWeapon);
        PrimaryWeapon = equipedWeapon;
        if (SecondaryWeapon != null)
        { SecondaryWeapon = Instantiate(SecondaryWeapon); }
        equipedArmor = Instantiate(equipedArmor);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem(_mainStrikeArea.TestWeapon,null);
        }

    }
       
#endif

public void EquipItem(Item item, Transform fromHere)
    {
        Transform goingHere = null;
        
        if (item.GetType() == typeof(Weapon))
        {
            /*if (lootingUpgradesEnabled && item.name == _mainStrikeArea.equipedWeapon.name)
            {
                _mainStrikeArea.equipedWeapon.itemLevel = Mathf.Clamp(_mainStrikeArea.equipedWeapon.itemLevel + 1, 0, 3);
                foreach (ExtraStrikeArea ex in extraStrikeAreas)
                {
                    ex.SetExtrasWeapon(_mainStrikeArea.equipedWeapon);
                }
            }*/
            if (twoWeapons)
            {
                if (SecondaryWeapon != null)
                { GetComponent<Armory>().AddItemToArmory(SecondaryWeapon); }
                SecondaryWeapon = PrimaryWeapon;
                SecondaryWeaponIcon.sprite = SecondaryWeapon.PanelIcon;
            }
            else if(item!=equipedWeapon)//if its not a new weapon we don't want to add a copy into armory
            {
                GetComponent<Armory>().AddItemToArmory(equipedWeapon);
            }
            

            equipedWeapon = (Weapon)item;
            PrimaryWeapon = equipedWeapon;
            _mainStrikeArea.SetWeapon(item as Weapon);
            PrimaryweaponIcon.sprite = item.PanelIcon;
            goingHere = PrimaryweaponIcon.gameObject.transform;
            //we will  have to update this to if unlocked and no secondary add it there or stock pile
           

        }
        if (item.GetType() == typeof(Armor))
        {
            /*if (lootingUpgradesEnabled && item.name == _playerHP.myArmor.name)
            {
                _playerHP.myArmor.itemLevel = Mathf.Clamp(_playerHP.myArmor.itemLevel + 1, 0, 3);
            }*/
            if (item!=equipedArmor)
            {
                GetComponent<Armory>().AddItemToArmory(equipedArmor);
            }
            equipedArmor = (Armor)item; 
            _playerHP.SetArmor(item as Armor);
            armorIcon.sprite = item.PanelIcon;
            goingHere = armorIcon.gameObject.transform;
        }
        if (item.GetType() == typeof(Curio))
        {
            //equip Curio to HP and strike Point and Strike area
            //get it to not equip consumables replacing an item
            if(equipedCurio!=null)
            {
                GetComponent<Armory>().AddItemToArmory(equipedCurio);
            }
            //equipedCurio = (Curio)item;
            //curioIcon.sprite = item.itemPanelIcon;
            ResolveCurioEffect(item as Curio);
            _playerHP.SetCurio(item as Curio);//handles any hp changing
            goingHere = curioIcon.gameObject.transform;
        }

        UpdateItemUpgrades();
        if(goingHere!=null && fromHere!=null)
        {
            StartCoroutine(ItemRecievedRoutine(fromHere,goingHere,item));
        }
        
    }

    //I need a way to handle getting the secondary stuff equiped
    public void EquipSecondary(Item item)
    {
        if (item.GetType() == typeof(Weapon))
        {
            //this is a place holder gotta afigure out a good structure
        }
        if (item.GetType() == typeof(Armor))
        {  

        }
        if (item.GetType() == typeof(Curio))
        {
            
        }
    }

    private void ResolveCurioEffect(Curio cur)
    {
        switch (cur.curiEef)
        {
            case CurioEffect.quick:
                //this gets called twice and Im not sure why
                _strikePointer.bonusSpeed = 1;
                Debug.Log("Pointer speed: "+_strikePointer.baseSpeed);
                break;
            case CurioEffect.greed:
                _gm.ReducePrice(cur.CurioNum);
                break;
        }
        if (cur.curiEef != CurioEffect.greed) { _gm.ReducePrice(0); }
        if (cur.curiEef != CurioEffect.quick) { _strikePointer.bonusSpeed = 0; }

        if(!cur.IsConsumable)
        {
            equipedCurio = cur;
            curioIcon.sprite = cur.PanelIcon;
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
            EquipItem(equipedWeapon, null);
        }
        //armor
        else if(kind == 2)
        {
            equipedArmor.itemLevel = Mathf.Clamp(equipedArmor.itemLevel - 1, 0, 3);
            EquipItem(equipedArmor, null);
        }
    }

    public void UnlockTwoWeapons()
    {
        twoWeapons = true;
        SecondaryWeaponUI.SetActive(true);
    }

    public bool HasTwoWeapons()
    {
        return twoWeapons;
    }

    public Weapon getPrimaryWeapon()// for Style Display
    {
        return PrimaryWeapon;
    }

    public Weapon getEquipedWeapon()// for Style Display
    {
        return equipedWeapon;
    }

    IEnumerator ItemRecievedRoutine(Transform fromHere, Transform goingHere,Item item)
    {
        goingHere.gameObject.SetActive(false);
        
        Vector3 endPos = goingHere.position;
        Vector3 startPos = fromHere.position;
        Transform moveObj = Instantiate(itemAnimPrefab).GetComponent<Transform>();
        moveObj.SetParent(parentForUIAnim);
        moveObj.GetComponent<Image>().sprite = item.PanelIcon;
        float timeEslapsed=0;
        float duration = Vector3.Distance(startPos,endPos) / showSpeed;

        while (moveObj.position!=endPos)
        {
            float t = timeEslapsed/ duration;
            moveObj.position = Vector3.Lerp(startPos, endPos,t);

            timeEslapsed += Time.deltaTime;

            yield return null;
        }

        goingHere.gameObject.SetActive(true);
        Destroy(moveObj.gameObject);
    }

}
