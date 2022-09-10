using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    private GameManager _gm;
    public Text EventPopUpText;
    public GameObject EventPanel;
    private Event _nextEvent;
    public List<GameObject> Buttons;
    public List<Text> ButtonTexts;
    private EnemysManager _enemyManager;
    private BlackSmithShop _blacksmith;
    private VillageDefense _villageDefense;

    [SerializeField] GameObject blacksmithInvestButton;
    [SerializeField] GameObject farmInvestButton;
    private bool _investingEnabled = false;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _enemyManager = GetComponent<EnemysManager>();
        _blacksmith = GetComponent<BlackSmithShop>();
        _villageDefense = GetComponent<VillageDefense>();
        blacksmithInvestButton.SetActive(false);
        farmInvestButton.SetActive(false);

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
            DisplayEvent();
        }
    }

    public bool CheckNextEvent()
    {
        //is it more efficent to send the data with the call or have a refrence to gm public variable like below?
        //or should i make it a static call so the gm doesn't need a private refrence to this?
        bool isEvent = false;
        if (_gm.playerCoins >= 40 && !_investingEnabled)
        {
            _investingEnabled = true;
            _nextEvent = Resources.Load<Event>("Events/Investments");
            isEvent = true;
        }
        if (_enemyManager.WaveControlVariable == 2)
        {
            _nextEvent = Resources.Load<Event>("Events/BlackSmith");
            isEvent = true;
        }
        if (_villageDefense.DamageTaken >= 10)
        {
            _nextEvent = Resources.Load<Event>("Events/DamagedCity");
            isEvent = true;
        }

        return isEvent;
    }

    public void DisplayEvent()
    {
        if (_nextEvent != null)
        {
            EventPanel.SetActive(true);
            EventPopUpText.text = _nextEvent.textStatements[0];
            for (int lcv = 0; lcv < Buttons.Count; lcv++)
            {
                Buttons[lcv].SetActive(false);

            }
            for (int lcv = 0; lcv < _nextEvent.buttonOptions.Count; lcv++)
            {
                Buttons[lcv].SetActive(true);
                ButtonTexts[lcv].text = _nextEvent.buttonOptions[lcv];
            }
            
        }

    }

    public void CloseEventPanel()
    {
        EventPanel.SetActive(false);
        _nextEvent = null;
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
        CloseEventPanel();
    }

    public void firstResault()
    {
        if (_nextEvent.myeventEffect == EventEffect.blackSmith)
        {
            BlackSmithArived();
        }
        if (_nextEvent.myeventEffect == EventEffect.damagedCity && _gm.playerCoins>10)
        {
            _villageDefense.RepairVillage();
            //also if the player can't afford or doesn't pay it needs to enable a button that can repair on the village panel
        }
    }

    public void SecondResault()
    {
        if (_nextEvent.myeventEffect == EventEffect.damagedCity)
        {
            _villageDefense.DamagedVillage();
        }
    }


    public void ResolvePassiveEffect()
    {
        //now player can invest
        if (_nextEvent.myeventEffect == EventEffect.invest)
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
    }

    public void EnableInvesting()
    {
        blacksmithInvestButton.SetActive(true);
        farmInvestButton.SetActive(true);
    }

    #endregion
}
