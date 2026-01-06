namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the display name, description, and status of an achievement.")]
	[ActionCategory("steamworks.NET")]
	public class SteamAchievementGet : FsmStateAction
	{
		[RequiredField]
		[Tooltip("This is the 'API Name' in the Steam Partner backend")]
		public FsmString achievementId;

		[Tooltip("Returns the Achievement Name.")]
		[UIHint(UIHint.Variable)]
		public FsmString achievementName;

		[Tooltip("Returns the Achievement Description.")]
		[UIHint(UIHint.Variable)]
		public FsmString achievementDescription;

		[UIHint(UIHint.Variable)]
		[Tooltip("True if the Achievement is unlocked, False if it is locked.")]
		public FsmBool achievementUnlocked;

		public override void Reset()
		{
			achievementId = null;
			achievementName = null;
			achievementDescription = null;
			achievementUnlocked = null;
		}

		public override void OnEnter()
		{
			Achievement_t achievement = SteamManager.StatsAndAchievements.GetAchievement(achievementId.Value);
			if (achievement != null)
			{
				achievementName.Value = achievement.m_strName;
				achievementDescription.Value = achievement.m_strDescription;
				achievementUnlocked.Value = achievement.m_bAchieved;
			}
			Finish();
		}
	}
}
