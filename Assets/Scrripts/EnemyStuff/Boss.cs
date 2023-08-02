using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Ninja
{
    [SerializeField] Image bossHP;
    private bool hasswap = false;

    protected override void fillMyHP()
    {
        bossHP.fillAmount = HP / maxHP;
    }

    protected override void Update()
    {
        base.Update();
        if ((base.myAbilities[1] == Ability.ninja) && HP < maxHP / 2 && !hasswap)
        {
            SwapSpots();
            hasswap = true;
        }
    }

    protected override void Start()
    {
        base.Start();
        bossHP = enmsSys.bossHPBar;
    }

    protected override void StartMyRoutine()
    {
        base.StartMyRoutine();
    }

    private void SwapSpots()
    {
        if (posInList +1 < enmsSys.aliveEnemys.Count)
        {
            var targetToSwap = enmsSys.aliveEnemys[posInList + 1].gameObject;
            var targetPos = targetToSwap.transform.position;
            var myOldPos = this.gameObject.transform.position;
            

            this.transform.position = targetPos;
            targetToSwap.transform.position = myOldPos;

            enmsSys.aliveEnemys[posInList + 1] = this;
            enmsSys.aliveEnemys[posInList] = targetToSwap.GetComponent<enemy>();
            Debug.Log(targetToSwap.GetComponent<enemy>().myAbilities[0]);

            enmsSys.UpdateEnmsPosRefrence();
            //this is pretty jank but i think it will work, it did repeat this but thats cause update obviously
            //there is an error if you kill the other guy first he will stop attacking all together which is bad obviously
        }
    }
}
