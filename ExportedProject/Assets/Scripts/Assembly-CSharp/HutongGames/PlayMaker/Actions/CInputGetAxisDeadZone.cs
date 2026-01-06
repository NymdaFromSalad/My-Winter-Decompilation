namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the axis deadzone and puts it into a fsm variable.")]
	[ActionCategory("cInput")]
	public class CInputGetAxisDeadZone : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The axis name.")]
		public FsmString axisName;

		[Tooltip("Store the result")]
		public FsmFloat storeAxisDeadzone;

		[Tooltip("update every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			axisName = null;
			storeAxisDeadzone = null;
			everyFrame = null;
		}

		public override void OnUpdate()
		{
			storeAxisDeadzone.Value = cInput.GetAxisDeadzone(axisName.Value);
			if (!everyFrame.Value)
			{
				Finish();
			}
		}
	}
}
