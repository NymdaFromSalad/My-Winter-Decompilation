using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Destroys a PlayMakerHashTableProxy Component of a Game Object.")]
	[ActionCategory("ArrayMaker/HashTable")]
	public class DestroyHashTable : HashTableActions
	{
		[Tooltip("The gameObject with the PlayMaker HashTable Proxy component")]
		[CheckForComponent(typeof(PlayMakerHashTableProxy))]
		[ActionSection("Set up")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker HashTable proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the HashTable proxy component is destroyed")]
		[ActionSection("Result")]
		public FsmEvent successEvent;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the HashTable proxy component was not found")]
		public FsmEvent notFoundEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			successEvent = null;
			notFoundEvent = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (SetUpHashTableProxyPointer(ownerDefaultTarget, reference.Value))
			{
				DoDestroyHashTable(ownerDefaultTarget);
			}
			else
			{
				base.Fsm.Event(notFoundEvent);
			}
			Finish();
		}

		private void DoDestroyHashTable(GameObject go)
		{
			PlayMakerHashTableProxy[] components = proxy.GetComponents<PlayMakerHashTableProxy>();
			PlayMakerHashTableProxy[] array = components;
			foreach (PlayMakerHashTableProxy playMakerHashTableProxy in array)
			{
				if (playMakerHashTableProxy.referenceName == reference.Value)
				{
					Object.Destroy(playMakerHashTableProxy);
					base.Fsm.Event(successEvent);
					return;
				}
			}
			base.Fsm.Event(notFoundEvent);
		}
	}
}
