using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{
    //private lists and load resources or public sprites?
    [SerializeField] List<Sprite> BowPointers;
    [SerializeField] List<Sprite> ShurikenPointers;
    [SerializeField] List<Sprite> BlowPointers;
    [SerializeField] List<Sprite> OdachiPointers;
    
    void Start()
    {
        //if private we need to load the resources
    }

    //how is the system comunicated with? told when and given info by the enemy manager
    //then it based on equiped weapon from player tells the enemies what to do?

    //what need to change about enemies
    //change the name of bow pointers to skinny pointers
    //shuriken likley will need different placement for clarity
    //draw different color pointers for shuriken and darts

    public void UpdatePointers(List<enemy> enms)
    {
        //figure out which sprite list to use
        //or change weapons to have a list of pointers, everywhere they use the first its just [0]
    }


    /*this is how we were doing it before in enemy manager
     * 
     * private void SetSpecialPointers()
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

    public void SetMultiplePointers(List<sprites> currentSpriteList,List<enemy> aliveEnemys)
    {
        
        foreach (enemy enm in aliveEnemys)
        {
            foreach (GameObject pointer in enm.BowPointers)
            {
                pointer.SetActive(false);
            }
        }

        int enmIndex = 0;
        for (int PointerIndex = 0; PointerIndex < PointersList.Count; PointerIndex++)
        {
            enmIndex++;
            if (enmIndex >= aliveEnemys.Count)
            {
                enmIndex = 0;
            }

            var pointer = aliveEnemys[enmIndex].BowPointers[PointerIndex].SetActive(true);
            //will it work with.set active at the end?
            pointer.sprite=currrentSpriteList[PointerIndex];

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
     * 
     * 
     */
}
