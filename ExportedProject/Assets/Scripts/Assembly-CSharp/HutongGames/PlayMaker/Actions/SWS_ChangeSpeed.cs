using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Changes speed of a walker object. Consider not changing this per frame.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_ChangeSpeed : FsmStateAction
	{
		[Tooltip("Walker object")]
		[RequiredField]
		public FsmOwnerDefault walkerObject;

		[UIHint(UIHint.FsmFloat)]
		[Tooltip("Speed value")]
		public FsmFloat speed;

		[Tooltip("Update per frame")]
		[UIHint(UIHint.FsmBool)]
		public bool everyFrame;

		public override void Reset()
		{
			walkerObject = null;
			speed = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			Execute();
			Finish();
		}

		private void Execute()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(walkerObject);
			if (!(ownerDefaultTarget == null))
			{
				ownerDefaultTarget.SendMessage("ChangeSpeed", speed.Value);
			}
		}
	}
}
