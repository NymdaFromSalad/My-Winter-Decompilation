using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Get\u00b4s steam user ID.")]
	public class SteamGetUserID : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString userID;

		public override void Reset()
		{
			userID = null;
		}

		public override void OnEnter()
		{
			CSteamID steamID = SteamUser.GetSteamID();
			userID.Value = steamID.ToString();
		}
	}
}
