using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    private NetworkService network;

    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource music1Source;
    [SerializeField] AudioSource music2Source;

    private AudioSource activeMusic;
    private AudioSource inactiveMusic;

    public float crossFadeRate = 1.5f;
    private bool crossFading;

    [SerializeField] string introBGMusic;
    [SerializeField] string levelBGMusic;

    public float soundVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }
    public bool soundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    private float _musicVolume;
    public float musicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            _musicVolume = value;
            if (music1Source != null)
            {
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume;
            }
        }
    }
    public bool musicMute
    {
        get
        {
            if (music1Source != null)
            {
                return music1Source.mute;
            }
            return false;
        }
        set
        {
            if (music1Source != null)
            {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }

    // Add volume controls here (listing 11.4)
    public void Startup(NetworkService service)
    {
        Debug.Log("Audio manager starting...");
        network = service;

        music1Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerPause = true;

        soundVolume = 1f;
        musicVolume = 1f;


        activeMusic = music1Source;
        inactiveMusic = music2Source;

        // Initialize music sources here (listing 11.11)
        status = ManagerStatus.Started;
    }
    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void PlayIntroMusic()
    {
        PlayMusic(Resources.Load($"Music/{introBGMusic}") as AudioClip);
    }
    public void PlayLevelMusic()
    {
        PlayMusic(Resources.Load($"Music/{levelBGMusic}") as AudioClip);
    }
    private void PlayMusic(AudioClip clip)
    {
        if (crossFading) { return; }
        StartCoroutine(CrossFadeMusic(clip));
    }

    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        crossFading = true;
        inactiveMusic.clip = clip;
        inactiveMusic.volume = 0;
        inactiveMusic.Play();
        float scaledRate = crossFadeRate * musicVolume;
        while (activeMusic.volume > 0)
        {
            activeMusic.volume -= scaledRate * Time.deltaTime;
            inactiveMusic.volume += scaledRate * Time.deltaTime;
            yield return null;
        }
        AudioSource temp = activeMusic;
        activeMusic = inactiveMusic;
        activeMusic.volume = musicVolume;
        inactiveMusic = temp;
        inactiveMusic.Stop();
        crossFading = false;
    }
    public void StopMusic()
    {
        activeMusic.Stop();
        inactiveMusic.Stop();
    }

}
