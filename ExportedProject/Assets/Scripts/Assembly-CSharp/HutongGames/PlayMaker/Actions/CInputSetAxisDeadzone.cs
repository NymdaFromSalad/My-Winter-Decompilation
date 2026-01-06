namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Sets the deadzone of an axis.")]
	public class CInputSetAxisDeadzone : FsmStateAction
	{
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("The name of the axis deadzone.")]
		public FsmFloat axisDeadzone;

		public override void Reset()
		{
			axisName = null;
			axisDeadzone = null;
		}

		public override void OnEnter()
		{
			cInput.SetAxisDeadzone(axisName.Value, axisDeadzone.Value);
			Finish();
		}
	}
}
