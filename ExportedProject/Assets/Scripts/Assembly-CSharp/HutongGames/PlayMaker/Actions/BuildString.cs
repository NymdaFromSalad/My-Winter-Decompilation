namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Builds a String from other Strings.")]
	public class BuildString : FsmStateAction
	{
		[Tooltip("Array of Strings to combine.")]
		[RequiredField]
		public FsmString[] stringParts;

		[Tooltip("Separator to insert between each String. E.g. space character.")]
		public FsmString separator;

		[Tooltip("Store the final String in a variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		private string result;

		public override void Reset()
		{
			stringParts = new FsmString[3];
			separator = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoBuildString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoBuildString();
		}

		private void DoBuildString()
		{
			if (storeResult != null)
			{
				result = string.Empty;
				FsmString[] array = stringParts;
				foreach (FsmString fsmString in array)
				{
					result += fsmString;
					result += separator.Value;
				}
				storeResult.Value = result;
			}
		}
	}
}
