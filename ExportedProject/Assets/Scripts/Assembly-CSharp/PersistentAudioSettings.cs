using System.Collections.Generic;
using UnityEngine;

public static class PersistentAudioSettings
{
	private const string SfxVolKey = "MA_sfxVolume";

	private const string MusicVolKey = "MA_musicVolume";

	private static readonly Dictionary<string, float> GroupVolumesByName = new Dictionary<string, float>();

	private static readonly Dictionary<string, float> BusVolumesByName = new Dictionary<string, float>();

	public static float? MixerVolume
	{
		get
		{
			if (!PlayerPrefs.HasKey("MA_sfxVolume"))
			{
				return null;
			}
			return PlayerPrefs.GetFloat("MA_sfxVolume");
		}
		set
		{
			if (!value.HasValue)
			{
				PlayerPrefs.DeleteKey("MA_sfxVolume");
				return;
			}
			float value2 = value.Value;
			PlayerPrefs.SetFloat("MA_sfxVolume", value2);
			MasterAudio safeInstance = MasterAudio.SafeInstance;
			if (safeInstance != null)
			{
				MasterAudio.MasterVolumeLevel = value2;
			}
		}
	}

	public static float? MusicVolume
	{
		get
		{
			if (!PlayerPrefs.HasKey("MA_musicVolume"))
			{
				return null;
			}
			return PlayerPrefs.GetFloat("MA_musicVolume");
		}
		set
		{
			if (!value.HasValue)
			{
				PlayerPrefs.DeleteKey("MA_musicVolume");
				return;
			}
			float value2 = value.Value;
			PlayerPrefs.SetFloat("MA_musicVolume", value2);
			MasterAudio safeInstance = MasterAudio.SafeInstance;
			if (safeInstance != null)
			{
				MasterAudio.PlaylistMasterVolume = value2;
			}
		}
	}

	public static void SetBusVolume(string busName, float vol)
	{
		if (BusVolumesByName.ContainsKey(busName))
		{
			BusVolumesByName[busName] = vol;
		}
		else
		{
			BusVolumesByName.Add(busName, vol);
		}
		MasterAudio safeInstance = MasterAudio.SafeInstance;
		if (!(safeInstance == null) && MasterAudio.GrabBusByName(busName) != null)
		{
			MasterAudio.SetBusVolumeByName(busName, vol);
		}
	}

	public static float? GetBusVolume(string busName)
	{
		if (!BusVolumesByName.ContainsKey(busName))
		{
			return null;
		}
		return BusVolumesByName[busName];
	}

	public static void SetGroupVolume(string grpName, float vol)
	{
		if (GroupVolumesByName.ContainsKey(grpName))
		{
			GroupVolumesByName[grpName] = vol;
		}
		else
		{
			GroupVolumesByName.Add(grpName, vol);
		}
		MasterAudio safeInstance = MasterAudio.SafeInstance;
		if (!(safeInstance == null) && MasterAudio.GrabGroup(grpName, false) != null)
		{
			MasterAudio.SetGroupVolume(grpName, vol);
		}
	}

	public static float? GetGroupVolume(string grpName)
	{
		if (!GroupVolumesByName.ContainsKey(grpName))
		{
			return null;
		}
		return GroupVolumesByName[grpName];
	}

	public static void RestoreMasterSettings()
	{
		if (MixerVolume.HasValue)
		{
			MasterAudio.MasterVolumeLevel = MixerVolume.Value;
		}
		if (MusicVolume.HasValue)
		{
			MasterAudio.PlaylistMasterVolume = MusicVolume.Value;
		}
	}
}
