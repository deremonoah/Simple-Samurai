using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Curio", menuName = "Curio")]
public class Curio : Item
{
    public CurioEffect curiEef;
    public int CurioNum;
    public List<int> CurioValueLevel;
}
public enum CurioEffect { Koban,healing,heal,healOnGo,incrArea,revive,greed,quick,addedPoison,defenses,twoCurio,foresight }
/*healing passive heal eh maybe not if there is an armor one
 * healOnGo: the rice cake when your hp drops below half you heal 50 automatically
 * heal: on pickup it heals you
 * incrArea: im not sure if this is supposed to be increase strike area?
 * revive: also was gonna be an armor so maybe not or maybe you can double up
 * quick:quick strike guys hat increases pointer speed
 * greed: probably just the beconing cat
 * addedPoison: makes your weapon also do the poison effect when you strike an enemy
 * defenses: shovel of supperior defenses: it gives you a free extra defense slot so if fully upgraded you get 4 slots
 * twoCurio: allows you to have 2 curio on pickup you should keep the one you have might be just the best option, so maybe a downside
 * like if you take enough damage you loose the curio and one of your other 2 at random
 * scary kabudo mask: if forget if its a kabudo the masks samurai wear yeah same as the armor adds initial wait timer
 * foresight: it allows you to see what enemies are coming next round if the defenses can have a tower it will have a similar effect
 * something that increases the maximum time an enemy can be stunned for which is as easy as changing the enemy 7.5 in stunned to a variable that can be set but would it update if changed well I guess it could be static
*/