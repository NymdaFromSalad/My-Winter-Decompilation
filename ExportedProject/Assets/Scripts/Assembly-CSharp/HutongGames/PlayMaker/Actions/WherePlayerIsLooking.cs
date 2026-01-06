using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("\u00b4Where player is looking at")]
	[ActionCategory("♥ Caligula ♥")]
	public class WherePlayerIsLooking : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Direction to look")]
		public FsmVector3 direction;

		private Ray ray = default(Ray);

		public override void OnUpdate()
		{
			ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			direction.Value = ray.direction;
		}
	}
}
