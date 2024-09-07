using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    private PlayerHealthBar playerHP;
    [SerializeField] WeaponEffect TrapEffect;
    [SerializeField] float damageFromTrap;
    private void Start()
    {
        playerHP = FindObjectOfType<PlayerHealthBar>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "strike point" && damageFromTrap>0)
        {
            //it is anti armor now
            if (TrapEffect == WeaponEffect.flame)
            {
                playerHP.DamagePlayer(null, 0, 8);
            }
            else
            { playerHP.DamagePlayer(null, damageFromTrap, 2); }
            Destroy(this.gameObject);
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

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<StrikeArea>().BeingBlocked(false);
    }
}

