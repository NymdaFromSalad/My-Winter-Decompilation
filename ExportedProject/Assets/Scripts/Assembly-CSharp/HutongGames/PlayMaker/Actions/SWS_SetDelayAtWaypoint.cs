using SWS;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets delay at a waypoint of a walker object.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_SetDelayAtWaypoint : FsmStateAction
	{
		[Tooltip("Walker object")]
		[RequiredField]
		public FsmOwnerDefault walkerObject;

		[UIHint(UIHint.FsmInt)]
		[Tooltip("Waypoint index")]
		public FsmInt wpIndex;

		[Tooltip("Min Delay at waypoint")]
		[UIHint(UIHint.FsmFloat)]
		public FsmFloat min;

		[UIHint(UIHint.FsmFloat)]
		[Tooltip("Max Delay at waypoint")]
		public FsmFloat max;

		public override void Reset()
		{
			walkerObject = null;
			wpIndex = null;
			min = 0f;
			max = 0f;
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
			splineMove componentInChildren = ownerDefaultTarget.GetComponentInChildren<splineMove>();
			if ((bool)componentInChildren)
			{
				componentInChildren.SetDelay(wpIndex.Value, min.Value, max.Value);
				return;
			}
			navMove componentInChildren2 = ownerDefaultTarget.GetComponentInChildren<navMove>();
			if ((bool)componentInChildren2)
			{
				componentInChildren2.SetDelay(wpIndex.Value, min.Value, max.Value);
			}
		}
	}
}
