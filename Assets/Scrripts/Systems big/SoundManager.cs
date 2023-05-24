using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip hit1, hit2, coin1, coin2, coin3, block1, smokebomb, upgrade, damageItem;
    private AudioSource myaudioSrc;
    private List<AudioClip> _senseiSounds, _yoinkSounds;
    

    [SerializeField] float Volume;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider _slider;
    void Start()
    {
        myaudioSrc = GetComponent<AudioSource>();
        hit1 = Resources.Load<AudioClip>("Sound/Effects/new Hit");
        hit2 = Resources.Load<AudioClip>("Sound/Effects/hit 2 wav");
        coin1 = Resources.Load<AudioClip>("Sound/Effects/coin wav 1");
        coin2 = Resources.Load<AudioClip>("Sound/Effects/coin wav 2");
        coin3 = Resources.Load<AudioClip>("Sound/Effects/coin wav 3");
        block1 = Resources.Load<AudioClip>("Sound/Effects/metal hit try 2");
        smokebomb = Resources.Load<AudioClip>("Sound/Effects/Smoke 1");
        upgrade = Resources.Load<AudioClip>("Sound/Effects/UpgradeSound");
        damageItem = Resources.Load<AudioClip>("Sound/Effects/Breaking Noise temp");

        _senseiSounds = new List<AudioClip>();
        _senseiSounds.Add(Resources.Load<AudioClip>("Sound/Sensei/sensei 1"));
        _senseiSounds.Add(Resources.Load<AudioClip>("Sound/Sensei/sensei 2"));
        _senseiSounds.Add(Resources.Load<AudioClip>("Sound/Sensei/sensei 3"));
        _senseiSounds.Add(Resources.Load<AudioClip>("Sound/Sensei/sensei 4"));
        _senseiSounds.Add(Resources.Load<AudioClip>("Sound/Sensei/sensei 5"));

        _yoinkSounds = new List<AudioClip>();
        _yoinkSounds.Add(Resources.Load<AudioClip>("Sound/Effects/Yoink 1"));
        _yoinkSounds.Add(Resources.Load<AudioClip>("Sound/Effects/Yoink 2"));

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
            case "sensei":
                rand = Random.Range(0, _senseiSounds.Count);
                myaudioSrc.PlayOneShot(_senseiSounds[rand]);
                break;
            case "yoink":
                rand = Random.Range(0, _yoinkSounds.Count);
                myaudioSrc.PlayOneShot(_yoinkSounds[rand]);
                break;
            case "smoke":
                myaudioSrc.PlayOneShot(smokebomb);
                break;
            case "upgrade":
                myaudioSrc.PlayOneShot(upgrade);
                break;
            case "breakItem":
                myaudioSrc.PlayOneShot(damageItem);
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

    /*private void Update()
    {
        _slider.onValueChanged.AddListener((v) +>{
            myaudioSrc.volume = (float)v;
        });
        myaudioSrc.volume = soundVolume;
    //fuck this shit why does it never just work!!!!!
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
    }*/

    

    public void SetSoundVolume()
    {
        myaudioSrc.volume = _slider.value;
    }
}
