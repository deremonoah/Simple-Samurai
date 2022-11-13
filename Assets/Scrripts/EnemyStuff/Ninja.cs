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

    protected override void StartMyRoutine()
    {
        Debug.Log("got ninja action");
        int rand = Random.Range(0, 5);
        if (rand == 0)
        { myActionRoutine = StartCoroutine(SpawnCaltrop()); }
        else if (rand == 1)
        { myActionRoutine = StartCoroutine(SpawnSmoke()); }
        else { myActionRoutine = base.StartCoroutine(TheAttackRoutine()); }
        
    }

    

    IEnumerator SpawnCaltrop()
    {
        int rand = Random.Range(0, _caltropSpots.Count+2);
        rand = Mathf.Clamp(rand - 2, 0, _caltropSpots.Count - 1);
        Debug.Log(_caltropSpots.Count);
        Instantiate(specialPrefabs[0], _caltropSpots[rand].position, transform.rotation);
        yield return new WaitForSeconds(2f);
        StartMyRoutine();
    }

    IEnumerator SpawnSmoke()
    {
        int rand = Random.Range(0,_smokeSpots.Count);
        Instantiate(specialPrefabs[1], _smokeSpots[rand].position, transform.rotation);
        yield return new WaitForSeconds(1f);
        StartMyRoutine();
    }
    
}
