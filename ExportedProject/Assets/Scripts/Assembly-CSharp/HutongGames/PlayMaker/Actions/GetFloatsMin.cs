using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Returns the minimum float from a list")]
	public class GetFloatsMin : FsmStateAction
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
			DoMinFromFloats();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMinFromFloats();
		}

		private void DoMinFromFloats()
		{
			float num = float.PositiveInfinity;
			FsmFloat[] array = floats;
			foreach (FsmFloat fsmFloat in array)
			{
				float value = fsmFloat.Value;
				if (fsmFloat.UseVariable && !fsmFloat.IsNone)
				{
					value = base.Fsm.Variables.GetFsmFloat(fsmFloat.Name).Value;
				}
				num = Mathf.Min(num, value);
			}
			storeResult.Value = num;
		}
	}
}
