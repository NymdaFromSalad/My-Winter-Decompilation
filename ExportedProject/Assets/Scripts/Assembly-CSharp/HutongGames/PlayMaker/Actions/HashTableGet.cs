namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/HashTable")]
	[Tooltip("Gets an item from a PlayMaker HashTable Proxy component")]
	public class HashTableGet : HashTableActions
	{
		[RequiredField]
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("The Key value for that hash set")]
		[RequiredField]
		[UIHint(UIHint.FsmString)]
		public FsmString key;

		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmVar result;

		[Tooltip("The event to trigger when key is found")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent KeyFoundEvent;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger when key is not found")]
		public FsmEvent KeyNotFoundEvent;

		public override void Reset()
		{
			gameObject = null;
			key = null;
			KeyFoundEvent = null;
			KeyNotFoundEvent = null;
			result = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				Get();
			}
			Finish();
		}

		public void Get()
		{
			if (isProxyValid())
			{
				if (!proxy.hashTable.ContainsKey(key.Value))
				{
					base.Fsm.Event(KeyNotFoundEvent);
					return;
				}
				PlayMakerUtils.ApplyValueToFsmVar(base.Fsm, result, proxy.hashTable[key.Value]);
				base.Fsm.Event(KeyFoundEvent);
			}
		}
	}
}
