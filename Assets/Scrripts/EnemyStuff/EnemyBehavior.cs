using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //should have enemy stats as seperate
    Coroutine myActionRoutine;
    [SerializeField] List<GameObject> actionPrefabs;
    [SerializeField] Transform enemyAttackPoint;
    enemy stats;
    public List<GameObject> currentAttacks = new List<GameObject>();
    [SerializeField] float MoveToShowSpeed;

    public void Start()
    {
        stats = GetComponent<enemy>();
        myActionRoutine = StartCoroutine(actionRoutine());
    }

    private void DecideNextAction()
    {
        myActionRoutine = null;
        myActionRoutine = StartCoroutine(actionRoutine());
    }

    IEnumerator actionRoutine()
    {
        //randomly generate wait time
        yield return new WaitForSeconds(stats.getRandomWaitTime());//or should it que up attacks, so we make sure that they are better timed

        yield return moveToShowAttack();//show attack also throws attack, this also starts the routine

        //DecideNextAction();
    }

    IEnumerator moveToShowAttack()
    {
        //move to Demo point
        Vector3 PosToReturnTo = transform.position;

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, EnemysManager.instance.getDemoAttackPoint().y,0);
        float timer = 0;
        float duration = Mathf.Abs(startPos.y - endPos.y) / MoveToShowSpeed;
        while (transform.position != endPos)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;
        }

        //move to the point the throw attack
        //calculate duration,so if we are already at the top its fine
        
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, EnemysManager.instance.getRandomAttackPoint().y,0);
        timer = 0;
        duration = Mathf.Abs(startPos.y - endPos.y) / MoveToShowSpeed;
        while (transform.position!=endPos)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("past moved to attack pos");
        //now throw attack
        //curState = attackState.ThrowingAttack; TO DO:I have to redo animation fuck
        yield return new WaitForSeconds(0.5f);
        BasicAction();//TO DO: delate action?
        Debug.Log("should have attacked");
        
        //return to original position
        //calculate duration,so if we are already at the top its fine
        startPos = transform.position;
        endPos = PosToReturnTo;
        timer = 0;
        duration = Mathf.Abs(startPos.y - endPos.y) / MoveToShowSpeed;
        while (transform.position != endPos)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            Debug.Log("in final move back loop");
            yield return null;
        }
        Debug.Log("should have moved back to starting pos");
    }

    //we do need to deal with getting parried
    public void Blocked()
    {
        if (myActionRoutine != null)
        {
            //currentRageCount++;
            StopCoroutine(myActionRoutine);
            myActionRoutine = StartCoroutine(actionRoutine());
            //DecideNStartAction();can I just have multiple scripts that are actions the enemy can preform? like attackUI, or heal, or other stuff?
        }
    }

    public void ClearAttacks()//called by enemyStats when enemy dies
    {
        foreach (var atk in currentAttacks)
            Destroy(atk);
    }

    public void BasicAction()//sends out on of its attacks or specials
    {
        Debug.Log("in basic action");
        int rand = Random.Range(0, actionPrefabs.Count);
        GameObject attack = Instantiate(actionPrefabs[rand], enemyAttackPoint.position, actionPrefabs[rand].transform.rotation);//not sure if the rotation is right, but why can't i get transform of prefab?
        currentAttacks.Add(attack);
        var atk = attack.GetComponent<attack>();
        if (atk != null)
        {
            atk.Setstuff(stats, stats.getRandomAttackDirection());//how to make it so certain attacks only move certain ways? just on attack?
        }
    }//I plan to add non basic actions as heal self like sumo or... I feel like there was another specific one in mind
    //ah right swapping spots, though I don't think anyone payed attention to that one
}
