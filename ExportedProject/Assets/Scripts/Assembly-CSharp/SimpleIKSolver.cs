using System;
using UnityEngine;

public class SimpleIKSolver : MonoBehaviour
{
	[Serializable]
	public class JointEntity
	{
		public Transform Joint;

		public AngleRestriction AngleRestrictionRange;

		internal Quaternion _initialRotation;
	}

	[Serializable]
	public class AngleRestriction
	{
		public bool xAxis;

		public float xMin = -180f;

		public float xMax = 180f;

		public bool yAxis;

		public float yMin = -180f;

		public float yMax = 180f;

		public bool zAxis;

		public float zMin = 180f;

		public float zMax = 180f;
	}

	private const float IK_POS_THRESH = 0.00125f;

	private const int MAX_IK_TRIES = 20;

	public bool IsActive = true;

	public Transform Target;

	public JointEntity[] JointEntities;

	public bool IsDamping;

	public float DampingMax = 0.5f;

	private void Start()
	{
		if (Target == null)
		{
			Target = base.transform;
		}
		JointEntity[] jointEntities = JointEntities;
		foreach (JointEntity jointEntity in jointEntities)
		{
			jointEntity._initialRotation = jointEntity.Joint.localRotation;
		}
	}

	private void LateUpdate()
	{
		if (IsActive)
		{
			Solve();
		}
	}

	private void Solve()
	{
		Transform joint = JointEntities[JointEntities.Length - 1].Joint;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		Vector3 zero3 = Vector3.zero;
		int num = JointEntities.Length - 1;
		int num2 = 0;
		Vector3 position2;
		do
		{
			if (num < 0)
			{
				num = JointEntities.Length - 1;
			}
			Vector3 position = JointEntities[num].Joint.position;
			position2 = joint.position;
			zero2 = position2 - position;
			zero = Target.position - position;
			zero2.Normalize();
			zero.Normalize();
			float num3 = Vector3.Dot(zero2, zero);
			if (num3 < 0.99999f)
			{
				zero3 = Vector3.Cross(zero2, zero);
				zero3.Normalize();
				float num4 = Mathf.Acos(num3);
				if (IsDamping && num4 > DampingMax)
				{
					num4 = DampingMax;
				}
				num4 *= 57.29578f;
				JointEntities[num].Joint.rotation = Quaternion.AngleAxis(num4, zero3) * JointEntities[num].Joint.rotation;
				CheckAngleRestrictions(JointEntities[num]);
			}
			num--;
		}
		while (num2++ < 20 && (position2 - Target.position).sqrMagnitude > 0.00125f);
	}

	private void CheckAngleRestrictions(JointEntity jointEntity)
	{
		Vector3 eulerAngles = jointEntity.Joint.localRotation.eulerAngles;
		if (jointEntity.AngleRestrictionRange.xAxis)
		{
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			eulerAngles.x = Mathf.Clamp(eulerAngles.x, jointEntity.AngleRestrictionRange.xMin, jointEntity.AngleRestrictionRange.xMax);
		}
		if (jointEntity.AngleRestrictionRange.yAxis)
		{
			if (eulerAngles.y > 180f)
			{
				eulerAngles.y -= 360f;
			}
			eulerAngles.y = Mathf.Clamp(eulerAngles.y, jointEntity.AngleRestrictionRange.yMin, jointEntity.AngleRestrictionRange.yMax);
		}
		if (jointEntity.AngleRestrictionRange.zAxis)
		{
			if (eulerAngles.z > 180f)
			{
				eulerAngles.z -= 360f;
			}
			eulerAngles.z = Mathf.Clamp(eulerAngles.z, jointEntity.AngleRestrictionRange.zMin, jointEntity.AngleRestrictionRange.zMax);
		}
		jointEntity.Joint.localEulerAngles = eulerAngles;
	}

	public void ResetJoints()
	{
		JointEntity[] jointEntities = JointEntities;
		foreach (JointEntity jointEntity in jointEntities)
		{
			jointEntity.Joint.localRotation = jointEntity._initialRotation;
		}
	}
}
