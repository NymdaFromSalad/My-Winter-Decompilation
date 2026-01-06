namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Gets the gravity of an axis and stores it in a fsm variable.")]
	public class CInputGetAxisGravity : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("The value of the axis gravity.")]
		public FsmFloat storeAxisGravity;

		[Tooltip("Get axis value every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			axisName = null;
			storeAxisGravity = null;
		}

		public override void OnUpdate()
		{
			storeAxisGravity.Value = cInput.GetAxisGravity(axisName.Value);
			if (!everyFrame.Value)
			{
				Finish();
			}
		}
	}
}
