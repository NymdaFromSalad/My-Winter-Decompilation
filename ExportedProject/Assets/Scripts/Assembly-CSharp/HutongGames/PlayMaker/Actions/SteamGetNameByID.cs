using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Get\u00b4s Friend Name By His ID.")]
	public class SteamGetNameByID : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString UserID;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString friendName;

		public override void Reset()
		{
			UserID = null;
			friendName = null;
		}

		public override void OnEnter()
		{
			ulong steamID = ulong.Parse(UserID.Value);
			CSteamID steamID2 = SteamUser.GetSteamID();
			steamID2.m_SteamID = steamID;
			friendName.Value = SteamFriends.GetFriendPersonaName(steamID2);
		}
	}
}
