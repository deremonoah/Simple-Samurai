using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class Armor : Item
{
    public float armor;
    public int eefNumOne, eefNumTwo;
    public ArmorEffect armrEef;
}
public enum ArmorEffect { none,turtle,greed,pheenix,sturdy,regen,spiked,crane,revive,beserker,leaf,scary }
/* turtle: while not readying a strike you are turtled up and are better protected
 * pheenix: internet is out idk how to spell, but you come back with half your health so like 50 then 25 then death so if more health or you get it back your okay maybe
 * sturdy: the sturdy ability in pokemon but its just when you would die you are on 1 hp
 * regen: slowly regen this could be kinda op so maybe a heal limit
 * spiked: when you are hit enemy takes damage
 * crane: this could give you those extra strike points at the end to hit the second and third enemy if using on of the appropriate weapons(katanas)
 * revive: the rising sun armor where you get one full revive
 * beserker:you get a bonus to base damage but have lower armor than these other armor sets
 * leaf on the wind: it allows you to fully dodge every fourth strike and your first maybe or something like that(have an effect on player to indicate swirling leaves around them)
 * scary: this could also go with a mask curio but it adds to the enemies initial timer
*/