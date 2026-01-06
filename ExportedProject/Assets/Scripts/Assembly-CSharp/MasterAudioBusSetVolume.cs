using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Audio)]
[HutongGames.PlayMaker.Tooltip("Set a single bus volume level in Master Audio")]
public class MasterAudioBusSetVolume : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("Check this to perform action on all Buses")]
	public FsmBool allBuses;

	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Bus")]
	public FsmString busName;

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
		if (!allBuses.Value && string.IsNullOrEmpty(busName.Value))
		{
			Debug.LogError("You must either check 'All Buses' or enter the Bus Name");
		}
		else if (allBuses.Value)
		{
			List<string> runtimeBusNames = MasterAudio.RuntimeBusNames;
			for (int i = 0; i < runtimeBusNames.Count; i++)
			{
				MasterAudio.SetBusVolumeByName(runtimeBusNames[i], volume.Value);
			}
		}
		else
		{
			MasterAudio.SetBusVolumeByName(busName.Value, volume.Value);
		}
	}

	public override void Reset()
	{
		allBuses = new FsmBool(false);
		busName = new FsmString(string.Empty);
		volume = new FsmFloat(1f);
	}
}
