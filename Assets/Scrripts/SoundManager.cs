using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip hit1, hit2, coin1, coin2, coin3;
    public AudioSource audioSrc;

    void Start()
    {
        hit1 = Resources.Load<AudioClip>("hit 1 wav");
        hit2 = Resources.Load<AudioClip>("hit 2 wav");
        coin1 = Resources.Load<AudioClip>("coin wav 1");
        coin2 = Resources.Load<AudioClip>("coin wav 2");
        coin3 = Resources.Load<AudioClip>("coin wav 3");

    }

    
    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "hit":
                audioSrc.PlayOneShot(hit1);
                break;
            case "coin":
                audioSrc.PlayOneShot(coin1);
                break;
        }
    }
}
