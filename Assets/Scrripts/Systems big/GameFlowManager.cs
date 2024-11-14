using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    private GameManager _gm;
    private PickPanManager _PickPanManager;
    private EventManager _eventManager;
    
    [SerializeField] PickPanManager _PickPanelManager;
    [SerializeField] GameObject _eventPanel;
    [SerializeField] Animator _villagePanel;
    private FarmShop _farm;
    private bool _isEvent;
    private PlayerHealthBar _php;

    [ContextMenu("initialize")]

    private void Start()
    {
        _gm = GetComponent<GameManager>();
        _PickPanManager = FindObjectOfType<PickPanManager>();
        _eventManager = GetComponent<EventManager>();
        _farm = GetComponent<FarmShop>();
        _php = GetComponent<PlayerHealthBar>();
    }

    public void StartMenues()
    {
        _php.HPIsInCombat(false);
        StopAllCoroutines();
        StartCoroutine(FlowRoutine());
        _farm.ResetHealPurchases();
    }

    IEnumerator FlowRoutine()
    {
        
        //looting stuff
        _PickPanelManager.OpenPickPan(0);
        StrikeArea.SwitchPlayerOn(false);
        _PickPanManager.RandomItemPull();

        while (_PickPanelManager.isPanelOpen())
        {
            yield return null;
            continue;
        }


        //event stuff
        _isEvent = _eventManager.CheckNextEvent();

        if (_isEvent)
        {
            _eventManager.CallDisplayEvents();
            while (_eventPanel.activeInHierarchy)
            {
                yield return null;
                continue;
                //where to put _isEvent =false?
            }
        }

        //after event could be unlock thing maybe

        //a call to if the player learns unlocks rn

        while(_PickPanelManager.isPanelOpen())
        {
            yield return null;
            continue;
        }


        _gm.InvestmentsPayOut();
        _eventManager.ClearEventList();

        //village stuff
        _villagePanel.SetBool("Open", true);
        while (_villagePanel.GetBool("Open"))
        {
            yield return null;
            continue;
        }
        StrikeArea.SwitchPlayerOn(true);
        _php.HPIsInCombat(true);
    }
}
