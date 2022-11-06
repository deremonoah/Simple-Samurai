using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    private PlayerHealthBar playerHP;
    private void Start()
    {
        playerHP = FindObjectOfType<PlayerHealthBar>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
            //maybe make it random range or antiarmor
            playerHP.DamagePlayer(5f,0);
            Destroy(this.gameObject);
        }
    }
}

