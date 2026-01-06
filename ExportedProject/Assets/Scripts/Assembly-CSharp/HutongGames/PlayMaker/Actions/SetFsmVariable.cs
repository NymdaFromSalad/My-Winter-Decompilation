using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value of a variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmVariable : FsmStateAction
	{
		[Tooltip("The GameObject that owns the FSM")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		public FsmString variableName;

		[RequiredField]
		[HideTypeFilter]
		public FsmVar setValue;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private GameObject cachedGO;

		private PlayMakerFSM sourceFsm;

		private INamedVariable sourceVariable;

		private NamedVariable targetVariable;

		public override void Reset()
		{
			gameObject = null;
			fsmName = string.Empty;
			setValue = new FsmVar();
		}

		public override void OnEnter()
		{
			InitFsmVar();
			DoGetFsmVariable();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmVariable();
		}

		private void InitFsmVar()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null) && ownerDefaultTarget != cachedGO)
			{
				sourceFsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
				sourceVariable = sourceFsm.FsmVariables.GetVariable(setValue.variableName);
				targetVariable = base.Fsm.Variables.GetVariable(setValue.variableName);
				setValue.Type = FsmUtility.GetVariableType(targetVariable);
				if (!string.IsNullOrEmpty(setValue.variableName) && sourceVariable == null)
				{
					LogWarning("Missing Variable: " + setValue.variableName);
				}
				cachedGO = ownerDefaultTarget;
			}
		}

		private void DoGetFsmVariable()
		{
			if (!setValue.IsNone)
			{
				InitFsmVar();
				setValue.GetValueFrom(sourceVariable);
				setValue.ApplyValueTo(targetVariable);
			}
		}
	}
}
