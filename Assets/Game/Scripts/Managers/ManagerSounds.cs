using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS;
using Sirenix.OdinInspector;
using UnityEngine;
using ZeroX.SingletonSystem;

public enum MusicType
{
	Gameplay,
	Tournament,
}

public enum SoundType
{
	Explode,
	Shoot,
	Victory,
	Lose,
}

[System.Serializable] public struct MusicStruct
{
	public MusicType       musicType;
	public List<AudioClip> musicClips;
	
	public MusicStruct(MusicType musicType, List<AudioClip> musicClip)
	{
		this.musicType  = musicType;
		this.musicClips = musicClip;
	}
}

[System.Serializable] public struct SoundStruct
{
	public SoundType       soundType;
	public List<AudioClip> soundClips;
	
	public SoundStruct(SoundType soundType, List<AudioClip> soundClip)
	{
		this.soundType  = soundType;
		this.soundClips = soundClip;
	}
}

public class ManagerSounds : Singleton_ManualSpawn<ManagerSounds>
{
	#region Inspector Variables
	
	[SerializeField] [TableList] private List<MusicStruct> musicList;
	[SerializeField] [TableList] private List<SoundStruct> soundList;
	
	#endregion
	
	#region Member Variables
	
	private List<AudioSource> musicAudiosourceList = new();
	private List<AudioSource> soundAudiosourceList = new();
	private List<AudioClip>   musicClips           = new();
	private List<AudioClip>   soundClips           = new();
	private GameObject        musicObject;
	private GameObject        soundObject;
	private AudioClip         musicClip;
	private AudioClip         soundClip;
	
	#endregion
	
	#region Properties
	
	#endregion
	
	#region Unity Methods
	
	private void Awake()
	{
		CreateMusicAudioSource(5);
		CreateSoundAudioSource(10);
		GameEvents.OnSettingsChanged.SubscribeUntilDestroy(OnSettingsChanged, this);
	}
	
	private void Start()
	{
		PlayMusic(MusicType.Gameplay);
	}
	
	#endregion
	
	#region Public Methods
	
	public void PlaySound(SoundType soundType, bool isRandom = true, bool isLoop = false)
	{
		if (!ManagerData.SETTINGS_SOUND_ON)
		{
			return;
		}
		
		soundClips = soundList.FirstOrDefault(sound => sound.soundType == soundType).soundClips;
		
		if (soundClips != null && soundClips.Count != 0)
		{
			soundClip = soundClips[isRandom ? Random.Range(0, soundClips.Count) : 0];
		}
		
		var soundAudiosource = soundAudiosourceList.FirstOrDefault(audiosource => (!audiosource.clip));
		StartAudio(soundAudiosource, soundClip, isLoop);
	}
	
	public void PlayMusic(MusicType musicType, bool isRandom = true, bool isLoop = true)
	{
		if (!ManagerData.SETTINGS_SOUND_ON)
		{
			return;
		}
		
		musicClips = musicList.FirstOrDefault(music => music.musicType == musicType).musicClips;
		
		if (musicClips != null && musicClips.Count != 0)
		{
			musicClip = musicClips[isRandom ? Random.Range(0, musicClips.Count) : 0];
		}
		
		var musicAudiosource = musicAudiosourceList.FirstOrDefault(audiosource => (audiosource.clip == null));
		StartAudio(musicAudiosource, musicClip, isLoop);
	}
	
	public void StopSound(SoundType soundType)
	{
		AudioSource audioSource = soundAudiosourceList.FirstOrDefault(soundSource => GetSoundTypeFromClip(soundSource.clip) == soundType);
		StopAudio(audioSource);
	}
	
	public void StopMusic(MusicType musicType)
	{
		AudioSource audioSource = musicAudiosourceList.FirstOrDefault(musicSource => GetMusicTypeFromClip(musicSource.clip) == musicType);
		StopAudio(audioSource);
	}
	
	public void StopAllMusic()
	{
		musicAudiosourceList.ForEach(musicSource => StopAudio(musicSource));
	}
	
	public void StopAllSound()
	{
		soundAudiosourceList.ForEach(soundSource => StopAudio(soundSource));
	}
	
	#endregion
	
	#region Protected Methods
	
	#endregion
	
	#region Private Methods
	
	private void CreateMusicAudioSource(int count)
	{
		musicObject = new GameObject("[Music]");
		musicObject.transform.SetParent(transform);
		
		for (int i = 0; i < count; i++)
		{
			GameObject audioObject = new GameObject("[Audiosource]");
			audioObject.transform.SetParent(musicObject.transform);
			
			AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
			
			musicAudiosourceList.Add(newAudioSource);
		}
	}
	
	private void CreateSoundAudioSource(int count)
	{
		soundObject = new GameObject("[Sound]");
		soundObject.transform.SetParent(transform);
		
		for (int i = 0; i < count; i++)
		{
			GameObject audioObject = new GameObject("[Audiosource]");
			audioObject.transform.SetParent(soundObject.transform);
			
			AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
			
			soundAudiosourceList.Add(newAudioSource);
		}
	}
	
	private SoundType GetSoundTypeFromClip(AudioClip audioClip)
	{
		return soundList.FirstOrDefault(sound => sound.soundClips.FirstOrDefault(clip => clip == audioClip)).soundType;
	}
	
	private MusicType GetMusicTypeFromClip(AudioClip audioClip)
	{
		return musicList.FirstOrDefault(music => music.musicClips.FirstOrDefault(clip => clip == audioClip)).musicType;
	}
	
	private void StartAudio(AudioSource audioSource, AudioClip clip, bool loop)
	{
		audioSource.clip = clip;
		audioSource.loop = loop;
		audioSource.Play();
		
		if (!loop)
		{
			StartCoroutine(IEClearAudioSourceWhenFinished(audioSource));
		}
	}
	
	private void StopAudio(AudioSource audioSource)
	{
		if (audioSource)
		{
			audioSource.Stop();
			audioSource.clip = null;
			audioSource.loop = false;
		}
	}
	
	private IEnumerator IEClearAudioSourceWhenFinished(AudioSource audioSource)
	{
		yield return Yielder.Wait(audioSource.clip.length);
		StopAudio(audioSource);
	}
	
	private void OnSettingsChanged(bool isOn)
	{
		if (isOn)
		{
			PlayMusic(MusicType.Gameplay);
		}
		else
		{
			StopAllMusic();
			StopAllSound();
		}
	}
	
	#endregion
}