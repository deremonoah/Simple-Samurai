using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject lossPan;
    public GameObject pickPan;
    private EnemysSystem enmsSys;
    public Text TextCoins;
    private int playerCoins;

    [SerializeField] Image[] buttonImages;
    public List<Item> lootList;
    private List<Item> randLootPicks = new List<Item>();
    [SerializeField] StrikeArea mainStrkArea;

    void Start()
    {
        enmsSys = GetComponent<EnemysSystem>();
        playerCoins = 0;

    }


    void Update()
    {
        /*if (enmsSys.hasAllDied()<1)
        {
            OpenPickPan();
        }*/
    }
    public void OpenPickPan()
    {
        pickPan.GetComponent<Animator>().SetBool("Open", true);
        RandomItemPull();
    }
    public void ClosePickPan()
    {
        if (pickPan.GetComponent<Animator>().GetBool("Open"))
        {
            pickPan.GetComponent<Animator>().SetBool("Open", false);
            enmsSys.StartNextWave();
            Debug.Log("stated wave");
        }
    }


    public void OpenLossPan()
    {
        lossPan.SetActive(true);
        
    }
    public void CloseLossPan()
    {
        lossPan.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PayOut(int coin)
    {
        playerCoins += coin;
        TextCoins.text = playerCoins.ToString();
    }

    public void PickButton(int buttonID)
    {
        if (pickPan.GetComponent<Animator>().GetBool("Open") == true)
        {
            if (randLootPicks[buttonID].GetType() == typeof(Weapon))
            {
                mainStrkArea.SetWeapon(randLootPicks[buttonID] as Weapon);
            }
            if (randLootPicks[buttonID].GetType() == typeof(Armor))
            {
                FindObjectOfType<HealthBar>().SetArmor(randLootPicks[buttonID] as Armor);
            }
            randLootPicks.RemoveAt(buttonID);
            Debug.Log("button proc");
            ClosePickPan();
        }

    }


    private void RandomItemPull()
    {
        //add to the randweapon list from list of weapons but can't repeat numbers so we could generate 3 random numbers
        //then we also should make sure the weapon doesn't match the one the player has and it could be an item so armor
        //or curio but the player can only have 1 of each weapon armor and curio that this should check if the random 
        //yeah pretty complicated we gonna start with just random numbers
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
        }

        Debug.Log(lootList.Count);
    }
}
