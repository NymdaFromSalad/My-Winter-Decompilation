using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Audio)]
[HutongGames.PlayMaker.Tooltip("Mute all Audio in a Bus in Master Audio")]
public class MasterAudioBusMute : FsmStateAction
{
	[HutongGames.PlayMaker.Tooltip("Check this to perform action on all Buses")]
	public FsmBool allBuses;

	[HutongGames.PlayMaker.Tooltip("Name of Master Audio Bus")]
	public FsmString busName;

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
				MasterAudio.MuteBus(runtimeBusNames[i]);
			}
		}
		else
		{
			MasterAudio.MuteBus(busName.Value);
		}
		Finish();
	}

	public override void Reset()
	{
		allBuses = new FsmBool(false);
		busName = new FsmString(string.Empty);
	}
}
