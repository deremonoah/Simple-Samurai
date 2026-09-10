using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SenseiPanel : ShopGiveReward
{
    [Header("most recent score")]

    [SerializeField] GameObject panelButton;
    [SerializeField] List<StyleID> stylesKnown;//starts with simple
    [SerializeField] bool twoStyled;
    private StyleDisplay stylesOnPanel;
    private int previousPressed = 0;
    private int secondPreviousPressed = 0;
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
        secondPreviousPressed = previousPressed;
        previousPressed = (int)newStyle;
        
    //FindObjectOfType<SoundManager>().PlaySound("sensei");
    EnableButton();//for when you get a style from loot this will open,not from events
    }
    private void EnableButton()
    {
        panelButton.SetActive(true);
    }
    //so at certain points there should be new styles made available at current set up these being revealed in pairs or groups after like 3-5 waves
    //I likley want events to tell the player to visit the sensie panel which I should also disable the button while that isn't an option
    
    public List<StyleID> getListOfKnownStyles()
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
    public bool KnowsTwoStyleFighting()
    {
        return twoStyled;
    }

    public void UnlockTwoStyleSkill()//called in senseiRewards
    {
        twoStyled = true;
        //maybe add a ui to enable or disable it?
    }

    public int getLastPressed()
    {
        return previousPressed;
    }

    public int getSecondToLastPressed()
    {
        return secondPreviousPressed;
    }

    public void SetLastPressed(int last,int secLast)//called by style display
    {
        previousPressed = last;
        secondPreviousPressed = secLast;
    }
}
public enum StyleID { simple,Serpent,Creset,Mountain,Boar}