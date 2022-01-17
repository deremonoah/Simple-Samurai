using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemysSystem : MonoBehaviour
{
    public GameObject[] enmSpawns;
    
    public List<enmy> enms;

    //public bool[] hasenmy;

    //public bool[] diedList;
    //private int spawnedEnms = 0;
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

    public GameObject EnmHPPointer;
    

    void Start()
    {

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

        if (spawned && enms.Count < 1)
        {
            GM.OpenPickPan();
            spawned = false;
        }



    }

    public void SpawnEnemy(int point, GameObject enmPrefab)
    {
        enmy enm = Instantiate(enmPrefab, enmSpawns[point].transform.position, enmSpawns[point].transform.rotation).GetComponent<enmy>();
        enms.Add(enm);
        enm.GetComponent<enmy>().SetThings(atkPrefab, atkStart, atkEnd);
        spawned = true;
        recPos = point;
    }
    public void DamageEnemy(float damg, int target, Effect effect)
    {

        if(enms.Count !=0)
        {
            enms[target].damgEnemy(damg, effect);
        }
    }
    public int GetPos()
    {
        return recPos;
    }

    public void Died(enmy me)
    {
        if (enms.Contains(me))
        {
            enms.Remove(me);
        }

        if (enms.Count != 0)
        {
            enms[0].SetAsTarget();
        }
        
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(2);
        for (int lcv = 0; lcv < waves[wcv].enmsInWave.Length; lcv++)
        {
            SpawnEnemy(lcv, waves[wcv].enmsInWave[lcv]);

            if (lcv == 0)
                enms[0].SetAsTarget();

            yield return new WaitForSeconds(0.5f);

        }

        wcv++;

    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }

    
}
