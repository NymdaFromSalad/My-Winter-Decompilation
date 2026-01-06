namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Gets the value of a cInput axis and stores it in a fsm variable.")]
	public class CInputGetAxis : FsmStateAction
	{
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("The value of the axis.")]
		public FsmFloat storeAxisValue;

		[Tooltip("Get axis value every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			axisName = null;
			storeAxisValue = null;
		}

		public override void OnUpdate()
		{
			if (axisName != null)
			{
				storeAxisValue.Value = cInput.GetAxis(axisName.Value);
				if (!everyFrame.Value)
				{
					Finish();
				}
			}
		}
	}
}
