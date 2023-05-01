using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] List<int> DifficultyWaves;

    //this is the number that is the top end of the random number for if enemies move up
    private int maxAgression;
    void Start()
    {

        GM = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnWave());
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
            aliveEnemys[target].damgEnemy(damg, effects);
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

        UpdateEnmsPos();
        if (aliveEnemys.Count != 0)
        {
            aliveEnemys[0].SetTargetPointer(PlayerStrikeArea.equipedWeapon.strikePointer);
        }

        SetSpecialPointers();

        if (me.myAbilities[0] == enemy.Ability.boss)
        { bossHPContainter.SetActive(false); }

        Destroy(me.gameObject);
    }

    public void UpdateEnmsPos()
    {
        for (int lcv = 0; lcv < aliveEnemys.Count; lcv++)
        {
            aliveEnemys[lcv].SetPosInList(lcv);
        }
    }

    IEnumerator SpawnWave()
    {

        yield return new WaitForSeconds(1);
        _villageDefense.startDefending();
        if (WaveControlVariable >= DifficultyWaves.Count)
        {
            GM.PlayerWins();
            yield return null;
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

            List<GameObject> currentWave = GenerateWave();

            for (int lcv = 0; lcv < currentWave.Count; lcv++)
            {
                SpawnEnemy(lcv, currentWave[lcv]);

                if (lcv == 0)
                    aliveEnemys[0].SetTargetPointer(PlayerStrikeArea.equipedWeapon.strikePointer);

                yield return new WaitForSeconds(0.5f);

            }



            WaveControlVariable++;
            Debug.Log("wcv in spawn=" + WaveControlVariable);
        }

        SetSpecialPointers();

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
        Debug.Log("wcv in generate"+WaveControlVariable);
        var roundDifficulty = DifficultyWaves[WaveControlVariable];

        //now I need a loop to put enemies in a list or
        List<GameObject> templist = enemyPrefabs;
        List<GameObject> currentWave= new List<GameObject>();

        while(roundDifficulty>0)
        {
            int rand = Random.Range(0, enemyPrefabs.Count);
            int individualDificulty = templist[rand].GetComponent<enemy>().difficulty[currentWave.Count];
            Debug.Log("individual dif "+individualDificulty+"     ||  round dif "+roundDifficulty);
            if (individualDificulty<=roundDifficulty)
            {
                currentWave.Add(templist[rand]);
                roundDifficulty -= individualDificulty;
            }
            else
            {
                templist.RemoveAt(rand);
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

    private void SetSpecialPointers()
    {
        if (PlayerStrikeArea.equipedWeapon.effs[0] == WeaponEffect.bow && aliveEnemys.Count != 0)
        {
            SetBowPointers();
        }
        else if (PlayerStrikeArea.equipedWeapon.effs[0] == WeaponEffect.odachi && aliveEnemys.Count != 0)
        {
            SetOdachiPointers();
        }
    }

    public void SetBowPointers()
    {
        foreach (enemy enm in aliveEnemys)
        {
            foreach (GameObject pointer in enm.BowPointers)
            {
                pointer.SetActive(false);
            }
        }

        int enmIndex = 0;
        for (int PointerIndex = 0; PointerIndex < 3; PointerIndex++)
        {
            enmIndex++;
            if (enmIndex >= aliveEnemys.Count)
            {
                enmIndex = 0;
            }

            aliveEnemys[enmIndex].BowPointers[PointerIndex].SetActive(true);

        }
    }

    public void SetOdachiPointers()
    {
        if (aliveEnemys.Count > 2)
        {
            aliveEnemys[1].SetTargetPointer(OdachiSprites[1]);
            aliveEnemys[2].SetTargetPointer(OdachiSprites[2]);
        }
        else if(aliveEnemys.Count == 2)
        {
            aliveEnemys[1].SetTargetPointer(OdachiSprites[0]);
        }

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
}
