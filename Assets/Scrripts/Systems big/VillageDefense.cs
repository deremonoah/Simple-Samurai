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
        threashHoldForFire = (float)_villagersAtStart / 11;
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
                if ((float)_villagersAtStart - villagers >= threashHoldForFire)
                {
                    Debug.Log("if check: "+ ((float)_villagersAtStart - villagers));
                    if (FireIndex  < FireSprites.Count)
                    { FireSprites[FireIndex].SetActive(true); }
                    _villagersAtStart = villagers;

                    threashHoldForFire =_villagersAtStart / Mathf.Clamp((FireSprites.Count - FireIndex),1,50);
                    FireIndex++;
                }
            }
            else { _villagerLifeTimer -= Time.deltaTime; }
        }
        if (FireIndex >= 11 && villagers <=0)
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
        threashHoldForFire = (float)_villagersAtStart / 11;
        FireIndex = 0;
        Debug.Log("fire therash hold: " + threashHoldForFire);
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
}
