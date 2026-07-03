using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessSpawnManager : MonoBehaviour
{
    public static WeaknessSpawnManager instance;
    [SerializeField] GameObject weakPointPrefab;
    [Header("frequency of random weakpoint")]
    [SerializeField] float SpawnTimerMin;//controls how often these spawn randomly
    [SerializeField] float SpawnTimerMax;
    [Header("Position along player's path")]
    [SerializeField] float SpawnPosMin;
    [SerializeField] float SpawnPosMax;
    [SerializeField] float SpawnPosOffset;
    private float spawnTimer;
    private GameObject spawnedWeakPoint;
    private bool inCombat;
    private StrikePoint point;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Debug.LogError("there are 2 weaknesSpawn managers in the scene");
        }
        else
        {
            instance = this;
            inCombat = true;//kind of need it for testing & so it can happen in first fight
        }
    }

    private void Start()
    {
        point = FindObjectOfType<StrikePoint>();
        spawnTimer = Random.Range(SpawnTimerMin, SpawnTimerMax);
    }

    private void Update()
    {
        if(inCombat)
        {
            if(spawnTimer<=0)
            {
                spawnTimer = Random.Range(SpawnTimerMin, SpawnTimerMax);
                SpawnWeakPoint();
            }
            else
            {
                spawnTimer -= Time.deltaTime;
            }
        }
    }

    //will be called by enemies after they fluryStrike(multiple attacks I have yet to implement)
    public void SpawnWeakPoint()
    {
        if(spawnedWeakPoint==null)
        {
            float randpos = SpawnPosOffset+Random.Range(SpawnPosMin, SpawnPosMax);//idk man, just based off what I have in buff areas so just some nummbers
            spawnedWeakPoint=Instantiate(weakPointPrefab, point.currentPath.path.GetPointAtDistance(randpos), transform.rotation);
        }
    }

    public void InCombat(bool ic)
    {
        inCombat = ic;
        if(!inCombat)
        {
            Destroy(spawnedWeakPoint);
            spawnedWeakPoint = null;//I don't think I need to set it to null but eh
        }
    }
}
