using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplayPanel : MonoBehaviour
{
    [SerializeField] Image strikeAreaImage;
    [SerializeField] Image formPatternImage;
    [SerializeField] List<GameObject> boxesForStyles;
    [SerializeField] List<GameObject> anvilLevelIcons;
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] TextMeshProUGUI ItemDescription;
    [SerializeField] TextMeshProUGUI ItemAmorValue;

    public void OpenItemDescriptionPanel(Item item)
    {
        int stylesToShow=FindObjectOfType<SenseiPanel>().getNumberOfKnownStyles();
        //just keep the styles in the same order
        //I also thought getting styles from beating certain enemies or by making certain decisions is way cooler than just getitng them randomly
        //problem is you can't exactly explore in the same way, or even choose which enemies to fight
        for(int lcv=0;lcv<stylesToShow && lcv<boxesForStyles.Count;lcv++)
        {
            boxesForStyles[lcv].SetActive(true);
        }
    }

    public void SetForWeapon()
    {

    }

    public void SetForArmor()
    {

    }

    public void SetForCurio()
    {

    }
    public void DisplayStyle(Sprite stylePic)
    {
        formPatternImage.sprite = stylePic;
    }
}
