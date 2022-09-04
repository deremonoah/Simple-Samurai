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

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _enemyManager = GetComponent<EnemysManager>();
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

    public void CheckNextEvent()
    {
        //is it more efficent to send the data with the call or have a refrence to gm public variable like below?
        //or should i make it a static call so the gm doesn't need a private refrence to this?
        if (_gm.playerCoins >= 40)
        {
            //when the fight is over and looting done investing event
            _nextEvent = Resources.Load<Event>("Events/Investments");
        }
        if (_enemyManager.WaveControlVariable == 2)
        {
            _nextEvent = Resources.Load<Event>("Events/BlackSmith");
        }

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

    public void BlackSmithArived()
    {
        _enemyManager.enemyWaves.Insert(2,Resources.Load<EnmWave>("Waves/Wave3.5"));
        _enemyManager.enemyWaves.RemoveAt(3);
    }

    public void CloseEventPanel()
    {
        EventPanel.SetActive(false);
    }
}
