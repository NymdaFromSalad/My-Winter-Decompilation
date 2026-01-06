using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Return the key for a value ofna PlayMaker hashtable Proxy component. It will return the first entry found.")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class HashTableGetKeyFromValue : HashTableActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker HashTable Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[RequiredField]
		[ActionSection("Value")]
		[Tooltip("The value to search")]
		public FsmVar theValue;

		[UIHint(UIHint.Variable)]
		[Tooltip("The key of that value")]
		[ActionSection("Result")]
		public FsmString result;

		[Tooltip("The event to trigger when value is found")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent KeyFoundEvent;

		[Tooltip("The event to trigger when value is not found")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent KeyNotFoundEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
		}

		public override void OnEnter()
		{
			if (SetUpHashTableProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				SortHashTableByValues();
			}
			Finish();
		}

		public void SortHashTableByValues()
		{
			if (!isProxyValid())
			{
				return;
			}
			object valueFromFsmVar = PlayMakerUtils.GetValueFromFsmVar(base.Fsm, theValue);
			foreach (DictionaryEntry item in proxy.hashTable)
			{
				if (item.Value.Equals(valueFromFsmVar))
				{
					result.Value = (string)item.Key;
					base.Fsm.Event(KeyFoundEvent);
					return;
				}
			}
			base.Fsm.Event(KeyNotFoundEvent);
		}
	}
}
