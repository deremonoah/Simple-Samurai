using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : enemy
{
    private List<Transform> _caltropSpots;
    private List<Transform> _smokeSpots;
    [SerializeField] List<GameObject> trapsSet;

    protected override void Start()
    {
        base.Start();
        List<List<Transform>> temp = new List<List<Transform>>();
        temp = enmsSys.GetNinjaInfo();
        _caltropSpots = temp[0];
        _smokeSpots = temp[1];
    }

    protected override void Update()
    {
        if(base.HP<=0)
        {
            foreach (var trap in trapsSet)
                Destroy(trap);
        }
        base.Update();
    }

    protected override void StartMyRoutine()
    {
        bool hasStarted = false;
        for (int lcv = 0; lcv < myAbilities.Count; lcv++)
        {
            if (myAbilities[lcv] == Ability.ninja)
            {
                int rand = Random.Range(0, 5);
                if (rand == 0)
                { 
                    myActionRoutine = StartCoroutine(SpawnCaltrop());
                    hasStarted = true;
                }
                else if (rand == 1)
                { 
                    myActionRoutine = StartCoroutine(SpawnSmoke());
                    hasStarted = true;
                }
                
            }
        }

        if (!hasStarted)
        {
            base.StartMyRoutine();
        }
        

    }

    

    IEnumerator SpawnCaltrop()
    {
        int rand = Random.Range(0, _caltropSpots.Count+2);
        rand = Mathf.Clamp(rand - 2, 0, _caltropSpots.Count);
        //look up better way of weighting outcomes of randomness
        Debug.Log(_caltropSpots.Count);
        trapsSet.Add(Instantiate(specialPrefabs[0], _caltropSpots[rand].position, transform.rotation));
        yield return new WaitForSeconds(2f);
        StartMyRoutine();
    }

    IEnumerator SpawnSmoke()
    {
        int rand = Random.Range(0,_smokeSpots.Count);
        trapsSet.Add(Instantiate(specialPrefabs[1], _smokeSpots[rand].position, transform.rotation));
        yield return new WaitForSeconds(1f);
        StartMyRoutine();
    }
    
}
