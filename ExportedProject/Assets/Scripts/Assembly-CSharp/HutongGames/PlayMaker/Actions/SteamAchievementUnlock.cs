namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Unlocks an Achievement by name.")]
	public class SteamAchievementUnlock : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Achievement name.")]
		public FsmString achievementId;

		[Tooltip("Returns true on success, false on failure.")]
		[UIHint(UIHint.Variable)]
		public FsmBool success;

		public override void Reset()
		{
			achievementId = null;
			success = null;
		}

		public override void OnEnter()
		{
			success.Value = SteamManager.StatsAndAchievements.UnlockAchievement(achievementId.Value);
		}
	}
}
