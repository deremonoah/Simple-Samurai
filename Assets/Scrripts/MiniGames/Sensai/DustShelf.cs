using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DustShelf : MiniGame
{
    [SerializeField] List<Image> stuffToClean;
    [SerializeField] List<Transform> MoveToPoints;
    [SerializeField] Transform duster;
    [SerializeField] float dusterSpeed;
    private float DelayTimer=0;
    [SerializeField] private int ItemToDust;

    private void OnEnable()
    {
        StartCoroutine(DustRoutine());
        ItemToDust = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            dusterSpeed = 0;
            DelayTimer = 0.5f;//so spamming keeps the timer going
            if(ItemToDust>-1)
            {
                CleanThis(ItemToDust);
            }
        }

        if(DelayTimer<=0)
        {
            dusterSpeed = 3;
        }
        else
        {
            DelayTimer -= Time.deltaTime;
        }
    }

    public void CleanThis(int slot)//can be called by this or by
    {
        Debug.Log("clean this is called");
        float randClean = Random.Range(0.01f, 0.17f);
        stuffToClean[slot].fillAmount -= randClean;//do we need clamp?
    }
    //make a second ^ if we want ab testing for cleaning

    private IEnumerator DustRoutine()
    {
        int lastPoint = MoveToPoints.Count - 1;
        int nextPoint = 0;
        while (duster.position !=MoveToPoints[lastPoint].position)
        {
            //Vector3 startPos = duster.position;
            Vector3 endPos = MoveToPoints[nextPoint].position;
            //float timeElapsed = 0;
            Debug.Log(nextPoint);
            while (duster.position!=endPos)
            {
                //move towards I rememember doesn't always end with it getting there
                duster.position = Vector2.MoveTowards(duster.position,
                endPos,
                dusterSpeed * Time.deltaTime);

                yield return null;
            }
            nextPoint++;
            
        }
    }

    public void SetDustingItem(int slot)
    {
        ItemToDust = slot;//it is set to -1 when it goes off the item
        Debug.Log("called set dusting item");
    }

    //for calculating score its just if you got all of the fill images to 0, so maybe there is an acceptable amount left?
    public override float CalculateScore()
    {
        //add all the enabled ones up, then subtract from the count of total enabled ones
        float totalDust = 0;
        int totalEnabled = 0;
        foreach (Image im in stuffToClean)
        {
            if (im.gameObject.activeInHierarchy)
            {
                totalDust += im.fillAmount;
                totalEnabled++;
            }
        }
        float score = (totalEnabled - totalDust) / totalEnabled;

        return score;
    }
}
