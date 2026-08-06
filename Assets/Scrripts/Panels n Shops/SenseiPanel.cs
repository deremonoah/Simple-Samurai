using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SenseiPanel : MonoBehaviour
{
    [SerializeField] List<GameObject> styleUIs;
    [SerializeField] GameObject panelButton;
    int stylesKnown=1;
    //A list of the styles excluding simple so it isn't disable on start

    void Start()
    {
        panelButton.SetActive(false);
        foreach (GameObject style in styleUIs)
        {
            style.SetActive(false);
        }
    }

    public void newStyles(int num)
    {
        stylesKnown = num;
        
        FindObjectOfType<SoundManager>().PlaySound("sensei");
    }
    public void EnableButton()
    {
        panelButton.SetActive(true);
    }
    //so at certain points there should be new styles made available at current set up these being revealed in pairs or groups after like 3-5 waves
    //I likley want events to tell the player to visit the sensie panel which I should also disable the button while that isn't an option
    
    public int getNumberOfKnownStyles()
    {
        return stylesKnown;
    }
}
