using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPPointer : MonoBehaviour
{
    public Weapon theWeapon;
    private Sprite mySprite;
    private void FixedUpdate()
    {

        theWeapon = FindObjectOfType<GameManager>().mainStrkArea.equipedWeapon;
        mySprite = FindObjectOfType<GameManager>().mainStrkArea.equipedWeapon.strikePointer;
        GetComponent<SpriteRenderer>().sprite = mySprite;
        
        
    }
}
