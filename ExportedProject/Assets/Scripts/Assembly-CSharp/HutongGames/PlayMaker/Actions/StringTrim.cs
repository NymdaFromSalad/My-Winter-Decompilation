namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Removes all leading and trailing white-space characters from a String")]
	public class StringTrim : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringInput;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			stringInput = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoReplace();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoReplace();
		}

		private void DoReplace()
		{
			if (stringInput != null && storeResult != null)
			{
				storeResult.Value = stringInput.Value.Trim();
			}
		}
	}
}
