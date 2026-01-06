using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]
public class Meshinator : MonoBehaviour
{
	public enum CacheOptions
	{
		None = 0,
		CacheAfterCollision = 1,
		CacheOnLoad = 2
	}

	public enum ImpactShapes
	{
		FlatImpact = 0,
		SphericalImpact = 1
	}

	public enum ImpactTypes
	{
		Compression = 0,
		Fracture = 1
	}

	private const int c_FixedUpdateCountToIgnoreCollisions = 3;

	public CacheOptions m_CacheOption = CacheOptions.CacheAfterCollision;

	public ImpactShapes m_ImpactShape;

	public ImpactTypes m_ImpactType;

	public float m_ForceResistance = 10f;

	public float m_MaxForcePerImpact = 12f;

	public float m_ForceMultiplier = 0.25f;

	private bool m_Calculating;

	private Hull m_Hull;

	private bool m_BoundsSet;

	private Bounds m_InitialBounds;

	private bool m_ClearedForCollisions = true;

	private int m_CollisionCount;

	private int m_FixedUpdatesSinceLastCollision;

	public void Start()
	{
		if (m_CacheOption == CacheOptions.CacheOnLoad)
		{
			MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
			if (!(component == null) && !(component.sharedMesh == null))
			{
				m_InitialBounds = component.sharedMesh.bounds;
				m_BoundsSet = true;
				m_Hull = new Hull(component.sharedMesh);
			}
		}
	}

	public void FixedUpdate()
	{
		if (!m_ClearedForCollisions)
		{
			if (m_CollisionCount != 0)
			{
				m_FixedUpdatesSinceLastCollision = 0;
			}
			else
			{
				m_FixedUpdatesSinceLastCollision++;
			}
			if (m_FixedUpdatesSinceLastCollision > 3)
			{
				m_ClearedForCollisions = true;
			}
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		m_CollisionCount++;
		if (!m_ClearedForCollisions || !(collision.impactForceSum.magnitude >= m_ForceResistance))
		{
			return;
		}
		ContactPoint[] contacts = collision.contacts;
		for (int i = 0; i < contacts.Length; i++)
		{
			ContactPoint contactPoint = contacts[i];
			if (contactPoint.otherCollider == collision.collider)
			{
				Impact(contactPoint.point, collision.impactForceSum, m_ImpactShape, m_ImpactType);
				break;
			}
		}
	}

	public void OnCollisionExit()
	{
		m_CollisionCount--;
	}

	public void DelayCollisions()
	{
		m_ClearedForCollisions = false;
	}

	public void Impact(Vector3 point, Vector3 force, ImpactShapes impactShape, ImpactTypes impactType)
	{
		if (!CanDoImpact(point, force))
		{
			return;
		}
		m_Calculating = true;
		InitializeHull();
		if (force.magnitude > m_MaxForcePerImpact)
		{
			force = force.normalized * m_MaxForcePerImpact;
		}
		float num = (force.magnitude - m_ForceResistance) * m_ForceMultiplier;
		if (num <= 0f)
		{
			return;
		}
		Vector3 impactPoint = base.transform.InverseTransformPoint(point);
		Vector3 impactForce = base.transform.InverseTransformDirection(force.normalized) * num;
		float x = Mathf.Max(Mathf.Min(impactForce.x, m_InitialBounds.extents.x), 0f - m_InitialBounds.extents.x);
		float y = Mathf.Max(Mathf.Min(impactForce.y, m_InitialBounds.extents.y), 0f - m_InitialBounds.extents.y);
		float z = Mathf.Max(Mathf.Min(impactForce.z, m_InitialBounds.extents.z), 0f - m_InitialBounds.extents.z);
		impactForce = new Vector3(x, y, z);
		ThreadManager.RunAsync(delegate
		{
			m_Hull.Impact(impactPoint, impactForce, impactShape, impactType);
			ThreadManager.QueueOnMainThread(delegate
			{
				MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
				if (component != null)
				{
					component.sharedMesh = null;
				}
				MeshCollider component2 = base.gameObject.GetComponent<MeshCollider>();
				if (component2 != null)
				{
					component2.sharedMesh = null;
				}
				Mesh mesh = m_Hull.GetMesh();
				if (impactType == ImpactTypes.Fracture)
				{
					Mesh subHullMesh = m_Hull.GetSubHullMesh();
					if (subHullMesh != null)
					{
						GameObject gameObject = Object.Instantiate(base.gameObject);
						MeshFilter component3 = gameObject.GetComponent<MeshFilter>();
						MeshCollider component4 = gameObject.GetComponent<MeshCollider>();
						if (component3 != null)
						{
							component3.sharedMesh = subHullMesh;
						}
						if (component4 != null)
						{
							component4.sharedMesh = subHullMesh;
						}
						DelayCollisions();
						Meshinator component5 = gameObject.GetComponent<Meshinator>();
						if (component5 != null)
						{
							component5.DelayCollisions();
						}
						if (base.gameObject.GetComponent<Rigidbody>() != null && gameObject.GetComponent<Rigidbody>() != null)
						{
							Vector3 size = mesh.bounds.size;
							float num2 = size.x * size.y * size.z;
							Vector3 size2 = subHullMesh.bounds.size;
							float num3 = size2.x * size2.y * size2.z;
							float num4 = num2 + num3;
							float mass = base.gameObject.GetComponent<Rigidbody>().mass;
							base.gameObject.GetComponent<Rigidbody>().mass = mass * (num2 / num4);
							gameObject.GetComponent<Rigidbody>().mass = mass * (num3 / num4);
							gameObject.GetComponent<Rigidbody>().velocity = base.gameObject.GetComponent<Rigidbody>().velocity;
							base.gameObject.GetComponent<Rigidbody>().centerOfMass = mesh.bounds.center;
							gameObject.GetComponent<Rigidbody>().centerOfMass = subHullMesh.bounds.center;
						}
					}
				}
				if (component != null)
				{
					component.sharedMesh = mesh;
				}
				if (component2 != null)
				{
					component2.sharedMesh = mesh;
				}
				if (m_CacheOption == CacheOptions.None)
				{
					m_Hull = null;
				}
				m_Calculating = false;
			});
		});
	}

	private void InitializeHull()
	{
		if (m_Hull != null)
		{
			return;
		}
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		if (component == null || component.sharedMesh == null)
		{
			m_Calculating = false;
			return;
		}
		if (!m_BoundsSet)
		{
			m_InitialBounds = component.sharedMesh.bounds;
			m_BoundsSet = true;
		}
		m_Hull = new Hull(component.sharedMesh);
	}

	private bool CanDoImpact(Vector3 point, Vector3 force)
	{
		if (m_Calculating)
		{
			return false;
		}
		float magnitude = force.magnitude;
		if (magnitude - m_ForceResistance <= 0f)
		{
			return false;
		}
		float num = (magnitude - m_ForceResistance) * m_ForceMultiplier;
		if (num <= 0f)
		{
			return false;
		}
		return true;
	}
}
