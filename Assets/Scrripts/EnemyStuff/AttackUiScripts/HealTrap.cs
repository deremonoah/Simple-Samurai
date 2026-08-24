using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTrap : EnemyTrap
{
    [SerializeField] Vector2 MinMaxHeal;
    [SerializeField] float growDuration;
    [SerializeField] float shrinkDuration;

    private void HealAnEnemy()
    {
        float healAmount = Random.Range(MinMaxHeal.x, MinMaxHeal.y);
        FindObjectOfType<EnemyHPBarPlacerManager>().HealEnemy(healAmount);
    }

    protected override void EffectOnStart()
    {
        StartCoroutine(OpportunityRoutine());
    }

    private IEnumerator OpportunityRoutine()
    {
        float rand = Random.Range(1.4f, 2.01f);
        Vector3 targetSize = new Vector3(rand, rand, 1);
        Vector3 startSize = new Vector3(.001f, .001f, 0);
        float timeElapsed = 0;
        while (transform.localScale != targetSize)
        {
            float t = timeElapsed / growDuration;

            transform.localScale = Vector3.Lerp(startSize, targetSize, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //shrink part
        targetSize = new Vector3(.001f, .001f, 0);
        startSize = transform.localScale;
        timeElapsed = 0;
        while (transform.localScale != targetSize)
        {
            float t = timeElapsed / shrinkDuration;

            transform.localScale = Vector3.Lerp(startSize, targetSize, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        HealAnEnemy();

        Destroy(this.gameObject);
    }

}
