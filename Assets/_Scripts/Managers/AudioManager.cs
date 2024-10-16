using System.Collections.Generic;
using System.Linq;
using Data;
using Enums;
using Event_System;
using Scriptable_Objects;
using UnityEngine;
using Utility;

namespace Managers
{
	public class AudioManager : PersistentSingleton<AudioManager>
	{
		private const int initial_sfx_count = 5;
		[SerializeField]
		private AudioLibrarySO audioLibrarySO;
		[SerializeField]
		private AudioSource musicSource;
		[SerializeField]
		private GameObject audioSourcePrefab;
		private AudioClip[] musicClips;
		private SettingsData settingsData;
		private AudioClip[] sfxClips;
		private List<AudioSource> sfxSources;

		protected override void Awake()
		{
			base.Awake();
			settingsData = SaveManager.Instance.GetSettingsData();
			sfxSources = new List<AudioSource>();

			for (int i = 0; i < initial_sfx_count; i++)
			{
				GameObject newAudioSourceObject = Instantiate(audioSourcePrefab, transform);
				sfxSources.Add(newAudioSourceObject.GetComponent<AudioSource>());
			}

			musicClips = audioLibrarySO.MusicClips;
			sfxClips = audioLibrarySO.SFXClips;
			PlayMusic(MusicClips.BackgroundMusic);
		}

		private void OnEnable()
		{
			EventManager.Subscribe<SettingsChangedEventArgs>(HandleMusicToggle);
		}

		private void OnDisable()
		{
			EventManager.UnSubscribe<SettingsChangedEventArgs>(HandleMusicToggle);
		}

		private void HandleMusicToggle(SettingsChangedEventArgs settings)
		{
			settingsData = settings.settingsData;

			if (settingsData.IsMusicOn)
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
			if (!settingsData.IsMusicOn) return;
			if (musicSource.isPlaying) musicSource.Stop();
			musicSource.clip = musicClips[(int) clipEnum];
			musicSource.Play();
		}

		public AudioSource PlaySFX(SFXClips clipEnum, bool isLooping = false)
		{
			if (!settingsData.IsSFXOn)
			{
				return null;
			}

			AudioSource availableSource = sfxSources.FirstOrDefault(source => !source.isPlaying);

			// If there is no available AudioSource, create a new one
			if (availableSource == null)
			{
				GameObject newAudioSourceObject = Instantiate(audioSourcePrefab, transform);
				availableSource = newAudioSourceObject.GetComponent<AudioSource>();
				sfxSources.Add(availableSource);
			}

			availableSource.clip = sfxClips[(int) clipEnum];
			availableSource.loop = isLooping;

			if (isLooping)
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
}
