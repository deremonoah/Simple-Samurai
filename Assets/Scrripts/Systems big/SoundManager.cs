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
        block1 = Resources.Load<AudioClip>("metal hit try 2");
    }

    
    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "hit":
                myaudioSrc.PlayOneShot(hit1);
                break;
            case "coin":
                myaudioSrc.PlayOneShot(coin1);
                break;
            case "block":
                myaudioSrc.PlayOneShot(block1);
                break;
        }
    }
}
