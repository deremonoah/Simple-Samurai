using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTweening : MonoBehaviour
{
    [SerializeField] AnimationCurve animationThing;

    [Min(0.01f)]
    [SerializeField] float speed = 1;


    [ContextMenu("ExecuteTween")]
    public void ExecuteTween()
    {
        StartCoroutine(TweenRoutine());
    }


    private IEnumerator TweenRoutine()
    {

        float timer = 0f;
        Vector3 InitialScale = transform.localScale;
        var Duration = animationThing.keys[animationThing.keys.Length - 1].time;


        while (timer <= Duration)
        {
            transform.localScale = InitialScale * animationThing.Evaluate(timer);
            timer += Time.deltaTime * speed;
            yield return null;
        }
        transform.localScale = InitialScale;

    }

}
