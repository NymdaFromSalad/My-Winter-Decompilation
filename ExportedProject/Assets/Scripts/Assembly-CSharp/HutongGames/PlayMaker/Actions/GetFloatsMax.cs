using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns the maximum float from a list")]
	[ActionCategory(ActionCategory.Math)]
	public class GetFloatsMax : FsmStateAction
	{
		[Tooltip("The float variables.")]
		[RequiredField]
		public FsmFloat[] floats;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			floats = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoMaxFromFloats();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMaxFromFloats();
		}

		private void DoMaxFromFloats()
		{
			float num = float.NegativeInfinity;
			FsmFloat[] array = floats;
			foreach (FsmFloat fsmFloat in array)
			{
				float value = fsmFloat.Value;
				if (fsmFloat.UseVariable && !fsmFloat.IsNone)
				{
					value = base.Fsm.Variables.GetFsmFloat(fsmFloat.Name).Value;
				}
				num = Mathf.Max(num, value);
			}
			storeResult.Value = num;
		}
	}
}
