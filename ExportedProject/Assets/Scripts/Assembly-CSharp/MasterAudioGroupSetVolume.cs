using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Audio)]
[HutongGames.PlayMaker.Tooltip("Set a single Sound Group volume level in Master Audio")]
public class MasterAudioGroupSetVolume : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("Check this to perform action on all Sound Groups")]
	public FsmBool allGroups;

	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Sound Group")]
	public FsmString soundGroupName;

	[HasFloatSlider(0f, 1f)]
	[RequiredField]
	public FsmFloat volume = new FsmFloat(1f);

	[HutongGames.PlayMaker.Tooltip("Repeat every frame while the state is active.")]
	public bool everyFrame;

	public override void OnEnter()
	{
		SetVolume();
		if (!everyFrame)
		{
			Finish();
		}
	}

	public override void OnUpdate()
	{
		SetVolume();
	}

	private void SetVolume()
	{
		if (!allGroups.Value && string.IsNullOrEmpty(soundGroupName.Value))
		{
			Debug.LogError("You must either check 'All Groups' or enter the Sound Group Name");
		}
		else if (allGroups.Value)
		{
			List<string> runtimeSoundGroupNames = MasterAudio.RuntimeSoundGroupNames;
			for (int i = 0; i < runtimeSoundGroupNames.Count; i++)
			{
				MasterAudio.SetGroupVolume(runtimeSoundGroupNames[i], volume.Value);
			}
		}
		else
		{
			MasterAudio.SetGroupVolume(soundGroupName.Value, volume.Value);
		}
	}

	public override void Reset()
	{
		allGroups = new FsmBool(false);
		soundGroupName = new FsmString(string.Empty);
		volume = new FsmFloat(1f);
	}
}
