using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Subtract the value of a Int Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SubtractFsmInt : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[UIHint(UIHint.FsmInt)]
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		public FsmString variableName;

		[RequiredField]
		[Tooltip("Subtract this from the target variable.")]
		public FsmInt subtractValue;

		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		[Tooltip("Use with Every Frame only to continue over time")]
		public bool perSecond;

		private GameObject goLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			perSecond = false;
			fsmName = string.Empty;
			subtractValue = null;
		}

		public override void OnEnter()
		{
			DoSubtractFsmInt();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSubtractFsmInt()
		{
			if (subtractValue == null)
			{
				return;
			}
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
				LogWarning("Could not find FSM: " + fsmName.Value);
				return;
			}
			FsmInt fsmInt = fsm.FsmVariables.GetFsmInt(variableName.Value);
			if (fsmInt != null && !perSecond)
			{
				fsmInt.Value -= subtractValue.Value;
			}
			if (fsmInt != null && perSecond)
			{
				fsmInt.Value -= subtractValue.Value * (int)Time.deltaTime;
			}
		}

		public override void OnUpdate()
		{
			DoSubtractFsmInt();
		}
	}
}
