using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    private PlayerHealthBar playerHP;
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
            playerHP.DamagePlayer(null, damageFromTrap, 2);
            Destroy(this.gameObject);
        }
    }
}

