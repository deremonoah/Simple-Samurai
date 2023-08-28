using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootingManager : MonoBehaviour
{
    [SerializeField] GameObject LootingPanel;
    private GameManager _gm;
    private EventManager _eventManager;
    private PlayerEquipedItemsManager _playerEquipedItems;
    private BlackSmithShop _blacksmithShop;
    private PlayerHealthBar _playerHP;

    [SerializeField] Image[] buttonImages;
    public List<Item> lootList;
    private List<Item> randLootPicks = new List<Item>();
    //refrences of the background boxes for tool tips
    public List<HoverTip> HoverHelpers;
    //this is for changing their colors
    public List<Image> BackGroundHoverBoxes;
    public List<Image> PlayerItemBoarders;
    

    void Start()
    {
        _gm = GetComponent<GameManager>();
        _playerEquipedItems = FindObjectOfType<PlayerEquipedItemsManager>();
        _eventManager = GetComponent<EventManager>();
        _blacksmithShop = GetComponent<BlackSmithShop>();
        _playerHP = GetComponent<PlayerHealthBar>();


        for (int lcv = 0; lcv < lootList.Count; lcv++)
        {
            lootList[lcv] = Instantiate(lootList[lcv]);
        }

        //below is for the boarders around players equiped items
        PlayerItemBoarders[0].color = FindObjectOfType<ColorManager>().weaponColor;
        PlayerItemBoarders[3].color = FindObjectOfType<ColorManager>().weaponColor;
        PlayerItemBoarders[1].color = FindObjectOfType<ColorManager>().armorColor;
        PlayerItemBoarders[2].color = FindObjectOfType<ColorManager>().curioColor;



        for (int lcv = 0; lcv < PlayerItemBoarders.Count; lcv++)
        {
            PlayerItemBoarders[lcv].gameObject.SetActive(false);
        }
    }

    public void OpenPickPan(string kind)
    {

        LootingPanel.GetComponent<Animator>().SetBool("Open", true);
        _eventManager.CheckNextEvent();

        
    }
    public void ClosePickPan()
    {
        if (LootingPanel.GetComponent<Animator>().GetBool("Open"))
        {
            LootingPanel.GetComponent<Animator>().SetBool("Open", false);
        }

        for (int lcv = 0; lcv < PlayerItemBoarders.Count; lcv++)
        {
            PlayerItemBoarders[lcv].gameObject.SetActive(false);
        }
    }

    public void PickButton(int buttonID)
    {
        if (LootingPanel.GetComponent<Animator>().GetBool("Open") == true)
        {


            if (randLootPicks[buttonID].GetType() == typeof(Curio))
            {
                ResolveManagerCurioEffect((Curio)randLootPicks[buttonID]);
            }
            _playerEquipedItems.EquipItem(randLootPicks[buttonID], _blacksmithShop.lootingUpgradesEnabled);


            randLootPicks.Clear();

            ClosePickPan();
        }
        if(learning)
        {
            //LearnPicks[buttonID] resolve its effect for now enstantiating a buff area
        }
        

    }

    public void RandomItemPull()
    {
        randLootPicks.Clear();
        var tempList = new List<Item>(lootList);
        var temp1 = Random.Range(0, tempList.Count);
        randLootPicks.Add(tempList[temp1]);
        tempList.RemoveAt(temp1);

        var temp2 = Random.Range(0, tempList.Count);
        randLootPicks.Add(tempList[temp2]);
        tempList.RemoveAt(temp2);

        var temp3 = Random.Range(0, tempList.Count);
        randLootPicks.Add(tempList[temp3]);
        tempList.RemoveAt(temp3);

        for (int lcv = 0; lcv < 3; lcv++)
        {
            buttonImages[lcv].sprite = randLootPicks[lcv].itemPanelIcon;
            HoverHelpers[lcv].tipToShow = randLootPicks[lcv].itemDescription;
            if(randLootPicks[lcv].GetType()==typeof(Weapon))
            {
                BackGroundHoverBoxes[lcv].color = FindObjectOfType<ColorManager>().weaponColor;
            }
            else if(randLootPicks[lcv].GetType()==typeof(Armor))
            {
                BackGroundHoverBoxes[lcv].color = FindObjectOfType<ColorManager>().armorColor;
            }
            else
            {
                BackGroundHoverBoxes[lcv].color = FindObjectOfType<ColorManager>().curioColor;
            }
        }


        //turning on item boarders for equiped items
        for (int lcv = 0; lcv < PlayerItemBoarders.Count; lcv++)
        {
            PlayerItemBoarders[lcv].gameObject.SetActive(true);
        }

        //updating hovertips
        PlayerItemBoarders[0].GetComponent<HoverTip>().tipToShow = _playerEquipedItems.equipedWeapon.itemDescription;
        PlayerItemBoarders[1].GetComponent<HoverTip>().tipToShow = _playerEquipedItems.equipedArmor.itemDescription;
        if (_playerEquipedItems.equipedCurio != null)
        { PlayerItemBoarders[2].GetComponent<HoverTip>().tipToShow = _playerEquipedItems.equipedCurio.itemDescription; }
    }


    public void Learning()
    {
        //this either needs to take in an event with specific choices or have random bonus effects and multiple lists for different things

    }

    private void ResolveManagerCurioEffect(Curio cur)
    {
        switch (cur.curiEef)
        {
            case CurioEffect.Koban:
                _gm.playerCoins += cur.CurioNum;
                break;
            case CurioEffect.heal:
                _playerHP.HealPlayer(cur.CurioNum);
                break;
            case CurioEffect.quick:
                _playerEquipedItems.EquipItem(cur, _blacksmithShop.lootingUpgradesEnabled);
                break;
        }
    }

}
