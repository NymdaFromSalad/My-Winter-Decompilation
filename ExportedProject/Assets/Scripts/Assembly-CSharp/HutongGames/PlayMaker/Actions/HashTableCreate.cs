using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/HashTable")]
	[Tooltip("Adds a PlayMakerHashTableProxy Component to a Game Object. Use this to create arrayList on the fly instead of during authoring.\n Optionally remove the HashTable component on exiting the state.\n Simply point to existing if the reference exists already.")]
	public class HashTableCreate : HashTableActions
	{
		[ActionSection("Set up")]
		[RequiredField]
		[Tooltip("The Game Object to add the Hashtable Component to.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker arrayList proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("Remove the Component when this State is exited.")]
		public FsmBool removeOnExit;

		[ActionSection("Result")]
		[Tooltip("The event to trigger if the hashtable exists already")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent alreadyExistsEvent;

		private PlayMakerHashTableProxy addedComponent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			alreadyExistsEvent = null;
		}

		public override void OnEnter()
		{
			DoAddPlayMakerHashTable();
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && addedComponent != null)
			{
				Object.Destroy(addedComponent);
			}
		}

		private void DoAddPlayMakerHashTable()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			PlayMakerHashTableProxy hashTableProxyPointer = GetHashTableProxyPointer(ownerDefaultTarget, reference.Value, true);
			if (hashTableProxyPointer != null)
			{
				base.Fsm.Event(alreadyExistsEvent);
				return;
			}
			addedComponent = ownerDefaultTarget.AddComponent<PlayMakerHashTableProxy>();
			if (addedComponent == null)
			{
				Debug.LogError("Can't add PlayMakerHashTableProxy");
			}
			else
			{
				addedComponent.referenceName = reference.Value;
			}
		}
	}
}
