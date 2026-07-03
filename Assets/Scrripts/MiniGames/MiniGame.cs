using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public virtual float CalculateScore()
    {
        Debug.LogError("called parent calculate score, doesn't do anything");
        return -1;
    }
}
