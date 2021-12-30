using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave",menuName ="Wave")]
public class EnmWave : ScriptableObject
{
    public enmy[] enmsInWave;

    public enmy getEnm(int lcv)
    {
        return enmsInWave[lcv];
    }
    
    public int getLength()
    {
        return enmsInWave.Length;
    }

    public bool hasThere(int lcv)
    {
        if (enmsInWave[lcv] == null)
        {
            return false;
        }
        else return true;
    }
}
