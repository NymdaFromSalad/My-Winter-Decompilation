using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Audio)]
[HutongGames.PlayMaker.Tooltip("Stop all of a Sound Group in Master Audio")]
public class MasterAudioStopAllOfSound : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("Check this to perform action on all Sound Groups")]
	public FsmBool allGroups;

	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

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
				MasterAudio.StopAllOfSound(runtimeSoundGroupNames[i]);
			}
		}
		else
		{
			MasterAudio.StopAllOfSound(soundGroupName.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allGroups = new FsmBool(false);
		soundGroupName = new FsmString(string.Empty);
	}
}
