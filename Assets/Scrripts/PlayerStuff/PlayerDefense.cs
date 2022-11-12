using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : MonoBehaviour
{
    //0= none 1=pit 2= palasade 3= spikes
    private int[] EquipedDefense;
    [SerializeField] int defenseSlotsLevel;

    void Start()
    {
        EquipedDefense = new int[1];
    }

    public bool isDefended()
    {
        if (EquipedDefense[0] < 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DefendPlayer(enemy enmy)
    {
        //depending on which of the defenses is selected it will do a different thing the to enemy

        //spikes deals damage to the enemy(does it stop their attack? and is it for a whole round or does it go away after 
        //an amount of damge?)

        //pit stops the attack and stuns the enemy for a time (one time use or upgrade for multiple?)

        //palacade extra hp bar will appear and tank some early damage(I would think it can stop multiple attacks)
    }

    private void ChangeDefenseSlots(int amount)
    {
        //amount will either be positive or negative(if we unequip the curio)
        //would need to take gold unless its a curio effect(which would also need to be able to be undone)
        var temp = EquipedDefense;
        EquipedDefense = new int[EquipedDefense.Length+1];

        for (int lcv = 0; lcv < temp.Length; lcv++)
        {
            EquipedDefense[lcv] = temp[lcv];
        }
    }

    public void IncreaseSlotsButton()
    {

    }
}
