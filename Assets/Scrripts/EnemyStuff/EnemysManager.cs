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
    void Start()
    {

        GM = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnWave());
        PlayerStrikeArea = FindObjectOfType<StrikeArea>();
        _villageDefense = FindObjectOfType<VillageDefense>();
        _flowManager = GetComponent<GameFlowManager>();
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


        if (PlayerStrikeArea.equipedWeapon.effs[0] == WeaponEffect.bow && aliveEnemys.Count != 0)
        {
            SetBowPointers();
        }
        if (PlayerStrikeArea.equipedWeapon.effs[0] == WeaponEffect.odachi && aliveEnemys.Count != 0)
        {
            SetOdachiPointers();
        }

        if (me.myAbilities[0] == enemy.Ability.boss)
        { bossHPContainter.SetActive(false); }
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
        if (WaveControlVariable >= enemyWaves.Count)
        {
            GM.PlayerWins();
            yield return null;
        }
        else
        {

            for (int lcv = 0; lcv < enemyWaves[WaveControlVariable].enmsInWave.Length; lcv++)
            {
                SpawnEnemy(lcv, enemyWaves[WaveControlVariable].enmsInWave[lcv]);

                if (lcv == 0)
                    aliveEnemys[0].SetTargetPointer(PlayerStrikeArea.equipedWeapon.strikePointer);

                yield return new WaitForSeconds(0.5f);

            }

            WaveControlVariable++;
        }

        if (PlayerStrikeArea.equipedWeapon.effs[0] == WeaponEffect.bow && aliveEnemys.Count != 0)
        {
            SetBowPointers();
        }else if (PlayerStrikeArea.equipedWeapon.effs[0] == WeaponEffect.odachi && aliveEnemys.Count != 0)
        {
            SetOdachiPointers();
        }


    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }

    public void SetTargetEnmPointer(int num, Sprite pointer)
    {
        aliveEnemys[num].SetTargetPointer(pointer);
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
}
