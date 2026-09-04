using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingTrap : EnemyTrap
{

    [Header("Shrinking anim stats")]
    [SerializeField] float growDuration;
    [SerializeField] float shrinkDuration;
    [SerializeField] Vector2 MinMaxSize;

    protected virtual void ResolveTrapEffect()
    {
        //will be replaced by other scripts like heal or run away
    }

    protected override void EffectOnStart()//this is from enemy trap, called on enable or start
    {
        StartCoroutine(OpportunityRoutine());
    }

    protected IEnumerator OpportunityRoutine()
    {
        float randSize = Random.Range(MinMaxSize.x, MinMaxSize.y);
        Vector3 targetSize = new Vector3(randSize, randSize, 1);
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

        ResolveTrapEffect();

        Destroy(this.gameObject);
    }

    protected IEnumerator OpportunityRoutine(float value, float Max)
    {
        //make the size and transparency propotional to the value, oh but it doesn't have min max which i think it would need
        float randSize = Mathf.Clamp((value/Max)*MinMaxSize.y,MinMaxSize.x,MinMaxSize.y);
        Vector3 targetSize = new Vector3(randSize, randSize, 1);
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

        ResolveTrapEffect();

        Destroy(this.gameObject);
    }
}
