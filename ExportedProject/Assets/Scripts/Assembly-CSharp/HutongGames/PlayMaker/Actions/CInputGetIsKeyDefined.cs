namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets if a cInput key is defined and stores the value in a fsm variable.")]
	[ActionCategory("cInput")]
	public class CInputGetIsKeyDefined : FsmStateAction
	{
		[Tooltip("The name of the key.")]
		public FsmString keyName;

		[Tooltip("Store if the key is defined.")]
		public FsmBool storeKeyIsDefined;

		[Tooltip("Get axis value every frame or finish.")]
		public FsmBool everyFrame;

		public override void Reset()
		{
			keyName = null;
		}

		public override void OnUpdate()
		{
			if (keyName != null)
			{
				storeKeyIsDefined.Value = cInput.IsKeyDefined(keyName.Value);
				if (!everyFrame.Value)
				{
					Finish();
				}
			}
		}
	}
}
