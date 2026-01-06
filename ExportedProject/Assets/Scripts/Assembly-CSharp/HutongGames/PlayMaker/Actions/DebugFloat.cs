namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Float Variable in the PlayMaker Log Window.")]
	public class DebugFloat : FsmStateAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("Prints the value of a Float variable in the PlayMaker log window.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			floatVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!floatVariable.IsNone)
			{
				text = floatVariable.Name + ": " + floatVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
