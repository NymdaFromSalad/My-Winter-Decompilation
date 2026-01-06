namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Remove an item by key ( key/value pairs) in a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy).")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableRemove : HashTableActions
	{
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("The item key in that hashTable")]
		[RequiredField]
		public FsmString key;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			key = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				doHashTableRemove();
			}
			Finish();
		}

		public void doHashTableRemove()
		{
			if (isProxyValid())
			{
				proxy.hashTable.Remove(key.Value);
			}
		}
	}
}
