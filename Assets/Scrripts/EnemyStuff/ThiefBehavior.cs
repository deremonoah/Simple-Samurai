using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefBehavior : EnemyBehavior
{
    [SerializeField] int BountyToCauseRun;
    bool runningAway;
    
    protected override void DecideNextAction()
    {
        myActionRoutine = null;
        DelegateAction = null;



        //rage highest priority
        if (DelegateAction == null && currentRageCount >= RageThreashold)
        {
            int rand = Random.Range(0, 2);
            if (rand > 0)
            {
                DelegateAction = RageRoutine();
            }
        }
        if (DelegateAction == null && stats.HowMuchHaveYouStolen()>= BountyToCauseRun)
        {
            //flip sprite around
            DelegateAction = SpeccialUIRoutine();
        }
        if (DelegateAction == null)
        {
            DelegateAction = AttackUIRoutine();
        }

        myActionRoutine = StartCoroutine(actionRoutine());
    }
}
