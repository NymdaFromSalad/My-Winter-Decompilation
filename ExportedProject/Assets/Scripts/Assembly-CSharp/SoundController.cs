using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	public AudioClip engineThrottle;

	public float engineThrottleVolume = 0.35f;

	public float engineThrottlePitchFactor = 1f;

	public AudioClip engineNoThrottle;

	public float engineNoThrottleVolume = 0.35f;

	public float engineNoThrottlePitchFactor = 1f;

	public AudioClip startEngine;

	public float startEngineVolume = 1f;

	public float startEnginePitch = 1f;

	public GameObject enginePosition;

	public AudioClip transmission;

	public float transmissionVolume = 0.5f;

	public float transmissionVolumeReverse = 0.5f;

	public float transmissionSourcePitch = 1f;

	public AudioClip brakeNoise;

	public float brakeNoiseVolume = 0.2f;

	public AudioClip skid;

	public float skidVolume = 1f;

	public float skidPitchFactor = 1f;

	public AudioClip crashHiSpeed;

	public float crashHighVolume = 0.75f;

	public AudioClip crashLowSpeed;

	public float crashLowVolume = 0.7f;

	public AudioClip scrapeNoise;

	public float scrapeNoiseVolume = 1f;

	public AudioClip ABSTrigger;

	public float ABSTriggerVolume = 0.2f;

	public AudioClip shiftTrigger;

	public float shiftTriggerVolume = 1f;

	public AudioClip wind;

	public float windVolume = 1f;

	public AudioClip rollingNoiseGrass;

	public AudioClip rollingNoiseSand;

	public AudioClip rollingNoiseOffroad;

	private AudioSource engineThrottleSource;

	private AudioSource engineNoThrottleSource;

	private AudioSource startEngineSource;

	private AudioSource transmissionSource;

	private AudioSource brakeNoiseSource;

	private AudioSource[] skidSource;

	private AudioSource crashHiSpeedSource;

	private AudioSource crashLowSpeedSource;

	private AudioSource scrapeNoiseSource;

	private AudioSource ABSTriggerSource;

	private AudioSource shiftTriggerSource;

	private AudioSource windSource;

	private AudioSource[] rollingNoiseSource;

	[HideInInspector]
	public CarController carController;

	private CarDynamics cardynamics;

	private Drivetrain drivetrain;

	private Axles axles;

	private bool rimScraping;

	private float volumeFactor;

	private int i;

	private int k;

	private AudioSource CreateAudioSource(AudioClip clip, bool loop, bool playImmediately, Vector3 position)
	{
		GameObject gameObject = new GameObject("audio");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = position;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.AddComponent(typeof(AudioSource));
		gameObject.GetComponent<AudioSource>().clip = clip;
		gameObject.GetComponent<AudioSource>().playOnAwake = false;
		if (loop)
		{
			gameObject.GetComponent<AudioSource>().volume = 0f;
			gameObject.GetComponent<AudioSource>().loop = true;
		}
		else
		{
			gameObject.GetComponent<AudioSource>().loop = false;
		}
		if (playImmediately)
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
		gameObject.GetComponent<AudioSource>().spatialBlend = 1f;
		return gameObject.GetComponent<AudioSource>();
	}

	private void Start()
	{
		carController = GetComponent<CarController>();
		cardynamics = GetComponent<CarDynamics>();
		drivetrain = GetComponent<Drivetrain>();
		axles = GetComponent<Axles>();
		Vector3 position = Vector3.zero;
		if (enginePosition != null)
		{
			position = enginePosition.transform.position;
		}
		engineThrottleSource = CreateAudioSource(engineThrottle, true, true, position);
		engineNoThrottleSource = CreateAudioSource(engineNoThrottle, true, true, position);
		transmissionSource = CreateAudioSource(transmission, true, true, Vector3.zero);
		brakeNoiseSource = CreateAudioSource(brakeNoise, true, true, Vector3.zero);
		startEngineSource = CreateAudioSource(startEngine, true, false, position);
		startEngineSource.volume = startEngineVolume;
		startEngineSource.pitch = startEnginePitch;
		Array.Resize(ref skidSource, axles.allWheels.Length);
		this.i = 0;
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			skidSource[this.i] = CreateAudioSource(skid, true, true, wheel.transform.localPosition);
			this.i++;
		}
		crashHiSpeedSource = CreateAudioSource(crashHiSpeed, false, false, Vector3.zero);
		crashLowSpeedSource = CreateAudioSource(crashLowSpeed, false, false, Vector3.zero);
		scrapeNoiseSource = CreateAudioSource(scrapeNoise, false, false, Vector3.zero);
		ABSTriggerSource = CreateAudioSource(ABSTrigger, false, false, Vector3.zero);
		ABSTriggerSource.volume = ABSTriggerVolume;
		shiftTriggerSource = CreateAudioSource(shiftTrigger, false, false, Vector3.zero);
		shiftTriggerSource.volume = shiftTriggerVolume;
		windSource = CreateAudioSource(wind, true, true, Vector3.zero);
		Array.Resize(ref rollingNoiseSource, axles.allWheels.Length);
		this.i = 0;
		Wheel[] allWheels2 = axles.allWheels;
		foreach (Wheel wheel2 in allWheels2)
		{
			rollingNoiseSource[this.i] = CreateAudioSource(rollingNoiseGrass, true, false, wheel2.transform.localPosition);
			this.i++;
		}
	}

	private void Update()
	{
		if (carController != null)
		{
			if (carController.ABSTriggered)
			{
				ABSTriggerSource.PlayOneShot(ABSTrigger);
			}
			brakeNoiseSource.volume = Mathf.Clamp01(carController.brake * Mathf.Abs(AverageWheelVelo()) * 0.1f) * brakeNoiseVolume;
		}
		if (!(drivetrain != null))
		{
			return;
		}
		if (drivetrain.startEngine && drivetrain.rpm < drivetrain.minRPM)
		{
			if (!startEngineSource.isPlaying)
			{
				startEngineSource.Play();
			}
		}
		else
		{
			startEngineSource.Stop();
		}
	}

	private float AverageWheelVelo()
	{
		float num = 0f;
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			num += wheel.wheelTireVelo;
		}
		return num / (float)axles.allWheels.Length;
	}

	private void FixedUpdate()
	{
		if (drivetrain != null && drivetrain.shiftTriggered)
		{
			shiftTriggerSource.PlayOneShot(shiftTrigger);
			drivetrain.shiftTriggered = false;
			drivetrain.soundPlayed = true;
		}
		if (Application.isEditor)
		{
			if (shiftTriggerSource != null)
			{
				shiftTriggerSource.volume = shiftTriggerVolume;
			}
			if (ABSTriggerSource != null)
			{
				ABSTriggerSource.volume = ABSTriggerVolume;
			}
		}
		if ((bool)drivetrain)
		{
			float num = drivetrain.rpm / drivetrain.maxRPM;
			engineNoThrottleSource.volume = Mathf.Clamp01((1f - drivetrain.throttle) * engineNoThrottleVolume * num);
			engineNoThrottleSource.pitch = 0.5f + engineNoThrottlePitchFactor * num;
			engineThrottleSource.volume = Mathf.Clamp01(drivetrain.throttle * engineThrottleVolume * num + engineThrottleVolume * 0.2f * num);
			engineThrottleSource.pitch = 0.5f + engineThrottlePitchFactor * num;
			if (drivetrain.clutch != null)
			{
				if (drivetrain.clutch.GetClutchPosition() != 0f && drivetrain.ratio != 0f)
				{
					float num2 = Mathf.Abs(drivetrain.differentialSpeed * 0.01f);
					float num3 = Mathf.Abs(drivetrain.ratio / drivetrain.lastGearRatio);
					float num4 = transmissionVolume;
					if (drivetrain.ratio < 0f)
					{
						num4 = transmissionVolumeReverse;
					}
					transmissionSource.volume = Mathf.Clamp01((num4 - (1f - drivetrain.throttle) * num4 * 0.25f) / num3);
					transmissionSource.pitch = num2 * transmissionSourcePitch * num3;
				}
				else
				{
					transmissionSource.volume = 0f;
				}
			}
			if (drivetrain.shiftTriggered)
			{
				shiftTriggerSource.PlayOneShot(shiftTrigger);
				drivetrain.shiftTriggered = false;
			}
		}
		if (windSource != null)
		{
			windSource.volume = Mathf.Clamp01(Mathf.Abs(cardynamics.velo) * windVolume * 0.006f);
		}
		k = 0;
		rimScraping = false;
		Wheel[] allWheels = axles.allWheels;
		foreach (Wheel wheel in allWheels)
		{
			if (wheel.rimScraping)
			{
				rimScraping = true;
				scrapeNoiseSource.volume = Mathf.Clamp01(Mathf.Abs(wheel.angularVelocity) * 0.01f + Mathf.Abs(wheel.slipVelo) * 0.035f) * scrapeNoiseVolume;
				scrapeNoiseSource.loop = true;
				if (!scrapeNoiseSource.isPlaying)
				{
					scrapeNoiseSource.Play();
				}
			}
			else
			{
				scrapeNoiseSource.loop = false;
			}
			if (skidSource[k] != null)
			{
				skidSource[k].pitch = skidPitchFactor;
				skidSource[k].volume = Mathf.Clamp(Mathf.Abs(wheel.slipVelo) * 0.00875f, 0f, skidVolume);
				if (skidSource[k].volume <= 0.01f)
				{
					skidSource[k].volume = 0f;
				}
				if (wheel.surfaceType == CarDynamics.SurfaceType.track)
				{
					rollingNoiseSource[k].volume = 0f;
				}
				else if (wheel.surfaceType == CarDynamics.SurfaceType.grass)
				{
					rollingNoiseSource[k].clip = rollingNoiseGrass;
					rollingNoiseSource[k].volume = Mathf.Clamp01(Mathf.Abs(wheel.angularVelocity) * 0.01f + Mathf.Abs(wheel.slipVelo) * 0.035f);
					if (!rollingNoiseSource[k].isPlaying)
					{
						rollingNoiseSource[k].Play();
					}
					if (rollingNoiseSource[k].volume <= 0.01f || !wheel.onGroundDown)
					{
						rollingNoiseSource[k].volume = 0f;
					}
					skidSource[k].volume = 0f;
				}
				else if (wheel.surfaceType == CarDynamics.SurfaceType.sand)
				{
					rollingNoiseSource[k].clip = rollingNoiseSand;
					rollingNoiseSource[k].volume = Mathf.Clamp01(Mathf.Abs(wheel.angularVelocity) * 0.01f + Mathf.Abs(wheel.slipVelo) * 0.035f);
					if (!rollingNoiseSource[k].isPlaying)
					{
						rollingNoiseSource[k].Play();
					}
					if (rollingNoiseSource[k].volume <= 0.01f || !wheel.onGroundDown)
					{
						rollingNoiseSource[k].volume = 0f;
					}
					skidSource[k].volume = 0f;
				}
				else if (wheel.surfaceType == CarDynamics.SurfaceType.offroad)
				{
					rollingNoiseSource[k].clip = rollingNoiseOffroad;
					rollingNoiseSource[k].volume = Mathf.Clamp01(Mathf.Abs(wheel.angularVelocity) * 0.01f + Mathf.Abs(wheel.slipVelo) * 0.035f);
					if (!rollingNoiseSource[k].isPlaying)
					{
						rollingNoiseSource[k].Play();
					}
					if (rollingNoiseSource[k].volume <= 0.01f || !wheel.onGroundDown)
					{
						rollingNoiseSource[k].volume = 0f;
					}
					skidSource[k].volume = 0f;
				}
				else if (wheel.surfaceType == CarDynamics.SurfaceType.oil)
				{
					rollingNoiseSource[k].clip = null;
					rollingNoiseSource[k].volume = 0f;
					if (!rollingNoiseSource[k].isPlaying)
					{
						rollingNoiseSource[k].Stop();
					}
					skidSource[k].volume = 0f;
				}
			}
			k++;
		}
	}

	private void OnCollisionEnter(Collision collInfo)
	{
		if (collInfo.contacts.Length > 0 && collInfo.contacts[0].thisCollider.gameObject.layer != LayerMask.NameToLayer("Wheel"))
		{
			volumeFactor = Mathf.Clamp01(collInfo.relativeVelocity.magnitude * 0.1f);
			volumeFactor *= Mathf.Clamp01(0.3f + Mathf.Abs(Vector3.Dot(collInfo.relativeVelocity.normalized, collInfo.contacts[0].normal)));
			if (volumeFactor > 0.9f && !crashHiSpeedSource.isPlaying)
			{
				crashHiSpeedSource.volume = Mathf.Clamp01(volumeFactor * crashHighVolume);
				crashHiSpeedSource.Play();
			}
			if (!crashLowSpeedSource.isPlaying)
			{
				crashLowSpeedSource.volume = Mathf.Clamp01(volumeFactor * crashLowVolume);
				crashLowSpeedSource.Play();
			}
			if (!scrapeNoiseSource.isPlaying)
			{
				scrapeNoiseSource.volume = SetScrapeNoiseVolume(collInfo, 1f);
				scrapeNoiseSource.loop = false;
				scrapeNoiseSource.Play();
			}
		}
	}

	private void OnCollisionExit()
	{
		if (!rimScraping)
		{
			scrapeNoiseSource.volume = 0f;
			scrapeNoiseSource.loop = false;
		}
	}

	private void OnCollisionStay(Collision collInfo)
	{
		scrapeNoiseSource.volume = SetScrapeNoiseVolume(collInfo, 10f);
		scrapeNoiseSource.loop = true;
		if (!scrapeNoiseSource.isPlaying)
		{
			scrapeNoiseSource.Play();
		}
	}

	private float SetScrapeNoiseVolume(Collision collInfo, float factor = 1f)
	{
		float num = collInfo.relativeVelocity.magnitude / factor;
		float num2 = 0f;
		if (collInfo.contacts.Length > 0)
		{
			num2 = 1f - Mathf.Abs(Vector3.Dot(collInfo.contacts[0].normal, collInfo.relativeVelocity.normalized));
		}
		return Mathf.Clamp01(num * num2 * scrapeNoiseVolume);
	}
}
