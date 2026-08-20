using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmShop : ShopGiveReward
{
    [Header("most recent score")]
    [SerializeField] float recentScore;//I plan to remove this

    private GameManager _gm;
    private PlayerHealthBar _playerHP;
    private PlayerEquipedItemsManager pEquip;
    private PickPanManager _picPanMan;

    private float FarmHeal = 40;
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

    [Header("Helping rewards")]
    [SerializeField] int aproval;//for how much they like you
    [SerializeField] int FavoredCostReduction;//perminent
    [SerializeField] Transform rewardFromHere;
    [SerializeField] int ScoreAboveForThreeRewards;

    [Header("common rewards")]
    [SerializeField] List<Reward> RewardsCommon;

    [Header("Rare rewards")]
    [SerializeField] List<Reward> RewardsRare;
    [SerializeField] float rareScoreAboveToGet;

    [Header("Temporary helping buffs")]//do I need to see this in inspector?
    [SerializeField] int healCostReduction;
    [SerializeField] int maxHPCostReduction;
    //private int improveFarmProgress=0; //would be cool to add progress bar for any time you help
    

    void Start()
    {
        _gm = GetComponent<GameManager>();
        _playerHP = GetComponent<PlayerHealthBar>();
        pEquip = FindObjectOfType<PlayerEquipedItemsManager>();
        _picPanMan = FindObjectOfType<PickPanManager>();
    }

    public void FarmHealButton()
    {
        int cost = Mathf.Clamp((healCost * healPurchasesThisTurn) - (reduceCost+healCostReduction+FavoredCostReduction),0,10000);
        if (_gm.playerCoins >= cost)
        {
            _gm.playerCoins -= cost;
            _playerHP.HealPlayer(FarmHeal);
            healPurchasesThisTurn += 1;
            healCostReduction = 0;//temporary effect
        }
        SetButtonCostsText();
    }

    public void IncreaseMaxHPButton()
    {
        int cost = Mathf.Clamp((improveHPCost * IncreasedMaxHPtimes) - (reduceCost + maxHPCostReduction + FavoredCostReduction),0,10000);
        if (_gm.playerCoins >= cost)
        {
            _gm.playerCoins -= cost;
            _playerHP.IncreaseMaxHPBy(FarmIncHP);
            if (cost != 0)//if you get for free shouldn't increase in price like you bought one
            { IncreasedMaxHPtimes += 1; }
            maxHPCostReduction = 0;//temporary cost reset
        }
        SetButtonCostsText();
    }

    public void ImproveFarmButton()
    {
        int cost = improveFarmCost - reduceCost;
        if (_gm.playerCoins >= cost && FarmLvl < 4)
        {
            _gm.playerCoins -= cost;
            resolveImproveFarm();
        }
        SetButtonCostsText();
    }

    private void resolveImproveFarm()
    {
        FarmLvl++;
        switch (FarmLvl)
        {
            case 2:
                FarmHeal = 60;
                FarmIncHP = 40;
                farmLvlImages[0].SetActive(true);
                IncreasedMaxHPtimes = 1;
                healPurchasesThisTurn = 1;
                break;
            case 3:
                FarmHeal = 80;
                FarmIncHP = 60;
                farmLvlImages[1].SetActive(true);
                IncreasedMaxHPtimes = 1;
                healPurchasesThisTurn = 1;
                break;
            case 4:
                FarmHeal = 100;
                FarmIncHP = 100;
                farmLvlImages[2].SetActive(true);
                IncreasedMaxHPtimes = 1;
                healPurchasesThisTurn = 1;
                break;

        }
        improveHPCost = 10;//reset so if they spent time increasing max hp they can reset the price
    }

    public void SetButtonCostsText()
    {
        //heal text
        int cost = Mathf.Clamp((healCost * healPurchasesThisTurn) - (reduceCost + healCostReduction + FavoredCostReduction), 0, 10000);
        healText.text = "Heal "+FarmHeal+"HP for "+ cost +"g";

        //max hp up text
        cost = Mathf.Clamp((improveHPCost * IncreasedMaxHPtimes) - (reduceCost + maxHPCostReduction + FavoredCostReduction), 0, 10000);
        improveHealthText.text = FarmIncHP+" More Max HP " + cost +"g";

        cost = Mathf.Clamp(improveFarmCost - (reduceCost+FavoredCostReduction),0,10000);
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

    public void RewardFromFarmer(float score)
    {
        recentScore = score;
        aproval +=5;//might change amount to be vairable based ons score
        int rand = Random.Range(0, (int)score) + aproval;
        ShowAppreciation(heartOverHead, null);

        //figuring out rewards
        List<Reward> rewardsToSend=new();
        int itemLength = 0;
        Debug.Log("Rand for loot length " + rand);
        if(rand>=ScoreAboveForThreeRewards)
        {
            itemLength = 3;
        }
        else { itemLength = 2; }
        while(rewardsToSend.Count<itemLength)
        {
            rand = Random.Range(0, (int)score) + aproval;
            if(rand>rareScoreAboveToGet)
            {
                int randRare = Random.Range(0, RewardsRare.Count);
                if(!rewardsToSend.Contains(RewardsRare[randRare]))
                {
                    rewardsToSend.Add(RewardsRare[randRare]);
                }
            }
            else
            {
                int randCom = Random.Range(0, RewardsRare.Count);
                if (!rewardsToSend.Contains(RewardsCommon[randCom]))
                {
                    rewardsToSend.Add(RewardsCommon[randCom]);
                }
            }
        }

        //sending to pickPan
        _picPanMan.OpenPickPanForRewarding(rewardsToSend);

        //seing a progress bar for improving the farm could be cool, motivating for players and they see with their help farm is getting better
    }

    public void DiscountHeal()
    {
        healCostReduction = healCost;//free, even on refugee rounds
        ShowAppreciation(heartOverHead, healText.transform);
        SetButtonCostsText();
    }

    public void DiscountMaxHP()
    {
        maxHPCostReduction = improveHPCost;//so its free no matter the current price
        ShowAppreciation(heartOverHead, improveHealthText.transform);
        SetButtonCostsText();
    }

    public void DiscountAll()
    {
        FavoredCostReduction += 1;
        ShowAppreciation(heartOverHead, healText.transform);
        ShowAppreciation(heartOverHead, improveHealthText.transform);
        ShowAppreciation(heartOverHead, improveFarmText.transform);
        SetButtonCostsText();
    }

    public void ImproveFarmReward()
    {
        resolveImproveFarm();
        ShowAppreciation(heartOverHead, farmLvlImages[FarmLvl-2].transform);//farm level starts at 1 when improved will be 2, but list is 0,1,2
    }
}
