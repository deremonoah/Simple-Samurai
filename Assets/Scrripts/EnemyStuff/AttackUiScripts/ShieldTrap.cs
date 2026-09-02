using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTrap : EnemyTrap
{
    [SerializeField] Vector2 TrapLifeTimeMinMax;
    //maybe in future a conditional as to if enemy commits an action to it?

    void Start()
    {
        float randLife = Random.Range(TrapLifeTimeMinMax.x, TrapLifeTimeMinMax.y);
        StartCoroutine(LifeTImeRoutine(randLife));
    }

    IEnumerator LifeTImeRoutine(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }
}
