using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Get\u00b4s number of friends on Steam.")]
	public class SteamGetFriendCount : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt friendCount;

		public override void Reset()
		{
			friendCount = null;
		}

		public override void OnEnter()
		{
			friendCount.Value = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
		}
	}
}
