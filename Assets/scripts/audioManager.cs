using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    public GameObject GameOver;
    public GameObject Pause;

    // refrences to the audio clips
    public AudioClip gameMusic;
    public AudioClip gameOverMusic;
    public AudioClip pauseMusic;
    
    // refrences to SFX clips
    public AudioClip minatourSFX;
    public AudioClip goblinSFX;
    public AudioClip one_eyeSFX;
    public AudioClip mooshroomSFX;
    public AudioClip skeletonSFX;


    private void Update()
    {   
        if(SceneManager.GetActiveScene().name == "game")
        {
            if (GameOver.activeSelf)
            {
                PlayMusic(gameOverMusic);
            }
            else if (Pause.activeSelf)
            {
                PlayMusic(pauseMusic);
            }
            else 
            {
                PlayMusic(gameMusic);
            }
        }
        
    }

    // play music
    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    // play SFX
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
