using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Get\u00b4s steam app ID.")]
	public class SteamGetAppID : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString appID;

		public override void Reset()
		{
			appID = null;
		}

		public override void OnEnter()
		{
			AppId_t appId_t = SteamUtils.GetAppID();
			appID.Value = appId_t.ToString();
		}
	}
}
