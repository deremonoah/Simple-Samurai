using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    private float aliveTimer = 12;
    private void Update()
    {
        if (aliveTimer <= 0)
        {
            Destroy(this.gameObject);
        }
        else { aliveTimer -= Time.deltaTime; }
    }
}
