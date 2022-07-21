using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemysSystem : MonoBehaviour
{
    public GameObject[] enmSpawns;
    
    public List<enmy> enms;

    
    private bool spawned = false;

    [SerializeField] int recPos;
    

    [SerializeField] GameObject atkStart;
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
            GM.OpenPickPan();
            spawned = false;
        }
        
    }

    public void SpawnEnemy(int point, GameObject enmPrefab)
    {
        enmy enm = Instantiate(enmPrefab, enmSpawns[point].transform.position, enmSpawns[point].transform.rotation).GetComponent<enmy>();
        enms.Add(enm);
        enm.GetComponent<enmy>().SetThings(atkStart, atkEnd);
        spawned = true;
        recPos = point;
    }
    public void DamageEnemy(float damg, int target, List<WeaponEffect> effects)
    {
        if(enms.Count > target)
        {
            enms[target].damgEnemy(damg, effects);
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
            enms[0].SetTargetPointer(PlayerSA.equipedWeapon.strikePointer);
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
        

    }

    public void StartNextWave()
    {
        StartCoroutine(SpawnWave());
    }

    public void SetTargetEnmPointer(int num,Sprite pointer)
    {
        /*if (FindObjectOfType<StrikeArea>().equipedWeapon.effs[0] == WeaponEffect.bow)
        {
            enms[num].SetTargetPointer(pointer);
        }*/
        enms[num].SetTargetPointer(pointer);
    }
}
