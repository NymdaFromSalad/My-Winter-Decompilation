using UnityEngine;

public class ArcadeCarNoPhysics : MonoBehaviour
{
	public bool playMakerOhjaa = true;

	[Header("Ajotuntuma")]
	public float huippuNopeus = 8f;

	public float kiihtyvyys = 12f;

	public float kaantymisNopeus = 230f;

	public float normaaliKitka = 2f;

	public float normaaliPito = 4f;

	[Header("Nurmikko (GPS-tunnistus)")]
	public Transform rataKansio;

	public float tienLeveys = 1.05f;

	public float offRoadKitka = 10f;

	public float offRoadPito = 2f;

	public bool onRadalla;

	[Header("Käsijarru")]
	public float kasijarruVoima = 1f;

	public float kasijarruLuisto = 0.9f;

	[Header("Muut")]
	public float maanPintaTaso = 0.3f;

	[Header("PlayMaker Kontrollit")]
	public float kaasuSyote;

	public float ohjausSyote;

	public bool kasijarruPaalla;

	[Header("Vauhti Debug")]
	public float debugNopeus;

	public Vector3 liikeVektori;

	private Transform[] kaikkiTienPalat;

	private float tormaysValahdysAika;

	private string debugTeksti = string.Empty;

	[Header("AI Kontrolli")]
	public bool onAI;

	[HideInInspector]
	public float ulkoinenKaasu;

	[HideInInspector]
	public float ulkoinenKaanto;

	[HideInInspector]
	public Vector3 tormaysTyonto = Vector3.zero;

	private void Start()
	{
		if (rataKansio != null)
		{
			kaikkiTienPalat = rataKansio.GetComponentsInChildren<Transform>();
		}
		else
		{
			Debug.LogError("VIRHE: Aseta Rata Kansio!");
		}
	}

	private void Update()
	{
		if (onAI)
		{
			kaasuSyote = ulkoinenKaasu;
			ohjausSyote = ulkoinenKaanto;
			kasijarruPaalla = false;
		}
		else if (!playMakerOhjaa)
		{
			kaasuSyote = Input.GetAxis("Vertical");
			ohjausSyote = Input.GetAxis("Horizontal");
			kasijarruPaalla = Input.GetKey(KeyCode.Space);
		}
		if (tormaysValahdysAika > 0f)
		{
			tormaysValahdysAika -= Time.deltaTime;
		}
		onRadalla = false;
		if (kaikkiTienPalat != null)
		{
			Vector3 position = base.transform.position;
			Transform[] array = kaikkiTienPalat;
			foreach (Transform transform in array)
			{
				if (!(transform == rataKansio))
				{
					float num = position.x - transform.position.x;
					float num2 = position.z - transform.position.z;
					float num3 = Mathf.Sqrt(num * num + num2 * num2);
					if (num3 < tienLeveys)
					{
						onRadalla = true;
					}
				}
			}
		}
		float num4 = normaaliKitka;
		float num5 = normaaliPito;
		if (!onRadalla)
		{
			num4 = offRoadKitka;
			num5 = offRoadPito;
		}
		if (kasijarruPaalla)
		{
			num5 = kasijarruLuisto;
			num4 = kasijarruVoima;
		}
		if (Mathf.Abs(ohjausSyote) > 0.1f)
		{
			float num6 = ((!kasijarruPaalla) ? 1f : 1.2f);
			float yAngle = ohjausSyote * kaantymisNopeus * num6 * Time.deltaTime;
			base.transform.Rotate(0f, yAngle, 0f, Space.World);
		}
		if (kaasuSyote != 0f && !kasijarruPaalla)
		{
			liikeVektori += base.transform.up * kaasuSyote * kiihtyvyys * Time.deltaTime;
		}
		liikeVektori = Vector3.Lerp(liikeVektori, Vector3.zero, num4 * Time.deltaTime);
		if (liikeVektori.magnitude > 0.1f)
		{
			float magnitude = liikeVektori.magnitude;
			if (magnitude > huippuNopeus)
			{
				magnitude = huippuNopeus;
				liikeVektori = liikeVektori.normalized * huippuNopeus;
			}
			Vector3 vector = ((kaasuSyote == 0f || kasijarruPaalla) ? Vector3.Lerp(liikeVektori.normalized, base.transform.up, num5 * Time.deltaTime) : Vector3.Lerp(liikeVektori.normalized, base.transform.up * Mathf.Sign(kaasuSyote), num5 * Time.deltaTime));
			liikeVektori = vector * magnitude;
		}
		debugNopeus = liikeVektori.magnitude;
		Vector3 position2 = base.transform.position;
		position2.y = maanPintaTaso;
		base.transform.position = position2;
		base.transform.position += liikeVektori * Time.deltaTime;
		base.transform.position += tormaysTyonto * Time.deltaTime;
		tormaysTyonto = Vector3.Lerp(tormaysTyonto, Vector3.zero, Time.deltaTime * 5f);
	}

	private void OnDrawGizmosSelected()
	{
		if (kaikkiTienPalat == null)
		{
			return;
		}
		Gizmos.color = Color.cyan;
		Transform[] array = kaikkiTienPalat;
		foreach (Transform transform in array)
		{
			if (!(transform == rataKansio))
			{
				Gizmos.DrawWireSphere(transform.position, tienLeveys);
			}
		}
	}

	private void OnGUI()
	{
		if (tormaysValahdysAika > 0f)
		{
			GUI.color = Color.red;
			GUI.Box(new Rect(10f, 10f, 300f, 200f), string.Empty);
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
			gUIStyle.fontSize = 30;
			gUIStyle.fontStyle = FontStyle.Bold;
			GUI.Label(new Rect(30f, 50f, 300f, 50f), debugTeksti, gUIStyle);
		}
		else
		{
			GUI.color = new Color(0f, 0f, 0f, 0.5f);
			GUI.Box(new Rect(10f, 10f, 300f, 200f), "DEBUGGER");
		}
		GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
		gUIStyle2.fontSize = 20;
		gUIStyle2.normal.textColor = Color.white;
		GUI.color = Color.white;
		GUI.Label(new Rect(30f, 150f, 250f, 30f), "Nopeus: " + (liikeVektori.magnitude * 10f).ToString("F0"), gUIStyle2);
		if (onRadalla)
		{
			GUI.color = Color.green;
		}
		else
		{
			GUI.color = Color.red;
		}
		GUI.Label(new Rect(30f, 180f, 250f, 30f), (!onRadalla) ? "NURMIKOLLA" : "RADALLA", gUIStyle2);
	}

	public void NollaaVauhti()
	{
		liikeVektori = Vector3.zero;
		kaasuSyote = 0f;
		ohjausSyote = 0f;
	}
}
