using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    private float aliveTimer = 12;
    IEnumerator Start()
    {
        FindObjectOfType<SoundManager>().PlaySound("smoke");
        yield return new WaitForSeconds(aliveTimer);
        Destroy(this.gameObject);
    }
}
