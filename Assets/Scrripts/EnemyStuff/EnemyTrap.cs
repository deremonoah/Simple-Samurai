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
            //it is anti armor now
            playerHP.DamagePlayer(10f,2);
            Destroy(this.gameObject);
        }
    }
}

