namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Gets a axis sensitivity and stores the value in a fsm variable.")]
	public class CInputGetAxisSensitivity : FsmStateAction
	{
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("The value of the axis.")]
		public FsmFloat storeAxisSensitivity;

		[Tooltip("Get axis value every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			axisName = null;
			storeAxisSensitivity = null;
		}

		public override void OnUpdate()
		{
			if (axisName != null)
			{
				storeAxisSensitivity.Value = cInput.GetAxisSensitivity(axisName.Value);
				if (!everyFrame.Value)
				{
					Finish();
				}
			}
		}
	}
}
