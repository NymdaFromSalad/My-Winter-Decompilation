using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarCollision : MonoBehaviour
{
	[Serializable]
	public class OsumaPallo
	{
		public string nimi = "Pallo";

		public Vector3 paikkaOffset;

		public float sade = 0.5f;
	}

	[Header("Törmäyspallot")]
	public List<OsumaPallo> osumaPallot = new List<OsumaPallo>();

	[Header("Fysiikka")]
	public float massa = 10f;

	public float kimmoisuus = 0.5f;

	[Header("Debug")]
	public bool naytaPallot = true;

	private ArcadeCarNoPhysics omaMoottori;

	private static List<SimpleCarCollision> kaikkiAutot = new List<SimpleCarCollision>();

	private void OnEnable()
	{
		if (!kaikkiAutot.Contains(this))
		{
			kaikkiAutot.Add(this);
		}
		omaMoottori = GetComponent<ArcadeCarNoPhysics>();
		if (osumaPallot.Count == 0)
		{
			osumaPallot.Add(new OsumaPallo
			{
				nimi = "Keskusta",
				sade = 0.5f,
				paikkaOffset = Vector3.zero
			});
		}
	}

	private void OnDisable()
	{
		kaikkiAutot.Remove(this);
	}

	private void Update()
	{
		TarkistaTormaykset();
	}

	private void TarkistaTormaykset()
	{
		foreach (SimpleCarCollision item in kaikkiAutot)
		{
			if (item == this)
			{
				continue;
			}
			foreach (OsumaPallo item2 in osumaPallot)
			{
				foreach (OsumaPallo item3 in item.osumaPallot)
				{
					Vector3 vector = base.transform.TransformPoint(item2.paikkaOffset);
					Vector3 vector2 = item.transform.TransformPoint(item3.paikkaOffset);
					float num = Vector3.Distance(vector, vector2);
					float num2 = item2.sade + item3.sade;
					if (num < num2)
					{
						KäsitteleOsuma(item, vector, vector2, num, num2);
						return;
					}
				}
			}
		}
	}

	private void KäsitteleOsuma(SimpleCarCollision toinenAuto, Vector3 minunOsumaKohta, Vector3 toisenOsumaKohta, float etaisyys, float sallittuEtaisyys)
	{
		Vector3 normalized = (minunOsumaKohta - toisenOsumaKohta).normalized;
		float num = massa + toinenAuto.massa;
		float num2 = toinenAuto.massa / num;
		float num3 = sallittuEtaisyys - etaisyys;
		base.transform.position += normalized * num3 * num2;
		if (omaMoottori != null)
		{
			Vector3 vector = normalized * (kimmoisuus * 5f) * num2;
			omaMoottori.tormaysTyonto += vector;
		}
	}

	private void OnDrawGizmos()
	{
		if (!naytaPallot || osumaPallot == null)
		{
			return;
		}
		Gizmos.color = Color.green;
		foreach (OsumaPallo item in osumaPallot)
		{
			Vector3 center = base.transform.TransformPoint(item.paikkaOffset);
			Gizmos.DrawWireSphere(center, item.sade);
		}
	}
}
