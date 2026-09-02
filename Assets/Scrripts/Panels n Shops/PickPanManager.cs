using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickPanManager : MonoBehaviour
{
    [SerializeField] GameObject LootingPanel;
    private GameManager _gm;
    private EventManager _eventManager;
    private PlayerEquipedItemsManager _playerEquipedItems;
    private PlayerHealthBar _playerHP;

    [SerializeField] Image[] buttonImages;
    [SerializeField] List<GameObject> buttonContainers=new();
    public List<Item> lootList;//specifically items for post combat looting
    private List<Reward> randLootPicks = new List<Reward>();
    //this is for changing their colors
    public List<Image> BackGroundHoverBoxes;
    //public List<Image> PlayerItemBoarders;

    void Start()
    {
        _gm = GetComponent<GameManager>();
        _playerEquipedItems = FindObjectOfType<PlayerEquipedItemsManager>();
        _eventManager = GetComponent<EventManager>();
        _playerHP = GetComponent<PlayerHealthBar>();


        for (int lcv = 0; lcv < lootList.Count; lcv++)
        {
            lootList[lcv] = Instantiate(lootList[lcv]);
        }
    }

    public void OpenPickPanForLooting()
    {
        foreach(GameObject go in buttonContainers)
        {
            go.SetActive(true);
        }
        //enable all for 3 picks

        LootingPanel.GetComponent<Animator>().SetBool("Open", true);
        _eventManager.CheckNextEvent();
        
        RandomItemPull();
        return;
    }

    public void OpenPickPanForRewarding(List<Reward> rewards)//from mini games how player learns new skills
    {
        foreach (GameObject go in buttonContainers)
        {
            go.SetActive(false);
        }//disable all for 2-3 picks
        for (int lcv=0;lcv<rewards.Count;lcv++)
        {
            buttonContainers[lcv].SetActive(true);
        }

        LootingPanel.GetComponent<Animator>().SetBool("Open", true);
        randLootPicks = rewards;
        LoadLootPicks();
        //has to know what pool  to pull from, sesei, farmer, or blacksmith
        //could get sent list as an alternative call for OpenPickPan(List<rewards>)
        //the reward options have already been decided when sent. 
    }

    public void ClosePickPan()
    {
        if (LootingPanel.GetComponent<Animator>().GetBool("Open"))
        {
            LootingPanel.GetComponent<Animator>().SetBool("Open", false);
        }

        /*for (int lcv = 0; lcv < PlayerItemBoarders.Count; lcv++)
        {
            PlayerItemBoarders[lcv].color = new Color(0, 0, 0, 0);//we only disable the square to help with selecting of the object, so it can still be hovered
        }*/

        //updatePlayerEquipedHoverTips();//so if they want to look at it in town they can
    }

    public void PickButton(int buttonID)
    {
        Debug.Log("type of loot is "+randLootPicks[buttonID].GetType());
        if(randLootPicks[buttonID] is ShopReward)// to check if is of this linage (or derives from that class)
        {
            Debug.Log("shop reward is the type");
            ShopReward re = (ShopReward)randLootPicks[buttonID];
            re.ResolveReward();
            randLootPicks.Clear();
            ClosePickPan();
            return;//so it doesn't equip a non item item()
        }
        else if (randLootPicks[buttonID].GetType() == typeof(Curio))
        {
                bool isConsumable=IsResolveConsumable((Curio)randLootPicks[buttonID]);
            if(isConsumable)
            {
                randLootPicks.Clear();
                ClosePickPan();
                return;//so it doesn't waste time trying to equip a non item
            }
        }
        _playerEquipedItems.EquipItem((Item)randLootPicks[buttonID], buttonImages[buttonID].transform);
        randLootPicks.Clear();
        ClosePickPan();
    }

    public void inspectItem(int itemToPick)
    {
        //wraping when we go past the pick items
        if (itemToPick >= randLootPicks.Count) { itemToPick = 0; }
        else if (itemToPick < 0) { itemToPick = randLootPicks.Count - 1; }

        var dis=FindObjectOfType<ItemDisplayPanel>();
        dis.OpenItemDescriptionPanel((Reward)randLootPicks[itemToPick], itemToPick,itemDisplayOpenedFrom.PickPan);
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

        LoadLootPicks();
    }
    
    private void LoadLootPicks()
    {
        for (int lcv = 0; lcv < randLootPicks.Count; lcv++)//setting the color is now like enabling the image
        {
            buttonImages[lcv].sprite = randLootPicks[lcv].PanelIcon;
            //HoverHelpers[lcv].tipToShow = randLootPicks[lcv].itemDescription;
            if (randLootPicks[lcv].GetType() == typeof(Weapon))
            {
                BackGroundHoverBoxes[lcv].color = FindObjectOfType<ColorManager>().weaponColor;
            }
            else if (randLootPicks[lcv].GetType() == typeof(Armor))
            {
                BackGroundHoverBoxes[lcv].color = FindObjectOfType<ColorManager>().armorColor;
            }
            else
            {
                BackGroundHoverBoxes[lcv].color = FindObjectOfType<ColorManager>().curioColor;
            }
        }
    }

    private bool IsResolveConsumable(Curio cur)
    {
        switch (cur.curiEef)
        {
            case CurioEffect.Koban:
                _gm.playerCoins += cur.CurioNum;
                return true;
                //break; unreachable whines at me
            case CurioEffect.heal:
                _playerHP.HealPlayer(cur.CurioNum);
                return true;
                //break; unreachable whines at me
        }
        return false;
    }

    public bool isPanelOpen()
    {
        return LootingPanel.GetComponent<Animator>().GetBool("Open");
    }
}
