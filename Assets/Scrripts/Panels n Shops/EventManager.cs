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

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _enemyManager = GetComponent<EnemysManager>();
        _blacksmith = GetComponent<BlackSmithShop>();
        _villageDefense = GetComponent<VillageDefense>();
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
        if (_gm.playerCoins >= 40)
        {
            //this event should only happen once
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
            _nextEvent = null;
        }

    }

    public void BlackSmithArived()
    {
        //take in temp variable then call shop.TurnOnButton() so the temp = whatever the shop is
        _enemyManager.enemyWaves.Insert(2,Resources.Load<EnmWave>("Waves/Wave3.5"));
        _enemyManager.enemyWaves.RemoveAt(3);
        _blacksmith.TurnOnButton();
    }

    public void RepairVillage()
    {
        //pay gold to repair the village also the progress on improvement or shop needs to be shut down
    }

    public void DamagedVillage()
    {
        //has a chance to disable a shop or 2 if damage was bad enough
    }

    public void CloseEventPanel()
    {
        EventPanel.SetActive(false);
    }

    public void EventButton(int num)
    {
        if (num == 0)
        {
            //yes stuff
        }

        if (num == 1)
        {
            //no response strat
        }
        CloseEventPanel();
    }
}
