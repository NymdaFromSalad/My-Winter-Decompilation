using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Subtract the value of a Float Variable in another FSM.")]
	public class SubtractFsmFloat : FsmStateAction
	{
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmFloat)]
		[Tooltip("The name of the FSM variable.")]
		public FsmString variableName;

		[Tooltip("Subtract this from the target variable.")]
		[RequiredField]
		public FsmFloat subtractValue;

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
			DoSubtractFsmFloat();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSubtractFsmFloat()
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
			FsmFloat fsmFloat = fsm.FsmVariables.GetFsmFloat(variableName.Value);
			if (fsmFloat != null && !perSecond)
			{
				fsmFloat.Value -= subtractValue.Value;
			}
			if (fsmFloat != null && perSecond)
			{
				fsmFloat.Value -= subtractValue.Value * Time.deltaTime;
			}
		}

		public override void OnUpdate()
		{
			DoSubtractFsmFloat();
		}
	}
}
