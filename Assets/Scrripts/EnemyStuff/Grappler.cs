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

    protected override void Start()
    {
        base.Start();
        List<List<Transform>> temp = new List<List<Transform>>();
        temp = enmsSys.GetNinjaInfo();
        _blockSpots = temp[0];
        actionCount = 0;
    }

    protected override void Update()
    {
        if (base.HP <= 0)
        {
            foreach (var trap in blocksSet)
                Destroy(trap);
        }
        base.Update();
    }

    protected override void StartMyRoutine()
    {
        bool hasStarted = false;
        int rand = Random.Range(0, 10);
        actionCount++;

        //this should stop duplicate attacks and too many actions
        StopCoroutine(TheAttackRoutine());

        if(actionCount>3)
        {
            foreach (var block in blocksSet)
                Destroy(block);
            blocksSet.Clear(); }
        if(rand<4)//has 40% chance to put up block
        {
            hasStarted = true;
            myActionRoutine = StartCoroutine(SpawnBlock());
        }

        rand = Random.Range(0, 10);
        //has and 70% chance to heal we may adjust later
        if (base.HP<base.maxHP/2 && rand>2)
        {
            base.StartRegen(6f);
            hasStarted = true;
        }
        //should also check if they have the self heal ability if they want to self heal based on stuff or maybe put that in base startmyroutine
        if (!hasStarted)
        {
            base.StartMyRoutine();
        }
    }

    IEnumerator SpawnBlock()
    {
        int rand = Random.Range(0, _blockSpots.Count);
        blocksSet.Add(Instantiate(blockprefab, _blockSpots[rand].position, transform.rotation));

        yield return new WaitForSeconds(1f);
        StartMyRoutine();
    }
}
