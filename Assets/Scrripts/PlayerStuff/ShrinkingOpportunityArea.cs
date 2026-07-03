using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingOpportunityArea : MonoBehaviour
{
    [SerializeField] float shrinkDuration;
    [SerializeField] float growDuration;

    private void OnEnable()//in case I change to object pooling, which probably should
    {
        StartCoroutine(OpportunityRoutine());
    }
    
    private IEnumerator OpportunityRoutine()
    {
        float rand = Random.Range(1.4f, 2.01f);
        Vector3 targetSize = new Vector3(rand,rand,1);
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
        targetSize=new Vector3(.001f,.001f,0);
        startSize = transform.localScale;
        timeElapsed=0;
        while(transform.localScale!=targetSize)
        {
            float t = timeElapsed / shrinkDuration;

            transform.localScale = Vector3.Lerp(startSize, targetSize, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        Destroy(this.gameObject);
    }
}
