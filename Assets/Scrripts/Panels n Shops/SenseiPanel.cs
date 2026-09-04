using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SenseiPanel : ShopGiveReward
{
    [Header("most recent score")]

    [SerializeField] GameObject panelButton;
    [SerializeField] List<StyleID> stylesKnown;//starts with simple
    private StyleDisplay stylesOnPanel;
    //A list of the styles excluding simple so it isn't disable on start

    //rewards for something else?

    private PlayerEquipedItemsManager pEquip;
    void Start()
    {
        panelButton.SetActive(false);
        pEquip = FindObjectOfType<PlayerEquipedItemsManager>();//for giving player items
        stylesOnPanel = parentObj.GetComponentInChildren<StyleDisplay>();
    }

    public void newStyles(StyleID newStyle)//should this not add them?
    {
        stylesKnown.Add(newStyle);
        
        //FindObjectOfType<SoundManager>().PlaySound("sensei");
        EnableButton();
    }
    private void EnableButton()
    {
        panelButton.SetActive(true);
    }
    //so at certain points there should be new styles made available at current set up these being revealed in pairs or groups after like 3-5 waves
    //I likley want events to tell the player to visit the sensie panel which I should also disable the button while that isn't an option
    
    public List<StyleID> getNumberOfKnownStyles()
    {
        return stylesKnown;
    }

    //rewards are calculated from shopGiveReward, using the lists of scriptable objects

    /*public void LearnedNewStyle()
    {
        //stylesKnown += 1;
        stylesOnPanel.DisplayStyles();
        ShowAppreciation(heartOverHead, heartOverHead);
    }*/
}
public enum StyleID { simple,Serpent,Creset,Mountain,Boar}