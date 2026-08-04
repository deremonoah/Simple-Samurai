using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : enemyStats
{
    [SerializeField] float healMin, healMax;
    private List<GameObject> _mycurrentAttacks = new List<GameObject>();
    private enemyStats targetally;

    protected override void Start()
    {
        base.Start();
        //StopAllCoroutines(); if base starts the coroutine we don't want to stop it
        //DecideNStartAction(); base class already calls it
    } 

    public void healAllyNow()
    {
        if(targetally!=null)
        {
            targetally.healEnm(Random.Range(healMin, healMax));
            //soundMRef.PlaySound("heal"); make a heal sound
        }
    }

}
