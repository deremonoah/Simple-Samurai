using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : enemyStats
{
    private List<Transform> _caltropSpots;
    private List<Transform> _smokeSpots;
    [SerializeField] List<GameObject> trapsSet;

    protected override void Start()
    {
        base.Start();
        List<List<Transform>> temp = new List<List<Transform>>();
        temp = enmsSys.GetTrapSpawnSpots();
        _caltropSpots = temp[0];
        _smokeSpots = temp[1];
    }

    protected override void Update()
    {
        if(base.getCurrentHP() <= 0)
        {
            foreach (var trap in trapsSet)
                Destroy(trap);
        }
        base.Update();
    }
    
}
