using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    private GameManager _gm;
    private LootingManager _lootManager;
    private EventManager _eventManager;
    
    [SerializeField] Animator _lootPanel;
    [SerializeField] GameObject _eventPanel;
    [SerializeField] Animator _villagePanel;
    private FarmShop _farm;
    private bool _isEvent;

    [ContextMenu("initialize")]

    private void Start()
    {
        _gm = GetComponent<GameManager>();
        _lootManager = GetComponent<LootingManager>();
        _eventManager = GetComponent<EventManager>();
        _farm = GetComponent<FarmShop>();
    }

    public void StartMenues()
    {
        StopAllCoroutines();
        StartCoroutine(FlowRoutine());
        _farm.ResetHealPurchases();
    }

    IEnumerator FlowRoutine()
    {
        _lootPanel.SetBool("Open", true);
        StrikeArea.SwitchPlayerOn(false);
        _lootManager.RandomItemPull();
        while (_lootPanel.GetBool("Open"))
        {
            yield return null;
            continue;
        }
        
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

        _gm.InvestmentsPayOut();

        _villagePanel.SetBool("Open", true);
        while (_villagePanel.GetBool("Open"))
        {
            yield return null;
            continue;
        }
        StrikeArea.SwitchPlayerOn(true);
    }
}
