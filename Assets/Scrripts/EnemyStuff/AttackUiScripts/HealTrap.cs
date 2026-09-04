using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTrap : ShrinkingTrap
{
    [SerializeField] Vector2 MinMaxHeal;
    private float amountToHeal;

    protected override void ResolveTrapEffect()
    {
        FindObjectOfType<EnemyHPBarPlacerManager>().HealEnemy(amountToHeal);
    }

    protected override void EffectOnStart()
    {
        float amountToHeal = Random.Range(MinMaxHeal.x, MinMaxHeal.y);
        StartCoroutine(OpportunityRoutine(amountToHeal,MinMaxHeal.y));
    }

}
