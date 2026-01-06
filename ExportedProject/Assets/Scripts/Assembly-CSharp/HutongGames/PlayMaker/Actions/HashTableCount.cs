namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Count the number of items ( key/value pairs) in a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy).")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableCount : HashTableActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[RequiredField]
		[Tooltip("The number of items in that hashTable")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmInt count;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			count = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				doHashTableCount();
			}
			Finish();
		}

		public void doHashTableCount()
		{
			if (isProxyValid())
			{
				count.Value = proxy.hashTable.Count;
			}
		}
	}
}
