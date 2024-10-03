using System.Collections.Generic;
using System.Linq;
using Data;
using Managers;
using Rimaethon.Scripts.Utility;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioLibrary audioLibrary;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameObject audioSourcePrefab;
    private List<AudioSource> _sfxSources;
    private AudioClip[] _musicClips;
    private AudioClip[] _sfxClips;
    private int initialSfxCount = 5;
    private SettingsData _settingsData;

    private void OnEnable()
    {
        EventManager.RegisterHandler<OnSettingsChanged>(HandleMusicToggle);
    }

    private void OnDisable()
    {
        EventManager.UnregisterHandler<OnSettingsChanged>(HandleMusicToggle);
    }

    protected override void Awake()
    {
        base.Awake();
        _settingsData = SaveManager.Instance.GetSettingsData();
        _sfxSources = new List<AudioSource>();
        for (int i = 0; i < initialSfxCount; i++)
        {
            GameObject newAudioSourceObject = Instantiate(audioSourcePrefab, transform);
            _sfxSources.Add(newAudioSourceObject.GetComponent<AudioSource>());
        }
        _musicClips = audioLibrary.MusicClips;
        _sfxClips = audioLibrary.SFXClips;
        PlayMusic(MusicClips.BackgroundMusic);
    }

    private void HandleMusicToggle(OnSettingsChanged settings)
    {
        _settingsData = settings.settingsData;
        if (_settingsData.IsMusicOn)
        {
            PlayMusic(MusicClips.BackgroundMusic);
        }
        else
        {
            musicSource.Stop();
        }
    }

    private void PlayMusic(MusicClips clipEnum)
    {
        if (!_settingsData.IsMusicOn) return;
        if (musicSource.isPlaying) musicSource.Stop();
        musicSource.clip = _musicClips[(int)clipEnum];
        musicSource.Play();
    }

    public AudioSource PlaySFX(SFXClips clipEnum, bool isLooping = false)
    {
        if (!_settingsData.IsSFXOn) return null;


        AudioSource availableSource = _sfxSources.FirstOrDefault(source => !source.isPlaying);

        // If there is no available AudioSource, create a new one
        if (availableSource == null)
        {
            GameObject newAudioSourceObject = Instantiate(audioSourcePrefab, transform);
            availableSource = newAudioSourceObject.GetComponent<AudioSource>();
            _sfxSources.Add(availableSource);
        }
        availableSource.clip = _sfxClips[(int)clipEnum];
        availableSource.loop = isLooping;
        if(isLooping)
        {
            availableSource.Play();
        }
        else
        {
            availableSource.PlayOneShot(availableSource.clip);
        }
        return availableSource;
    }
}
