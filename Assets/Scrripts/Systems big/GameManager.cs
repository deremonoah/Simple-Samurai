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
    private EnemysManager _enemyManager;
    private EventManager _eventManager;
    public Text TextCoins;
    public int playerCoins;
    [SerializeField] PlayerHealthBar playerHP;

    public SoundManager SoundMng;

    public StrikeArea mainStrikeArea;

    private PlayerEquipedItemsManager _playerEquipedItems;
    private LootingManager _lootManager;
    private BlackSmithShop _blacksmithShop;
    private FarmShop _farmShop;

    

    void Start()
    {
        _eventManager = GetComponent<EventManager>();
        Time.timeScale = 1f;
        _enemyManager = GetComponent<EnemysManager>();
        playerCoins = 0;
        playerHP = GetComponent<PlayerHealthBar>();
        StrikeArea.SwitchPlayerOn(true);
        _playerEquipedItems = GetComponent<PlayerEquipedItemsManager>();
        _blacksmithShop = GetComponent<BlackSmithShop>();
        _farmShop = GetComponent<FarmShop>();
        _lootManager = GetComponent<LootingManager>();
        
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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _lootManager.RandomItemPull();
        }
#endif
    }


    public void OpenShopPan()
    {
        shopPan.GetComponent<Animator>().SetBool("Open", true);
    }

    public void CloseShopPan()
    {
        if (shopPan.GetComponent<Animator>().GetBool("Open"))
        {
            shopPan.GetComponent<Animator>().SetBool("Open", false);
            _enemyManager.StartNextWave();
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

    public void togglePanel(GameObject panel)
    {
        _blacksmithShop.SetUpgradeCostsButtonsText();
        _farmShop.SetButtonCostsText();


        if (panel.activeInHierarchy == false)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }

    public void PayOut(int coin)
    {
        SoundMng.PlaySound("coin");
        int tempgold = 0;
        Armor equipedArmor = _playerEquipedItems.equipedArmor;
        if (equipedArmor.armrEef == ArmorEffect.greed)
        {
            int minInclusive = equipedArmor.effectNumberOneLevel[equipedArmor.itemLevel];
            int maxExclusive = equipedArmor.effectNumberTwoLevel[equipedArmor.itemLevel] + 1;
            tempgold = Random.Range(minInclusive, maxExclusive);
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

    public void PlayerWins()
    {
        winPan.SetActive(true);
    }

    public void ReducePrice(int gold)
    {
        _blacksmithShop.reduceCost = gold;
        _farmShop.reduceCost = gold;
    }

}
