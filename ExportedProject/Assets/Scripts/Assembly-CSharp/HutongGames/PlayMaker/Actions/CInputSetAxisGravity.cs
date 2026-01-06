namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Set the sensitivity of a axis.")]
	public class CInputSetAxisGravity : FsmStateAction
	{
		[Tooltip("The name of the axis.")]
		public FsmString axisName;

		[Tooltip("The name of the axis deadzone.")]
		public FsmFloat axisGravity;

		public override void Reset()
		{
			axisName = null;
			axisGravity = null;
		}

		public override void OnEnter()
		{
			cInput.SetAxisGravity(axisName.Value, axisGravity.Value);
			Finish();
		}
	}
}
