using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDefense : MonoBehaviour
{
    //0= none 1=pit 2= palasade 3= spikes
    public int[] EquipedDefense;
    [SerializeField] int defenseSlotsLevel;
    [SerializeField] float _palisadeHPMax=40,_palisadeHP =40;
    [SerializeField] GameObject PalisadeUI;
    [SerializeField] Image fillPalisade;
    [SerializeField] List<Dragable> DefenseUIs;

    void Start()
    {
        EquipedDefense = new int[1];
        PalisadeUI.SetActive(false);
    }

    private void Update()
    {
        if (PalisadeUI.activeSelf)
        {
            fillPalisadeBar();
        }
    }
    private void fillPalisadeBar()
    {
        fillPalisade.fillAmount = _palisadeHP / _palisadeHPMax;
    }
    //below is a proto type to test stuff
    

    public bool isDefended()
    {
        for (int lcv = 0; lcv < EquipedDefense.Length; lcv++)
        {
            if (EquipedDefense[lcv] > 0)
            {
                return true;
            }
        }
        
        
            return false;
        
    }

    public void DefendPlayer(enemy enmy,float Damg)
    {
        //depending on which of the defenses is selected it will do a different thing the to enemy

        //spikes(3) deals damage to the enemy(does it stop their attack? and is it for a whole round or does it go away after 
        //an amount of damge?)

        //pit(1) stops the attack and stuns the enemy for a time (one time use or upgrade for multiple?)

        //palacade(2) extra hp bar will appear and tank some early damage(I would think it can stop multiple attacks)

        //this for loop will resolve the first defense that isn't 0 and then remove itself and exit the loop so it doesn't resolve multiple against one enemy
        for(int lcv =0; lcv<EquipedDefense.Length;lcv++)
        {
            //palacade
            if(EquipedDefense[lcv]==2)
            {
                _palisadeHP -= Damg;
                if (_palisadeHP <= 0)
                { EquipedDefense[lcv] = 0; PalisadeUI.SetActive(false); }
                break;
            }
            else if (EquipedDefense[lcv] == 1)
            {
                //enemy is basically stunned currently trying block but should add a stunned function
                EquipedDefense[lcv] = 0;
                enmy.Blocked();
                break;
            }
            else if(EquipedDefense[lcv]==3)
            {
                //deal damage to enemy then destroyed
                EquipedDefense[lcv] = 0;
                List<WeaponEffect> temp= new List<WeaponEffect>();
                temp.Add(WeaponEffect.none);
                enmy.damgEnemy(30, temp);
                break;
            }

        }


    }

    private void ChangeDefenseSlots(int amount)
    {
        //amount will either be positive or negative(if we unequip the curio)
        //would need to take gold unless its a curio effect(which would also need to be able to be undone)
        var temp = EquipedDefense;
        EquipedDefense = new int[amount];

        for (int lcv = 0; lcv < temp.Length; lcv++)
        {
            EquipedDefense[lcv] = temp[lcv];
        }
    }

    //this is the same as the above that I made later
    public void IncreaseSlotsButton()
    {
        int[] temp = new int[EquipedDefense.Length+1];
        for(int lcv = 0; lcv<EquipedDefense.Length; lcv++)
        {
            temp[lcv] = EquipedDefense[lcv];
        }
        EquipedDefense = temp;
    }

    public void ReadyDefense(int def)
    {
        //I am changing to a ui click and drag slot based system will just be more readable
        if(def == 2)/*Palisade*/
        {
            EquipedDefense[0] = 2;
            _palisadeHP = _palisadeHPMax;
            PalisadeUI.SetActive(true);
        }
        else if(def == 1)/*Pit*/
        {
            EquipedDefense[0] = 1;
            //enable ui for pit and diable others
            PalisadeUI.SetActive(false);
            //I will need to figure out how the defense works adding if there is only 1 slot stuff like that 
        }
        else if(def == 3)/*Spikes*/
        {
            EquipedDefense[0] = 3;
            PalisadeUI.SetActive(false);
            //enable ui which I will change to be setting the image not activating and deactivating one
        }
    }

    public int DefenseCosts()
    {
        int overallCost= 0;
        for(int lcv = 0; lcv<EquipedDefense.Length;lcv++)
        {
            overallCost += 5;
        }
        return overallCost;
    }

}
