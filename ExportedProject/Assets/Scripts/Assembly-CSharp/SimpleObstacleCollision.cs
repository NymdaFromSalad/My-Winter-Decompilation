using System.Collections.Generic;
using UnityEngine;

public class SimpleObstacleCollision : MonoBehaviour
{
	[Header("Asetukset")]
	public Transform esteKansio;

	public float esteenSade = 0.35f;

	[Header("Fysiikka")]
	public float esteenMassa = 100f;

	public float kimmoisuus;

	[Range(0f, 1f)]
	public float tormaysKitka = 0.815f;

	[Header("Debug")]
	public bool naytaSade = true;

	private ArcadeCarNoPhysics moottori;

	private Transform[] kaikkiEsteet;

	private float tormaysViive;

	private void Start()
	{
		moottori = GetComponent<ArcadeCarNoPhysics>();
		if (esteKansio != null)
		{
			List<Transform> list = new List<Transform>();
			Transform[] componentsInChildren = esteKansio.GetComponentsInChildren<Transform>();
			foreach (Transform transform in componentsInChildren)
			{
				if (transform != esteKansio)
				{
					list.Add(transform);
				}
			}
			kaikkiEsteet = list.ToArray();
		}
		else
		{
			Debug.LogWarning("SimpleObstacleCollision: Este Kansio puuttuu!");
		}
	}

	private void Update()
	{
		if (kaikkiEsteet != null)
		{
			if (tormaysViive > 0f)
			{
				tormaysViive -= Time.deltaTime;
			}
			CheckCollisions();
		}
	}

	private void CheckCollisions()
	{
		Vector3 position = base.transform.position;
		Transform[] array = kaikkiEsteet;
		foreach (Transform transform in array)
		{
			float num = position.x - transform.position.x;
			float num2 = position.z - transform.position.z;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 < esteenSade)
			{
				if (tormaysViive <= 0f)
				{
					KasitteleOsuma(transform);
					tormaysViive = 0.2f;
				}
				Vector3 normalized = (base.transform.position - transform.position).normalized;
				base.transform.position += normalized * (esteenSade - num3);
			}
		}
	}

	private void KasitteleOsuma(Transform este)
	{
		if (!(moottori == null))
		{
			Vector3 normalized = (base.transform.position - este.position).normalized;
			moottori.liikeVektori *= 1f - tormaysKitka;
			float num = 5f * kimmoisuus * (esteenMassa / 10f);
			moottori.tormaysTyonto += normalized * num;
			Debug.Log("Osuma esteeseen: " + este.name);
		}
	}

	private void OnDrawGizmos()
	{
		if (!naytaSade || kaikkiEsteet == null)
		{
			return;
		}
		Gizmos.color = Color.magenta;
		Transform[] array = kaikkiEsteet;
		foreach (Transform transform in array)
		{
			if (transform != null)
			{
				Gizmos.DrawWireSphere(transform.position, esteenSade);
			}
		}
	}
}
