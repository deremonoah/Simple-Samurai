using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : enemy
{
    [SerializeField] float healMin, healMax;
    [SerializeField] List<Vector2> MySpecialDirs;

    protected override void Start()
    {
        base.Start();
        StopAllCoroutines();
        myActionRoutine = StartCoroutine(healEnmRoutine());
    }

    protected override void StartMyRoutine()
    {
        if (myAbilities[0] == Ability.heal)
        {
            myActionRoutine = StartCoroutine(healEnmRoutine());
        }
    }


    IEnumerator healEnmRoutine()
    {
        curState = attackState.waiting;
        bool hitIf = false;
        targetally = new enemy();


        yield return new WaitForSeconds(Random.Range(randWaitmin + waitTimerOffset, randWaitmax + waitTimerOffset));

        foreach (enemy i in enmsSys.aliveEnemys)
        {
            if (i.HP < i.maxHP)
            {
                targetally = i;
                targetally = i;
                hitIf = true;
                break;
            }
        }


        if (hitIf)
        {
            HealingUI();
            curState = attackState.readying;
            yield return new WaitForSeconds(readyingTimer);

            curState = attackState.swinging;
            yield return new WaitForSeconds(strikeTimer);


            //soundMRef.PlaySound("heal"); make a heal sound

            myActionRoutine = StartCoroutine(healEnmRoutine());
        }
        else
        {
            myActionRoutine = StartCoroutine(TheAttackRoutine());
        }


    }

    public void healAllyNow()
    {
        targetally.healEnm(Random.Range(healMin, healMax));
    }

    public void HealingUI()
    {
        GameObject heal = Instantiate(specialPrefabs[0], atkStarts[3].transform.position, atkStarts[3].transform.rotation);
        //var dir = Random.Range(0, healDirs.Count);
        heal.GetComponent<EnmAtKArea>().Setstuff(this, atkStarts[0].transform, MySpecialDirs[0]);
        var newList = new List<GameObject>();
        if (currentAttacks.Count > 0)
            foreach (var swing in currentAttacks)
                if (swing != null)
                    newList.Add(swing);

        newList.Add(heal);
        currentAttacks = newList;
    }
}
