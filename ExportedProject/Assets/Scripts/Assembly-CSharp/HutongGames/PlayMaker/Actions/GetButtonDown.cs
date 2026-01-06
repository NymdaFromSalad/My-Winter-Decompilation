namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when a Button is pressed.")]
	public class GetButtonDown : FsmStateAction
	{
		[Tooltip("The name of the button. Set in the Unity Input Manager.")]
		[RequiredField]
		public FsmString buttonName;

		[Tooltip("Event to send if the button is pressed.")]
		public FsmEvent sendEvent;

		[Tooltip("Set to True if the button is pressed.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public override void Reset()
		{
			buttonName = "Fire1";
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool keyDown = cInput.GetKeyDown(buttonName.Value);
			if (keyDown)
			{
				base.Fsm.Event(sendEvent);
			}
			storeResult.Value = keyDown;
		}
	}
}
