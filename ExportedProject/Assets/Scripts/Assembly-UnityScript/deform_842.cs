using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class deform_842 : MonoBehaviour
{
	public float minForce;

	public float multiplier;

	public float deformRadius;

	public float maxDeform;

	public float bounceBackSpeed;

	public float bounceBackSleepCap;

	public bool onCollision;

	public bool onCall;

	public bool updateCollider;

	public bool updateColliderOnBounce;

	private Mesh mesh;

	private Vector3[] permaVerts;

	private bool sleep;

	public deform_842()
	{
		minForce = 1f;
		multiplier = 0.1f;
		deformRadius = 1f;
		bounceBackSleepCap = 0.001f;
		onCollision = true;
		onCall = true;
		sleep = true;
	}

	public virtual void OnCollisionEnter(Collision collision)
	{
		if (!onCollision || collision.relativeVelocity.magnitude < minForce)
		{
			return;
		}
		sleep = false;
		Vector3[] vertices = mesh.vertices;
		Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;
		for (int i = 0; i < ((System.Array)vertices).Length; i++)
		{
			int j = 0;
			ContactPoint[] contacts = collision.contacts;
			for (int length = contacts.Length; j < length; j++)
			{
				Vector3 vector = worldToLocalMatrix.MultiplyPoint(contacts[j].point);
				Vector3 vector2 = worldToLocalMatrix.MultiplyVector(collision.relativeVelocity * UsedMass(collision));
				if (!((vector - vertices[i]).magnitude >= deformRadius))
				{
					vertices[i] += vector2 * (deformRadius - (vector - vertices[i]).magnitude) / deformRadius * multiplier;
					if (!(maxDeform <= 0f) && !((vertices[i] - permaVerts[i]).magnitude <= maxDeform))
					{
						vertices[i] = permaVerts[i] + (vertices[i] - permaVerts[i]).normalized * maxDeform;
					}
				}
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		if (updateCollider)
		{
			((MeshCollider)GetComponent(typeof(MeshCollider))).sharedMesh = mesh;
		}
	}

	public virtual void Deform(Vector3 point, Vector3 direction)
	{
		if (!onCall || direction.magnitude < minForce)
		{
			return;
		}
		sleep = false;
		Vector3[] vertices = mesh.vertices;
		Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;
		point = worldToLocalMatrix.MultiplyPoint(point);
		Vector3 vector = worldToLocalMatrix.MultiplyVector(direction);
		for (int i = 0; i < ((System.Array)vertices).Length; i++)
		{
			if (!((point - vertices[i]).magnitude > deformRadius))
			{
				vertices[i] += vector * (deformRadius - (point - vertices[i]).magnitude) / deformRadius * multiplier;
				if (!(maxDeform <= 0f) && !((vertices[i] - permaVerts[i]).magnitude <= maxDeform))
				{
					vertices[i] = permaVerts[i] + (vertices[i] - permaVerts[i]).normalized * maxDeform;
				}
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		if (updateCollider)
		{
			((MeshCollider)GetComponent(typeof(MeshCollider))).sharedMesh = mesh;
		}
	}

	public virtual void Update()
	{
		if (sleep || bounceBackSpeed <= 0f)
		{
			return;
		}
		sleep = true;
		Vector3[] vertices = mesh.vertices;
		for (int i = 0; i < ((System.Array)vertices).Length; i++)
		{
			vertices[i] += (permaVerts[i] - vertices[i]) * (Time.deltaTime * bounceBackSpeed);
			if (!((permaVerts[i] - vertices[i]).magnitude < bounceBackSleepCap))
			{
				sleep = false;
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		if (updateColliderOnBounce)
		{
			((MeshCollider)GetComponent(typeof(MeshCollider))).sharedMesh = mesh;
		}
	}

	public virtual float UsedMass(Collision collision)
	{
		return collision.rigidbody ? ((!GetComponent<Rigidbody>()) ? collision.rigidbody.mass : ((collision.rigidbody.mass <= GetComponent<Rigidbody>().mass) ? GetComponent<Rigidbody>().mass : collision.rigidbody.mass)) : ((!GetComponent<Rigidbody>()) ? 1f : GetComponent<Rigidbody>().mass);
	}

	public virtual void Main()
	{
		mesh = ((MeshFilter)GetComponent(typeof(MeshFilter))).mesh;
		if (!(MeshCollider)GetComponent(typeof(MeshCollider)))
		{
			updateCollider = false;
			bool flag = false;
		}
		permaVerts = ((MeshFilter)GetComponent(typeof(MeshFilter))).mesh.vertices;
	}
}
