using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnmyHPsSystem : MonoBehaviour
{
    public GameObject[] enmysHPSpots;
    public GameObject[] enmyHPs;
    public Image HPbarPrefab;
    public Image HPBackPrefab;
    public Canvas cnvs;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public Image GetMyHPBar(int pos)
    {
        Image img;
        //Image bac;
        //background of hp
        //bac = Instantiate(HPBackPrefab, enmysHPSpots[pos].transform.position, enmysHPSpots[pos].transform.rotation);
        //bac.transform.SetParent(cnvs.transform);
        img = Instantiate(HPbarPrefab, enmysHPSpots[pos].transform.position,enmysHPSpots[pos].transform.rotation);
        img.transform.SetParent(cnvs.transform);
        return img;
    }
}
