namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Bool Variable in the PlayMaker Log Window.")]
	public class DebugBool : FsmStateAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("Prints the value of a Bool variable in the PlayMaker log window.")]
		[UIHint(UIHint.Variable)]
		public FsmBool boolVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			boolVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!boolVariable.IsNone)
			{
				text = boolVariable.Name + ": " + boolVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
