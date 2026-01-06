using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Resumes movement of a previously paused walker object.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_ResumeMovement : FsmStateAction
	{
		[Tooltip("Walker object")]
		[RequiredField]
		public FsmOwnerDefault walkerObject;

		public override void Reset()
		{
			walkerObject = null;
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
				ownerDefaultTarget.SendMessage("Resume");
			}
		}
	}
}
