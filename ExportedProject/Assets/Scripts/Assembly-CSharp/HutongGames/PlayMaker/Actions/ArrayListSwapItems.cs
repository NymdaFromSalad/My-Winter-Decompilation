using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Swap two items at a specified indexes of a PlayMaker ArrayList Proxy component")]
	public class ArrayListSwapItems : ArrayListActions
	{
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[RequiredField]
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("The first index to swap")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt index1;

		[Tooltip("The second index to swap")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt index2;

		[ActionSection("Result")]
		[Tooltip("The event to trigger if the removeAt throw errors")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			gameObject = null;
			failureEvent = null;
			reference = null;
			index1 = null;
			index2 = null;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				doArrayListSwap();
			}
			Finish();
		}

		public void doArrayListSwap()
		{
			if (!isProxyValid())
			{
				return;
			}
			try
			{
				object value = proxy.arrayList[index2.Value];
				proxy.arrayList[index2.Value] = proxy.arrayList[index1.Value];
				proxy.arrayList[index1.Value] = value;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				base.Fsm.Event(failureEvent);
			}
		}
	}
}
