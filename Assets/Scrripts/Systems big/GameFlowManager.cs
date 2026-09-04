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
    private EnemyHPBarPlacerManager _eHPm;

    [ContextMenu("initialize")]

    private void Start()
    {
        _gm = GetComponent<GameManager>();
        _PickPanManager = FindObjectOfType<PickPanManager>();
        _eventManager = GetComponent<EventManager>();
        _farm = GetComponent<FarmShop>();
        _php = GetComponent<PlayerHealthBar>();
        _pd = FindObjectOfType<PlayerDefense>();
        _eHPm = EnemyHPBarPlacerManager.instance;

        StartCoroutine(FlowRoutine());
    }

    public void StartMenues()
    {
        _pd.RearmTraps();
        _farm.ResetHealPurchases();
        //FindObjectOfType<MiniGameManager>().RollToSeeIfTheyNeedHelp();
    }

    IEnumerator FlowRoutine()
    {
        InCombat(true);

        yield return new WaitForSeconds(2f);//wait for them to spawn in?
        while (_eHPm.AnyAliveEnemies())
        {
            yield return null;

        }

        StartMenues();
        //looting stuff
        _PickPanelManager.OpenPickPanForLooting();
        InCombat(false);

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

        StartCoroutine(FlowRoutine());
    }

    private void InCombat(bool isInCombat)
    {
        StrikeArea.SwitchPlayerOn(isInCombat);
        _php.HPIsInCombat(isInCombat);//TODO: make these 1 simple inCombatCall for this class
        _pd.inCombatHudUpdate(isInCombat);
        WeaknessSpawnManager.instance.InCombat(isInCombat);
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
