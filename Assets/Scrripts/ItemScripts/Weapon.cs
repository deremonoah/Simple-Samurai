using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : Item
{
    public Sprite myStrikeArea,strikePointer;
    public float baseDamg, maxDamg;
    
    public List<WeaponEffect> effs;
}
public enum WeaponEffect { none, flame, greed, antiarmor, odachi, bow }
