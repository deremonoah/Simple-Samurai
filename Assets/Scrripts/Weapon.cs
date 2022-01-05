using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite myStrikeArea,strikePointer,strikeHPPointer;
    public float baseDamg, maxDamg;
}
