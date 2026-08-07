using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithShop : MonoBehaviour
{
    [Header("most recent score")]
    [SerializeField] float recentScore;//I plan to remove this

    private PlayerEquipedItemsManager pEquip;
    private GameManager _gm;
    public GameObject panelBSButton;
    private SoundManager _soundM;
    private int baseCost = 10;
    public Text improveWeaponText;
    public Text improveArmorText;
    public Text lootingUpgradeText;
    public bool lootingUpgradesEnabled = false;
    public int curioReduceCost;

    [Header("Helping rewards")]
    [SerializeField] int aproval;//for how much they like you
    [SerializeField] List<Item> itemRewards;
    [SerializeField] int perminentCostReduction;
    [SerializeField] int tempWeaponCostReduction;
    [SerializeField] int tempArmorCostRecution;
    //maybe add a 3rd for blanket?

    void Start()
    {
        pEquip = GetComponent<PlayerEquipedItemsManager>();
        _gm = GetComponent<GameManager>();
        _soundM = FindObjectOfType<SoundManager>();
        panelBSButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR       
        if(Input.GetKeyDown(KeyCode.R))
        { 
            pEquip.equipedWeapon.itemLevel = 0;
            pEquip.EquipItem(pEquip.equipedWeapon, lootingUpgradesEnabled);
        }
#endif
    }

    

    public void ImproveWeaponButton()
    {
        var itemLvl = Mathf.Clamp(pEquip.equipedWeapon.itemLevel + 1, 0, 4);
        var cost = Mathf.Clamp((baseCost * itemLvl)-(curioReduceCost+tempWeaponCostReduction+perminentCostReduction),0,10000);

        if (itemLvl <= 3 && _gm.playerCoins >= cost)
        {
            pEquip.equipedWeapon.itemLevel = Mathf.Clamp(itemLvl, 0, 3);
            pEquip.EquipItem(pEquip.equipedWeapon, lootingUpgradesEnabled);

            _gm.playerCoins -= cost;
            tempWeaponCostReduction = 0;
            _soundM.PlaySound("upgrade");
            //improveWeaponCost += 10;
            SetUpgradeCostsButtonsText();
        }
    }

    public void ImproveArmorButton()
    {
        var itemLvl = Mathf.Clamp(pEquip.equipedArmor.itemLevel + 1, 0, 4);
        var cost = Mathf.Clamp((baseCost * itemLvl)-(curioReduceCost+tempArmorCostRecution+perminentCostReduction),0,10000);

        if (itemLvl <= 3 && _gm.playerCoins >= cost)
        {
            pEquip.equipedArmor.itemLevel = Mathf.Clamp(itemLvl, 0, 3);
            pEquip.EquipItem(pEquip.equipedArmor, lootingUpgradesEnabled);

            _soundM.PlaySound("upgrade");
            tempArmorCostRecution = 0;
            _gm.playerCoins -= cost;
            //improveArmorCost += 10;
            SetUpgradeCostsButtonsText();
        }
    }

    public void EnableLootingUpgrades()
    {
        if (_gm.playerCoins >= 5 && lootingUpgradesEnabled == false)
        {
            lootingUpgradesEnabled = true;
            _gm.playerCoins -= 5;
        }
    }

    public void SetUpgradeCostsButtonsText()
    {
        int itemLvl = Mathf.Clamp(pEquip.equipedWeapon.itemLevel + 1,0,4);
        int temp = Mathf.Clamp((baseCost * itemLvl) - (curioReduceCost + tempWeaponCostReduction + perminentCostReduction), 0, 10000);
        improveWeaponText.text = "Improve Weapon " + temp + "g";

        itemLvl = Mathf.Clamp(pEquip.equipedArmor.itemLevel + 1, 0, 4);
        temp = Mathf.Clamp((baseCost * itemLvl) - (curioReduceCost + tempArmorCostRecution + perminentCostReduction), 0, 10000);
        improveArmorText.text = "Improve Armor " + temp + "g";
    }
    public void TurnOnBlackSmith()
    {
        panelBSButton.SetActive(true);
        //TurnedOnShopButtons.Add(BlackSmithButton);
    }

    public void RewardFromBlacksmith(float score)
    {
        recentScore = score;
        aproval += 5;//might change amount to be vairable based ons score
        int rand = Random.Range(0, (int)score) + aproval;

        if (rand < 20)
        {
            //nothing given
            //increase liked more
            return;
        }
        else if (rand >= 20 && rand <= 49)
        {
            //reduced cost upgrade or gives you 3-7 gold?
            int coins = Random.Range(3, 7);
            _gm.playerCoins += coins;
        }
        else if (rand >= 50 && rand <= 69)
        {
            //temporary buff to weapon max damage or base damage
            tempWeaponCostReduction = 40;
        }
        else if (rand >= 70 && rand <= 89)
        {
            //free upgrade for either?
            tempArmorCostRecution = 40;
        }
        else if (rand > 90)
        {
            //the premo stuff here
            //free weapon or armor, maybe perminent reduce cost
            rand = Random.Range(0, itemRewards.Count);
            pEquip.EquipItem(itemRewards[rand], false);
        }

        SetUpgradeCostsButtonsText();
        //blacksmith options: free upgrade to weapon or armor, maybe player picks? Reduce cost use?
        //what else? gold? a random armor or weapon? sharpens your weapon? temp buff, shore up your armor, temp buff?
        //make your weapon pointer bigger? strike area bigger?
    }
}
