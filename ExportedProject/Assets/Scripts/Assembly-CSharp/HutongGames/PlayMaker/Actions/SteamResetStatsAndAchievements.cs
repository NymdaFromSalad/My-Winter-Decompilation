using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Resets all Stats and Achievements.")]
	[ActionCategory("steamworks.NET")]
	public class SteamResetStatsAndAchievements : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Returns true on success, false on failure.")]
		public FsmBool success;

		public override void Reset()
		{
			success = null;
		}

		public override void OnEnter()
		{
			if (SteamManager.Initialized)
			{
				success.Value = SteamManager.StatsAndAchievements.ResetStatsAndAchievements();
				if (!success.Value)
				{
					Debug.LogError("Steamworks.NET - ResetStatsAndAchievements() returned false.");
				}
			}
			Finish();
		}
	}
}
