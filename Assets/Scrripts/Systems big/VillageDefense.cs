using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageDefense : MonoBehaviour
{
    public float VillageDefenseTimerMax;
    [SerializeField] float _villageDefenseTimer;

    public float VillagerLifeTimerMax;
    private float _villagerLifeTimer;

    [SerializeField] bool _defending;
    [SerializeField] bool _beingRaided;
    [SerializeField] int villagers;
    private int _villagersAtStart;
    private int _tempVillagerCount;
    public List<GameObject> FireSprites;
    [SerializeField] int FireIndex;
    [SerializeField] float threashHoldForFire;
    private GameManager _gm;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _villageDefenseTimer = VillageDefenseTimerMax;
        _villagerLifeTimer = VillagerLifeTimerMax;
        _villagersAtStart = villagers;
        threashHoldForFire = _villagersAtStart / 11;
        FireIndex = 0;
    }

    
    void Update()
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
                _villagerLifeTimer = VillagerLifeTimerMax;
                if (_villagersAtStart - villagers >= threashHoldForFire)
                {
                    FireSprites[FireIndex].SetActive(true);
                    _villagersAtStart = villagers;
                    FireIndex++;
                }
            }
            else { _villagerLifeTimer -= Time.deltaTime; }
        }
        if (FireIndex >= 11)
        {
            _gm.OpenLossPan();
        }
    }

    public void startDefending()
    {
        _defending = true;
        _villageDefenseTimer = VillageDefenseTimerMax;
        _villagerLifeTimer = VillagerLifeTimerMax;
        _villagersAtStart = villagers;
        threashHoldForFire = _villagersAtStart / 11;
        FireIndex = 0;
        Debug.Log(threashHoldForFire);
    }

    public void ResetVillage()
    {
        Debug.Log("called reset");
        _beingRaided = false;
        _defending = false;
        _villagersAtStart = villagers;
        foreach (GameObject spr in FireSprites)
        {
            spr.SetActive(false);
        }

    }
}
