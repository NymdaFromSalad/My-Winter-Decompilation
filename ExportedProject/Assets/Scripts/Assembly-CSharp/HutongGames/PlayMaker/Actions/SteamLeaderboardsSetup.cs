namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Sets up and Initializes the Steam Achievements.")]
	public class SteamLeaderboardsSetup : FsmStateAction
	{
		[Tooltip("List of Leaderboard names.")]
		[RequiredField]
		public FsmString[] Leaderboards;

		public override void Reset()
		{
			Leaderboards = null;
		}

		public override void OnEnter()
		{
			if (SteamManager.Initialized)
			{
				string[] array = new string[Leaderboards.Length];
				for (int i = 0; i < Leaderboards.Length; i++)
				{
					array[i] = Leaderboards[i].Value;
				}
				SteamManager.Leaderboards.InitLeaderboards(array);
			}
			Finish();
		}
	}
}
