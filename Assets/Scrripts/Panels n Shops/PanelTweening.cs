using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTweening : MonoBehaviour
{
    [SerializeField] AnimationCurve animationThing;
    [SerializeField] float speed=1;


    [ContextMenu("ExecuteTween")]
    public void ExecuteTween()
    {
        Debug.Log("executeTween");
        StartCoroutine(TweenRoutine());
    }


    private IEnumerator TweenRoutine()
    {
        float timer = 0f;
        Vector3 InitialScale = transform.localScale;
        var Duration = animationThing.keys[^1].time;
        Debug.Log(Duration);

        while(timer<=1)
        {
            transform.localScale = InitialScale * animationThing.Evaluate(timer);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = InitialScale;
    }
}
