namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Coverts a string to upper or lower case.")]
	public class ConvertCase : FsmStateAction
	{
		public enum Case
		{
			Lower = 0,
			Upper = 1
		}

		[RequiredField]
		public FsmString stringVariable;

		[Tooltip("Select upper or lower case.")]
		public Case operation;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringValue;

		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			stringValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetStringValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetStringValue();
		}

		private void DoSetStringValue()
		{
			if (stringVariable != null && stringValue != null)
			{
				switch (operation)
				{
				case Case.Lower:
					stringValue.Value = stringVariable.Value.ToLower();
					break;
				case Case.Upper:
					stringValue.Value = stringVariable.Value.ToUpper();
					break;
				}
			}
		}
	}
}
