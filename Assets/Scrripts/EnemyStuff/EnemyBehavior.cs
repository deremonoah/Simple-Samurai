
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //should have enemy stats as seperate
    Coroutine myActionRoutine;
    IEnumerator DelegateAction;
    enemyStats stats;

    [Header("attack info")]
    [SerializeField] List<GameObject> attackPrefabs;
    [SerializeField] Transform enemyAttackPoint;
    private List<GameObject> currentAttacks = new List<GameObject>();
    [SerializeField] float MoveToShowSpeed;
    [Header("attack anim durations")]
    [SerializeField] float DrawBackDuration;
    [SerializeField] float ThrustForwardDuration;

    [Header("trap prefabs")]
    [SerializeField] List<GameObject> trapPrefabs;
    private List<GameObject> currentTraps = new List<GameObject>();
    [SerializeField] int maxTraps;
    [Range(1,100)]
    [SerializeField] int TrapPercentage;

    [Header("Rage (Inclusive,Exclusive)")]
    [SerializeField] Vector2 minMaxRageAttacks;
    [SerializeField] float TimeBetweenRageAttacks;
    [SerializeField] int RageThreashold;
    private int currentRageCount;
    private float YAttackOffset;
    private Vector3 posToReturnTo;

    public void Start()
    {
        stats = GetComponent<enemyStats>();
        posToReturnTo = EnemysManager.instance.getPosToReturnTo(stats.posInList);
        DecideNextAction();
    }

    private void DecideNextAction()
    {
        myActionRoutine = null;
        DelegateAction = null;

        //rage highest priority
        if(DelegateAction==null &&currentRageCount>=RageThreashold)
        {
            int rand = Random.Range(0, 2);
            if(rand>0)
            {
                DelegateAction = RageRoutine();
            }
        }
        if(DelegateAction==null && trapPrefabs.Count>0)
        {
            int rand = Random.Range(1, 101);
            if(rand<=TrapPercentage)
            {
                DelegateAction = TrapUIRoutine();
            }    
        }
        if(DelegateAction==null)
        {
            DelegateAction = AttackUIRoutine();
        }

        myActionRoutine = StartCoroutine(actionRoutine());
    }

    IEnumerator actionRoutine()
    {
        //randomly generate wait time
        yield return ReturnRoutine();//to make sure they are in the right spot

        yield return new WaitForSeconds(stats.getRandomWaitTime());//or should it que up attacks, so we make sure that they are better timed

        yield return moveToShowAttack();//show attack also throws attack, this also starts the routine

        DecideNextAction();
    }

    IEnumerator moveToShowAttack()
    {
        //move to Demo point
        yield return JumpToShow();

        //move to the point the throw attack
        //calculate duration,so if we are already at the top its fine
        yield return SlamBeforeAttackRoutine();

        //curState = attackState.ThrowingAttack; TO DO:I have to redo animation fuck
        yield return DrawBackToAttackRoutine();
        yield return DelegateAction;//should work as long as they have traps

        yield return ReturnRoutine();
        
    }
    IEnumerator JumpToShow()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, EnemysManager.instance.getDemoAttackPoint().y, 0);
        float timer = 0;
        float duration = Mathf.Abs(startPos.y - endPos.y) / MoveToShowSpeed;
        while (Vector3.Distance(transform.position,endPos)>0.1f)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SlamBeforeAttackRoutine()
    {
        Vector3 startPos = transform.position;
        YAttackOffset = enemyAttackPoint.position.y- transform.position.y;
        Vector3 endPos = new Vector3(transform.position.x, EnemysManager.instance.getRandomAttackPoint().y - YAttackOffset, 0);
        float timer = 0;
        float duration = Mathf.Abs(startPos.y - endPos.y) / MoveToShowSpeed;
        while (transform.position != endPos)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;

        }
    }

    IEnumerator DrawBackToAttackRoutine()
    {
        //we are at the right height so we just move back being positive in x value then forward faster
        //back
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x+2,transform.position.y, 0);
        float timer = 0;
        while (transform.position != endPos)
        {
            float t = timer / DrawBackDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;
        }
        //forward
        startPos = transform.position;
        endPos = new Vector3(transform.position.x - 2, transform.position.y, 0);
        timer = 0;
        while (transform.position.x != endPos.x)
        {
            float t = timer / ThrustForwardDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ReturnRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = posToReturnTo;
        float timer = 0;
        float duration = Mathf.Abs(startPos.y - endPos.y) / MoveToShowSpeed;
        while (transform.position != endPos)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    //we do need to deal with getting parried
    public void Blocked()
    {
        if (myActionRoutine != null)
        {
            currentRageCount++;
            StopAllCoroutines();
            DelegateAction = null;
            //StopAllCoroutines();
            DecideNextAction();
        }
    }

    public void ClearAttacksNTraps()//called by enemyStats when enemy dies
    {
        foreach (var atk in currentAttacks)
            Destroy(atk);
        foreach (var trap in currentTraps)
            Destroy(trap);
    }

    IEnumerator AttackUIRoutine()//sends out on of its attacks
    {
        int rand = Random.Range(0, attackPrefabs.Count);
        GameObject attack = Instantiate(attackPrefabs[rand], enemyAttackPoint.position, attackPrefabs[rand].transform.rotation);//not sure if the rotation is right, but why can't i get transform of prefab?
        currentAttacks.Add(attack);
        var atk = attack.GetComponent<attack>();
        if (atk != null)
        {
            atk.Setstuff(stats);//TODO: in enemy attack have the attack decide which direction to go
        }
        yield return null;
    }//I plan to add non basic actions as heal self like sumo or... I feel like there was another specific one in mind
    //ah right swapping spots, though I don't think anyone payed attention to that one

    IEnumerator TrapUIRoutine()//should probably just be a method but idk if i can store and call it the same either way?
    {
        int rand = Random.Range(0, trapPrefabs.Count);
        GameObject trap = Instantiate(trapPrefabs[rand], enemyAttackPoint.position, trapPrefabs[rand].transform.rotation);//not sure if the rotation is right, but why can't i get transform of prefab?
        currentTraps.Add(trap);
        var atk = trap.GetComponent<attack>();
        yield return null;
    }

    public IEnumerator RageRoutine()
    {
        int Rager = 0;
        int randomRageAttack = (int)Random.Range(minMaxRageAttacks.x, minMaxRageAttacks.y);//I realize if its 2,3 it can't get 3 attacks
        //DelegateAction= AttackUIRoutine();
        while (Rager < randomRageAttack)
        {
            yield return AttackUIRoutine();
            yield return new WaitForSeconds(TimeBetweenRageAttacks); //would be nice if this number changed imo, like over time many at first but then slower
            Rager++;
            if (Rager < randomRageAttack)//only draw back if there will be another attack
            { yield return DrawBackToAttackRoutine(); }
        }
        currentRageCount = 0;
        WeaknessSpawnManager.instance.SpawnWeakPoint();
    }
}
