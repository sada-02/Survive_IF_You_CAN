using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudio : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    public AudioClip mainMenuMusic;

    private void Start()
    {
        PlayMusic();
    }

    private void PlayMusic()
    {   
        musicSource.clip = mainMenuMusic;
        musicSource.loop = true; 
        musicSource.Play();
    }
}
