using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;

public class EnemysManager : MonoBehaviour
{
    public GameObject[] enemySpawnsPoints;

    public List<enemy> aliveEnemys;
    public float OpenTimer = 0.5f;

    private bool spawned = false;

    [SerializeField] int recPos;


    [SerializeField] List<GameObject> attackStartPoints;
    [SerializeField] GameObject attackEndPointStandard;

    public List<EnmWave> enemyWaves;
    public int WaveControlVariable;

    private GameManager GM;
    private GameFlowManager _flowManager;
    private VillageDefense _villageDefense;
    private StrikeArea PlayerStrikeArea;

    [SerializeField] List<Sprite> OdachiSprites;

    public Image bossHPBar;
    [SerializeField] GameObject bossHPContainter;
    public GameObject BossHPPointer;
    public List<GameObject> BossBowPointers;

    [SerializeField] List<Transform> caltropSpots;
    [SerializeField] List<Transform> smokeSpots;

    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] List<GameObject> bossPrefabs;
    [SerializeField] List<int> DifficultyWaves;
    [SerializeField] List<GameObject> FreshEnemies;

    [SerializeField] List<PathCreator> thrownPaths;

    private PointerManager PM;
    //above refering to enemies not yet used

    //this is the number that is the top end of the random number for if enemies move up
    private int maxAgression;
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnWave());
        PM = FindObjectOfType<PointerManager>();
        PlayerStrikeArea = FindObjectOfType<StrikeArea>();
        _villageDefense = FindObjectOfType<VillageDefense>();
        _flowManager = GetComponent<GameFlowManager>();
        maxAgression = 11;
    }


    void Update()
    {
        if (spawned && aliveEnemys.Count < 1)
        {
            if (OpenTimer <= 0)
            {
                spawned = false;
                OpenTimer = 0.5f;
                _flowManager.StartMenues();
                _villageDefense.ResetVillage();
            }
            else
            { OpenTimer -= Time.deltaTime; }
        }

    }

    public void SpawnEnemy(int point, GameObject enmPrefab)
    {
        enemy enm = Instantiate(enmPrefab, enemySpawnsPoints[point].transform.position, enemySpawnsPoints[point].transform.rotation).GetComponent<enemy>();
        aliveEnemys.Add(enm);
        enm.GetComponent<enemy>().SetThings(attackStartPoints, attackEndPointStandard, point);
        if(enm.myAbilities[0] == enemy.Ability.boss)
        { bossHPContainter.SetActive(true); }
        spawned = true;
        recPos = point;
    }
    public void DamageEnemy(float damg, int target, List<WeaponEffect> effects)
    {
        if (aliveEnemys.Count > target)
        {
            aliveEnemys[target].damageEnemy(damg, effects);
        }
    }
    public int GetPos()
    {
        return recPos;
    }

    public void OnDied(enemy me)
    {
        if (aliveEnemys.Contains(me))
        {
            aliveEnemys.Remove(me);
        }

        UpdateEnmsPosRefrence();
        if (aliveEnemys.Count != 0)
        {
            aliveEnemys[0].SetTargetPointer(PlayerStrikeArea.equipedWeapon.strikePointer);
        }

        PM.UpdatePointers(aliveEnemys);

        if (me.myAbilities[0] == enemy.Ability.boss)
        { bossHPContainter.SetActive(false); }

        Destroy(me.gameObject);
    }

    public void UpdateEnmsPosRefrence()
    {
        for (int lcv = 0; lcv < aliveEnemys.Count; lcv++)
        {
            aliveEnemys[lcv].SetPosInList(lcv);
        }
        //maybe call pointer manager here?
    }

    IEnumerator SpawnWave()
    {

        yield return new WaitForSeconds(1);
        _villageDefense.startDefending();
        
        if (WaveControlVariable >= DifficultyWaves.Count)
        {
            GM.PlayerWins();
            yield return null;
        }else if (enemyWaves.Count > 0)
        {
            //this uses whatever wave is there 

            for (int lcv = 0; lcv < enemyWaves[0].enmsInWave.Length || lcv > 4; lcv++)
            {
                SpawnEnemy(lcv, enemyWaves[0].enmsInWave[lcv]);

                if (lcv == 0)
                    aliveEnemys[0].SetTargetPointer(PlayerStrikeArea.equipedWeapon.strikePointer);

                yield return new WaitForSeconds(0.5f);

            }

            enemyWaves.RemoveAt(0);
        }
        else
        {
            /*for (int lcv = 0; lcv < enemyWaves[WaveControlVariable].enmsInWave.Length; lcv++)
            {
                SpawnEnemy(lcv, enemyWaves[WaveControlVariable].enmsInWave[lcv]);

                if (lcv == 0)
                    aliveEnemys[0].SetTargetPointer(PlayerStrikeArea.equipedWeapon.strikePointer);

                yield return new WaitForSeconds(0.5f);

            } this is the old code that spawned in the dudes*/

            List<GameObject> currentWave = new List<GameObject>();
            currentWave = GenerateWave();

            for (int lcv = 0; lcv < currentWave.Count ||lcv > 4; lcv++)
            {
                SpawnEnemy(lcv, currentWave[lcv]);

                if (lcv == 0)
                    aliveEnemys[0].SetTargetPointer(PlayerStrikeArea.equipedWeapon.strikePointer);

                yield return new WaitForSeconds(0.5f);

            }
            WaveControlVariable++;


        }


        PM.UpdatePointers(aliveEnemys);

        //because this is the start of a new combat this is a good time to
        ResetAgressionMax();
    }

    private List<GameObject> GenerateWave()
    {
        //this might take in a value of difficulty
        //planning so I need to be able to toss enemies in a wave
        //well i don't need a wave I just need to generate a list of the enemies using the prefabs

        //that means I need a list of all the enemies and they all need some dificulty value on them
        //but maybe there would need to be another value to indicate that there should be a boss on x wave

        //this will get the difficulty for the round
        //Debug.Log("wcv in generate"+WaveControlVariable);
        int roundDifficulty = DifficultyWaves[WaveControlVariable];

        //now I need a loop to put enemies in a list or
        List<GameObject> tempEnemylist = new List<GameObject>();
        List<GameObject> currentWave = new List<GameObject>();

        //temporary boss part
        if (WaveControlVariable>7)
        {
            int bossRand = Random.Range(0, bossPrefabs.Count);
            currentWave.Add(bossPrefabs[bossRand]);
            bossPrefabs.RemoveAt(bossRand);
        }

        
        //loading temp list with enemy prefabs
        
        
        for (int lcv = 0; lcv < enemyPrefabs.Count; lcv++)
        {
            tempEnemylist.Add(enemyPrefabs[lcv]);
        }
        //this makes the fresh enemies more likley
        for (int lcv = 0; lcv < FreshEnemies.Count; lcv++)
        {
            tempEnemylist.Add(FreshEnemies[lcv]);
            
        }
        
        
        
        while(roundDifficulty>0 && tempEnemylist.Count>0 && currentWave.Count < 5)
        {
            int rand = Random.Range(0, tempEnemylist.Count);
            int individualDificulty = tempEnemylist[rand].GetComponent<enemy>().difficulty[currentWave.Count];

            //Debug.Log("individual dif "+individualDificulty+"     ||  round dif "+roundDifficulty);
            if (individualDificulty<=roundDifficulty && rand<tempEnemylist.Count)
            {
                //adding enemy to current wave
                currentWave.Add(tempEnemylist[rand]);
                roundDifficulty -= individualDificulty;
                
                //check if they were in fresh enemies they aren't now so they shouldn't be in there
                //I think this current system will favor more difficult enemies because they will get rejected more and make it more likley they then appear
                for(int lcv= 0;lcv<FreshEnemies.Count;lcv++)
                {
                    if(tempEnemylist[rand] == FreshEnemies[lcv])
                    {
                        FreshEnemies.RemoveAt(lcv);
                        lcv--;
                        //incase there are repeats it doesn't skip over any
                    }
                }

                //then maybe removing it to have less repeats and can't have this before becasue above code would lose refrence
                int coin = Random.Range(0, 2);
                if (coin == 0)
                { tempEnemylist.RemoveAt(rand); }

                //so we don't go past 4 enemies in a wave
                if (currentWave.Count == 4)
                {
                    break;
                }
            }
            else
            {
                FreshEnemies.Add(tempEnemylist[rand]);
                tempEnemylist.RemoveAt(rand);
            }
        }

        return currentWave;

    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }

    public void SetTargetEnmPointer(int num, Sprite pointer)
    {
        aliveEnemys[num].SetTargetPointer(pointer);
    }

    

    public List<List<Transform>> GetNinjaInfo()
    {
        List<List<Transform>> temp = new List<List<Transform>>();
        temp.Add(caltropSpots);
        temp.Add(smokeSpots);

        return temp;
    }

    public void ClearCurrentWave()
    {
        for(int lcv = aliveEnemys.Count-1;lcv >= 0;lcv--)
        {
            Debug.Log(lcv + " " + aliveEnemys.Count);
            OnDied(aliveEnemys[lcv]);
        }
    }

    public PathCreator GetRandomThrowPath()
    {
        int rand = Random.Range(0, thrownPaths.Count);
        return thrownPaths[rand];
    }

    public void CycleEnemyList()
    {
        //set position just tells the enemy which spots its in for the list
        //aliveEnemys is the list enemy classes alive rn
        if (aliveEnemys.Count > 1)
        {


            aliveEnemys.Add(aliveEnemys[0]);
            aliveEnemys.RemoveAt(0);
            //this puts the first guy last in the list but their positions in space still need to be changed so player sees

            for (int lcv = 0; lcv < aliveEnemys.Count; lcv++)
            {
                //this gets the specific enemy game object and then moves them to the proper spawn spot they should now be in
                aliveEnemys[lcv].gameObject.transform.position = enemySpawnsPoints[lcv].transform.position;
            }


            // I think i need below
            UpdateEnmsPosRefrence();
            if (aliveEnemys.Count != 0)
            {
                aliveEnemys[0].SetTargetPointer(PlayerStrikeArea.equipedWeapon.strikePointer);
            }

            //have to removepointer from enemy in back
            aliveEnemys[aliveEnemys.Count - 1].DisablePointer();

            PM.UpdatePointers(aliveEnemys);
            //would call pointer manager here once working
        }
    }

 #region Enemy Agression

    public int GetMaxAgression()
    {
        return maxAgression;
    }

    //this is called after an enemy moves up to make it harder in higher agression waves that there isn't excessive swapping
    public void IncreaseAgressionRange(int agro)
    {
        maxAgression += 8;
        //I could change this to increase by the specific enemies agression, so 3 would make it effectivley 13 and that specific enemy to be under or equal to 3
        //if it were higher like 8 to 18 yeah ill try it. eh still feels like too much swaping also this happens right after a block
    }

    private void ResetAgressionMax()
    {
        maxAgression = 11;
    }
 #endregion

    public void IncreaseNextWaveDifficulty(int much)
    {
        DifficultyWaves[WaveControlVariable] += much;
    }
}
