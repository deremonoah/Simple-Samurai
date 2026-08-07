using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public helpedWho RewardFrom;
    public virtual float CalculateScore()
    {
        Debug.LogError("called parent calculate score, doesn't do anything");
        return -1;
    }
}
public enum helpedWho { Farmer, Blacksmith, Sensei}