using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : enemy
{
    [SerializeField] Image bossHP;

    protected override void fillMyHP()
    {
        bossHP.fillAmount = HP / maxHP;
    }

    protected override void Start()
    {
        base.Start();
        bossHP = enmsSys.bossHPBar;

    }
}
