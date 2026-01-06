using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Destroys a PlayMakerArrayListProxy Component of a Game Object.")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class DestroyArrayList : ArrayListActions
	{
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Set up")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmString)]
		[Tooltip("Author defined Reference of the PlayMaker ArrayList proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("The event to trigger if the ArrayList proxy component is destroyed")]
		[UIHint(UIHint.FsmEvent)]
		[ActionSection("Result")]
		public FsmEvent successEvent;

		[Tooltip("The event to trigger if the ArrayList proxy component was not found")]
		[UIHint(UIHint.FsmEvent)]
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
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				DoDestroyArrayList();
			}
			else
			{
				base.Fsm.Event(notFoundEvent);
			}
			Finish();
		}

		private void DoDestroyArrayList()
		{
			PlayMakerArrayListProxy[] components = proxy.GetComponents<PlayMakerArrayListProxy>();
			PlayMakerArrayListProxy[] array = components;
			foreach (PlayMakerArrayListProxy playMakerArrayListProxy in array)
			{
				if (playMakerArrayListProxy.referenceName == reference.Value)
				{
					Object.Destroy(playMakerArrayListProxy);
					base.Fsm.Event(successEvent);
					return;
				}
			}
			base.Fsm.Event(notFoundEvent);
		}
	}
}
