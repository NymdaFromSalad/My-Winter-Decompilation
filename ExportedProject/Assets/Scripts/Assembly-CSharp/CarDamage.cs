using System;
using UnityEngine;

public class CarDamage : MonoBehaviour
{
	private struct permaVertsColl
	{
		public Vector3[] permaVerts;
	}

	public MeshCollider meshCollider;

	public MeshFilter[] meshFilters;

	private MeshFilter[] m_meshFilters;

	public float deformNoise = 0.005f;

	public float deformRadius = 0.5f;

	private float bounceBackSleepCap = 0.002f;

	public float bounceBackSpeed = 2f;

	private Vector3[] colliderVerts;

	private permaVertsColl[] originalMeshData;

	private bool sleep = true;

	public float maxDeform = 0.5f;

	public float minForce = 1f;

	public float multiplier = 0.1f;

	public float YforceDamp = 1f;

	[HideInInspector]
	public bool repair;

	private Vector3 vec;

	private Transform trs;

	private Transform myTransform;

	private GameObject GObody;

	private CarDynamics carDynamics;

	private Axles axles;

	private Rigidbody body;

	public float sign = 1f;

	private Quaternion rot = Quaternion.identity;

	private int wheelLayer;

	private int carLayer;

	private int i;

	private void Start()
	{
		myTransform = base.transform;
		body = GetComponent<Rigidbody>();
		if (sign < -1f || sign > 1f)
		{
			sign = 1f;
		}
		if (meshFilters.Length == 0)
		{
			m_meshFilters = GetComponentsInChildren<MeshFilter>();
			int num = 0;
			for (i = 0; i < m_meshFilters.Length; i++)
			{
				if (m_meshFilters[i].GetComponent<Collider>() == null)
				{
					num++;
				}
			}
			meshFilters = new MeshFilter[num];
			num = 0;
			for (i = 0; i < m_meshFilters.Length; i++)
			{
				if (m_meshFilters[i].GetComponent<Collider>() == null)
				{
					meshFilters[num] = m_meshFilters[i];
					num++;
				}
			}
		}
		if (meshCollider != null)
		{
			colliderVerts = meshCollider.sharedMesh.vertices;
		}
		LoadoriginalMeshData();
		foreach (Transform item in base.transform)
		{
			if (item.gameObject.tag == "Body" || item.gameObject.name == "Body" || item.gameObject.name == "body")
			{
				GObody = item.gameObject;
			}
		}
		if ((bool)GObody)
		{
			if (sign == 0f)
			{
				sign = Mathf.Cos(GObody.transform.localEulerAngles.y * ((float)Math.PI / 180f));
			}
			if (GObody.transform.localEulerAngles.x != 0f)
			{
				rot = Quaternion.AngleAxis(GObody.transform.localEulerAngles.x * 3f, Vector3.right);
			}
		}
		carDynamics = GetComponent<CarDynamics>();
		axles = GetComponent<Axles>();
		wheelLayer = axles.allWheels[0].transform.gameObject.layer;
		carLayer = base.transform.gameObject.layer;
	}

	private void LoadoriginalMeshData()
	{
		originalMeshData = new permaVertsColl[meshFilters.Length];
		for (i = 0; i < meshFilters.Length; i++)
		{
			originalMeshData[i].permaVerts = meshFilters[i].mesh.vertices;
		}
	}

	private void Update()
	{
		if (!sleep && repair && bounceBackSpeed > 0f)
		{
			sleep = true;
			for (int i = 0; i < meshFilters.Length; i++)
			{
				Vector3[] vertices = meshFilters[i].mesh.vertices;
				if (originalMeshData == null)
				{
					LoadoriginalMeshData();
				}
				for (int j = 0; j < vertices.Length; j++)
				{
					vertices[j] += (originalMeshData[i].permaVerts[j] - vertices[j]) * (Time.deltaTime * bounceBackSpeed);
					if ((originalMeshData[i].permaVerts[j] - vertices[j]).magnitude >= bounceBackSleepCap)
					{
						sleep = false;
					}
				}
				meshFilters[i].mesh.vertices = vertices;
				meshFilters[i].mesh.RecalculateNormals();
				meshFilters[i].mesh.RecalculateBounds();
			}
			if (meshCollider != null)
			{
				Mesh mesh = new Mesh();
				mesh.vertices = colliderVerts;
				mesh.triangles = meshCollider.sharedMesh.triangles;
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				meshCollider.sharedMesh = mesh;
				body.centerOfMass = carDynamics.centerOfMass.localPosition;
			}
			if (sleep)
			{
				repair = false;
			}
		}
		if (Application.isEditor)
		{
			YforceDamp = Mathf.Clamp01(YforceDamp);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.contacts.Length <= 0 || !(myTransform != null))
		{
			return;
		}
		Vector3 relativeVelocity = collision.relativeVelocity;
		relativeVelocity *= 1f - Mathf.Abs(Vector3.Dot(myTransform.up, collision.contacts[0].normal)) * YforceDamp;
		float num = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, relativeVelocity.normalized));
		if (!(relativeVelocity.magnitude * num >= minForce))
		{
			return;
		}
		sleep = false;
		vec = myTransform.InverseTransformDirection(relativeVelocity) * multiplier * 0.1f;
		if (originalMeshData == null)
		{
			LoadoriginalMeshData();
		}
		for (int i = 0; i < meshFilters.Length; i++)
		{
			if (meshFilters[i].gameObject.layer != wheelLayer || carLayer == wheelLayer)
			{
				DeformMesh(meshFilters[i].mesh, originalMeshData[i].permaVerts, collision, num, meshFilters[i].transform, sign, rot);
			}
		}
		if (meshCollider != null)
		{
			Mesh mesh = new Mesh();
			mesh.vertices = meshCollider.sharedMesh.vertices;
			mesh.triangles = meshCollider.sharedMesh.triangles;
			DeformMesh(mesh, colliderVerts, collision, num, meshCollider.transform, 1f, Quaternion.identity);
			meshCollider.sharedMesh = mesh;
			meshCollider.sharedMesh.RecalculateNormals();
			meshCollider.sharedMesh.RecalculateBounds();
			body.centerOfMass = carDynamics.centerOfMass.localPosition;
		}
	}

	private void DeformMesh(Mesh mesh, Vector3[] originalMesh, Collision collision, float cos, Transform meshTransform, float sign, Quaternion rot)
	{
		Vector3[] vertices = mesh.vertices;
		ContactPoint[] contacts = collision.contacts;
		foreach (ContactPoint contactPoint in contacts)
		{
			Vector3 vector = meshTransform.InverseTransformPoint(contactPoint.point);
			for (int j = 0; j < vertices.Length; j++)
			{
				if ((vector - vertices[j]).magnitude < deformRadius)
				{
					vertices[j] += rot * (vec * (deformRadius - (vector - vertices[j]).magnitude) / deformRadius * cos + UnityEngine.Random.onUnitSphere * deformNoise) * sign;
					if (maxDeform > 0f && (vertices[j] - originalMesh[j]).magnitude > maxDeform)
					{
						vertices[j] = originalMesh[j] + (vertices[j] - originalMesh[j]).normalized * maxDeform;
					}
				}
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
}
