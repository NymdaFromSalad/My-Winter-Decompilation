using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[HutongGames.PlayMaker.Tooltip("Fade a Bus in Master Audio to a specific volume over X seconds.")]
[ActionCategory(ActionCategory.Audio)]
public class MasterAudioBusFade : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("Check this to perform action on all Buses")]
	public FsmBool allBuses;

	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Bus")]
	public FsmString busName;

	[HutongGames.PlayMaker.Tooltip("Target Bus volume")]
	[RequiredField]
	[HasFloatSlider(0f, 1f)]
	public FsmFloat targetVolume;

	[HasFloatSlider(0f, 10f)]
	[RequiredField]
	[HutongGames.PlayMaker.Tooltip("Amount of time to complete fade (seconds)")]
	public FsmFloat fadeTime;

	public override void OnEnter()
	{
		if (!allBuses.Value && string.IsNullOrEmpty(busName.Value))
		{
			Debug.LogError("You must either check 'All Buses' or enter the Bus Name");
			return;
		}
		if (allBuses.Value)
		{
			List<string> runtimeBusNames = MasterAudio.RuntimeBusNames;
			for (int i = 0; i < runtimeBusNames.Count; i++)
			{
				MasterAudio.FadeBusToVolume(runtimeBusNames[i], targetVolume.Value, fadeTime.Value);
			}
		}
		else
		{
			MasterAudio.FadeBusToVolume(busName.Value, targetVolume.Value, fadeTime.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allBuses = new FsmBool(false);
		busName = new FsmString(string.Empty);
		targetVolume = new FsmFloat();
		fadeTime = new FsmFloat();
	}
}
