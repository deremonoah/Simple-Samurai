using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : Item
{
    public Sprite myStrikeArea,strikePointer;
    public float baseDamg, maxDamg;
    
    public Effect eff;
}
public enum Effect { none, flame, greed, antiarmor, odachi }
