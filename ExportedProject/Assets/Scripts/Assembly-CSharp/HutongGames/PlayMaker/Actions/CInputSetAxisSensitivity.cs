namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Sets the sensitivity of an axis.")]
	public class CInputSetAxisSensitivity : FsmStateAction
	{
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("The name of the axis deadzone.")]
		public FsmFloat axisSensitivity;

		public override void Reset()
		{
			axisName = null;
			axisSensitivity = null;
		}

		public override void OnEnter()
		{
			cInput.SetAxisSensitivity(axisName.Value, axisSensitivity.Value);
			Finish();
		}
	}
}
