using UnityEngine;

namespace SWS
{
	public class MoveAnimator : MonoBehaviour
	{
		public enum MovementType
		{
			splineMove = 0,
			bezierMove = 1,
			navMove = 2
		}

		public MovementType mType;

		private splineMove hMove;

		private bezierMove bMove;

		private NavMeshAgent nAgent;

		private Animator animator;

		private float lastRotY;

		private void Start()
		{
			animator = GetComponentInChildren<Animator>();
			switch (mType)
			{
			case MovementType.splineMove:
				hMove = GetComponent<splineMove>();
				break;
			case MovementType.bezierMove:
				bMove = GetComponent<bezierMove>();
				break;
			case MovementType.navMove:
				nAgent = GetComponent<NavMeshAgent>();
				break;
			}
		}

		private void OnAnimatorMove()
		{
			float value = 0f;
			float value2 = 0f;
			switch (mType)
			{
			case MovementType.splineMove:
				value = ((hMove.tween != null && !hMove.tween.isPaused) ? hMove.speed : 0f);
				value2 = (base.transform.eulerAngles.y - lastRotY) * 10f;
				lastRotY = base.transform.eulerAngles.y;
				break;
			case MovementType.bezierMove:
				value = ((bMove.tween != null && !bMove.tween.isPaused) ? bMove.speed : 0f);
				value2 = (base.transform.eulerAngles.y - lastRotY) * 10f;
				lastRotY = base.transform.eulerAngles.y;
				break;
			case MovementType.navMove:
			{
				value = nAgent.velocity.magnitude;
				Vector3 vector = Quaternion.Inverse(base.transform.rotation) * nAgent.desiredVelocity;
				value2 = Mathf.Atan2(vector.x, vector.z) * 180f / 3.14159f;
				break;
			}
			}
			animator.SetFloat("Speed", value);
			animator.SetFloat("Direction", value2, 0.15f, Time.deltaTime);
		}
	}
}
