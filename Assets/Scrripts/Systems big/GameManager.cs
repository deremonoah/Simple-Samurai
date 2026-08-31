using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject lossPan;
    public GameObject pickPan;
    public GameObject shopPan;
    public GameObject winPan;
    public GameObject pausePan;
    //for time scale testing
    [SerializeField] TextMeshProUGUI pauseTimeScaleDisplay;
    [SerializeField] TextMeshProUGUI lossTimeScaleDisplay;
    [SerializeField] TextMeshProUGUI WinTimeScaleDisplay;

    private EnemysManager _enemyManager;
    private EventManager _eventManager;
    public Text TextCoins;
    public int playerCoins;

    public SoundManager SoundMng;

    public StrikeArea mainStrikeArea;

    private PlayerEquipedItemsManager _playerEquipedItems;
    private PickPanManager _PickPanManager;
    private BlackSmithShop _blacksmithShop;
    private FarmShop _farmShop;

    private bool _blacksmithInvested;
    private bool _farmInvested;

    public static GameManager instance;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            if(instance!=this)
            {
                Destroy(this);//shouldn't need but hey why not
            }
        }
    }

    void Start()
    {
        _eventManager = GetComponent<EventManager>();
        Time.timeScale = 1f;
        _enemyManager = GetComponent<EnemysManager>();
        playerCoins = 0;
        _playerEquipedItems = GetComponent<PlayerEquipedItemsManager>();
        _blacksmithShop = GetComponent<BlackSmithShop>();
        _farmShop = GetComponent<FarmShop>();
        _PickPanManager = GetComponent<PickPanManager>();

        lossTimeScaleDisplay.text ="";
        pauseTimeScaleDisplay.text = "";
        WinTimeScaleDisplay.text = "";
    }

    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        TextCoins.text = ("" + playerCoins);
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _PickPanManager.RandomItemPull();
        }
#endif
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!pausePan.activeSelf)
            { OpenPausePan(); }
            else
            { ClosePausePan(); }
        }
    }


    public void OpenShopPan()
    {
        pauseTimeScaleDisplay.text = SaveData.instance.getTimeScaleValue() + "";
        shopPan.GetComponent<Animator>().SetBool("Open", true);
    }

    public void CloseShopPan()
    {
        if (shopPan.GetComponent<Animator>().GetBool("Open"))
        {
            shopPan.GetComponent<Animator>().SetBool("Open", false);
            _enemyManager.StartNextWave();
        }
    }

    public void OpenLossPan()
    {
        lossTimeScaleDisplay.text= SaveData.instance.getTimeScaleValue()+"";

        Debug.Log("OpenLossPanCalled");
        lossPan.SetActive(true);
        SaveData.instance.PlayerDied(_enemyManager.WaveControlVariable);
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

    public void PayOut(int min, int maxInclusive)
    {
        SoundMng.PlaySound("coin");
        maxInclusive++;
        //above when it calculates to make it inclusive for the random number
        int ExtraCoins = 0;
        int dropedCoins = Random.Range(min, maxInclusive);
        Armor equipedArmor = _playerEquipedItems.equipedArmor;
        if (equipedArmor.armrEef == ArmorEffect.greed)
        {
            int minInclusive = equipedArmor.effectNumberOneLevel[equipedArmor.itemLevel];
            int maxExclusive = equipedArmor.effectNumberTwoLevel[equipedArmor.itemLevel] + 1;
            ExtraCoins = Random.Range(minInclusive, maxExclusive);
        }
        playerCoins += dropedCoins+ ExtraCoins;
        TextCoins.text = playerCoins.ToString();
    }

    public void SkipPickPayOut()
    {
        PayOut(2, 5);
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
        WinTimeScaleDisplay.text = SaveData.instance.getTimeScaleValue() + "";
        FindObjectOfType<VillageDefense>().PlayerWon();
    }

    public void ReducePrice(int gold)
    {
        _blacksmithShop.curioReduceCost = gold;
        _farmShop.reduceCost = gold;
    }

    public void InvestmentsPayOut()
    {
        int payout = 0;
        if (_blacksmithInvested)
        {
            payout += Random.Range(1, 11);
        }
        if (_farmInvested)
        {
            payout += Random.Range(2, 5);
        }
        playerCoins += payout;
    }

    public void BlacksmithInvest()
    {
        if (playerCoins >= 20 && !_blacksmithInvested)
        {
            playerCoins -= 20;
            _blacksmithInvested = true;
            //in future make this scalable with population
        }
    }

    public void FarmInvest()
    {
        if (playerCoins >= 20 && !_farmInvested)
        {
            playerCoins -= 20;
            _farmInvested = true;
            //in future make this scalable with population
        }
    }

    public bool canBuy(int price)
    {
        return price <= playerCoins;
    }
}
