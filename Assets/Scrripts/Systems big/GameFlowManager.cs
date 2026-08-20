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
    private bool playerIsReadyToFight;
    private PlayerDefense _pd;

    [ContextMenu("initialize")]

    private void Start()
    {
        _gm = GetComponent<GameManager>();
        _PickPanManager = FindObjectOfType<PickPanManager>();
        _eventManager = GetComponent<EventManager>();
        _farm = GetComponent<FarmShop>();
        _php = GetComponent<PlayerHealthBar>();
        _pd = FindObjectOfType<PlayerDefense>();
    }

    public void StartMenues()
    {
        _php.HPIsInCombat(false);
        _pd.inCombatHudUpdate(false);
        _pd.RearmTraps();
        StopAllCoroutines();
        StartCoroutine(FlowRoutine());
        _farm.ResetHealPurchases();
    }

    IEnumerator FlowRoutine()
    {
        FindObjectOfType<MiniGameManager>().RollToSeeIfTheyNeedHelp();
        //looting stuff
        _PickPanelManager.OpenPickPanForLooting();
        StrikeArea.SwitchPlayerOn(false);
        WeaknessSpawnManager.instance.InCombat(false);

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
        _php.ResetArmorAfterCombat();

        //village stuff
        _villagePanel.SetBool("Open", true);
        playerIsReadyToFight = false;
        while (!playerIsReadyToFight)
        {
            yield return null;
            continue;
        }
        StrikeArea.SwitchPlayerOn(true);
        _php.HPIsInCombat(true);//TODO: make these 1 simple inCombatCall for this class
        _pd.inCombatHudUpdate(true);
        WeaknessSpawnManager.instance.InCombat(true);
    }

    public void villageStillOpen()
    {
        _villagePanel.SetBool("Open", true);
    }

    public void PlayerisReadyToFight()
    {
        playerIsReadyToFight = true;
    }
}
