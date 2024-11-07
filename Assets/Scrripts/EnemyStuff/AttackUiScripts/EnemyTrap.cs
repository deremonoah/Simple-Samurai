using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    private PlayerHealthBar playerHP;
    [SerializeField] WeaponEffect TrapEffect;
    [SerializeField] float damageFromTrap;
    private bool PointerOnTrap;
    private void Start()
    {
        playerHP = FindObjectOfType<PlayerHealthBar>();
        PointerOnTrap = false;
    }

    private void Update()
    {
        if((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)))
        {
            //when the player releases the button they resolve teh effect
            if (PointerOnTrap)
            {
                if (TrapEffect == WeaponEffect.flame)
                {
                    playerHP.DamagePlayer(null, 0, 8);
                }
                else
                { playerHP.DamagePlayer(null, damageFromTrap, 2); }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("in trigger");
        if (other.name == "strike point" && damageFromTrap>0)
        {
               PointerOnTrap = true;
        }
        else
        {
            //for sumo block
            FindObjectOfType<StrikeArea>().BeingBlocked(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "strike point" && damageFromTrap == 0)
        {
            FindObjectOfType<StrikeArea>().BeingBlocked(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if its a block then it won't, I am realizing that these should probably be 2 scripts and inheerit from 1
        //and the blocking thing should only happen if its a thing to do the blocking, yes
        Debug.Log("exited");
        FindObjectOfType<StrikeArea>().BeingBlocked(false);
        PointerOnTrap = false;
    }
}

