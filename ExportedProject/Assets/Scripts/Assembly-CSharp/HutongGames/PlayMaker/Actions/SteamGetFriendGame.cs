using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get\u00b4s steam user actual activity.\r\nGet game app ID, 0 equal playing nothing.")]
	[ActionCategory("steamworks.NET")]
	public class SteamGetFriendGame : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString UserID;

		[UIHint(UIHint.Variable)]
		public FsmString game;

		[UIHint(UIHint.Variable)]
		public FsmBool play;

		public override void Reset()
		{
			UserID = null;
			game = null;
			play = false;
		}

		public override void OnEnter()
		{
			ulong steamID = ulong.Parse(UserID.Value);
			CSteamID steamID2 = SteamUser.GetSteamID();
			steamID2.m_SteamID = steamID;
			FriendGameInfo_t pFriendGameInfo;
			play = SteamFriends.GetFriendGamePlayed(steamID2, out pFriendGameInfo);
			game.Value = pFriendGameInfo.m_gameID.ToString();
		}
	}
}
