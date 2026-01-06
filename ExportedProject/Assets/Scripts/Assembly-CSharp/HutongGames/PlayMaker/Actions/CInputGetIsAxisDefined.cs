namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Gets if an axis is defined and puts the value into a fsm variable.")]
	public class CInputGetIsAxisDefined : FsmStateAction
	{
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("Store if the axis is defined.")]
		public FsmBool storeAxisIsDefined;

		[Tooltip("Get axis value every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			axisName = null;
		}

		public override void OnUpdate()
		{
			if (axisName != null)
			{
				storeAxisIsDefined.Value = cInput.IsAxisDefined(axisName.Value);
				if (!everyFrame.Value)
				{
					Finish();
				}
			}
		}
	}
}
