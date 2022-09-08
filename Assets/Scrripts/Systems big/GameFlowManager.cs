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
    [SerializeField] bool skipEvent;

    [ContextMenu("initialize")]

    private void Start()
    {
        _lootManager = GetComponent<LootingManager>();
    }

    public void StartMenues()
    {
        StopAllCoroutines();
        StartCoroutine(FlowRoutine());
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
        _eventManager.CheckNextEvent();

        if (!skipEvent)
        {
            _eventManager.DisplayEvent();
            while (_eventPanel.activeInHierarchy)
            {
                yield return null;
                continue;
            }
        }

        _villagePanel.SetBool("Open", true);
        while (_villagePanel.GetBool("Open"))
        {
            yield return null;
            continue;
        }
        StrikeArea.SwitchPlayerOn(true);
    }
}
