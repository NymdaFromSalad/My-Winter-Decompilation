using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[HutongGames.PlayMaker.Tooltip("Solo a Sound Group in Master Audio")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioGroupSolo : FsmStateAction
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
				MasterAudio.SoloGroup(runtimeSoundGroupNames[i]);
			}
		}
		else
		{
			MasterAudio.SoloGroup(soundGroupName.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allGroups = new FsmBool(false);
		soundGroupName = new FsmString(string.Empty);
	}
}
