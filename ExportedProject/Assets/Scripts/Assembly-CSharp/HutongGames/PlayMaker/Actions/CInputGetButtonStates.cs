namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Gets cInput button states to store them.")]
	public class CInputGetButtonStates : FsmStateAction
	{
		[Tooltip("The name of the button.")]
		[RequiredField]
		public FsmString buttonName;

		[Tooltip("store if the button is down.")]
		public FsmBool storeButtonDown;

		[Tooltip("store if the button is up.")]
		public FsmBool storeButtonUp;

		[Tooltip("store if the button is pressed.")]
		public FsmBool storeButtonPressed;

		[Tooltip("update every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			buttonName = null;
			storeButtonDown = null;
			storeButtonUp = null;
			storeButtonPressed = null;
			everyFrame = null;
		}

		public override void OnLateUpdate()
		{
			storeButtonDown.Value = cInput.GetKeyDown(buttonName.Value);
			storeButtonUp.Value = cInput.GetButtonUp(buttonName.Value);
			storeButtonPressed.Value = cInput.GetButton(buttonName.Value);
			if (!everyFrame.Value)
			{
				Finish();
			}
		}
	}
}
