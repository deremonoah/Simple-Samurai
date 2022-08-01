using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Curio", menuName = "Curio")]
public class Curio : Item
{
    public CurioEffect curiEef;
    public int CurioNum;
}
public enum CurioEffect { Koban,healing,heal,incrArea,revive,greed }