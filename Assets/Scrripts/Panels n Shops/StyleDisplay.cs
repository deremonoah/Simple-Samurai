using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleDisplay : MonoBehaviour
{
    private SenseiPanel sp;
    private PlayerEquipedItemsManager pe;
    private ItemDisplayPanel ip;

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

        List<StyleID> knownStyles = sp.getNumberOfKnownStyles();

        for (int lcv = 0; lcv < boxesForStyles.Count; lcv++)
        {
            boxesForStyles[lcv].SetActive(false);
        }


        for (int lcv = 0; lcv < knownStyles.Count; lcv++)
        {
            boxesForStyles[(int)knownStyles[lcv]].SetActive(true);
        }

        strikeAreaImage.sprite = we.DisplayStrikeAreaIcon;

    }

    private Weapon getweapon()
    {
        if(where==whereStyle.itemDisplayPanel)
        {
            //get it from Item display Panel
            return ip.getWeapon();//null exception here might mean you left display weapon enabled on the item display panel
            //as it gets enabled then disabled, there is a frame or less its enabled and will try to get a refrence that isn't there yet
        }
        else if(where==whereStyle.sensaiPanel)
        {
            //get it from primary equip
            return pe.getPrimaryWeapon();
        }

        Debug.LogError("somehow its neither enum in styleDisplay");
        return null;
    }

    public void DisplayStyle(Sprite stylePic)//I use this on the check boxes, ideally default to the right one in future
    {
        StylePatternImage.sprite = stylePic;
        //equiping style happens 
    }

    public Transform getPosFromStylesKnown(int num)//for SenseiPanel
    {
        return boxesForStyles[num-1].transform;//styles known starts at 1 so it is off by 1
    }
}