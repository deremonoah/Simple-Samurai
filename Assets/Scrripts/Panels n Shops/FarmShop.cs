using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmShop : MonoBehaviour
{
    private GameManager _gm;
    private PlayerHealthBar _playerHP;

    private float FarmHeal = 30;
    private float FarmIncHP = 25;
    private int FarmLvl = 1;

    public Text healText;
    public Text improveHealthText;
    public Text improveFarmText;

    [SerializeField] List<GameObject> farmLvlImages;

    private int healCost = 5;
    private int improveHPCost = 10;
    private int improveFarmCost = 15;

    private int healPurchasesThisTurn = 1;
    private int IncreasedMaxHPtimes = 1;

    public int reduceCost;
    void Start()
    {
        _gm = GetComponent<GameManager>();
        _playerHP = GetComponent<PlayerHealthBar>();
    }

    void Update()
    {
        
    }


    public void FarmHealButton()
    {
        int cost = (healCost * healPurchasesThisTurn) - reduceCost;
        if (_gm.playerCoins >= cost)
        {
            _gm.playerCoins -= cost;
            _playerHP.HealPlayer(FarmHeal);
            healPurchasesThisTurn += 1;
        }
        SetButtonCostsText();
    }

    public void IncreaseMaxHPButton()
    {
        int cost = (improveHPCost * IncreasedMaxHPtimes) - reduceCost;
        if (_gm.playerCoins >= cost)
        {
            _gm.playerCoins -= cost;
            _playerHP.IncreaseMaxHPBy(FarmIncHP);
            IncreasedMaxHPtimes += 1;
        }
        SetButtonCostsText();
    }

    public void ImproveFarmButton()
    {
        int cost = improveFarmCost - reduceCost;
        if (_gm.playerCoins >= cost && FarmLvl < 4)
        {
            _gm.playerCoins -= cost;
            FarmLvl++;
            switch (FarmLvl)
            {
                case 2:
                    FarmHeal = 40;
                    FarmIncHP = 30;
                    farmLvlImages[0].SetActive(true);
                    break;
                case 3:
                    FarmHeal = 60;
                    FarmIncHP = 60;
                    farmLvlImages[1].SetActive(true);
                    break;
                case 4:
                    FarmHeal = 80;
                    FarmIncHP = 100;
                    farmLvlImages[2].SetActive(true);
                    break;

            }
        }
    }

    public void SetButtonCostsText()
    {
        int cost = (healCost * healPurchasesThisTurn) - reduceCost;
        healText.text = "Heal "+ cost +"g";

        cost = (improveHPCost * IncreasedMaxHPtimes) - reduceCost;
        improveHealthText.text = "More Max HP " + cost +"g";

        cost = improveFarmCost - reduceCost;
        improveFarmText.text = "Improve Farm " + cost + "g";
    }

    public void ResetHealPurchases()
    {
        healPurchasesThisTurn = 1;
    }

    public void GotMoreVillagers()
    {
        healPurchasesThisTurn += 1;
    }
}
