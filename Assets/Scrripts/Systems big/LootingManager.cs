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

    void Start()
    {
        _gm = GetComponent<GameManager>();
        _playerEquipedItems = GetComponent<PlayerEquipedItemsManager>();
        _eventManager = GetComponent<EventManager>();
        _blacksmithShop = GetComponent<BlackSmithShop>();
        _playerHP = GetComponent<PlayerHealthBar>();

        for (int lcv = 0; lcv < lootList.Count; lcv++)
        {
            lootList[lcv] = Instantiate(lootList[lcv]);
        }
    }

    public void OpenPickPan()
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
            buttonImages[lcv].GetComponent<HoverTip>().tipToShow = randLootPicks[lcv].itemDescription;
        }

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
