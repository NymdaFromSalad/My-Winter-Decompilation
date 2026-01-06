using Steamworks;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("steamworks.NET")]
	[Tooltip("Send\u00b4s message to friend by ID.")]
	public class SteamSendMessage : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString friendID;

		[Tooltip("Message text as string.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString message;

		[Tooltip("Return true if sended, return false if not possible.")]
		public FsmBool send;

		public override void Reset()
		{
			friendID = null;
			message = null;
		}

		public override void OnEnter()
		{
			ulong steamID = ulong.Parse(friendID.Value);
			CSteamID steamID2 = SteamUser.GetSteamID();
			steamID2.m_SteamID = steamID;
			send.Value = SteamFriends.ReplyToFriendMessage(steamID2, message.Value);
		}
	}
}
