using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[HutongGames.PlayMaker.Tooltip("Fade a Sound Group in Master Audio to a specific volume over X seconds.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioGroupFade : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("Check this to perform action on all Sound Groups")]
	public FsmBool allGroups;

	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

	[HutongGames.PlayMaker.Tooltip("Target Sound Group volume")]
	[HasFloatSlider(0f, 1f)]
	[RequiredField]
	public FsmFloat targetVolume;

	[HasFloatSlider(0f, 10f)]
	[HutongGames.PlayMaker.Tooltip("Amount of time to complete fade (seconds)")]
	[RequiredField]
	public FsmFloat fadeTime;

	public override void OnEnter()
	{
		if (!allGroups.Value && string.IsNullOrEmpty(soundGroupName.Value))
		{
			Debug.LogError("You must either check 'All Groups' or enter the Sound Group Name");
			return;
		}
		if (allGroups.Value)
		{
			List<string> runtimeSoundGroupNames = MasterAudio.RuntimeSoundGroupNames;
			for (int i = 0; i < runtimeSoundGroupNames.Count; i++)
			{
				MasterAudio.FadeSoundGroupToVolume(runtimeSoundGroupNames[i], targetVolume.Value, fadeTime.Value);
			}
		}
		else
		{
			MasterAudio.FadeSoundGroupToVolume(soundGroupName.Value, targetVolume.Value, fadeTime.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allGroups = new FsmBool(false);
		soundGroupName = new FsmString(string.Empty);
		targetVolume = new FsmFloat();
		fadeTime = new FsmFloat();
	}
}
