using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Ninja
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
