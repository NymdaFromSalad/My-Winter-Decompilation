namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Uploads a score to a Steam Leaderboard.")]
	public class SteamUploadScore : FsmStateAction
	{
		[Tooltip("Leaderboard name.")]
		[RequiredField]
		public FsmString leaderboardName;

		[RequiredField]
		[Tooltip("The score to upload.")]
		public FsmInt score;

		public override void Reset()
		{
			leaderboardName = null;
			score = null;
		}

		public override void OnEnter()
		{
			if (SteamManager.Initialized)
			{
				SteamManager.Leaderboards.UploadScore(leaderboardName.Value, score.Value);
			}
			Finish();
		}
	}
}
