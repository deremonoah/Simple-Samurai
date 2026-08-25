using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGiveReward : MonoBehaviour
{
    private PickPanManager _picPanMan;

    [SerializeField] protected Transform heartOverHead;
    private float HeartMoveSpeed=1500;
    private GameObject heartPrefab;
    [SerializeField] protected Transform parentObj;

    [Header("most recent score")]
    [SerializeField] protected float recentScore;

    [Header("Helping rewards")]
    [SerializeField] protected int aproval;//for how much they like you
    [SerializeField] protected Transform rewardFromHere;
    [SerializeField] protected int ScoreAboveForThreeRewards;
    [SerializeField] protected int aprovalAfterHelp;

    [Header("common rewards")]
    [SerializeField] protected List<Reward> RewardsCommon;

    [Header("Rare rewards")]
    [SerializeField] protected List<Reward> RewardsRare;
    [SerializeField] protected float rareScoreAboveToGet;

    [Header("Cost reductions")]
    [SerializeField] protected int perminentCostReduction;

    void Awake()
    {
        heartPrefab =  Resources.Load<GameObject>("heart prefab");
        _picPanMan = FindObjectOfType<PickPanManager>();
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

    public void GenerateRewardsToSend(float score)
    {
        recentScore = score;
        aproval += aprovalAfterHelp;//might change amount to be vairable based ons score
        int rand = Random.Range(0, (int)score) + aproval;
        ShowAppreciation(heartOverHead, null);

        List<Reward> rewardsToSend = new();
        int itemLength = 0;
        Debug.Log("Rand for loot length " + rand);
        if (rand >= ScoreAboveForThreeRewards)
        {
            itemLength = 3;
        }
        else { itemLength = 2; }

        List<Reward> commonsToUse = new();
        List<Reward> raresToUse = new();
        commonsToUse.AddRange(RewardsCommon);
        raresToUse.AddRange(RewardsRare);
        bool canKeepGoing = true;

        while (rewardsToSend.Count < itemLength && canKeepGoing)
        {//while we haven't filled list, stop if you run out of rewards, or stop if you can't get a rare reward
            rand = Random.Range(0, (int)score) + aproval;
            if (rand > rareScoreAboveToGet &&raresToUse.Count>0)
            {
                int randRare = Random.Range(0, raresToUse.Count);

                rewardsToSend.Add(raresToUse[randRare]);
                raresToUse.RemoveAt(randRare);
            }
            else
            {
                int randCom = Random.Range(0, commonsToUse.Count);
                rewardsToSend.Add(commonsToUse[randCom]);
                commonsToUse.RemoveAt(randCom);
            }
            //can keep going if you have any commons left & if you have rares left while also being able to get a rare still
            canKeepGoing = commonsToUse.Count > 0||(raresToUse.Count >0 &&score+aproval>=rareScoreAboveToGet);
        }

        _picPanMan.OpenPickPanForRewarding(rewardsToSend);
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
