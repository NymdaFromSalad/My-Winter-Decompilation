namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sorts the sequence of elements in a PlayMaker ArrayList Proxy component")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListSort : ArrayListActions
	{
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Set up")]
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
				DoArrayListSort();
			}
			Finish();
		}

		public void DoArrayListSort()
		{
			if (isProxyValid())
			{
				proxy.arrayList.Sort();
			}
		}
	}
}
