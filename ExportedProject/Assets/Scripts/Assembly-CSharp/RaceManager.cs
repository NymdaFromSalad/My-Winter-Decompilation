using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
	private struct CheckpointTila
	{
		public Vector3 sijainti;

		public Quaternion asento;

		public bool onAjetuKerran;
	}

	[Header("Asetukset")]
	public GameObject auto;

	public Transform checkpointKansio;

	public int kierroksiaYhteensa = 10;

	public float lahtolaskentaAika = 3f;

	public float checkpointKoko = 8f;

	[Header("Respawn Asetukset")]
	public float respawnViive = 2f;

	[Header("Ohjaus (PlayMaker / Space)")]
	public ArcadeCarNoPhysics pelaajanAuto;

	public static RaceManager instance;

	private bool odotaStarttia = true;

	private float painallusViive = 1f;

	private string infoTeksti = "Paina SPACE (Jarru) aloittaaksesi";

	[Header("Tila (Vain luku - Debug)")]
	public bool peliKaynnissa;

	public bool kisaLoppui;

	public bool respawnKaynnissa;

	public int nykyinenKierros = 1;

	public int seuraavaCheckpointIndex;

	public float ajastin;

	private CheckpointTila[] tallennetutTilat;

	private Transform[] checkpointit;

	private ArcadeCarNoPhysics autonScripti;

	private float peliAika;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		ajastin = lahtolaskentaAika;
		if (checkpointKansio != null)
		{
			List<Transform> list = new List<Transform>();
			Transform[] componentsInChildren = checkpointKansio.GetComponentsInChildren<Transform>();
			foreach (Transform transform in componentsInChildren)
			{
				if (transform != checkpointKansio)
				{
					list.Add(transform);
				}
			}
			checkpointit = list.ToArray();
			tallennetutTilat = new CheckpointTila[checkpointit.Length];
			for (int j = 0; j < checkpointit.Length; j++)
			{
				tallennetutTilat[j].sijainti = checkpointit[j].position;
				tallennetutTilat[j].asento = checkpointit[j].rotation;
				tallennetutTilat[j].onAjetuKerran = false;
			}
		}
		else
		{
			Debug.LogError("RaceManager: Checkpoint Kansio puuttuu!");
		}
		if (auto != null)
		{
			autonScripti = auto.GetComponent<ArcadeCarNoPhysics>();
			if (autonScripti != null)
			{
				autonScripti.enabled = false;
			}
		}
		if (checkpointit != null && checkpointit.Length > 0)
		{
			seuraavaCheckpointIndex = 1;
		}
		if (!(pelaajanAuto == null))
		{
			return;
		}
		ArcadeCarNoPhysics[] array = Object.FindObjectsOfType<ArcadeCarNoPhysics>();
		foreach (ArcadeCarNoPhysics arcadeCarNoPhysics in array)
		{
			if (!arcadeCarNoPhysics.onAI)
			{
				pelaajanAuto = arcadeCarNoPhysics;
				break;
			}
		}
	}

	private void Update()
	{
		if (painallusViive > 0f)
		{
			painallusViive -= Time.deltaTime;
		}
		bool flag = false;
		if (Input.GetKey(KeyCode.Space))
		{
			flag = true;
		}
		if (pelaajanAuto != null && pelaajanAuto.kasijarruPaalla)
		{
			flag = true;
		}
		if (odotaStarttia)
		{
			if (flag && painallusViive <= 0f)
			{
				odotaStarttia = false;
				infoTeksti = string.Empty;
			}
			return;
		}
		if (!peliKaynnissa && !kisaLoppui)
		{
			ajastin -= Time.deltaTime;
			if (ajastin <= 0f)
			{
				AloitaKisa();
			}
			return;
		}
		if (kisaLoppui)
		{
			infoTeksti = "MAALI! Paina SPACE (KÄSiJarru) jatkaaksesi";
			if (flag && painallusViive <= 0f)
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		if (peliKaynnissa && !kisaLoppui && !respawnKaynnissa)
		{
			peliAika += Time.deltaTime;
			TarkistaKaikkiCheckpointit();
		}
	}

	private void AloitaKisa()
	{
		peliKaynnissa = true;
		ajastin = 0f;
		if (autonScripti != null)
		{
			autonScripti.enabled = true;
		}
	}

	private void TarkistaKaikkiCheckpointit()
	{
		if (checkpointit == null || checkpointit.Length == 0)
		{
			return;
		}
		Vector3 position = auto.transform.position;
		for (int i = 0; i < checkpointit.Length; i++)
		{
			Transform transform = checkpointit[i];
			float num = position.x - transform.position.x;
			float num2 = position.z - transform.position.z;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 < checkpointKoko)
			{
				if (i == seuraavaCheckpointIndex)
				{
					tallennetutTilat[i].sijainti = auto.transform.position;
					tallennetutTilat[i].asento = auto.transform.rotation;
					tallennetutTilat[i].onAjetuKerran = true;
					CheckpointSuoritettu(i);
					break;
				}
				if (i > seuraavaCheckpointIndex && (seuraavaCheckpointIndex != 0 || i != checkpointit.Length - 1))
				{
					Debug.Log("Väärä checkpoint! Palautetaan edelliseen.");
					StartCoroutine(RespawnAuto());
					break;
				}
			}
		}
	}

	private void CheckpointSuoritettu(int suoritettuIndex)
	{
		if (suoritettuIndex == 0)
		{
			nykyinenKierros++;
			if (nykyinenKierros > kierroksiaYhteensa)
			{
				LopetaKisa();
			}
			else
			{
				seuraavaCheckpointIndex = 1;
			}
		}
		else
		{
			seuraavaCheckpointIndex++;
			if (seuraavaCheckpointIndex >= checkpointit.Length)
			{
				seuraavaCheckpointIndex = 0;
			}
		}
	}

	private IEnumerator RespawnAuto()
	{
		respawnKaynnissa = true;
		if (autonScripti != null)
		{
			autonScripti.NollaaVauhti();
			autonScripti.enabled = false;
		}
		yield return new WaitForSeconds(respawnViive);
		int palautusIndex = seuraavaCheckpointIndex - 1;
		if (palautusIndex < 0)
		{
			palautusIndex = checkpointit.Length - 1;
		}
		CheckpointTila palautusTila = tallennetutTilat[palautusIndex];
		Vector3 uusiPos = palautusTila.sijainti;
		Quaternion uusiRot = palautusTila.asento;
		uusiPos.y = Mathf.Max(uusiPos.y, 0.3f);
		auto.transform.position = uusiPos;
		auto.transform.rotation = uusiRot;
		if (autonScripti != null)
		{
			autonScripti.NollaaVauhti();
			autonScripti.enabled = true;
		}
		respawnKaynnissa = false;
	}

	private void LopetaKisa()
	{
		peliKaynnissa = false;
		kisaLoppui = true;
		if (autonScripti != null)
		{
			autonScripti.enabled = false;
		}
	}

	private void OnGUI()
	{
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
		gUIStyle.fontSize = 40;
		gUIStyle.fontStyle = FontStyle.Bold;
		gUIStyle.normal.textColor = Color.white;
		gUIStyle.alignment = TextAnchor.MiddleCenter;
		float width = Screen.width;
		float num = Screen.height;
		if (infoTeksti != string.Empty)
		{
			GUI.Label(new Rect(0f, (float)Screen.height * 0.5f, Screen.width, 100f), infoTeksti, gUIStyle);
		}
		if (!peliKaynnissa && !kisaLoppui)
		{
			GUI.Label(new Rect(0f, num * 0.3f, width, 100f), Mathf.Ceil(ajastin).ToString("F0"), gUIStyle);
			GUI.Label(new Rect(0f, num * 0.4f, width, 100f), "VALMIINA...", gUIStyle);
		}
		if (respawnKaynnissa)
		{
			gUIStyle.normal.textColor = Color.red;
			GUI.Label(new Rect(0f, num * 0.4f, width, 100f), "CHECKPOINT MISSATTU!", gUIStyle);
			GUI.Label(new Rect(0f, num * 0.5f, width, 100f), "PALAUTETAAN...", gUIStyle);
		}
		if (peliKaynnissa && !respawnKaynnissa)
		{
			GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
			gUIStyle2.fontSize = 25;
			gUIStyle2.normal.textColor = Color.yellow;
			GUI.Label(new Rect(20f, 20f, 300f, 50f), "KIERROS: " + nykyinenKierros + " / " + kierroksiaYhteensa, gUIStyle2);
			GUI.Label(new Rect(20f, 50f, 300f, 50f), "AIKA: " + peliAika.ToString("F2"), gUIStyle2);
			GUI.Label(new Rect(20f, 80f, 400f, 30f), "SEURAAVA: Checkpoint " + seuraavaCheckpointIndex, gUIStyle2);
		}
		if (kisaLoppui)
		{
			gUIStyle.normal.textColor = Color.green;
			GUI.Label(new Rect(0f, num * 0.4f, width, 100f), "MAALI!", gUIStyle);
			GUI.Label(new Rect(0f, num * 0.5f, width, 100f), "LOPPUAIKA: " + peliAika.ToString("F2"), gUIStyle);
		}
	}

	private void OnDrawGizmos()
	{
		if (!(checkpointKansio != null))
		{
			return;
		}
		Gizmos.color = Color.yellow;
		Transform[] componentsInChildren = checkpointKansio.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (transform != checkpointKansio)
			{
				Gizmos.DrawWireSphere(transform.position, checkpointKoko);
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(transform.position, transform.forward * 4f);
				Gizmos.color = Color.yellow;
			}
		}
	}
}
