using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject lossPan;
    public GameObject pickPan;
    public GameObject shopPan;
    public GameObject winPan;
    public GameObject pausePan;
    public bool Paused = false;
    private EnemysManager _enemyManager;
    public Text TextCoins;
    [SerializeField] int playerCoins;
    [SerializeField] PlayerHealthBar playerHP;

    public SoundManager SoundMng;

    [SerializeField] Image[] buttonImages;
    public List<Item> lootList;
    private List<Item> randLootPicks = new List<Item>();
    public StrikeArea mainStrkArea;

    public bool bonusGold;
    #region Farm Varibles
    private float FarmHeal = 30;
    private float FarmIncHP = 10;
    private int FarmLvl = 1;
    [SerializeField] List<GameObject> farmLvlImages;
    
    #endregion

    void Start()
    {
        Time.timeScale = 1f;
        _enemyManager = GetComponent<EnemysManager>();
        playerCoins = 0;
        playerHP = GetComponent<PlayerHealthBar>();
        StrikeArea.SwitchPlayerOn(true);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P) )
        {
            if (!pausePan.activeSelf)
            { OpenPausePan(); }
            else
            { ClosePausePan(); }
        }
    }

    void Update()
    {
        TextCoins.text = ("" + playerCoins);
    }
    public void OpenPickPan()
    {
        pickPan.GetComponent<Animator>().SetBool("Open", true);
        RandomItemPull();
        Paused = true;
        StrikeArea.SwitchPlayerOn(false);
    }
    public void ClosePickPan()
    {
        if (pickPan.GetComponent<Animator>().GetBool("Open"))
        {
            pickPan.GetComponent<Animator>().SetBool("Open", false);
            //enmsSys.StartNextWave();
            OpenShopPan();
            Paused = false;
        }
    }

    public void OpenShopPan()
    {
        shopPan.GetComponent<Animator>().SetBool("Open", true);
        Paused = true;
    }

    public void CloseShopPan()
    {
        if (shopPan.GetComponent<Animator>().GetBool("Open"))
        {
            shopPan.GetComponent<Animator>().SetBool("Open", false);
            _enemyManager.StartNextWave();
            Paused = false;
            StrikeArea.SwitchPlayerOn(true);
        }
    }

    public void OpenLossPan()
    {
        lossPan.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseLossPan()
    {
        lossPan.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenPausePan()
    {
        pausePan.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePausePan()
    {
        pausePan.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenClosePanel(GameObject panel)
    {
        if (panel.activeInHierarchy == false)
        {
            panel.SetActive(true);
            Paused = true;
        }
        else
        {
            panel.SetActive(false);
            Paused = false;
        }
    }

    public void PayOut(int coin)
    {
        SoundMng.PlaySound("coin");
        int tempgold = 0;
        if (bonusGold)
        {
            tempgold = Random.Range(playerHP.myArmor.eefNumOne, playerHP.myArmor.eefNumTwo+1);
        }
        playerCoins += coin+ tempgold;
        TextCoins.text = playerCoins.ToString();
    }

    public void robPlayer(int coin)
    {
        playerCoins -= coin;
        if(playerCoins <= 0) { playerCoins = 0; }
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
                FindObjectOfType<PlayerHealthBar>().SetArmor(randLootPicks[buttonID] as Armor);
            }
            if (randLootPicks[buttonID].GetType() == typeof(Curio))
            {
                ResolveCurioEffect((Curio)randLootPicks[buttonID]);
            }
            randLootPicks.Clear();

            ClosePickPan();
        }

    }

    private void ResolveCurioEffect(Curio cur)
    {
        switch (cur.curiEef)
        {
            case CurioEffect.Koban:
                playerCoins += cur.CurioNum;
                break;
            case CurioEffect.heal:
                playerHP.HealPlayer(cur.CurioNum);
                break;
        }
    }

    public void PlayerWins()
    {
        winPan.SetActive(true);
    }

    #region Shop Buttons

    public void FarmHealButton()
    {
        if (playerCoins >= 5)
        {
            playerCoins -= 5;
            playerHP.HealPlayer(FarmHeal);
        }
        
    }

    public void IncreaseMaxHPButton()
    {
        if (playerCoins >= 10)
        {
            playerCoins -= 10;
            playerHP.maxHealth +=FarmIncHP;
        }
    }

    public void ImproveFarmButton()
    {
        if (playerCoins >=15 && FarmLvl<4)
        {
            playerCoins -= 15;
            FarmLvl++;
            switch (FarmLvl)
            {
                case 2:
                    FarmHeal = 40;
                    FarmIncHP = 15;
                    farmLvlImages[0].SetActive(true);
                    break;
                case 3:
                    FarmHeal = 60;
                    FarmIncHP = 25;
                    farmLvlImages[1].SetActive(true);
                    break;
                case 4:
                    FarmHeal = 80;
                    FarmIncHP = 40;
                    farmLvlImages[2].SetActive(true);
                    break;
                
            }
        }
    }


    #endregion

    private void RandomItemPull()
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
}
