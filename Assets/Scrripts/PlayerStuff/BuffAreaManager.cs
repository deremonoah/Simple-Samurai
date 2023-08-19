using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class BuffAreaManager : MonoBehaviour
{
    [SerializeField] List<GameObject> BuffPrefabs;
    private List<GameObject> placedBuffs;
    private StrikePoint point;
    [SerializeField] GameObject buffParent;
    //order of prefabs: SwapWeapon, SwapEnemy, DamageUp, SpeedUp
    [SerializeField] float test;

    void Start()
    {
        point = FindObjectOfType<StrikePoint>();
        placedBuffs = new List<GameObject>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            PlaceBuff(0);
        }
    }

    public void PlaceBuff(int bufftype)
    {
        float pos = 0;
        switch (placedBuffs.Count)
        {
            case 0:
                pos = 2.4f;
                break;
            case 1:
                pos = 6.4f;
                break;
            case 2:
                pos = 10.6f;
                break;
            case 3:
                pos = 15f;
                break;

        }
            GameObject BuffArea = Instantiate(BuffPrefabs[bufftype], point.currentPath.path.GetPointAtDistance(pos), buffParent.transform.rotation);
            placedBuffs.Add(BuffArea);
            //trying 10 idk
            //figure out how to change the order in layer of the prefab so it under pointer

        //this also will need a list of placed ones if the style changes
        //}
        //instantiate at appropriate point then make them a child of parent
    }
}
