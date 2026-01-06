using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[HutongGames.PlayMaker.Tooltip("Fade all of a Sound Group in Master Audio to zero volume over X seconds")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioFadeOutAllOfSound : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("Check this to perform action on all Sound Groups")]
	public FsmBool allGroups;

	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

	[RequiredField]
	[HasFloatSlider(0f, 10f)]
	[HutongGames.PlayMaker.Tooltip("Amount of time to complete fade (seconds)")]
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
				MasterAudio.FadeOutAllOfSound(runtimeSoundGroupNames[i], fadeTime.Value);
			}
		}
		else
		{
			MasterAudio.FadeOutAllOfSound(soundGroupName.Value, fadeTime.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allGroups = new FsmBool(false);
		soundGroupName = new FsmString(string.Empty);
		fadeTime = new FsmFloat();
	}
}
