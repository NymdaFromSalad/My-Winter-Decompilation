using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Sets the Hinge Joint Limits on a Hinge Joint Component")]
	public class SetHingeJointLimits : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(HingeJoint))]
		public FsmOwnerDefault gameObject;

		public FsmFloat min;

		public FsmFloat max;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			min = new FsmFloat
			{
				UseVariable = true
			};
			max = new FsmFloat
			{
				UseVariable = true
			};
		}

		public override void OnEnter()
		{
			DoSetLimits();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetLimits()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			HingeJoint component = ownerDefaultTarget.GetComponent<HingeJoint>();
			JointLimits limits = component.limits;
			limits.min = min.Value;
			limits.max = max.Value;
			component.limits = limits;
		}
	}
}
