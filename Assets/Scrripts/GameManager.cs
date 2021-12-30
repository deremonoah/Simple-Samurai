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
        pickPan.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ClosePickPan()
    {
        pickPan.SetActive(false);
        Time.timeScale = 1f;
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
