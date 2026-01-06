namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Sets if a axis is inverted or not inverted.")]
	public class CInputSetAxisInverted : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("True = axis is inverted.")]
		public FsmBool axisInverted;

		[Tooltip("Stores the result if needed")]
		public FsmBool storeResult;

		[Tooltip("Execute every frame.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			axisName = null;
			axisInverted = null;
			storeResult = null;
			everyFrame = null;
		}

		public override void OnUpdate()
		{
			storeResult.Value = cInput.AxisInverted(axisName.Value, axisInverted.Value);
			if (!everyFrame.Value)
			{
				Finish();
			}
		}
	}
}
