using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Audio)]
[HutongGames.PlayMaker.Tooltip("Toggle the solo button of a Sound Group in Master Audio")]
public class MasterAudioGroupToggleSolo : FsmStateAction
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
			string empty = string.Empty;
			for (int i = 0; i < runtimeSoundGroupNames.Count; i++)
			{
				empty = runtimeSoundGroupNames[i];
				MasterAudioGroup masterAudioGroup = MasterAudio.GrabGroup(empty);
				if (masterAudioGroup != null)
				{
					if (masterAudioGroup.isSoloed)
					{
						MasterAudio.UnsoloGroup(empty);
					}
					else
					{
						MasterAudio.SoloGroup(empty);
					}
				}
			}
		}
		else
		{
			MasterAudioGroup masterAudioGroup2 = MasterAudio.GrabGroup(soundGroupName.Value);
			if (masterAudioGroup2 != null)
			{
				if (masterAudioGroup2.isSoloed)
				{
					MasterAudio.UnsoloGroup(soundGroupName.Value);
				}
				else
				{
					MasterAudio.SoloGroup(soundGroupName.Value);
				}
			}
		}
		Finish();
	}

	public override void Reset()
	{
		allGroups = new FsmBool(false);
		soundGroupName = new FsmString(string.Empty);
	}
}
