using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : enemy
{
    [SerializeField] float healMin, healMax;
    private List<GameObject> _mycurrentAttacks = new List<GameObject>();
    private enemy targetally;

    protected override void Start()
    {
        base.Start();
        //StopAllCoroutines(); if base starts the coroutine we don't want to stop it
        //DecideNStartAction(); base class already calls it
    } 

    protected override void DecideNStartAction()
    {
        //myActionRoutine = StartCoroutine(healEnmRoutine());
        
        bool hitIf = false;
        targetally = null;
        foreach (enemy i in enmsSys.aliveEnemys)
        {
            if (i.getCurrentHP() < i.maxHP)
            {
                targetally = i;
                hitIf = true;
                break;
            }
        }
        Debug.Log("hit if is:" + hitIf);
        if (hitIf)
        {
            delegateAction = HealingUI;
            hasPickedAction = true;
        }
        else
        {
            //no one to heal then hit
            Debug.Log("in healer attack decision");
            //AttackUI(); calling it works, but deligating it doesn't why?
            delegateAction = AttackUI;
            hasPickedAction = true;
        }
        //this call after the action is set
        myActionRoutine = StartCoroutine(TheActionRoutine());
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


    /*IEnumerator healEnmRoutine()
    {
        curState = attackState.waiting;
        bool hitIf = false;
        targetally = new enemy();

        yield return new WaitForSeconds(base.stunTimer);

        foreach (enemy i in enmsSys.aliveEnemys)
        {
            if (i.getCurrentHP() < i.maxHP)
            {
                targetally = i;
                hitIf = true;
                break;
            }
        }

        

        yield return new WaitForSeconds(Random.Range(randWaitmin + waitTimerOffset, randWaitmax + waitTimerOffset));


        curState = attackState.waiting;
        if (hitIf)
        {
            //turn green
            spriteChild.GetComponent<SpriteRenderer>().color = FindObjectOfType<ColorManager>().AboutToHealColor;
        }
        yield return new WaitForSeconds(readyingTimer);

        if (hitIf)
        {
            HealingUI();
            

            curState = attackState.ThrowingAttack;
            yield return new WaitForSeconds(strikeTimer);

            myActionRoutine = StartCoroutine(healEnmRoutine());
            spriteChild.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            myActionRoutine = StartCoroutine(TheActionRoutine());
        }

    this was the old way I did the routine now I am using deligates hope it works
    with the new animation system
    }*/

    public void healAllyNow()
    {
        targetally.healEnm(Random.Range(healMin, healMax));
        //soundMRef.PlaySound("heal"); make a heal sound
    }

}
