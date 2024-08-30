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
    private PlayerEquipedItemsManager EqM;
    private EnemysManager EnmM;
    void Start()
    {
        //if private we need to load the resources
        EqM = FindObjectOfType<PlayerEquipedItemsManager>();
        EnmM = FindObjectOfType<EnemysManager>();
    }

    //how is the system comunicated with? told when and given info by the enemy manager
    //then it based on equiped weapon from player tells the enemies what to do?

    //what need to change about enemies
    //change the name of bow pointers to extra pointers
    //shuriken likley will need different placement for clarity
    //draw different color pointers for shuriken and darts


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

    public void UpdateAliveEnmsPointers(List<enemy> aliveEnms)
    {
        //check if list has at least 1 dude!
        if (aliveEnms.Count < 1) { return; }
        //This is for the individual Asking to get updated
        List<Sprite> pointersToGive=new List<Sprite>();
        Weapon we = EqM.equipedWeapon;
        pointersToGive = we.PointersList;
        int enmCount=0;
        List<List<Sprite>> ListOfEnemySprites = new List<List<Sprite>>();
        bool dont = false;

        for (int lcv = 0; lcv < aliveEnms.Count; lcv++)
        {
            ListOfEnemySprites.Add(new List<Sprite>());
        }
        //pcv is pointer control variable to go through list of Pointers
        for(int lcv=0;lcv<we.effs.Count;lcv++)
        {
            if(we.effs[lcv]==WeaponEffect.odachi)
            {
                //odachi special because if 2 enemies they both get the 1st
                //if 3 1st gets 1st then goes in order
                if(aliveEnms.Count<=2)
                {
                    ListOfEnemySprites[0].Add(pointersToGive[0]);
                    if (aliveEnms.Count == 2)
                    { ListOfEnemySprites[1].Add(pointersToGive[0]); }

                    dont = true;
                }
            }
        }
        //this if is to not repeat code
        if (!dont)
        {
            //every other pointer adding situation and hands out pointers like candy
            for (int pcv = 0; pcv < pointersToGive.Count; pcv++)
            {
                //assign Enm[enmCount] pcvTh sprite
                ListOfEnemySprites[enmCount].Add(pointersToGive[pcv]);
                enmCount++;
                if (enmCount >= aliveEnms.Count)
                {
                    enmCount = 0;
                }

            }
        }
        //Actually Telling the enemies what their list is
        for(int lcv=0;lcv<aliveEnms.Count;lcv++)
        {
            //catch if list is empty don't tell enemy
            if (ListOfEnemySprites[lcv].Count > 0)
            {
                aliveEnms[lcv].SetTargetPointers(ListOfEnemySprites[lcv]);
            }
        }
    }
}
