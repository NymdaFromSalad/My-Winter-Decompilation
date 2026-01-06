namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Edit a key from a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableEditKey : HashTableActions
	{
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[ActionSection("Set up")]
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[RequiredField]
		[Tooltip("The Key value to edit")]
		[UIHint(UIHint.FsmString)]
		public FsmString key;

		[UIHint(UIHint.FsmString)]
		[RequiredField]
		[Tooltip("The Key value to edit")]
		public FsmString newKey;

		[Tooltip("Event sent if this HashTable key does not exists")]
		[UIHint(UIHint.FsmEvent)]
		[ActionSection("Result")]
		public FsmEvent keyNotFoundEvent;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("Event sent if this HashTable already contains the new key")]
		public FsmEvent newKeyExistsAlreadyEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			key = null;
			newKey = null;
			keyNotFoundEvent = null;
			newKeyExistsAlreadyEvent = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				EditHashTableKey();
			}
			Finish();
		}

		public void EditHashTableKey()
		{
			if (isProxyValid())
			{
				if (!proxy.hashTable.ContainsKey(key.Value))
				{
					base.Fsm.Event(keyNotFoundEvent);
					return;
				}
				if (proxy.hashTable.ContainsKey(newKey.Value))
				{
					base.Fsm.Event(newKeyExistsAlreadyEvent);
					return;
				}
				object value = proxy.hashTable[key.Value];
				proxy.hashTable[newKey.Value] = value;
				proxy.hashTable.Remove(key.Value);
			}
		}
	}
}
