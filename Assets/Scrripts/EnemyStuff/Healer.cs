using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : enemy
{
    [SerializeField] float healMin, healMax;
    private List<GameObject> _mycurrentAttacks = new List<GameObject>();
    

    protected override void Start()
    {
        base.Start();
        StopAllCoroutines();
        myActionRoutine = StartCoroutine(healEnmRoutine());
    }


    protected override void StartMyRoutine()
    {
        myActionRoutine = StartCoroutine(healEnmRoutine());
    }

    public void HealingUI()
    {
        
        GameObject heal = Instantiate(specialPrefabs[0], base.atkStarts[3].transform.position, base.atkStarts[3].transform.rotation);
        heal.GetComponent<EnmAtKArea>().Setstuff(this, base.atkStarts[0].transform, SpecialDirs[0]);
        var newList = new List<GameObject>();
        if (_mycurrentAttacks.Count > 0)
            foreach (var swing in _mycurrentAttacks)
                if (swing != null)
                    newList.Add(swing);

        newList.Add(heal);
        base.currentAttacks = newList;
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
        //soundMRef.PlaySound("heal"); make a heal sound
    }

}
