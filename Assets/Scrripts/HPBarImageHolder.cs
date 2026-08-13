using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarImageHolder : MonoBehaviour
{
    [SerializeField] Image hpBar;
    [SerializeField] TextMeshProUGUI poisonText;
    [SerializeField] Transform TransformToChange;
    [SerializeField] Image sprite;

    public Image getHPBar()
    {
        return hpBar;
    }
    public TextMeshProUGUI getPoisonTextField()
    {
        return poisonText;
    }
    public Transform getTransformToScale()
    {
        return TransformToChange;
    }
    public void setSprite(Sprite img)
    {
        sprite.sprite = img;
    }
}
