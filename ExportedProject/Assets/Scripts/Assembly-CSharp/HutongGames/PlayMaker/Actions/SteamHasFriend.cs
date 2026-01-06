using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Check if a user is friend.")]
	public class SteamHasFriend : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString UserID;

		[UIHint(UIHint.Variable)]
		public FsmBool isFriend;

		public override void Reset()
		{
			UserID = null;
			isFriend = false;
		}

		public override void OnEnter()
		{
			ulong steamID = ulong.Parse(UserID.Value);
			CSteamID steamID2 = SteamUser.GetSteamID();
			steamID2.m_SteamID = steamID;
			isFriend.Value = SteamFriends.HasFriend(steamID2, EFriendFlags.k_EFriendFlagAll);
		}
	}
}
