using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : PlayerTrap
{
    [SerializeField] List<WeaponEffect> damageEffects;

    public override void DefendPlayer(float damage, enemyStats enemy)//called by playerDefense
    {
        //damage enemy, right now it will do 25% of the enemies hp, maybe change later to 1/5
        float dmg = enemy.maxHP / 4;
        Debug.Log(dmg);
        FindObjectOfType<EnemyHPBarPlacerManager>().DamageEnemy(dmg, enemy.posInList, damageEffects);
        base.DefendPlayer(damage, enemy);
    }
}
