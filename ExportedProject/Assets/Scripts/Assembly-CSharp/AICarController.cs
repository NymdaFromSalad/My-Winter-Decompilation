using System;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
	[Header("Asetukset")]
	public Transform checkpointKansio;

	public float kaantymisHerkkyys = 1f;

	public float etaisyysVaihtoon = 4f;

	[Header("Jumiutumisen Esto")]
	public float respawnAika = 4f;

	public float minimiLiike = 0.5f;

	[Header("Debug")]
	public string menossaKohti = string.Empty;

	private Transform[] reittipisteet;

	private int nykyinenIndex;

	private ArcadeCarNoPhysics moottori;

	private float jumiAjastin;

	private Vector3 viimeSijainti;

	private float tarkistusAjastin;

	private void Start()
	{
		moottori = GetComponent<ArcadeCarNoPhysics>();
		if (moottori != null)
		{
			moottori.onAI = true;
		}
		viimeSijainti = base.transform.position;
		if (!(checkpointKansio != null))
		{
			return;
		}
		List<Transform> list = new List<Transform>();
		Transform[] componentsInChildren = checkpointKansio.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (transform != checkpointKansio)
			{
				list.Add(transform);
			}
		}
		reittipisteet = list.ToArray();
		Array.Sort(reittipisteet, (Transform a, Transform b) => string.Compare(a.name, b.name));
	}

	private void Update()
	{
		if (reittipisteet != null && reittipisteet.Length != 0)
		{
			Aja();
			TarkistaJumiutuminen();
			if (reittipisteet.Length > 0)
			{
				Vector3 position = reittipisteet[nykyinenIndex].position;
				Vector3 end = position;
				end.y = base.transform.position.y;
				Debug.DrawLine(base.transform.position, end, Color.red);
				Debug.DrawRay(base.transform.position, base.transform.up * 5f, Color.green);
				menossaKohti = reittipisteet[nykyinenIndex].name;
			}
		}
	}

	private void Aja()
	{
		Vector3 position = reittipisteet[nykyinenIndex].position;
		position.y = base.transform.position.y;
		Vector3 normalized = (position - base.transform.position).normalized;
		float num = Vector3.Distance(base.transform.position, position);
		float num2 = LaskeSuuntaKulma(base.transform.up, normalized, Vector3.up);
		float num3 = Mathf.Clamp(num2 / 45f, -1f, 1f);
		float num4 = 1f - Mathf.Abs(num3) * 0.5f;
		if (num4 < 0.3f)
		{
			num4 = 0.3f;
		}
		if (moottori != null)
		{
			moottori.ulkoinenKaanto = num3 * kaantymisHerkkyys;
			moottori.ulkoinenKaasu = num4;
		}
		if (num < etaisyysVaihtoon)
		{
			nykyinenIndex++;
			if (nykyinenIndex >= reittipisteet.Length)
			{
				nykyinenIndex = 0;
			}
		}
	}

	private void TarkistaJumiutuminen()
	{
		tarkistusAjastin += Time.deltaTime;
		if (tarkistusAjastin > 1f)
		{
			float num = Vector3.Distance(base.transform.position, viimeSijainti);
			if (num < minimiLiike)
			{
				jumiAjastin += 1f;
			}
			else
			{
				jumiAjastin = 0f;
			}
			viimeSijainti = base.transform.position;
			tarkistusAjastin = 0f;
		}
		if (jumiAjastin > respawnAika)
		{
			RespawnAuto();
		}
	}

	private void RespawnAuto()
	{
		int num = nykyinenIndex - 1;
		if (num < 0)
		{
			num = reittipisteet.Length - 1;
		}
		Transform transform = reittipisteet[num];
		Vector3 position = transform.position;
		position.y = base.transform.position.y;
		base.transform.position = position;
		base.transform.rotation = transform.rotation;
		if (moottori != null)
		{
			moottori.tormaysTyonto = Vector3.zero;
			moottori.NollaaVauhti();
		}
		jumiAjastin = 0f;
	}

	private float LaskeSuuntaKulma(Vector3 eteenpain, Vector3 kohdeSuunta, Vector3 kiertoAkseli)
	{
		float num = Vector3.Angle(eteenpain, kohdeSuunta);
		Vector3 rhs = Vector3.Cross(eteenpain, kohdeSuunta);
		if (Vector3.Dot(kiertoAkseli, rhs) < 0f)
		{
			num = 0f - num;
		}
		return num;
	}
}
