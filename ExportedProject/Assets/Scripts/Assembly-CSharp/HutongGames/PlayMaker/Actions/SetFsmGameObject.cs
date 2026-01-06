using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Game Object Variable in another FSM. Accept null reference")]
	public class SetFsmGameObject : FsmStateAction
	{
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		[UIHint(UIHint.FsmGameObject)]
		public FsmString variableName;

		[Tooltip("Set the value of the variable.")]
		public FsmGameObject setValue;

		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		private GameObject goLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = string.Empty;
			setValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFsmGameObject();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmGameObject()
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
			if (!(fsm == null))
			{
				FsmGameObject fsmGameObject = fsm.FsmVariables.FindFsmGameObject(variableName.Value);
				if (fsmGameObject != null)
				{
					fsmGameObject.Value = ((setValue != null) ? setValue.Value : null);
				}
				else
				{
					LogWarning("Could not find variable: " + variableName.Value);
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmGameObject();
		}
	}
}
