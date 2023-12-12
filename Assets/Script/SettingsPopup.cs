using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    public Slider volSlider;
    public Button muteButton;
    public Button IntroButton;
    public Button LevelButton;
    public Button NoMusicButton;
    public Button ToggleMusic;
    public Slider musicVol;

    [SerializeField] AudioClip sound;

    private void Start()
    {
        volSlider.onValueChanged.AddListener(OnSoundValue);
        //muteButton.onClick.AddListener(OnSoundToggle);
        //IntroButton.onClick.AddListener(OnIntroMusic);
        //LevelButton.onClick.AddListener(OnLevelMusic);
        //NoMusicButton.onClick.AddListener(OnStopMusic);
        //ToggleMusic.onClick.AddListener(OnMusicToggle);
        musicVol.onValueChanged.AddListener(OnMusicValue);
    }
    public void OnSoundToggle()
    {
        Managers.Audio.soundMute = !Managers.Audio.soundMute;
        Managers.Audio.PlaySound(sound);
        if (Managers.Audio.soundMute)
        {
            Debug.Log("SOUND OFF!");
        }
        else
        {
            Debug.Log("SUND ON!");
        }
    }
    public void OnSoundValue(float volume)
    {
        Managers.Audio.soundVolume = volume;
        Debug.Log($"Sound Volume:{volume}");
    }

    public void OnPlayMusic(int selector)
    {
        Managers.Audio.PlaySound(sound);
        switch (selector)
        {
            case 1:
                Managers.Audio.PlayIntroMusic();
                break;
            case 2:
                Managers.Audio.PlayLevelMusic();
                break;
            default:
                Managers.Audio.StopMusic();
                break;
        }
    }

    public void OnIntroMusic()
    {
        OnPlayMusic(1);
    }

    public void OnLevelMusic()
    {
        OnPlayMusic(2);
    }

    public void OnStopMusic()
    {
        OnPlayMusic(3);
    }

    public void OnMusicToggle()
    {
        Managers.Audio.musicMute = !Managers.Audio.musicMute;
        Managers.Audio.PlaySound(sound);
    }
    public void OnMusicValue(float volume)
    {
        Managers.Audio.musicVolume = volume;
    }
}
