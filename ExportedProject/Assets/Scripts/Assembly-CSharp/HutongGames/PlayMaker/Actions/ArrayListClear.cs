namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Removes all elements from a PlayMaker ArrayList Proxy component")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListClear : ArrayListActions
	{
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
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
				ClearArrayList();
			}
			Finish();
		}

		public void ClearArrayList()
		{
			if (isProxyValid())
			{
				proxy.arrayList.Clear();
			}
		}
	}
}
