using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] Animator _lootPanel;
    [SerializeField] GameObject _eventPanel;
    [SerializeField] Animator _villagePanel;
    [SerializeField] bool skipEvent;

    [ContextMenu("initialize")]
    public void Initialized()
    {
        StopAllCoroutines();
        StartCoroutine(FlowRoutine());
    }

    IEnumerator FlowRoutine()
    {
         
        _lootPanel.SetBool("Open", true);
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

    }
}
