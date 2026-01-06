namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets up and Initializes the Steam Achievements.")]
	[ActionCategory("steamworks.NET")]
	public class SteamAchievementsSetup : FsmStateAction
	{
		[Tooltip("List of Achievement Ids.")]
		[RequiredField]
		public FsmString[] AchievementIds;

		public override void Reset()
		{
			AchievementIds = null;
		}

		public override void OnEnter()
		{
			if (SteamManager.Initialized)
			{
				string[] array = new string[AchievementIds.Length];
				for (int i = 0; i < AchievementIds.Length; i++)
				{
					array[i] = AchievementIds[i].Value;
				}
				SteamManager.StatsAndAchievements.InitAchievements(array);
			}
			Finish();
		}
	}
}
