using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Palisade : PlayerTrap
{
    [SerializeField] float _palisadeHPMax = 40, _palisadeHP = 40;
    [SerializeField] Image fillSection;
    //has max hp & current hp
    public override void DefendPlayer(float damage, enemyStats enemy)//called by playerDefense
    {
        //take damage away from this things current hp
        _palisadeHP -= damage;//maybe in future it cares about a particular kind of ability like an ax weilder dealing double damage to it
        fillSection.fillAmount = _palisadeHP / _palisadeHPMax;
        if (_palisadeHP<=0)
        {
            fillSection.fillAmount = 0 / _palisadeHPMax;
            armed = false;
            //does disarming remove the ui?
        }
    }

    public override void ReArmTrap()
    {
        _palisadeHP = _palisadeHPMax;
        fillSection.fillAmount = _palisadeHP / _palisadeHPMax;
        base.ReArmTrap();
    }
}
