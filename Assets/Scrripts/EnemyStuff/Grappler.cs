using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : enemy
{
    /*i need access to trap spawns
     * 
    */
    [SerializeField] GameObject blockprefab;
    [SerializeField] List<GameObject> blocksSet;
    [SerializeField] List<Transform> _blockSpots;
    private int actionCount;
    private int lastAction; //0 is attack, 1 is block, 2 is heal
    protected override void Start()
    {
        base.Start();
        List<List<Transform>> temp = new List<List<Transform>>();
        temp = enmsSys.GetTrapSpawnSpots();
        _blockSpots = temp[0];
        actionCount = 0;
    }

    protected override void Update()
    {
        if (base.getCurrentHP() <= 0)
        {
            foreach (var trap in blocksSet)
                Destroy(trap);
        }
        base.Update();
    }

    protected override void DecideNStartAction()
    {
        bool hasStarted = false;
        int rand = Random.Range(0, 10);
        actionCount++;

        //this should stop duplicate attacks and too many actions
        if (myActionRoutine != null)
        {
            StopCoroutine(myActionRoutine);
        }

        if(actionCount>3)
        {
            foreach (var block in blocksSet)
                Destroy(block);
            blocksSet.Clear(); }
        if(rand<4 &&lastAction!=1)//has 40% chance to put up block
        {
            hasStarted = true;
            myActionRoutine = StartCoroutine(SpawnBlock());
            lastAction = 1;
        }
        //has and 40% chance to heal and wont heal twice in a row
        else if (base.getCurrentHP() < base.maxHP/2 && rand>5 && lastAction!=2)
        {
            Debug.Log("started Regen & last action:" +lastAction);
            base.StartRegen(6f);
            hasStarted = true;
            lastAction = 2;
        }
        //should also check if they have the self heal ability if they want to self heal based on stuff or maybe put that in base startmyroutine
        if (!hasStarted)
        {
            base.DecideNStartAction();
            lastAction = 0;
        }
    }

    IEnumerator SpawnBlock()
    {
        int rand = Random.Range(0, _blockSpots.Count);
        blocksSet.Add(Instantiate(blockprefab, _blockSpots[rand].position, transform.rotation));

        yield return new WaitForSeconds(1f);
        DecideNStartAction();
    }
}
