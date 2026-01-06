using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get\u00b4s battery level.\r\nInt [0;100]. Will return 255 if laptop.")]
	[ActionCategory("steamworks.NET")]
	public class SteamGetBatteryLevel : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt batteryLevel;

		public override void Reset()
		{
			batteryLevel = null;
		}

		public override void OnEnter()
		{
			batteryLevel.Value = SteamUtils.GetCurrentBatteryPower();
		}
	}
}
