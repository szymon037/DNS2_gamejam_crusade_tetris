using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip CarSound;
    public AudioClip PickUpSound;
    public AudioClip BlockSound;
    public AudioClip GameOverSound;
    public AudioClip[] Music;
    public AudioClip MenuMusic;
    public AudioSource[] Source;

    private int CurrentTrack = 0;
    private bool isMenuMusicPlaying = false, isPlayingMusic = true;
    private float DeltaTime = 0.0f, TrackTime;

    public void PlayPickUpSound()
    {
        Source[0].PlayOneShot(PickUpSound);
    }

    public void PlayBlockSound()
    {
        Source[0].PlayOneShot(BlockSound);
        isPlayingMusic = true;
    }

    public void PlayGameOverSound()
    {
        Source[0].Stop();
        Source[0].PlayOneShot(GameOverSound);
        isPlayingMusic = true;
    }

    public void StopMusic()
    {
        isPlayingMusic = false;
        Source[1].Stop();
    }

    public void SwitchToMenuMusic()
    {
        Source[1].Stop();
        Source[1].PlayOneShot(MenuMusic);
        isMenuMusicPlaying = true;
        //Just in case
        CurrentTrack = 0;
    }

    public void SwitchToGameMusic()
    {
        isMenuMusicPlaying = false;
        Source[1].Stop();
        Source[1].PlayOneShot(Music[CurrentTrack]);
    }

    public void Init()
    {
        Source[0].loop = true;
        Source[0].playOnAwake = true;
        Source[0].clip = CarSound;

        Source[0].Play();

        isPlayingMusic = true;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if(isPlayingMusic)
        {
            TrackTime = (isMenuMusicPlaying ? MenuMusic.length : Music[CurrentTrack].length);

            if (DeltaTime > TrackTime)
            {
                CurrentTrack = (isMenuMusicPlaying ? 0 : (CurrentTrack + 1) % Music.Length);
                Source[1].PlayOneShot(isMenuMusicPlaying ? MenuMusic : Music[CurrentTrack]);
                DeltaTime = 0.0f;
            }
            else
            {
                DeltaTime += Time.deltaTime;
            }
        }
    }
}
