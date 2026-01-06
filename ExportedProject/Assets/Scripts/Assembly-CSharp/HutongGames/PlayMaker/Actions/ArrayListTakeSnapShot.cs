namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Takes a PlayMaker ArrayList Proxy component snapshot, use action ArrayListRevertToSnapShot was used. A Snapshot is taken by default at the beginning for the prefill data")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListTakeSnapShot : ArrayListActions
	{
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[RequiredField]
		[ActionSection("Set up")]
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
				DoArrayListTakeSnapShot();
			}
			Finish();
		}

		public void DoArrayListTakeSnapShot()
		{
			if (isProxyValid())
			{
				proxy.TakeSnapShot();
			}
		}
	}
}
