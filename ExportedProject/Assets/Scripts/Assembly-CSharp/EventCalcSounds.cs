using UnityEngine;

[AddComponentMenu("Dark Tonic/Master Audio/Event Calc Sounds")]
public class EventCalcSounds : MonoBehaviour
{
	public const int FramesEarlyToTrigger = 2;

	public MasterAudio.SoundSpawnLocationMode soundSpawnMode = MasterAudio.SoundSpawnLocationMode.CallerLocation;

	public bool disableSounds;

	public AudioEvent audioSourceEndedSound;

	public bool useAudioSourceEndedSound;

	private AudioSource _audio;

	private Transform _trans;

	private void Awake()
	{
		_trans = base.transform;
		_audio = GetComponent<AudioSource>();
		SpawnedOrAwake();
	}

	protected virtual void SpawnedOrAwake()
	{
	}

	protected virtual void _AudioSourceEnded()
	{
		PlaySound(audioSourceEndedSound);
	}

	private void PlaySound(AudioEvent aEvent)
	{
		if (disableSounds)
		{
			return;
		}
		float volume = aEvent.volume;
		string soundType = aEvent.soundType;
		float? pitch = aEvent.pitch;
		if (!aEvent.useFixedPitch)
		{
			pitch = null;
		}
		PlaySoundResult playSoundResult = null;
		switch (soundSpawnMode)
		{
		case MasterAudio.SoundSpawnLocationMode.CallerLocation:
			if (aEvent.emitParticles)
			{
				playSoundResult = MasterAudio.PlaySound3DAtTransform(soundType, _trans, volume, pitch);
			}
			else
			{
				MasterAudio.PlaySound3DAtTransformAndForget(soundType, _trans, volume, pitch);
			}
			break;
		case MasterAudio.SoundSpawnLocationMode.AttachToCaller:
			if (aEvent.emitParticles)
			{
				playSoundResult = MasterAudio.PlaySound3DFollowTransform(soundType, _trans, volume, pitch);
			}
			else
			{
				MasterAudio.PlaySound3DFollowTransformAndForget(soundType, _trans, volume, pitch);
			}
			break;
		case MasterAudio.SoundSpawnLocationMode.MasterAudioLocation:
			if (aEvent.emitParticles)
			{
				playSoundResult = MasterAudio.PlaySound(soundType, volume);
			}
			else
			{
				MasterAudio.PlaySoundAndForget(soundType, volume);
			}
			break;
		}
		if (playSoundResult != null && playSoundResult.SoundPlayed)
		{
			MasterAudio.TriggerParticleEmission(_trans, aEvent.particleCountToEmit);
		}
	}

	private void Update()
	{
		CheckForEvents();
	}

	private void CheckForEvents()
	{
		if (useAudioSourceEndedSound && !(_audio == null) && !(_audio.clip == null) && _audio.clip.length - _audio.time < Time.deltaTime * 2f)
		{
			_audio.Stop();
			if (_audio.loop)
			{
				_audio.Play();
			}
			_AudioSourceEnded();
		}
	}
}
