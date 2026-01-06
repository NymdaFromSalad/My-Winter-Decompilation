using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Get\u00b4s steam user state.\r\nWill return [Online;Away;Busy;LookingToPlay;LookingToTrade;Offline].")]
	public class SteamGetState : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString UserID;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString userState;

		public override void Reset()
		{
			UserID = null;
			userState = null;
		}

		public override void OnEnter()
		{
			ulong steamID = ulong.Parse(UserID.Value);
			CSteamID steamID2 = SteamUser.GetSteamID();
			steamID2.m_SteamID = steamID;
			EPersonaState friendPersonaState = SteamFriends.GetFriendPersonaState(steamID2);
			string text = friendPersonaState.ToString();
			switch (friendPersonaState)
			{
			case EPersonaState.k_EPersonaStateOnline:
				userState.Value = "Online";
				break;
			case EPersonaState.k_EPersonaStateOffline:
				userState.Value = "Offline";
				break;
			case EPersonaState.k_EPersonaStateAway:
				userState.Value = "Away";
				break;
			case EPersonaState.k_EPersonaStateSnooze:
				userState.Value = "Away";
				break;
			case EPersonaState.k_EPersonaStateBusy:
				userState.Value = "Busy";
				break;
			case EPersonaState.k_EPersonaStateLookingToPlay:
				userState.Value = "LookingToPlay";
				break;
			case EPersonaState.k_EPersonaStateLookingToTrade:
				userState.Value = "LookingToTrade";
				break;
			}
		}
	}
}
