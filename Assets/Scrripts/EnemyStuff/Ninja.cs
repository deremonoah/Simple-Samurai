using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : enemy
{
    private List<Transform> _caltropSpots;
    private List<Transform> _smokeSpots;

    protected override void Start()
    {
        base.Start();
        List<List<Transform>> temp = new List<List<Transform>>();
        temp = enmsSys.GetNinjaInfo();
        _caltropSpots = temp[0];
        _smokeSpots = temp[1];
    }

    private void SpawnCaltrop()
    {
        int rand = Random.Range(0, _caltropSpots.Count);
    }

    private void SpawnSmoke()
    {
        int rand = Random.Range(0,_smokeSpots.Count);
    }
    
}
