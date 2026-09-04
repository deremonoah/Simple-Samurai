using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayTrap : ShrinkingTrap
{
    protected override void ResolveTrapEffect()
    {
        myEnemy.gameObject.GetComponent<EnemyBehavior>().RunAwayAnimStart();
    }
}
