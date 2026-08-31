using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //in future will use this for save data, but for now I just want a singleton that tracks timeScaleValue
    [SerializeField] float TimeScaleValue;
    [SerializeField] float IncrementOnBigDeath;
    public static SaveData instance;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else 
        { 
            if(instance!=this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public float getTimeScaleValue()//for making attacks move slower
    {
        return TimeScaleValue;
    }

    public float getTimeScaledEnemyWaitTimeValue()//for making enemies wait longer
    {
        float value = 1 * (TimeScaleValue * TimeScaleValue) - 3.5f * TimeScaleValue + 3.5f;
        Debug.Log(value);
        return value;
    }

    public void PlayerDied(int wave)
    {
        Debug.Log("called player died on wave "+wave);
        if(wave<3)
        {
            TimeScaleValue = Mathf.Clamp(TimeScaleValue - IncrementOnBigDeath, .5f, 1);
        }
    }

    //maybe later add in that the timeScale Might add up
}
