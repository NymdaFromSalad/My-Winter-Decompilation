namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Gets the input name of an cInput action and puts it into a fsm variable.")]
	public class CInputGetText : FsmStateAction
	{
		[Tooltip("The name of the action.")]
		[RequiredField]
		public FsmString actionName;

		[Tooltip("0 = actionName, 1 = primary, 2 = secondary")]
		public FsmInt input;

		public FsmBool returnBlank;

		[Tooltip("The input name to store.")]
		public FsmString storeInputName;

		[Tooltip("update every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			actionName = null;
			input = null;
			returnBlank = null;
			storeInputName = null;
			everyFrame = null;
		}

		public override void OnLateUpdate()
		{
			storeInputName.Value = cInput.GetText(actionName.Value, input.Value, returnBlank.Value);
			if (!everyFrame.Value)
			{
				Finish();
			}
		}
	}
}
