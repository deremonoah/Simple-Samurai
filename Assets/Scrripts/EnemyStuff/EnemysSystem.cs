using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemysSystem : MonoBehaviour
{
    public GameObject[] enmSpawns;

    public List<enmy> enms;
    public float OpenTimer = 0.5f;

    private bool spawned = false;

    [SerializeField] int recPos;


    [SerializeField] List<GameObject> attackkStarts;
    [SerializeField] GameObject atkEnd;

    public List<EnmWave> waves;
    private int WaveControlVariable;

    private GameManager GM;

    public GameObject EnmHPPointer;
    private StrikeArea PlayerSA;


    void Start()
    {

        GM = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnWave());
        PlayerSA = FindObjectOfType<StrikeArea>();
    }


    void Update()
    {
        if (spawned && enms.Count < 1)
        {
            if (OpenTimer <= 0)
            {
                spawned = false;
                OpenTimer = 0.5f;
                GM.OpenPickPan();
            }
            else
            { OpenTimer -= Time.deltaTime; }
        }

    }

    public void SpawnEnemy(int point, GameObject enmPrefab)
    {
        enmy enm = Instantiate(enmPrefab, enmSpawns[point].transform.position, enmSpawns[point].transform.rotation).GetComponent<enmy>();
        enms.Add(enm);
        enm.GetComponent<enmy>().SetThings(attackkStarts, atkEnd, point);
        spawned = true;
        recPos = point;
    }
    public void DamageEnemy(float damg, int target, List<WeaponEffect> effects)
    {
        if (enms.Count > target)
        {
            enms[target].damgEnemy(damg, effects);
        }
    }
    public int GetPos()
    {
        return recPos;
    }

    public void OnDied(enmy me)
    {
        if (enms.Contains(me))
        {
            enms.Remove(me);
        }

        UpdateEnmsPos();
        if (enms.Count != 0)
        {
            enms[0].SetTargetPointer(PlayerSA.equipedWeapon.strikePointer);
        }


        if (PlayerSA.equipedWeapon.effs[0] == WeaponEffect.bow && enms.Count != 0)
        {
            SetBowPointers();
        }

    }

    public void UpdateEnmsPos()
    {
        for (int lcv = 0; lcv < enms.Count; lcv++)
        {
            enms[lcv].SetPosInList(lcv);
        }
    }

    IEnumerator SpawnWave()
    {

        yield return new WaitForSeconds(1);
        if (WaveControlVariable >= waves.Count)
        {

            GM.PlayerWins();
            yield return null;
        }
        else
        {

            for (int lcv = 0; lcv < waves[WaveControlVariable].enmsInWave.Length; lcv++)
            {
                SpawnEnemy(lcv, waves[WaveControlVariable].enmsInWave[lcv]);

                if (lcv == 0)
                    enms[0].SetTargetPointer(PlayerSA.equipedWeapon.strikePointer);

                yield return new WaitForSeconds(0.5f);

            }

            WaveControlVariable++;
        }

        if (PlayerSA.equipedWeapon.effs[0] == WeaponEffect.bow && enms.Count != 0)
        {
            SetBowPointers();
        }


    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }

    public void SetTargetEnmPointer(int num, Sprite pointer)
    {
        enms[num].SetTargetPointer(pointer);
    }

    public void SetBowPointers()
    {
        foreach (enmy enm in enms)
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
            if (enmIndex >= enms.Count)
            {
                enmIndex = 0;
            }

            enms[enmIndex].BowPointers[PointerIndex].SetActive(true);

        }
    }

}
