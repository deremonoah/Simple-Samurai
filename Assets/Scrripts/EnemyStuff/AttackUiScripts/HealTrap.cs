using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTrap : MonoBehaviour
{
    [SerializeField] Vector2 MinMaxHeal;
    [SerializeField] float growDuration;
    [SerializeField] float shrinkDuration;
    [Header("Positional info")]
    [SerializeField] Vector2 SpawnPosMinMax;
    [SerializeField] float SpawnPosOffset;

    private void OnEnable()//in case I change to object pooling, which probably should
    {
        //move to right pos
        StrikePoint point = FindObjectOfType<StrikePoint>();
        float randpos = SpawnPosOffset + Random.Range(SpawnPosMinMax.x, SpawnPosMinMax.y);//idk man, just based off what I have in buff areas so just some nummbers
        transform.position=point.currentPath.path.GetPointAtDistance(randpos);
        //start shrinking
        StartCoroutine(OpportunityRoutine());
    }

    private void HealAnEnemy()
    {
        float healAmount = Random.Range(MinMaxHeal.x, MinMaxHeal.y);
        FindObjectOfType<EnemyHPBarPlacerManager>().HealEnemy(healAmount);
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
