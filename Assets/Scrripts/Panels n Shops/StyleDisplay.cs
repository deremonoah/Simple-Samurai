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
    [SerializeField] Image StylePatternImage;
    [SerializeField] List<GameObject> boxesForStyles;

    private void Start()
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
        Weapon we = getweapon();

        List<int> StylesToDisplay = SetDisplayStylesFromContext();
        


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
            boxesForStyles[(int)sty.styleToLearn].GetComponent<Toggle>().isOn = true;
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

    public void DisplayStyle(Sprite stylePic)//I use this on the check boxes, ideally default to the right one in future
    {
        StylePatternImage.sprite = stylePic;
        //equiping style happens in strike point
    }

    public Transform getPosFromStylesKnown(int num)//for SenseiPanel
    {
        return boxesForStyles[num-1].transform;//styles known starts at 1 so it is off by 1
    }
}