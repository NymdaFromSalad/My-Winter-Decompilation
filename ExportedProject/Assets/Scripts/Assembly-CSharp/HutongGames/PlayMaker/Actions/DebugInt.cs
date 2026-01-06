namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of an Integer Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugInt : FsmStateAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("Prints the value of an Int variable in the PlayMaker log window.")]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			intVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!intVariable.IsNone)
			{
				text = intVariable.Name + ": " + intVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
