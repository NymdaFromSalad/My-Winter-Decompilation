namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("To setup a axis you have to define two keys with the SetKey action before, otherwise you will get an cInput error.")]
	public class CInputSetAxis : FsmStateAction
	{
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("The name of the positive axis value.")]
		public FsmString negativeInput;

		[Tooltip("The name of the negative axis value.")]
		public FsmString positiveInput;

		[Tooltip("The name of the axis sensitivity.")]
		public FsmFloat axisSensitivity;

		[Tooltip("The name of the axis gravity.")]
		public FsmFloat axisGravity;

		[Tooltip("The name of the axis deadzone.")]
		public FsmFloat axisDeadzone;

		public override void Reset()
		{
			axisName = null;
			negativeInput = null;
			positiveInput = null;
			axisSensitivity = null;
			axisGravity = null;
			axisDeadzone = null;
		}

		public override void OnEnter()
		{
			cInput.SetAxis(axisName.Value, negativeInput.Value, positiveInput.Value, axisSensitivity.Value, axisGravity.Value, axisDeadzone.Value);
			Finish();
		}
	}
}
