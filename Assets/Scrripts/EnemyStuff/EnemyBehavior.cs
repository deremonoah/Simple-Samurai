
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //should have enemy stats as seperate
    protected Coroutine myActionRoutine;
    protected IEnumerator DelegateAction;
    protected enemyStats stats;

    [Header("attack info")]
    [SerializeField] List<GameObject> attackPrefabs;
    [SerializeField] Transform enemyAttackPoint;
    [SerializeField] float MoveToShowSpeed;
    [Header("attack anim durations")]
    [SerializeField] float DrawBackDuration;
    [SerializeField] float ThrustForwardDuration;

    [Header("trap prefabs")]
    [SerializeField] protected List<GameObject> trapPrefabs;

    [SerializeField] int maxTraps;
    [Range(1,100)]
    [SerializeField] int TrapPercentage;

    [Header("special prefabs for custom abilities")]
    [SerializeField] protected GameObject SpecialPrefab;//for things like theifbehavior which enherits from this
    [SerializeField] float TimeToWaitAfterSpecial;

    [Header("Rage (Inclusive,Exclusive)")]
    [SerializeField] Vector2 minMaxRageAttacks;
    [SerializeField] float TimeBetweenRageAttacks;
    [SerializeField] protected int RageThreashold;
    [SerializeField] bool canAttackDifferentLineInRage;//for if they might move up or what not to attack a different line

    [Header("attacks and traps spawned and tracked")]
    [SerializeField] protected List<GameObject> SpawnedAttacks = new List<GameObject>();
    [SerializeField] protected List<GameObject> spawnedTraps = new List<GameObject>();

    protected int currentRageCount;
    private float YAttackOffset;
    private Vector3 posToReturnTo;

    public void Start()
    {
        stats = GetComponent<enemyStats>();
        posToReturnTo = EnemysManager.instance.getPosToReturnTo(stats.posInList);
        DecideNextAction();
    }

    protected virtual void DecideNextAction()
    {
        myActionRoutine = null;
        DelegateAction = null;

        

        //rage highest priority
        if (DelegateAction==null &&currentRageCount>=RageThreashold)
        {
            int rand = Random.Range(0, 2);
            if(rand>0)
            {
                DelegateAction = RageRoutine();
            }
        }
        if(DelegateAction==null && trapPrefabs.Count>0 && spawnedTraps.Count<=maxTraps)
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

    protected IEnumerator actionRoutine()
    {
        //randomly generate wait time
        yield return ReturnRoutine();//to make sure they are in the right spot

        SpawnedAttacks.RemoveAll(attack => attack == null);
        spawnedTraps.RemoveAll(trap => trap == null);

        yield return new WaitForSeconds(stats.getRandomWaitTime());//or should it que up attacks, so we make sure that they are better timed

        yield return moveToShowAttack();//show attack also throws attack, this also starts the routine

        SpawnedAttacks.RemoveAll(attack => attack == null);
        spawnedTraps.RemoveAll(trap => trap == null);

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
    public void Blocked(AttackEffect atkeef, Weapon playerWeapon)
    {
        currentRageCount++;
        /*if (myActionRoutine != null)
        {
            currentRageCount++;
            if(DelegateAction!=RageRoutine())
            {
                StopAllCoroutines();
                DelegateAction = null;
                //StopAllCoroutines();
                DecideNextAction();
            }
            //should blocking them really reset their times? this just has been the case for so long idk
        }*/
        if(playerWeapon.hasEffect(WeaponEffect.sasumata))//might rename to stun for weapon effect
        {
            StopAllCoroutines();
            DelegateAction = null;
            DecideNextAction();//so it will stop rages or can, and then apply the stun right away
        }
        //add resolving if 
        if(atkeef==AttackEffect.DamageWeapon)
        {
            FindObjectOfType<PlayerEquipedItemsManager>().DamageItem(1);
            SoundManager.instance.PlaySound("breakItem");
        }
        else if(atkeef == AttackEffect.DamageArmor)
        {
            FindObjectOfType<PlayerEquipedItemsManager>().DamageItem(2);
            SoundManager.instance.PlaySound("breakItem");
        }
    }

    public void ClearAttacksNTraps()//called by enemyStats when enemy dies
    {
        foreach (var atk in SpawnedAttacks)
            Destroy(atk);
        foreach (var trap in spawnedTraps)
            Destroy(trap);
    }

    protected IEnumerator AttackUIRoutine()//sends out on of its attacks
    {
        int rand = Random.Range(0, attackPrefabs.Count);
        GameObject attack = Instantiate(attackPrefabs[rand], enemyAttackPoint.position, attackPrefabs[rand].transform.rotation);//not sure if the rotation is right, but why can't i get transform of prefab?
        SpawnedAttacks.Add(attack);
        var atk = attack.GetComponent<attack>();
        if (atk != null)
        { atk.Setstuff(stats); }//TODO: in enemy attack have the attack decide which direction to go}
        else { Debug.LogError("prefab instantiated in attack ui rotuine (in enemy behavior) didn't have enemyTrap on it"); }
        yield return null;
    }//I plan to add non basic actions as heal self like sumo or... I feel like there was another specific one in mind
    //ah right swapping spots, though I don't think anyone payed attention to that one

    protected IEnumerator TrapUIRoutine()//should probably just be a method but idk if i can store and call it the same either way?
    {
        int rand = Random.Range(0, trapPrefabs.Count);
        GameObject trap = Instantiate(trapPrefabs[rand], enemyAttackPoint.position, trapPrefabs[rand].transform.rotation);//not sure if the rotation is right, but why can't i get transform of prefab?
        spawnedTraps.Add(trap);
        EnemyTrap t = trap.GetComponent<EnemyTrap>();
        if (t != null)
        { t.SetEnemy(stats); }//for at least running away & maybe something else in future?
        else { Debug.LogError("prefab instantiated in trap ui rotuine (in enemy behavior) didn't have enemyTrap on it"); }

        yield return null;
    }

    protected IEnumerator SpeccialUIRoutine()//should probably just be a method but idk if i can store and call it the same either way?
    {
        GameObject special = Instantiate(SpecialPrefab, enemyAttackPoint.position, SpecialPrefab.transform.rotation);//not sure if the rotation is right, but why can't i get transform of prefab?
        spawnedTraps.Add(special);
        EnemyTrap t = special.GetComponent<EnemyTrap>();//this is for theif run away & probably spawning enemies
        if (t != null)
        { t.SetEnemy(stats); }//for at least running away & maybe something else in future?
        else { Debug.LogError("prefab instantiated in trap ui rotuine (in enemy behavior) didn't have enemyTrap on it"); }

        yield return new WaitForSeconds(TimeToWaitAfterSpecial);//don't want them deciding another routine until they have for sure ran away
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
            {
                if(canAttackDifferentLineInRage)
                { yield return SlamBeforeAttackRoutine(); }
                
                yield return DrawBackToAttackRoutine(); 
            }
        }
        currentRageCount = 0;
        WeaknessSpawnManager.instance.SpawnWeakPoint();
    }

    private IEnumerator RanAwayAnimationRoutine()
    {
        EnemyHPBarPlacerManager.instance.RemoveMeFromList(stats);
        transform.rotation = Quaternion.Euler(0, 180, 0);
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x + 10, startPos.y, startPos.z);
        float timeElapsed = 0;

        while(transform.position!=endPos)
        {
            float t = timeElapsed/1f;//2 f is the duration of run away
            transform.position = Vector3.Lerp(startPos, endPos, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        
        Destroy(this.gameObject);
    }

    public void RunAwayAnimStart()
    {
        StopAllCoroutines();//so they don;t attack, they shouldn't attack or ready an attack while running away, how to do?
        StartCoroutine(RanAwayAnimationRoutine());
    }

    public void IncreaseRageCount(int mad)
    {
        currentRageCount += mad;
    }
}
