using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrap : PlayerTrap
{
    [SerializeField] List<WeaponEffect> damageEffects;
    public override void DefendPlayer(float damage, enemyStats enemy)//called by playerDefense
    {
        enemy.damageEnemy(0, damageEffects);
        base.DefendPlayer(damage, enemy);
    }
}
