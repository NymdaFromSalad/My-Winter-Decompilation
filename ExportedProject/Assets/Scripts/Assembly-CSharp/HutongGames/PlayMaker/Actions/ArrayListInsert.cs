using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Insert item at a specified index of a PlayMaker ArrayList Proxy component")]
	[ActionCategory("ArrayMaker/ArrayList")]
	public class ArrayListInsert : ArrayListActions
	{
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[RequiredField]
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[UIHint(UIHint.FsmInt)]
		[Tooltip("The index to remove at")]
		public FsmInt index;

		[Tooltip("The variable to add.")]
		[RequiredField]
		[ActionSection("Data")]
		public FsmVar variable;

		[Tooltip("The event to trigger if the removeAt throw errors")]
		[UIHint(UIHint.FsmEvent)]
		[ActionSection("Result")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			variable = null;
			failureEvent = null;
			index = null;
		}

		public override void OnEnter()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				doArrayListInsert();
			}
			Finish();
		}

		public void doArrayListInsert()
		{
			if (!isProxyValid())
			{
				return;
			}
			try
			{
				proxy.arrayList.Insert(index.Value, PlayMakerUtils.GetValueFromFsmVar(base.Fsm, variable));
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				base.Fsm.Event(failureEvent);
			}
		}
	}
}
