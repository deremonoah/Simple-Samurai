using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : Item
{
    public float armor;
    public ArmorEffect armrEef;
}
public enum ArmorEffect { none,turtle,greed,revive }
