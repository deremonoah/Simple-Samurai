using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : Item
{
    public Sprite myStrikeArea,StrikePoint;
    public List<Sprite> PointersList;
    public List<float> baseDamageLevel, maxDamageLevel;
    public List<WeaponEffect> effs;
    public float[] damageMults= new float[6];
}
public enum WeaponEffect { none, flame, greed, antiarmor, odachi, bow, lifeSteal, poison, frost, sasumata, shuriken,multiTarget, ThreeTarget, FourTarget }
