using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Get\u00b4s steam user country.")]
	public class SteamGetUserCountry : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString userCountry;

		public override void Reset()
		{
			userCountry = null;
		}

		public override void OnEnter()
		{
			userCountry.Value = SteamUtils.GetIPCountry();
		}
	}
}
