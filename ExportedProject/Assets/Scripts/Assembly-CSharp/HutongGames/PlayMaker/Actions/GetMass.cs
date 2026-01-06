using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Mass of a Game Object's Rigid Body.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetMass : ComponentAction<Rigidbody>
	{
		[CheckForComponent(typeof(Rigidbody))]
		[Tooltip("The GameObject that owns the Rigidbody")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Store the mass in a float variable.")]
		public FsmFloat storeResult;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoGetMass();
			Finish();
		}

		private void DoGetMass()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				storeResult.Value = base.rigidbody.mass;
			}
		}
	}
}
