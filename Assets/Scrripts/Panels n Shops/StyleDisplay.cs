using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StyleDisplay : MonoBehaviour
{
    private SenseiPanel sp;
    private PlayerEquipedItemsManager pe;
    private ItemDisplayPanel ip;
    private int EquipedstyleRefrence;

    [Header("where are we")]
    [SerializeField] whereStyle where;
    private enum whereStyle { itemDisplayPanel, sensaiPanel }

    [Header("Item Display objects")]
    [SerializeField] Image strikeAreaImage;
    [SerializeField] Image StylePatternImagePrimary;
    [SerializeField] Image StylePatternImageSecondary;
    [Header("CheckBox info")]
    [SerializeField] List<GameObject> boxesForStyles;
    [SerializeField] List<Sprite> StyleImages;
    [SerializeField] List<Image> checkMarks;
    private int previousPressed=0;
    private int secondPreviousPressed=0;

    [Header("colors")]
    [SerializeField] Color PrimaryStyleColor;
    [SerializeField] Color SecondaryStyleColor;

    private void Awake()
    {
        sp = FindObjectOfType<SenseiPanel>();
        pe = FindObjectOfType<PlayerEquipedItemsManager>();
        ip = FindObjectOfType<ItemDisplayPanel>();
    }

    private void OnEnable()
    {
        
        StartCoroutine(displayStyles());
    }

    public void DisplayStyles()
    {
        StartCoroutine(displayStyles());
    }

    IEnumerator displayStyles()
    {
        yield return new WaitForSeconds(0.001f);
        RefreshStyles();
        Weapon we = getweapon();

        List<int> StylesToDisplay = SetDisplayStylesFromContext();
        ReloadAllChecksInvisible();


        for (int lcv = 0; lcv < boxesForStyles.Count; lcv++)
        {
            boxesForStyles[lcv].SetActive(false);
        }


        for (int lcv = 0; lcv < StylesToDisplay.Count; lcv++)
        {
            boxesForStyles[StylesToDisplay[lcv]].SetActive(true);
        }

        strikeAreaImage.sprite = we.DisplayStrikeAreaIcon;

    }

    private List<int> SetDisplayStylesFromContext()
    {
        List<int> listToReturn = new();
        List<StyleID> enumList = FindObjectOfType<SenseiPanel>().getListOfKnownStyles();
        List<int> intList = enumList.Select(e => (int)e).ToList();
        listToReturn.AddRange(intList);

        //figure out equiped from somewhere

        if (ip.getRewardInspecting()is StyleReward)
        {
            StyleReward sty = (StyleReward)ip.getRewardInspecting();
            listToReturn.Add((int)sty.styleToLearn);//get the style id which is the int refrence for position of gameobject style in list
        }
        
        //set equiped style

        return listToReturn;
    }

    private Weapon getweapon()
    {
        if(where==whereStyle.itemDisplayPanel &&ip.getRewardInspecting()is Weapon)//if we are looking at a weapon only time to care
        {
            //get it from Item display Panel
            return (Weapon)ip.getRewardInspecting();//null exception here might mean you left display weapon enabled on the item display panel
            //as it gets enabled then disabled, there is a frame or less its enabled and will try to get a refrence that isn't there yet
        }
        else
        {
            //get it from primary equip
            return pe.getPrimaryWeapon();
        }

    }

    public void DisplayStyle(int sty)//I use this on the check boxes, ideally default to the right one in future
    {
        List<StyleID> enumList = sp.getListOfKnownStyles();
        bool isTwoStyled = sp.KnowsTwoStyleFighting();

        StylePatternImagePrimary.sprite = StyleImages[sty];
        checkMarks[sty].color = PrimaryStyleColor;
        Debug.Log("known styles count of " + enumList.Count);
        Debug.Log((StyleID)sty);
        if (enumList.Contains((StyleID)sty))
        {
            FindObjectOfType<StrikePoint>().EquipPrimaryStyle(sty);
        }

        //add if after we make sure it works
        if(isTwoStyled)
        {
            StylePatternImageSecondary.sprite = StyleImages[previousPressed];
            checkMarks[previousPressed].color = SecondaryStyleColor;
            if (enumList.Contains((StyleID)previousPressed) && enumList.Contains((StyleID)sty))
            {
                //equipSecondaryStyle only if first picked was also a real style otherwise we are just looking at things
                FindObjectOfType<StrikePoint>().EquipSecondaryStyle(previousPressed);
            }
        }

        secondPreviousPressed = previousPressed;
        previousPressed = sty;
        sp.SetLastPressed(previousPressed, secondPreviousPressed);
        ReloadAllChecksInvisible();
    }

    private void ReloadAllChecksInvisible()
    {
        foreach(Image im in checkMarks)
        {
            im.color = new Color(0, 0, 0, 0);
        }
        //add if we have 2 styles
        SenseiPanel sensei = FindObjectOfType<SenseiPanel>();
        bool isTwoStyled = sensei.KnowsTwoStyleFighting();
        if (isTwoStyled)
        { checkMarks[secondPreviousPressed].color = SecondaryStyleColor; }
        
        checkMarks[previousPressed].color= PrimaryStyleColor;
    }

    private void RefreshStyles()
    {
        previousPressed = sp.getLastPressed();//as it can be updated by unlocking new styles
        secondPreviousPressed = sp.getSecondToLastPressed();

        StylePatternImagePrimary.sprite = StyleImages[previousPressed];
        StylePatternImageSecondary.sprite = StyleImages[secondPreviousPressed];
    }

    public Transform getPosFromStylesKnown(int num)//for SenseiPanel
    {
        return boxesForStyles[num-1].transform;//styles known starts at 1 so it is off by 1
    }
}