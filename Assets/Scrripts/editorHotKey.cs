#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class editorHotKey : MonoBehaviour
{
    [SerializeField] KeyCode _hotKey;
    [SerializeField] UnityEvent _specialEvent;
    
    void Update()
    {
        if (Input.GetKeyDown(_hotKey))
        {
            _specialEvent.Invoke();
        }
    }
}
#endif