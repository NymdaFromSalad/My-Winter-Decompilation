using SWS;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a message to a walker object for calling your own FSM state at waypoints.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_AddMessageAtWaypoint : FsmStateAction
	{
		[Tooltip("Walker object")]
		[RequiredField]
		public FsmOwnerDefault walkerObject;

		[Tooltip("Waypoint index")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt wpIndex;

		[UIHint(UIHint.FsmGameObject)]
		[Tooltip("Receiver with the FSM event")]
		[RequiredField]
		public PlayMakerFSM fsmReceiver;

		[UIHint(UIHint.FsmString)]
		[RequiredField]
		[Tooltip("Receiver FSM event to call")]
		public FsmString fsmEvent;

		public override void Reset()
		{
			walkerObject = null;
			wpIndex = null;
			fsmReceiver = null;
			fsmEvent = null;
		}

		public override void OnEnter()
		{
			Execute();
			Finish();
		}

		private void Execute()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(walkerObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			MessageFSMEvent componentInChildren = ownerDefaultTarget.GetComponentInChildren<MessageFSMEvent>();
			if (componentInChildren == null)
			{
				componentInChildren = ownerDefaultTarget.AddComponent<MessageFSMEvent>();
			}
			Messages messages = null;
			splineMove componentInChildren2 = ownerDefaultTarget.GetComponentInChildren<splineMove>();
			if ((bool)componentInChildren2)
			{
				messages = componentInChildren2.messages;
			}
			else
			{
				navMove componentInChildren3 = ownerDefaultTarget.GetComponentInChildren<navMove>();
				if ((bool)componentInChildren3)
				{
					messages = componentInChildren3.messages;
				}
			}
			MessageOptions messageOption = messages.GetMessageOption(wpIndex.Value);
			messageOption.message.Add("FsmOwner");
			messageOption.type.Add(MessageOptions.ValueType.Object);
			messageOption.obj.Add(fsmReceiver);
			messages.FillOptionWithValues(messageOption);
			messageOption.message.Add("FsmEvent");
			messageOption.type.Add(MessageOptions.ValueType.Text);
			messageOption.text.Add(fsmEvent.Value);
			messages.FillOptionWithValues(messageOption);
		}
	}
}
