using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmShop : MonoBehaviour
{
    [Header("most recent score")]
    [SerializeField] float recentScore;//I plan to remove this

    private GameManager _gm;
    private PlayerHealthBar _playerHP;
    private PlayerEquipedItemsManager pEquip;

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
    [SerializeField] List<Item> itemRewards;
    [SerializeField] int FavoredCostReduction;//perminent
    [SerializeField] Transform rewardFromHere;

    [Header("Temporary helping buffs")]//do I need to see this in inspector?
    [SerializeField] int healCostReduction;
    [SerializeField] int maxHPCostReduction;
    private int improveFarmProgress=0;
    

    void Start()
    {
        _gm = GetComponent<GameManager>();
        _playerHP = GetComponent<PlayerHealthBar>();
        pEquip = FindObjectOfType<PlayerEquipedItemsManager>();
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
            IncreasedMaxHPtimes += 1;
            maxHPCostReduction = 0;//temporary
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

        if (rand < 10)
        {
            //nothing given
            //count up towards improve farm?
            //increase liked more
            aproval += 5;
        }
        else if (rand >= 11 && rand <= 49)
        {
            healCostReduction = 10;//probably free, even on refugee rounds
        }
        else if (rand >= 50 && rand <= 80)
        {
            maxHPCostReduction = improveHPCost;//so its free no matter the current price
        }
        else if (rand > 81)//loot is onigiri consumable and idk what else
        {
            //the premo stuff here
            //plus farm progress++
            //maybe also curio here
            FavoredCostReduction += 1;
        }
        else if(rand>100)
        {
            rand = Random.Range(0, itemRewards.Count);
            pEquip.EquipItem(itemRewards[rand], rewardFromHere);
        }
        improveFarmProgress += 1;

        if(improveFarmProgress>=3)
        {
            resolveImproveFarm();
            improveFarmProgress = 0;
        }
        SetButtonCostsText();
        //free heal, maybe with still having to click the button and saves for next round
        //improve farm if you help a few times of not getting anything
        //increase max hp free? onigiri curio
    }
}
