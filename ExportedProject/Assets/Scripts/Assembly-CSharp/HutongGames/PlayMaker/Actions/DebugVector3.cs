namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Vector3 Variable in the PlayMaker Log Window.")]
	public class DebugVector3 : FsmStateAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("Prints the value of a Vector3 variable in the PlayMaker log window.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			vector3Variable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!vector3Variable.IsNone)
			{
				text = vector3Variable.Name + ": " + vector3Variable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
