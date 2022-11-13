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

    private void Update()
    {
        if (HP < maxHP / 2)
        {
            //SwapSpots();
        }
    }

    protected override void Start()
    {
        base.Start();
        bossHP = enmsSys.bossHPBar;
    }

    private void SwapSpots()
    {
        if (posInList < enmsSys.aliveEnemys.Count)
        {
            var targetToSwap = enmsSys.aliveEnemys[posInList + 1].gameObject;
            var targetPos = targetToSwap.transform.position;
            var myOldPos = this.gameObject.transform.position;
            

            this.transform.position = targetPos;
            targetToSwap.transform.position = myOldPos;

            enmsSys.aliveEnemys[posInList + 1] = this;
            enmsSys.aliveEnemys[posInList] = targetToSwap.GetComponent<enemy>();

            enmsSys.UpdateEnmsPos();
            //this is pretty jank but i think it will work
        }
    }
}
