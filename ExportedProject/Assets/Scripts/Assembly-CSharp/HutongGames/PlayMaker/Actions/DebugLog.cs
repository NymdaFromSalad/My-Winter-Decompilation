namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends a log message to the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugLog : FsmStateAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("Text to print to the PlayMaker log window.")]
		public FsmString text;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			text = string.Empty;
		}

		public override void OnEnter()
		{
			if (!string.IsNullOrEmpty(text.Value))
			{
				ActionHelpers.DebugLog(base.Fsm, logLevel, text.Value);
			}
			Finish();
		}
	}
}
