using System;
using SWS;
using UnityEngine;

public class RuntimeDemo : MonoBehaviour
{
	[Serializable]
	public class ExampleClass1
	{
		public GameObject walkerPrefab;

		public GameObject pathPrefab;

		public bool done;
	}

	[Serializable]
	public class ExampleClass2
	{
		public splineMove moveRef;

		public string pathName1;

		public string pathName2;
	}

	[Serializable]
	public class ExampleClass3
	{
		public splineMove moveRef;
	}

	[Serializable]
	public class ExampleClass4
	{
		public splineMove moveRef;
	}

	[Serializable]
	public class ExampleClass5
	{
		public splineMove moveRef;
	}

	[Serializable]
	public class ExampleClass6
	{
		public splineMove moveRef;

		public GameObject target;

		public bool done;
	}

	public ExampleClass1 example1;

	public ExampleClass2 example2;

	public ExampleClass3 example3;

	public ExampleClass4 example4;

	public ExampleClass5 example5;

	public ExampleClass6 example6;

	private void OnGUI()
	{
		DrawExample1();
		DrawExample2();
		DrawExample3();
		DrawExample4();
		DrawExample5();
		DrawExample6();
	}

	private void DrawExample1()
	{
		GUI.Label(new Rect(10f, 10f, 20f, 20f), "1:");
		string text = "Walker (Path1)";
		string key = "Path1 (Instantiation)";
		Vector3 position = new Vector3(-25f, 0f, 10f);
		if (!example1.done && GUI.Button(new Rect(30f, 10f, 100f, 20f), "Instantiate"))
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(example1.walkerPrefab, position, Quaternion.identity);
			gameObject.name = text;
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(example1.pathPrefab, position, Quaternion.identity);
			gameObject2.name = key;
			WaypointManager.AddPath(gameObject2);
			gameObject.GetComponent<splineMove>().SetPath(WaypointManager.Paths[key]);
			example1.done = true;
		}
		if (example1.done && GUI.Button(new Rect(30f, 10f, 100f, 20f), "Destroy"))
		{
			UnityEngine.Object.Destroy(GameObject.Find(text));
			UnityEngine.Object.Destroy(GameObject.Find(key));
			WaypointManager.Paths.Remove(key);
			example1.done = false;
		}
	}

	private void DrawExample2()
	{
		GUI.Label(new Rect(10f, 30f, 20f, 20f), "2:");
		if (GUI.Button(new Rect(30f, 30f, 100f, 20f), "Switch Path"))
		{
			string text = example2.moveRef.pathContainer.name;
			example2.moveRef.moveToPath = true;
			if (text == example2.pathName1)
			{
				example2.moveRef.SetPath(WaypointManager.Paths[example2.pathName2]);
			}
			else
			{
				example2.moveRef.SetPath(WaypointManager.Paths[example2.pathName1]);
			}
		}
	}

	private void DrawExample3()
	{
		GUI.Label(new Rect(10f, 50f, 20f, 20f), "3:");
		if (example3.moveRef.tween == null && GUI.Button(new Rect(30f, 50f, 100f, 20f), "Start"))
		{
			example3.moveRef.StartMove();
		}
		if (example3.moveRef.tween != null && GUI.Button(new Rect(30f, 50f, 100f, 20f), "Stop"))
		{
			example3.moveRef.Stop();
		}
		if (example3.moveRef.tween != null && GUI.Button(new Rect(30f, 70f, 100f, 20f), "Reset"))
		{
			example3.moveRef.ResetMove(true);
		}
	}

	private void DrawExample4()
	{
		GUI.Label(new Rect(10f, 90f, 20f, 20f), "4:");
		if (example4.moveRef.tween != null && !example4.moveRef.tween.isPaused && GUI.Button(new Rect(30f, 90f, 100f, 20f), "Pause"))
		{
			example4.moveRef.Pause();
		}
		if (example4.moveRef.tween != null && example4.moveRef.tween.isPaused && GUI.Button(new Rect(30f, 90f, 100f, 20f), "Resume"))
		{
			example4.moveRef.Resume();
		}
	}

	private void DrawExample5()
	{
		GUI.Label(new Rect(10f, 110f, 20f, 20f), "5:");
		if (GUI.Button(new Rect(30f, 110f, 100f, 20f), "Change Speed"))
		{
			float speed = example5.moveRef.speed;
			float num = 1.5f;
			if (speed == num)
			{
				num = 4f;
			}
			example5.moveRef.ChangeSpeed(num);
		}
	}

	private void DrawExample6()
	{
		GUI.Label(new Rect(10f, 130f, 20f, 20f), "6:");
		if (!example6.done && GUI.Button(new Rect(30f, 130f, 100f, 20f), "Add Message"))
		{
			Messages messages = example6.moveRef.messages;
			MessageOptions messageOption = messages.GetMessageOption(1);
			messageOption.message.Add("ActivateForTime");
			messageOption.type.Add(MessageOptions.ValueType.Object);
			messageOption.obj.Add(example6.target);
			messages.FillOptionWithValues(messageOption);
			example6.done = true;
		}
	}
}
