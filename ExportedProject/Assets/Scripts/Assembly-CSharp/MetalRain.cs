using System.Collections;
using UnityEngine;

public class MetalRain : MonoBehaviour
{
	public GameObject RainObject;

	public Texture2D HardnessMap;

	private Deformable deformable;

	private bool map;

	private bool showMap;

	private void Start()
	{
		StartCoroutine(Rain());
		deformable = GetComponent<Deformable>();
	}

	private IEnumerator Rain()
	{
		while (true)
		{
			Object.Instantiate(RainObject, new Vector3((Random.value - 0.5f) * 20f, Random.value * 40f + 10f, (Random.value - 0.5f) * 20f), Quaternion.identity);
			yield return new WaitForSeconds(Random.value * 0.2f + 0.1f);
		}
	}

	private void OnMouseDown()
	{
		deformable.Repair(0.25f);
	}

	private void OnGUI()
	{
		GUILayout.Label("Hardness:");
		deformable.Hardness = GUILayout.HorizontalSlider(deformable.Hardness, 0.1f, 1f);
		deformable.DeformMeshCollider = GUILayout.Toggle(deformable.DeformMeshCollider, " Update mesh collider (high CPU usage)");
		bool flag = GUILayout.Toggle(map, " Use hardness map");
		if (flag != map)
		{
			map = flag;
			deformable.Repair(1f);
			if (map)
			{
				deformable.HardnessMap = HardnessMap;
			}
			else
			{
				deformable.HardnessMap = null;
				GetComponent<Renderer>().material.mainTexture = null;
				showMap = false;
			}
		}
		if (map)
		{
			showMap = GUILayout.Toggle(showMap, " Show hardness map");
			if (showMap)
			{
				GetComponent<Renderer>().material.mainTexture = HardnessMap;
			}
			else
			{
				GetComponent<Renderer>().material.mainTexture = null;
			}
		}
		GUILayout.Label("Click on the object to repair it");
	}
}
