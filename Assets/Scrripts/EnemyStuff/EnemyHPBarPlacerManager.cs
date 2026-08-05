using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBarPlacerManager : MonoBehaviour
{
    public static EnemyHPBarPlacerManager instance;
    [Header("Weapon Specific HP spots")]
    [SerializeField] Transform SingleTargetWeapon;
    [SerializeField] List<Transform> BowSpots;
    [SerializeField] float BowXForwardOffset;
    [SerializeField] List<Transform> BlowGunSpots;
    [SerializeField] List<Transform> ShurikenSpots;
    [SerializeField] List<Transform> OdachiSpots;

    [Header("Enemy stuff")]
    [SerializeField] List<enemyStats> aliveEnemies= new();

    //private Image[] InUseBars = new Image[] { null, null, null, null };
    [Header("HP Bar stuff")]
    [SerializeField] List<Transform> UIPool = new List<Transform>();
    [SerializeField] Vector3 HideHere;

    [Header("Boss HP Bar stuff")]
    [SerializeField] HPBarImageHolder bossHPUI;

    private StrikeArea areaToChangeTarget;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Debug.LogError("you have 2 enemyHPBarPlacerManagers in the scene");
        }
        else
        {
            instance = this;
            areaToChangeTarget = FindObjectOfType<StrikeArea>();
        }
    }

    public void PlaceMyHPBar(enemyStats enm,int posInList)
    {
        aliveEnemies.Add(enm);//should work with the timing of spawns
                              //might need to set uipool newest item to enabled
        HPBarImageHolder barb;
        Image barToUse = null;
        if (enm.HasAbility(enemyStats.Ability.boss))
        {
            barb = bossHPUI;
            barToUse = barb.getHPBar();
            aliveEnemies[posInList].HPBarToMove = bossHPUI.gameObject.transform;
            bossHPUI.gameObject.SetActive(true);
        }
        else//non boss
        {
            barb = UIPool[0].gameObject.GetComponent<HPBarImageHolder>();
            barToUse = barb.getHPBar();
            aliveEnemies[posInList].HPBarToMove = UIPool[0];
            barb.getTransformToScale().localScale = new Vector3(1, 1, 1);
            barb.getTransformToScale().localScale = new Vector3(enm.maxHP / 150, 1, 1);
        }
            aliveEnemies[posInList].myHPBar = barToUse;
            //set refrences for hpbar
            
        //enm.PoisonText=barb.
        //remove hpbar from pool
        barb.setSprite(enm.gameObject.GetComponentInChildren<SpriteRenderer>().sprite);
        UIPool.RemoveAt(0);
        //set that one's image, the one on the child to be InUseBars


        HandleListChanged();
    }

    public void HealEnemy(float heal)
    {
        for(int lcv=0;lcv<aliveEnemies.Count;lcv++)
        {
            if (aliveEnemies[lcv].HP < aliveEnemies[lcv].maxHP)
            {
                aliveEnemies[lcv].healEnm(heal);
                aliveEnemies[lcv].myHPBar.fillAmount = aliveEnemies[lcv].getCurrentHP() / aliveEnemies[lcv].maxHP;
            }
        }
    }

    public void DamageEnemy(float damg, int target, List<WeaponEffect> effects)
    {
        if(target<aliveEnemies.Count)
        {
            enemyStats enm = aliveEnemies[target];
            enm.damageEnemy(damg, effects);
            enm.myHPBar.fillAmount = enm.getCurrentHP() / enm.maxHP;
        }
    }

    public void CycleEnemyList()//this should get filled in from enemysManager
    {

    }

    public void RemoveMeFromList(enemyStats enm)
    {
        aliveEnemies.Remove(enm);//take enemy off list
        var bar = enm.HPBarToMove;
        bar.position = HideHere;//move it off screen
        if(enm.HasAbility(enemyStats.Ability.boss))
        {
            bossHPUI.gameObject.SetActive(false);
        }
        UIPool.Add(bar);//add it back to the pool of ui
        bar.GetComponent<HPBarImageHolder>().getHPBar().fillAmount = 1;//and put its filll back to 100%
        HandleListChanged();
    }

    private void HandleListChanged()
    {
        if(aliveEnemies.Count<1)
        {
            return;
        }
        Weapon weapon = FindObjectOfType<PlayerEquipedItemsManager>().equipedWeapon;
        for (int lcv=0;lcv<aliveEnemies.Count;lcv++)
        {
            if (!aliveEnemies[lcv].HasAbility(enemyStats.Ability.boss))
            { PlaceBar(weapon, lcv); }
        }
    }

    private void PlaceBar(Weapon weapon, int posInList)
    {
        //set as default, as some weapons will change it in certain isntances
        var barpos =aliveEnemies[posInList].HPBarToMove;
        List<Transform> ListToUse=null;

        if (!weapon.hasEffect(WeaponEffect.multiTarget) && posInList==0)
        {
            barpos.position = SingleTargetWeapon.position;
        }
        else if (weapon.hasEffect(WeaponEffect.FourTarget))
        {
            
            if (weapon.name == "Blow Gun(Clone)")
            {
                ListToUse = BlowGunSpots;
            }
            else if (weapon.name == "Bow(Clone)")
            {
                ListToUse = BowSpots;
            }

            if (aliveEnemies.Count == 4 || posInList==2 || posInList==3)
            { 
                barpos.position = ListToUse[posInList].position;
                areaToChangeTarget.SetTarget(3);//more smarter people would put this in the strike area, but lazy me
            }
            else if(posInList==1)
            {
                if(aliveEnemies.Count==2)
                {
                    Vector3 PosToGo = Vector3.Lerp(ListToUse[1].position, ListToUse[3].position, 0.5f);
                    barpos.position = PosToGo;
                }
                else if(aliveEnemies.Count==3)
                {
                    barpos.position = ListToUse[3].position;
                }
            }
            else if (posInList == 0)
            {
                if(aliveEnemies.Count==1 || aliveEnemies.Count==3)
                {
                    Vector3 PosToGo = Vector3.Lerp(ListToUse[0].position, ListToUse[1].position, 0.5f);
                    barpos.position = PosToGo;
                }
                else if(aliveEnemies.Count ==2)
                {
                    Vector3 PosToGo = Vector3.Lerp(ListToUse[0].position, ListToUse[2].position, 0.5f);
                    barpos.position = PosToGo;
                    
                }
                //setting strike area to have right target number based on new positional system
                Debug.Log(aliveEnemies.Count);
                if(aliveEnemies.Count>1)
                {
                    areaToChangeTarget.SetTarget(1);
                }
                else
                { areaToChangeTarget.SetTarget(0); }//might change this to more kinds of strike areas not always showing 4 same size
            }
        }
        else if (weapon.hasEffect(WeaponEffect.shuriken) && posInList<ShurikenSpots.Count)
        {
            aliveEnemies[posInList].HPBarToMove.position = ShurikenSpots[posInList].position;
        }else if(weapon.hasEffect(WeaponEffect.odachi) &&posInList<3)
        {
            ListToUse = OdachiSpots;
            //do we need an if?
            barpos.position = OdachiSpots[posInList].position;
        }
        else
        {
            //if there is no spot to put the hp bar on ui, it should go onto the character
            barpos.position = aliveEnemies[posInList].backUpHPBarSpot.position;
        }
    }
}
