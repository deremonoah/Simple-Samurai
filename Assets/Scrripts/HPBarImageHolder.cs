using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarImageHolder : MonoBehaviour
{
    [SerializeField] Image hpBar;
    [SerializeField] TextMeshProUGUI poisonText;

    public Image getHPBar()
    {
        return hpBar;
    }
    public TextMeshProUGUI getTextField()
    {
        return poisonText;
    }
}
