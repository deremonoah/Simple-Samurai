using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] Slider _slider;
    private float musicVolume;
    [SerializeField] AudioSource musicSource;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }

    public void SetMusicVolume()
    {
        musicSource.volume = _slider.value;
    }
}
