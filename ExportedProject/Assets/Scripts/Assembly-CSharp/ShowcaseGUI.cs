using UnityEngine;

public class ShowcaseGUI : MonoBehaviour
{
	private static ShowcaseGUI instance;

	private int levels = 8;

	private void Start()
	{
		if ((bool)instance)
		{
			Object.Destroy(base.gameObject);
		}
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		OnLevelWasLoaded(0);
	}

	private void OnLevelWasLoaded(int level)
	{
		GameObject gameObject = GameObject.Find("Floor_Tile");
		if (!gameObject)
		{
			return;
		}
		foreach (Transform item in gameObject.transform)
		{
			item.gameObject.SetActive(true);
		}
	}

	private void OnGUI()
	{
		int width = Screen.width;
		int num = 30;
		int num2 = 40;
		Rect rect = new Rect(width - num * 2 - 70, 10f, num, num2);
		if (Application.loadedLevel > 0 && GUI.Button(rect, "<"))
		{
			Application.LoadLevel(Application.loadedLevel - 1);
		}
		else if (GUI.Button(new Rect(rect), "<"))
		{
			Application.LoadLevel(levels - 1);
		}
		GUI.Box(new Rect(width - num - 70, 10f, 60f, num2), "Scene:\n" + (Application.loadedLevel + 1) + " / " + levels);
		Rect source = new Rect(width - num - 10, 10f, num, num2);
		if (Application.loadedLevel < levels - 1 && GUI.Button(new Rect(source), ">"))
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		else if (GUI.Button(new Rect(source), ">"))
		{
			Application.LoadLevel(0);
		}
	}
}
