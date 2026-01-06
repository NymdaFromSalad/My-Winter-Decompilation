using SWS;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Simple Waypoint System")]
	[Tooltip("Removes all messages of a walker object at a specific waypoint.")]
	public class SWS_RemoveMessagesAtWaypoint : FsmStateAction
	{
		[Tooltip("Walker object")]
		[RequiredField]
		public FsmOwnerDefault walkerObject;

		[UIHint(UIHint.FsmInt)]
		[Tooltip("Waypoint index")]
		public FsmInt wpIndex;

		public override void Reset()
		{
			walkerObject = null;
			wpIndex = null;
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
			Messages messages = null;
			splineMove componentInChildren = ownerDefaultTarget.GetComponentInChildren<splineMove>();
			if ((bool)componentInChildren)
			{
				messages = componentInChildren.messages;
			}
			else
			{
				bezierMove componentInChildren2 = ownerDefaultTarget.GetComponentInChildren<bezierMove>();
				if ((bool)componentInChildren2)
				{
					messages = componentInChildren2.messages;
				}
				else
				{
					navMove component = ownerDefaultTarget.GetComponent<navMove>();
					if ((bool)component)
					{
						messages = component.messages;
					}
				}
			}
			if (messages == null || messages.list == null)
			{
				Debug.Log("RemoveMessagesAtWaypoint action could not find messages on " + ownerDefaultTarget.name);
				return;
			}
			int count = messages.list.Count;
			if (wpIndex.Value >= count - 1)
			{
				wpIndex.Value = count - 1;
			}
			else if (wpIndex.Value <= 0)
			{
				wpIndex.Value = 0;
			}
			if (count >= wpIndex.Value)
			{
				MessageOptions messageOption = messages.GetMessageOption(wpIndex.Value);
				messageOption.message.Clear();
				messageOption.type.Clear();
				messageOption.obj.Clear();
				messageOption.text.Clear();
				messageOption.num.Clear();
				messageOption.vect2.Clear();
				messageOption.vect3.Clear();
				messages.AddEmptyToOption(messageOption);
			}
		}
	}
}
