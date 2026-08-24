using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombTrap : EnemyTrap
{
    [Header("bomb variables")]
    [SerializeField] float timeUntilExplode;
    private float timer;
    [SerializeField] GameObject explosionPrefab;//which is what does the damage
    [SerializeField] TextMeshProUGUI countDownText;

    protected override void EffectOnStart()
    {
        StartCoroutine(CountDownRoutine());
    }

    //we don't need to change resolveStrike trap effect as it doesn't hurt player
    //just gets destroyed if cut

    IEnumerator CountDownRoutine()
    {
        timer = timeUntilExplode;
        while(timer>0)
        {
            timer -= Time.deltaTime;

            countDownText.text = Mathf.Round(timer) + "";

            yield return null;
        }

        Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
