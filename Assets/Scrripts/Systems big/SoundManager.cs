using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip hit1, hit2, coin1, coin2, coin3,block1;
    private AudioSource myaudioSrc;

    void Start()
    {
        myaudioSrc = GetComponent<AudioSource>();
        hit1 = Resources.Load<AudioClip>("Sound/Effects/new Hit");
        hit2 = Resources.Load<AudioClip>("Sound/Effects/hit 2 wav");
        coin1 = Resources.Load<AudioClip>("Sound/Effects/coin wav 1");
        coin2 = Resources.Load<AudioClip>("Sound/Effects/coin wav 2");
        coin3 = Resources.Load<AudioClip>("Sound/Effects/coin wav 3");
        block1 = Resources.Load<AudioClip>("Sound/Effects/metal hit try 2");
    }

    
    public void PlaySound(string clip)
    {
        int rand = Random.Range(0, 2);
        switch (clip)
        {
            case "hit":
                if (rand == 0) { myaudioSrc.PlayOneShot(hit1); }
                else if(rand == 1) { myaudioSrc.PlayOneShot(hit2); }
                break;

            case "coin":
                rand = Random.Range(0, 3);
                if (rand == 0) { myaudioSrc.PlayOneShot(coin1); }
                else if (rand == 1) { myaudioSrc.PlayOneShot(coin2); }
                else if(rand == 2) { myaudioSrc.PlayOneShot(coin3); }
                break;

            case "block":
                myaudioSrc.PlayOneShot(block1);
                break;
        }
    }
    public void PlaySound(string clip,float value)
    {
        switch (clip)
        {
            case "hit":
                //checks the value which is the damage to out put a more fitting damage sound
                if (value <= 10) { myaudioSrc.PlayOneShot(hit2); }
                else { myaudioSrc.PlayOneShot(hit1); }
                break;
        }
    }
}
