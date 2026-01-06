using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Sets up and Initializes the Steam Stats.")]
	public class SteamStatsSetup : FsmStateAction
	{
		[Tooltip("List of Stats.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVar[] stats;

		public override void Reset()
		{
			stats = null;
		}

		public override void OnEnter()
		{
			if (SteamManager.Initialized)
			{
				Stat_t[] array = new Stat_t[stats.Length];
				for (int i = 0; i < stats.Length; i++)
				{
					Debug.Log(stats[i].variableName + " " + stats[i]);
					if (stats[i].IsNone)
					{
						throw new Exception("Steam stats must have a variable attached.");
					}
					if (stats[i].Type != VariableType.Float && stats[i].Type != VariableType.Int)
					{
						throw new Exception("Steam stats may only be floats and ints: " + stats[i].variableName);
					}
					array[i] = new Stat_t(stats[i].variableName, stats[i].RealType);
				}
				SteamManager.StatsAndAchievements.InitStats(array);
			}
			Finish();
		}
	}
}
