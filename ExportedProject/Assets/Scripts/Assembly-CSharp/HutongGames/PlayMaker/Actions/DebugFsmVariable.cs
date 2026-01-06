namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Print the value of an FSM Variable in the PlayMaker Log Window.")]
	public class DebugFsmVariable : FsmStateAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("Variable to print to the PlayMaker log window.")]
		[HideTypeFilter]
		[UIHint(UIHint.Variable)]
		public FsmVar fsmVar;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			fsmVar = null;
		}

		public override void OnEnter()
		{
			ActionHelpers.DebugLog(base.Fsm, logLevel, fsmVar.DebugString());
			Finish();
		}
	}
}
