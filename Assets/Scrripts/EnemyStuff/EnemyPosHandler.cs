using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosHandler : MonoBehaviour
{
    // This is meant to handle keeping the enemy in the right pos & update with footwork
    [SerializeField] private int posInList;
    private Vector3 posToReturnTo;
    private enemyStats myStats;

    private void Start()
    {
        myStats = GetComponent<enemyStats>();
        EnemyHPBarPlacerManager.instance.PlaceMyHPBar(myStats, posInList);
    }

    public void SetPosVariables(int index,Vector3 pos)//called for spawning enemies in
    {
        posInList = index;
        posToReturnTo = pos;
        //start coroutine to lerp only x pos over to the right spot
    }

    public void UpdatePosVariables(int index,Vector3 pos)
    {
        posInList = index;
        posToReturnTo = pos;
        EnemyHPBarPlacerManager.instance.PlaceMyHPBar(myStats, posInList);
    }

    public Vector3 getPosToReturnTo()//called by enemy behavior to know where to go back to
    {
        return posToReturnTo;
    }
}
