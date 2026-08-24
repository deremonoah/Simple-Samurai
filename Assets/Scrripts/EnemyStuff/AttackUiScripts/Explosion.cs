using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float damage;//maybe calculate based on center compared to the pointer
    [SerializeField] float explosionRadius;

    [Header("explosion timing variables")]
    [SerializeField] float timeToExpand;
    [SerializeField] float timeToHoldExplotion;//for holding at fill size just for player to see it
    [SerializeField] float timeToFade;

    void Start()
    {
        StartCoroutine(ExplodeRoutine());
    }

    private IEnumerator ExplodeRoutine()
    {
        //play explosion sound
        //expand explosion
        Vector3 targetSize = new Vector3(explosionRadius, explosionRadius, 1);
        Vector3 startSize = new Vector3(.001f, .001f, 1);
        float timeElapsed=0;
        while(transform.localScale!=targetSize)
        {
            float t = timeElapsed / timeToExpand;

            transform.localScale = Vector3.Lerp(startSize, targetSize, t);

           timeElapsed += Time.deltaTime;
           yield return null;
        }

        yield return new WaitForSeconds(timeToHoldExplotion);

        //shrink or show fading smoke, might need to add a parent

        Destroy(transform.parent.gameObject);//this is for future proofing for adding not scalling visuals and a seperate parent object
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //find distance from center to player pointer, the closer the more damage
        GameObject other = collision.gameObject;
        if(other.name== "strike point")
        {
            float dis = Vector3.Distance(other.transform.position, this.transform.position);
            //anywhere from 1.5? for full radius vs 0~ compare to the radius
            damage = damage * (dis / explosionRadius);//50*(1/1.5)
            PlayerHealthBar playerHP = FindObjectOfType<PlayerHealthBar>();
            playerHP.DamagePlayer(null, damage, 2);//2 is anti armor
        }
    }
}
