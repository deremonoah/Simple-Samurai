using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player plr;
    public GameObject lossPan;
    public GameObject pickPan;
    private EnemysSystem enmsSys;

    void Start()
    {
        plr = GetComponent<Player>();
        enmsSys = GetComponent<EnemysSystem>();
    }


    void Update()
    {
        /*if (enmsSys.hasAllDied()<1)
        {
            OpenPickPan();
        }*/
    }
    public void OpenPickPan()
    {
        pickPan.GetComponent<Animator>().SetBool("Open", true);
        
    }
    public void ClosePickPan()
    {
        pickPan.GetComponent<Animator>().SetBool("Open", false);
        
        enmsSys.StartNextWave();
    }


    public void OpenLossPan()
    {
        lossPan.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseLossPan()
    {
        lossPan.SetActive(false);
        Time.timeScale = 1f;
    }
}
