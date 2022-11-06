using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageDefense : MonoBehaviour
{
    public float VillageDefenseTimerMax;
    [SerializeField] float _villageDefenseTimer;

    public float VillagerLifeTimerMax;
    private float _villagerLifeTimer;

    [SerializeField] bool _defending;
    [SerializeField] bool _beingRaided;
    public int villagers;
    private int _villagersAtStart;
    private int _tempVillagerCount;
    public List<GameObject> FireSprites;
    [SerializeField] int FireIndex;
    [SerializeField] float threashHoldForFire;
    private GameManager _gm;
    public int DamageTaken;

    [SerializeField] List<GameObject> TurnedOnShopButtons;
    [SerializeField] GameObject BlackSmithButton;
    [SerializeField] GameObject RepairVillageButton;
    [SerializeField] List<GameObject> _damagedShops;

    [SerializeField] GameObject populationIcon;
    [SerializeField] Text populationText;

    public bool Turotialing;
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _villageDefenseTimer = VillageDefenseTimerMax;
        _villagerLifeTimer = VillagerLifeTimerMax;
        _villagersAtStart = villagers;
        threashHoldForFire = (float)_villagersAtStart / 11;
        FireIndex = 0;
        _damagedShops = new List<GameObject>();
    }

    
    void Update()
    {
        if (!Turotialing)
        {
            if (_defending)
            {
                _villageDefenseTimer -= Time.deltaTime;
            }
            if (_villageDefenseTimer <= 0)
            {
                _defending = false;
                _beingRaided = true;
                _villageDefenseTimer = VillageDefenseTimerMax;
                FireSprites[0].SetActive(true);
            }
            if (_beingRaided)
            {
                if (_villagerLifeTimer <= 0)
                {
                    villagers -= 1;
                    DamageTaken++;
                    _villagerLifeTimer = VillagerLifeTimerMax;
                    if ((float)_villagersAtStart - villagers >= threashHoldForFire)
                    {
                        //Debug.Log("if check: " + ((float)_villagersAtStart - villagers));
                        if (FireIndex < FireSprites.Count)
                        { FireSprites[FireIndex].SetActive(true); }
                        _villagersAtStart = villagers;

                        threashHoldForFire = _villagersAtStart / Mathf.Clamp((FireSprites.Count - FireIndex), 1, 50);
                        FireIndex++;
                    }
                }
                else { _villagerLifeTimer -= Time.deltaTime; }
            }
            if (FireIndex >= 11 && villagers <= 0)
            {
                _gm.OpenLossPan();
            }

            populationText.text = "" + villagers;
        }
    }

    public void startDefending()
    {
        _defending = true;
        _villageDefenseTimer = VillageDefenseTimerMax;
        _villagerLifeTimer = VillagerLifeTimerMax;
        _villagersAtStart = villagers;
        threashHoldForFire = (float)_villagersAtStart / 11;
        DamageTaken = 0;
        FireIndex = 0;
        //Debug.Log("fire therash hold: " + threashHoldForFire);
    }

    public void ResetVillage()
    {
        _beingRaided = false;
        _defending = false;
        _villagersAtStart = villagers;
        foreach (GameObject spr in FireSprites)
        {
            spr.SetActive(false);
        }

    }


    public void DamagedVillage()
    {
        int rand = Random.Range(0, TurnedOnShopButtons.Count);
        Debug.Log("rand range for village" + rand);
        GameObject temp = TurnedOnShopButtons[rand];
        TurnedOnShopButtons.RemoveAt(rand);
        temp.SetActive(false);
        _damagedShops.Add(temp);

        RepairVillageButton.SetActive(true);
    }

    public void RepairVillage()
    {
        if (_gm.playerCoins >= 10)
        {
            foreach (GameObject but in _damagedShops)
            {
                TurnedOnShopButtons.Add(but);
                but.SetActive(true);
            }
            RepairVillageButton.SetActive(false);
            _gm.playerCoins -= 10;
            _damagedShops.Clear();
            DamageTaken = 0;
        }
        /*else
        {
            DamagedVillage();
        }*/
    }

    public void TurnOnBlackSmith()
    {
        BlackSmithButton.SetActive(true);
        TurnedOnShopButtons.Add(BlackSmithButton);
    }

    public void DisplayPopulation(bool state)
    {
        populationIcon.SetActive(state);
    }
}
