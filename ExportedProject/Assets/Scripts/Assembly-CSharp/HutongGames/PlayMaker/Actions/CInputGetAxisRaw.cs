namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the raw axis input and stores it in a fsm variable.")]
	[ActionCategory("cInput")]
	public class CInputGetAxisRaw : FsmStateAction
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
				storeAxisValue.Value = cInput.GetAxisRaw(axisName.Value);
				if (!everyFrame.Value)
				{
					Finish();
				}
			}
		}
	}
}
