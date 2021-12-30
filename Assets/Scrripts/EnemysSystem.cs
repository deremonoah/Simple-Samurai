using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemysSystem : MonoBehaviour
{
    public GameObject[] enmSpawns;
    public GameObject[] enmPrefabs;
    public List<GameObject> enms;

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
        GameObject enm = Instantiate(enmPrefab, enmSpawns[point].transform.position, enmSpawns[point].transform.rotation);
        enms.Add(enm);
        enm.GetComponent<enmy>().SetThings(atkPrefab, atkStart, atkEnd);
        spawned = true;
        recPos = point;
    }
    public void DamageEnemy(float damg, int target)
    {


        enms[target].GetComponent<enmy>().damgEnemy(damg);

    }
    public int GetPos()
    {
        return recPos;
    }

    public void Died(GameObject me)
    {
        if (enms.Contains(me))
        {
            enms.Remove(me);
        }

    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(2);
        for (int lcv = 0; lcv < waves[wcv].enmsInWave.Length; lcv++)
        {
            SpawnEnemy(lcv, waves[wcv].enmsInWave[lcv]);
            yield return new WaitForSeconds(0.5f);

        }
        wcv++;

    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }

}
