using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    private GameManager _gm;
    public Text EventPopUpText;
    public GameObject EventPanel;
    private List<Event> _nextEvents = new List<Event>();
    private Event _currentEvent;
    public List<GameObject> Buttons;
    public List<Text> ButtonTexts;
    private EnemysManager _enemyManager;
    private BlackSmithShop _blacksmith;
    private VillageDefense _villageDefense;
    private bool _villageHasBeenDamaged = false;

    [SerializeField] GameObject blacksmithInvestButton;
    [SerializeField] GameObject farmInvestButton;
    private bool _investingEnabled = false;
    public GameObject blacksmithBackground;
    private bool _lostMany;
    [SerializeField] bool hasPicked;


    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _enemyManager = GetComponent<EnemysManager>();
        _blacksmith = GetComponent<BlackSmithShop>();
        _villageDefense = GetComponent<VillageDefense>();
        blacksmithInvestButton.SetActive(false);
        farmInvestButton.SetActive(false);
        blacksmithBackground.SetActive(false);

        for (int lcv = 0; lcv < Buttons.Count; lcv++)
        {
            Buttons[lcv].SetActive(false);

        }
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CheckNextEvent();
            CallDisplayEvents();
        }
    }

    public bool CheckNextEvent()
    {
        //is it more efficent to send the data with the call or have a refrence to gm public variable like below?
        //or should i make it a static call so the gm doesn't need a private refrence to this?

        _nextEvents = new List<Event>();

        if (_gm.playerCoins >= 40 && !_investingEnabled)
        {
            _investingEnabled = true;
            _nextEvents.Add(Resources.Load<Event>("Events/Investments"));
        }
        if (_enemyManager.WaveControlVariable >= 2 && !blacksmithBackground.activeSelf)
        {
            _nextEvents.Add(Resources.Load<Event>("Events/BlackSmith"));
        }



        var rand = Random.Range(0, 10);
        if (rand < 3 && (_lostMany || _enemyManager.WaveControlVariable >= 8))
        {
            _nextEvents.Add(Resources.Load<Event>("Events/Refugees"));
        }

        //make it a list of events not just one but the 2nd damage one can over write the first
        //if (_villageDefense.DamageTaken >= 1 && !_villageHasBeenDamaged)
        //{
        //    _nextEvents.Add(Resources.Load<Event>("Events/smolDamagedCity"));
        //    _villageDefense.DisplayPopulation(true);
        //    _villageHasBeenDamaged = true;
        //}
        //if (_villageDefense.DamageTaken >= 10 )
        //{
        //    _villageDefense.DamageTaken = 0;
        //    _nextEvents = Resources.Load<Event>("Events/DamagedCity");
        //    _villageDefense.DisplayPopulation(true);
        //    _lostMany = true;
        //}

        if (_villageDefense.DamageTaken >= 1)
        {
            _villageDefense.DisplayPopulation(true);
            if (_villageDefense.DamageTaken >= 10)
            {
                _nextEvents.Add(Resources.Load<Event>("Events/DamagedCity"));
                _lostMany = true;
            }
            else if (!_villageHasBeenDamaged)
            {
                _nextEvents.Add(Resources.Load<Event>("Events/smolDamagedCity"));
                _villageHasBeenDamaged = true;
            }
        }


        return _nextEvents.Count > 0;
    }

    public void CallDisplayEvents()
    {
        StartCoroutine(DisplayEventsRoutine());
    }

    private IEnumerator DisplayEventsRoutine()
    {
        EventPanel.SetActive(true);


        foreach (Event eve in _nextEvents)
        {
            _currentEvent = eve;
            EventPopUpText.text = eve.textStatements[0];
            for (int lcv = 0; lcv < Buttons.Count; lcv++)
            {
                Buttons[lcv].SetActive(false);

            }
            for (int lcv = 0; lcv < eve.buttonOptions.Count; lcv++)
            {
                Buttons[lcv].SetActive(true);
                ButtonTexts[lcv].text = eve.buttonOptions[lcv];
            }
            hasPicked = false;
            while (!hasPicked)
            {
                yield return null;
            }
        }
        EventPanel.SetActive(false);
    }


    public void EventButton(int num)
    {
        if (num == 0)
        {
            firstResault();
        }

        if (num == 1)
        {
            SecondResault();
        }
        ResolvePassiveEffect();
        hasPicked = true;
    }

    public void firstResault()
    {
        if (_currentEvent.myeventEffect == EventEffect.blackSmith)
        {
            BlackSmithArived();
        }
        if (_currentEvent.myeventEffect == EventEffect.damagedCity)
        {
            if (_gm.playerCoins >= 10)
            {
                _villageDefense.RepairVillage();
            }
            else
            {
                _villageDefense.DamagedVillage();
            }
            //also if the player can't afford or doesn't pay it needs to enable a button that can repair on the village panel
        }
        if (_currentEvent.myeventEffect == EventEffect.moreVillagers)
        {
            _villageDefense.villagers += Random.Range(5, 16);
            _gm._farmShop.GotMoreVillagers();
        }
    }

    public void SecondResault()
    {
        if (_currentEvent.myeventEffect == EventEffect.damagedCity)
        {
            _villageDefense.DamagedVillage();
        }
    }


    public void ResolvePassiveEffect()
    {
        //now player can invest
        if (_currentEvent.myeventEffect == EventEffect.invest)
        {
            EnableInvesting();
        }
    }

    #region Event Specific Classes
    public void BlackSmithArived()
    {
        //take in temp variable then call shop.TurnOnButton() so the temp = whatever the shop is
        _enemyManager.enemyWaves.Insert(2, Resources.Load<EnmWave>("Waves/Wave3.5"));
        _enemyManager.enemyWaves.RemoveAt(3);
        _villageDefense.TurnOnBlackSmith();
        blacksmithBackground.SetActive(true);
    }

    public void EnableInvesting()
    {
        blacksmithInvestButton.SetActive(true);
        farmInvestButton.SetActive(true);
    }

    #endregion
}
