using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPickPanel : MonoBehaviour
{

    public GameObject itempickpanel;

    [SerializeField] bool panUp;
    
    void Start()
    {
        itempickpanel.SetActive(false);
    }

    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
                if (panUp)
                {
                    dropPop();
                }else
                {
                    popUp();
                }
        }
        
    }

    public void popUp()
    {
        itempickpanel.SetActive(true);
        Time.timeScale = 0f;
        panUp = true;
    }

    public void dropPop()
    {
        itempickpanel.SetActive(false);
        Time.timeScale = 1f;
        panUp = false;
    }
}
