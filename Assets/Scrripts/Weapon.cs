using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite myStrikeArea,strikePointer, itemPanelIcon;
    public float baseDamg, maxDamg;
    
    public Effect eff;
}
public enum Effect { none, flame, greed, antiarmor }
