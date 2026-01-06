using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a value to an fsm Integer Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class AddToFsmInt : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmInt)]
		public FsmString variableName;

		[RequiredField]
		public FsmInt add;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optional storage of the result")]
		public FsmInt storeResult;

		public bool everyFrame;

		private GameObject goLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			add = null;
			fsmName = string.Empty;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoAddToFsmInt();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoAddToFsmInt();
		}

		private void DoAddToFsmInt()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != goLastFrame)
			{
				goLastFrame = ownerDefaultTarget;
				fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
			}
			if (fsm == null)
			{
				return;
			}
			FsmInt fsmInt = fsm.FsmVariables.GetFsmInt(variableName.Value);
			if (fsmInt != null)
			{
				fsmInt.Value += add.Value;
				if (storeResult != null)
				{
					storeResult.Value = fsmInt.Value;
				}
			}
		}
	}
}
