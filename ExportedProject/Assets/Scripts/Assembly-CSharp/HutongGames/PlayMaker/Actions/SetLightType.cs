using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Set Spot, Directional, or Point Light type.")]
	public class SetLightType : ComponentAction<Light>
	{
		[CheckForComponent(typeof(Light))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public LightType lightType;

		public override void Reset()
		{
			gameObject = null;
			lightType = LightType.Point;
		}

		public override void OnEnter()
		{
			DoSetLightType();
			Finish();
		}

		private void DoSetLightType()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.light.type = lightType;
			}
		}
	}
}
