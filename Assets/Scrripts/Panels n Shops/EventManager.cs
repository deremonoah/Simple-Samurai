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
    private bool _toldAboutHeal = false;
    //public GameObject blacksmithBackground; new background doesn't work
    private bool hasBlacksmith= false;
    private bool _lostMany;
    [SerializeField] bool hasPicked;
    public static bool PanelUP = false;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _enemyManager = GetComponent<EnemysManager>();
        _blacksmith = GetComponent<BlackSmithShop>();
        _villageDefense = GetComponent<VillageDefense>();
        blacksmithInvestButton.SetActive(false);
        farmInvestButton.SetActive(false);
        //blacksmithBackground.SetActive(false);

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
        if(EventPanel.active == true)
        {
            PanelUP = true;
        }else
        {
            PanelUP = false;
        }
    }

    public bool CheckNextEvent()
    {
        //is it more efficent to send the data with the call or have a refrence to gm public variable like below?
        //or should i make it a static call so the gm doesn't need a private refrence to this?

        _nextEvents = new List<Event>();

        var rand = Random.Range(0, 10);
        var wave = _enemyManager.WaveControlVariable;
        if ((wave >=1 && rand <= 4 ) && !hasBlacksmith)
        {
            _nextEvents.Add(Resources.Load<Event>("Events/BlackSmith"));
        }

        rand = Random.Range(0, 10);
        if (rand <= 3 && (_lostMany || _enemyManager.WaveControlVariable >= 6))
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

        if (_gm.playerCoins >= 40 && !_investingEnabled)
        {
            _investingEnabled = true;
            _nextEvents.Add(Resources.Load<Event>("Events/Investments"));
        }

        if (FindObjectOfType<PlayerHealthBar>().health < 100 && _toldAboutHeal == false)
        {
            _toldAboutHeal = true;
            _nextEvents.Add(Resources.Load<Event>("Events/UrHurt"));
        }

        //sensei style related ifs
        if(wave==2)
        {
            //tell player about next 2 styles and enable the ui and the sensei panel itself
            _nextEvents.Add(Resources.Load<Event>("Events/SensieUnlock"));
            FindObjectOfType<SenseiPanel>().newStyles(2);
            FindObjectOfType<SenseiPanel>().EnableButton();
            //above line should enable serpent strike and cresent moon
        }

        if(wave==6)
        {
            _nextEvents.Add(Resources.Load<Event>("Events/SensieUnlock"));
            FindObjectOfType<SenseiPanel>().newStyles(4);
            //this will enable mountains and boar
        }

        if(wave==5)
        {
            _nextEvents.Add(Resources.Load<Event>("Events/DefenseUnlock"));
            FindObjectOfType<PlayerDefense>().EnableDefenseButton();
        }

        //I have changed the numbers becuase the expo build will be shorter teh 3 above and the blacksmith

        return _nextEvents.Count > 0;
    }

    public void CallDisplayEvents()
    {
        StartCoroutine(DisplayEventsRoutine());
    }

    private IEnumerator OneEvent()
    {
        EventPanel.SetActive(true);
        SetUpPanel();

        hasPicked = false;
        while(!hasPicked)
        {
            yield return null;
        }

        EventPanel.SetActive(false);
    }

    private IEnumerator DisplayEventsRoutine()
    {
        EventPanel.SetActive(true);


        foreach (Event eve in _nextEvents)
        {
            _currentEvent = eve;
            SetUpPanel();
            
            hasPicked = false;
            while (!hasPicked)
            {
                yield return null;
            }
        }
        EventPanel.SetActive(false);
    }

    private void SetUpPanel()
    {
        EventPopUpText.text = _currentEvent.textStatements[0];
        for (int lcv = 0; lcv < Buttons.Count; lcv++)
        {
            Buttons[lcv].SetActive(false);

        }
        for (int lcv = 0; lcv < _currentEvent.buttonOptions.Count; lcv++)
        {
            Buttons[lcv].SetActive(true);
            ButtonTexts[lcv].text = _currentEvent.buttonOptions[lcv];
        }
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

        if(_currentEvent.myeventEffect == EventEffect.joinBandits)
        {
            //it should fade to black could be done with a panel transition on  a timer
            //and ending combat
            //then it needs to have the player and thier stuff flipped which will actually be harder than I initilally thought with the strike system ui's current set up
            //while in between rounds it needs a different background so a new one of those
            //new bandit equivalents for the different shops with maybe a different spin or take
            _enemyManager.ClearCurrentWave();
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
        _enemyManager.enemyWaves.Insert(_enemyManager.WaveControlVariable, Resources.Load<EnmWave>("Waves/Wave3.5"));
        _villageDefense.TurnOnBlackSmith();
        //blacksmithBackground.SetActive(true);
        hasBlacksmith = true;
    }

    public void EnableInvesting()
    {
        blacksmithInvestButton.SetActive(true);
        farmInvestButton.SetActive(true);
    }

    //some kind of event pop up mid combat
    public void AddEvent(Event evn)
    {
        _nextEvents.Add(evn);
    }

    public void PopUpEvent(Event evn)
    {
        _currentEvent = evn;
        StartCoroutine(OneEvent());
    }

    #endregion
}
