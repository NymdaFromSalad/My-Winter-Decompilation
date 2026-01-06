namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("cInput")]
	[Tooltip("Waits for key inputs and sends events in hierarchy keyDown -> keyUp -> any key pressed")]
	public class CInputGetKey : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The name of the key.")]
		public FsmString keyName;

		[Tooltip("key gets down.")]
		public FsmEvent keyDownEvent;

		[Tooltip("Key gets up.")]
		public FsmEvent keyUpEvent;

		[Tooltip("Key is still pressed.")]
		public FsmEvent keyPressedEvent;

		[Tooltip("update every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			keyName = null;
			keyDownEvent = null;
			keyUpEvent = null;
			keyPressedEvent = null;
			everyFrame = null;
		}

		public override void OnLateUpdate()
		{
			if (cInput.GetKeyDown(keyName.Value))
			{
				base.Fsm.Event(keyDownEvent);
			}
			else if (cInput.GetKeyUp(keyName.Value))
			{
				base.Fsm.Event(keyUpEvent);
			}
			else if (cInput.GetKey(keyName.Value))
			{
				base.Fsm.Event(keyPressedEvent);
			}
			if (!everyFrame.Value)
			{
				Finish();
			}
		}
	}
}
