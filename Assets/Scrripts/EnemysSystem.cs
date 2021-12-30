using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemysSystem : MonoBehaviour
{
    public GameObject[] enmSpawns;
    public GameObject[] enmPrefabs;
    public GameObject[] enms;
    
    public bool[] hasenmy;

    public bool[] diedList;
    private int spawnedEnms = 0;
    private bool spawned = false;


    [SerializeField] float timer, Max;
    [SerializeField] int recPos;
    [SerializeField] EnmyHPsSystem enmshpsSys;

    [SerializeField] GameObject atkPrefab;
    [SerializeField] GameObject atkStart;
    [SerializeField] GameObject atkEnd;

    public EnmWave[] waves;
    private int wcv;

    private GameManager GM;

    void Start()
    {
        timer = Max;
        GM = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnWave());
    }

    
    void Update()
    {

        /*if (timer > 0)
        {
            timer -= Time.deltaTime;
        }else
        {
            SpawnEnemy(Random.Range(0, 4),Random.Range(0,enmPrefabs.Length));
            timer = Max;
        }*/
        
        if (spawned && spawnedEnms < 1)
        {
            GM.OpenPickPan();
            spawned = false;
        }
        


    }
    
    public void SpawnEnemy(int point,int enmType)
    {
        if (!hasenmy[point])
        {
            enms[point] = Instantiate(enmPrefabs[enmType], enmSpawns[point].transform.position, enmSpawns[point].transform.rotation);
            enms[point].GetComponent<enmy>().SetThings(atkPrefab,atkStart,atkEnd);
            spawnedEnms++;
            spawned = true;
            recPos = point;
            hasenmy[point] = true;
            
        }
        
    }
    public void DamageEnemy(float damg, int target)
    {
        if (!diedList[target])
        {
            enms[target].GetComponent<enmy>().damgEnemy(damg);
        }
    }
    public int GetPos()
    {
        return recPos;
    }

    public void Died(int pos)
    {
        diedList[pos] = true;
        hasenmy[pos] = false;
        spawnedEnms--;
    }

    

    public int hasAllDied()
    {
        return spawnedEnms;
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(2);
        for (int lcv = 0; lcv < waves[wcv].getLength(); lcv++)
        {
            for (int lcv2 = 0; lcv2 < enmPrefabs.Length; lcv2++)
            {
                if (waves[wcv].getEnm(lcv).getType() == enmPrefabs[lcv2].GetComponent<enmy>().getType())
                {
                    SpawnEnemy(lcv, lcv2);
                    yield return new WaitForSeconds(1);
                }
            }
        }
        wcv++;
        
    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }

}
