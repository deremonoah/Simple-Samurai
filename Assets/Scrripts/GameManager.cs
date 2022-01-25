using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public GameObject lossPan;
    public GameObject pickPan;
    private EnemysSystem enmsSys;
    public Text TextCoins;
    private int playerCoins;

    [SerializeField] Image[] buttonImages;
    [SerializeField] List<Weapon> weaponList;
    private List<Weapon> randWeapons;
    
    void Start()
    {
        enmsSys = GetComponent<EnemysSystem>();
        playerCoins = 0;
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
        RandomItemPull();
    }
    public void ClosePickPan()
    {
        if(pickPan.GetComponent<Animator>().GetBool("Open"))
        {
            pickPan.GetComponent<Animator>().SetBool("Open", false);
            enmsSys.StartNextWave();
            Debug.Log("stated wave");
        }
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

    public void PayOut(int coin)
    {
        playerCoins += coin;
        TextCoins.text = playerCoins.ToString();
    }

    public void Button1()
    {
        
        ClosePickPan();
    }
    public void Button2()
    {

        ClosePickPan();
    }
    public void Button3()
    {

        ClosePickPan();
    }

    private void RandomItemPull()
    {
        //add to the randweapon list from list of weapons but can't repeat numbers so we could generate 3 random numbers
        //then we also should make sure the weapon doesn't match the one the player has and it could be an item so armor
        //or curio but the player can only have 1 of each weapon armor and curio that this should check if the random 
        //yeah pretty complicated we gonna start with just random numbers
        
    }
}
