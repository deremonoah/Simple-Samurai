using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    private GameManager _gm;
    
    
    [SerializeField] Animator _lootPanel;
    [SerializeField] GameObject _eventPanel;
    [SerializeField] Animator _villagePanel;
    [SerializeField] bool skipEvent;

    [ContextMenu("initialize")]

    private void Start()
    {
        _gm = GetComponent<GameManager>();
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
        _gm.RandomItemPull();
        while (_lootPanel.GetBool("Open"))
        {
            yield return null;
            continue;
        }

        if (!skipEvent)
        {
            _eventPanel.SetActive(true);
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
