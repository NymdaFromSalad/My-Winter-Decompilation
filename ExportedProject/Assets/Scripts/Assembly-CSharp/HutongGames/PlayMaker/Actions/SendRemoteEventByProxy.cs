using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Send an Fsm Event on a remote machine. Uses Unity RPC functions. Use this instead of SendRemoteEvent if you have multiple PlayMakerFSM components on the GameObject that you want to recieve the event.")]
	[ActionCategory(ActionCategory.Network)]
	public class SendRemoteEventByProxy : ComponentAction<NetworkView>
	{
		[Tooltip("The game object that sends the event.")]
		[CheckForComponent(typeof(NetworkView), typeof(PlayMakerRPCProxy))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The event you want to send.")]
		public FsmEvent remoteEvent;

		[Tooltip("Optional string data. Use 'Get Event Info' action to retrieve it.")]
		public FsmString stringData;

		[Tooltip("Option for who will receive an RPC.")]
		public RPCMode mode;

		public override void Reset()
		{
			gameObject = null;
			remoteEvent = null;
			mode = RPCMode.All;
			stringData = null;
			mode = RPCMode.All;
		}

		public override void OnEnter()
		{
			DoRPC();
			Finish();
		}

		private void DoRPC()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				if (!stringData.IsNone && stringData.Value != string.Empty)
				{
					base.networkView.RPC("ForwardEvent", mode, remoteEvent.Name, stringData.Value);
				}
				else
				{
					base.networkView.RPC("ForwardEvent", mode, remoteEvent.Name);
				}
			}
		}
	}
}
