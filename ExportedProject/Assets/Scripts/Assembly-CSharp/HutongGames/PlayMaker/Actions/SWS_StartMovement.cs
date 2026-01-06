using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Starts movement of a walker object.")]
	[ActionCategory("Simple Waypoint System")]
	public class SWS_StartMovement : FsmStateAction
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
				ownerDefaultTarget.SendMessage("StartMove");
			}
		}
	}
}
