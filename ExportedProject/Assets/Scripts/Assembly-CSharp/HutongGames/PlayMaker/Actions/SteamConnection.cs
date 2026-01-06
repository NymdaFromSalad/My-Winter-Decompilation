using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("True if the client has a live connection to the Steam servers.\r\nFalse means there is no active connection due to either a networking issue on the local machine, or the Steam server is down/busy.")]
	[ActionCategory("steamworks.NET")]
	public class SteamConnection : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool Connect;

		public override void Reset()
		{
			Connect = false;
		}

		public override void OnEnter()
		{
			Connect.Value = SteamUser.BLoggedOn();
		}
	}
}
