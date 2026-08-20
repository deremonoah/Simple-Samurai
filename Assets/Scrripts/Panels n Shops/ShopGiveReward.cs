using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGiveReward : MonoBehaviour
{
    [SerializeField] protected Transform heartOverHead;
    private float HeartMoveSpeed=1500;
    private GameObject heartPrefab;
    [SerializeField] protected Transform parentObj;

    void Awake()
    {
        heartPrefab =  Resources.Load<GameObject>("heart prefab");
    }

    //show appreciation, have a heart hover over the characters head

    //then have it fly to the button its effecting, later add a sound effect, like a discount sound or something?
    protected void ShowAppreciation(Transform start, Transform end)
    {
        if (end == null)
        {
            //just hover above the head
            StartCoroutine(ShowHeart(start));
        }
        else if(start==null)
        { Debug.LogError("start position is null for shop reward"); }
        else
        {
            StartCoroutine(ShowAppreciationRoutine(start, end));
        }
    }

    IEnumerator ShowHeart(Transform start)
    {
        Vector3 startPos = start.position;
        Transform moveObj = Instantiate(heartPrefab).GetComponent<Transform>();
        moveObj.SetParent(parentObj);
        moveObj.position = startPos;

        yield return new WaitForSeconds(3f);//TODO: add an animation for the heart assuming I keep this all in
        Destroy(moveObj.gameObject);
    }

    protected IEnumerator ShowAppreciationRoutine(Transform start, Transform end)
    {

        Vector3 startPos = start.position;
        Vector3 endPos = end.position;
        float timeElapsed = 0f;
        Transform moveObj = Instantiate(heartPrefab).GetComponent<Transform>();
        moveObj.SetParent(parentObj);
        moveObj.position = startPos;
        yield return new WaitForSeconds(2f);//so it hovers above head, prob need it to move up and down a bit

        float duration = Vector3.Distance(startPos,endPos)/ HeartMoveSpeed;

        while (moveObj.position!=endPos)
        {
            float t = timeElapsed / duration;
            moveObj.position = Vector3.Lerp(startPos, endPos, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(moveObj.gameObject);
    }
}
