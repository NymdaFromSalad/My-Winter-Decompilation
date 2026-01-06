namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Revert a PlayMaker ArrayList Proxy component to the prefill data, either defined at runtime or when the action ArrayListTakeSnapShot was used. ")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListRevertToSnapShot : ArrayListActions
	{
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				DoArrayListRevertToSnapShot();
			}
			Finish();
		}

		public void DoArrayListRevertToSnapShot()
		{
			if (isProxyValid())
			{
				proxy.RevertToSnapShot();
			}
		}
	}
}
