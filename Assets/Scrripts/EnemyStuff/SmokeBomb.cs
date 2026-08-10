using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    private float aliveTimer = 12;
    IEnumerator Start()
    {
        List<List<Transform>> temp = EnemysManager.instance.GetTrapSpawnSpots();
        int rand = Random.Range(0, temp[1].Count);
        List<Transform> smokeSpots = temp[1];
        this.transform.position = smokeSpots[rand].position;

        FindObjectOfType<SoundManager>().PlaySound("smoke");
        yield return new WaitForSeconds(aliveTimer);
        Destroy(this.gameObject);
    }
}
