using UnityEngine;

public class AI_OmaReitti : MonoBehaviour
{
	[Header("Reittipisteet (Raahaa tähän)")]
	public Transform[] reittipisteet;

	[Header("Ajoasetukset")]
	public float kaantymisHerkkyys = 1f;

	public bool kaannaOhjaus;

	[Header("Turvajärjestelmä")]
	public float respawnAika = 4f;

	public float minimiLiike = 0.5f;

	[Header("Debug (Vain luku)")]
	public int nykyinenIndex;

	public string menossaKohti = string.Empty;

	[Header("Kytkökset")]
	public RaceManager raceManager;

	public int aiKierros = 1;

	public bool aiMaalissa;

	private ArcadeCarNoPhysics moottori;

	private float jumiAjastin;

	private Vector3 viimeSijainti;

	private float tarkistusAjastin;

	private float nykyinenOhjaus;

	private float nykyinenKaasu;

	private float matkaKohteeseen;

	private float nykyinenOsumaSade;

	private void Start()
	{
		moottori = GetComponent<ArcadeCarNoPhysics>();
		if (moottori != null)
		{
			moottori.onAI = true;
		}
		viimeSijainti = base.transform.position;
		if (raceManager == null)
		{
			raceManager = Object.FindObjectOfType<RaceManager>();
		}
	}

	private void Update()
	{
		if (aiMaalissa)
		{
			if (moottori != null)
			{
				moottori.NollaaVauhti();
			}
		}
		else if (raceManager != null && !raceManager.peliKaynnissa)
		{
			if (moottori != null)
			{
				moottori.NollaaVauhti();
			}
		}
		else if (reittipisteet != null && reittipisteet.Length != 0)
		{
			Aja();
			TarkistaJumiutuminen();
		}
	}

	private void Aja()
	{
		Transform transform = reittipisteet[nykyinenIndex];
		if (transform == null)
		{
			return;
		}
		Vector3 position = transform.position;
		position.y = base.transform.position.y;
		Vector3 normalized = (position - base.transform.position).normalized;
		float num = (matkaKohteeseen = Vector3.Distance(base.transform.position, position));
		float num2 = LaskeSuuntaKulma(base.transform.up, normalized, Vector3.up);
		float num3 = Mathf.Clamp(num2 / 45f, -1f, 1f);
		if (kaannaOhjaus)
		{
			num3 = 0f - num3;
		}
		float num4 = 1f - Mathf.Abs(num3) * 0.5f;
		if (num4 < 0.2f)
		{
			num4 = 0.2f;
		}
		nykyinenOhjaus = num3;
		nykyinenKaasu = num4;
		if (moottori != null)
		{
			moottori.ulkoinenKaanto = num3 * kaantymisHerkkyys;
			moottori.ulkoinenKaasu = num4;
		}
		float num5 = transform.localScale.x / 2f;
		if (num5 < 0.5f)
		{
			num5 = 2f;
		}
		nykyinenOsumaSade = num5;
		if (num < num5)
		{
			nykyinenIndex++;
			if (nykyinenIndex >= reittipisteet.Length)
			{
				nykyinenIndex = 0;
				aiKierros++;
				if (raceManager != null && aiKierros > raceManager.kierroksiaYhteensa)
				{
					aiMaalissa = true;
				}
			}
		}
		menossaKohti = transform.name;
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
		if (reittipisteet[num] != null)
		{
			Vector3 position = reittipisteet[num].position;
			position.y = base.transform.position.y;
			base.transform.position = position;
			base.transform.LookAt(reittipisteet[nykyinenIndex]);
			base.transform.Rotate(90f, 0f, 0f);
			if (moottori != null)
			{
				moottori.NollaaVauhti();
			}
		}
		jumiAjastin = 0f;
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.box);
		gUIStyle.alignment = TextAnchor.UpperLeft;
		gUIStyle.fontSize = 12;
		gUIStyle.fontStyle = FontStyle.Bold;
		float num = 220f;
		float height = 160f;
		float num2 = 10f;
		Rect position = new Rect((float)Screen.width - num - num2, num2, num, height);
		GUI.Box(position, "AI DEBUGGER", gUIStyle);
		GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
		gUIStyle2.normal.textColor = Color.white;
		gUIStyle2.fontSize = 12;
		float num3 = 20f;
		float num4 = position.y + 25f;
		float left = position.x + 10f;
		GUI.Label(new Rect(left, num4, 200f, 20f), "Kohde: " + menossaKohti, gUIStyle2);
		num4 += num3;
		GUI.Label(new Rect(left, num4, 200f, 20f), "Etäisyys: " + matkaKohteeseen.ToString("F1") + "m (Raja: " + nykyinenOsumaSade.ToString("F1") + "m)", gUIStyle2);
		num4 += num3;
		GUI.Label(new Rect(left, num4, 200f, 20f), "Kaasu: " + (nykyinenKaasu * 100f).ToString("F0") + "%", gUIStyle2);
		num4 += num3;
		GUI.Label(new Rect(left, num4, 200f, 20f), "Ohjaus: " + nykyinenOhjaus.ToString("F2"), gUIStyle2);
		num4 += num3 + 5f;
		if (jumiAjastin > 1f)
		{
			gUIStyle2.normal.textColor = Color.yellow;
		}
		if (jumiAjastin > 3f)
		{
			gUIStyle2.normal.textColor = Color.red;
		}
		GUI.Label(new Rect(left, num4, 200f, 20f), "Jumi-ajastin: " + jumiAjastin.ToString("F1") + " / " + respawnAika.ToString("F1") + "s", gUIStyle2);
	}

	private void OnDrawGizmos()
	{
		if (reittipisteet == null || reittipisteet.Length < 2)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		for (int i = 0; i < reittipisteet.Length; i++)
		{
			if (reittipisteet[i] != null)
			{
				Transform transform = reittipisteet[i];
				float num = transform.localScale.x / 2f;
				if (num < 0.1f)
				{
					num = 0.5f;
				}
				Gizmos.DrawWireSphere(transform.position, num);
				int num2 = (i + 1) % reittipisteet.Length;
				if (reittipisteet[num2] != null)
				{
					Gizmos.DrawLine(transform.position, reittipisteet[num2].position);
				}
			}
		}
		if (Application.isPlaying && reittipisteet.Length > 0 && reittipisteet[nykyinenIndex] != null)
		{
			Gizmos.color = Color.red;
			Vector3 position = reittipisteet[nykyinenIndex].position;
			position.y = base.transform.position.y;
			Gizmos.DrawLine(base.transform.position, position);
		}
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
