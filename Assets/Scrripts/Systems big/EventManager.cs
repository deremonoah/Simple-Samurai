using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    private GameManager _gm;
    public Text EventPopUpText;
    public GameObject EventPanel;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        
    }

    public void CheckNextEvent()
    {
        //is it more efficent to send the data with the call or have a refrence to gm public variable like below?
        //or should i make it a static call so the gm doesn't need a private refrence to this?
        if (_gm.playerCoins >= 30)
        {
            //when the fight is over and looting done investing event
        }
    }
}
