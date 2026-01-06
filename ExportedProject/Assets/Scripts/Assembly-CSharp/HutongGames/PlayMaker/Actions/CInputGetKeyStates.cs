namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("stores key states in fsm variables")]
	[ActionCategory("cInput")]
	public class CInputGetKeyStates : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The name of the key.")]
		public FsmString keyName;

		[Tooltip("key gets down.")]
		public FsmBool storeKeyDown;

		[Tooltip("Key gets up.")]
		public FsmBool storeKeyUp;

		[Tooltip("Key is still pressed.")]
		public FsmBool storeKeyPressed;

		[Tooltip("update every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			keyName = null;
			storeKeyDown = null;
			storeKeyUp = null;
			storeKeyPressed = null;
			everyFrame = null;
		}

		public override void OnLateUpdate()
		{
			storeKeyDown.Value = cInput.GetKeyDown(keyName.Value);
			storeKeyUp.Value = cInput.GetKeyUp(keyName.Value);
			storeKeyPressed.Value = cInput.GetKey(keyName.Value);
			if (!everyFrame.Value)
			{
				Finish();
			}
		}
	}
}
