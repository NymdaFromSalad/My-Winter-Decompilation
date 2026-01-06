namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set key/value pairs to a PlayMaker HashTable Proxy component (PlayMakerHashTableProxy)")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableSetMany : HashTableActions
	{
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[CompoundArray("Count", "Key", "Value")]
		[ActionSection("Data")]
		[RequiredField]
		[UIHint(UIHint.FsmString)]
		[Tooltip("The Key values for that hash set")]
		public FsmString[] keys;

		[Tooltip("The variable to set.")]
		public FsmVar[] variables;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			keys = null;
			variables = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				SetHashTable();
			}
			Finish();
		}

		public void SetHashTable()
		{
			if (isProxyValid())
			{
				for (int i = 0; i < keys.Length; i++)
				{
					proxy.hashTable[keys[i].Value] = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variables[i]);
				}
			}
		}
	}
}
