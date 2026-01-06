using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get material mass from the rigidbody")]
	[ActionCategory("♥ Caligula ♥")]
	public class GetRigidbodyMass : FsmStateAction
	{
		[Tooltip("Object")]
		public FsmGameObject go;

		[Tooltip("Mass of the rigidbody")]
		public FsmFloat mass;

		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			mass.Value = getMass(go.Value);
			Finish();
		}

		public override void OnUpdate()
		{
		}

		private float getMass(GameObject go)
		{
			float result = 0f;
			if (go.GetComponent<Rigidbody>() != null)
			{
				result = go.GetComponent<Rigidbody>().mass;
			}
			return result;
		}
	}
}
