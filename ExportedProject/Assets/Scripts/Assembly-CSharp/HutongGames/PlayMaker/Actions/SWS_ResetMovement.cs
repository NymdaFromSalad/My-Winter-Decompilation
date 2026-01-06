using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Resets movement of a walker object. Optionally repositions it to the start.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_ResetMovement : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Walker object")]
		public FsmOwnerDefault walkerObject;

		[Tooltip("Reset to Start")]
		[UIHint(UIHint.FsmBool)]
		[RequiredField]
		public FsmBool reset;

		public override void Reset()
		{
			walkerObject = null;
			reset = null;
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
				ownerDefaultTarget.SendMessage("ResetMove", reset.Value);
			}
		}
	}
}
