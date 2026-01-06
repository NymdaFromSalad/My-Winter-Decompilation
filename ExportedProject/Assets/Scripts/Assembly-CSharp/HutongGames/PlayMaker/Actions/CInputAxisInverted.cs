namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Set if an axis is inverted.")]
	public class CInputAxisInverted : FsmStateAction
	{
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("Invert the axis.")]
		public FsmBool invertAxis;

		public override void Reset()
		{
			invertAxis = null;
		}

		public override void OnUpdate()
		{
			cInput.AxisInverted(axisName.Value, invertAxis.Value);
		}
	}
}
