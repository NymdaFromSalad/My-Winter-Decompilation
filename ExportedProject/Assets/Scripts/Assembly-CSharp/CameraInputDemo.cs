using SWS;
using UnityEngine;

public class CameraInputDemo : MonoBehaviour
{
	public string infoText = "Welcome to this customized input example";

	private splineMove myMove;

	private void Start()
	{
		myMove = base.gameObject.GetComponent<splineMove>();
		myMove.StartMove();
		myMove.Pause();
	}

	private void Update()
	{
		if (myMove.tween != null && myMove.tween.isPaused && Input.GetKeyDown(KeyCode.UpArrow))
		{
			myMove.Resume();
		}
	}

	private void OnGUI()
	{
		if (myMove.tween == null || myMove.tween.isPaused)
		{
			GUI.Box(new Rect(Screen.width - 150, Screen.height / 2, 150f, 100f), string.Empty);
			Rect position = new Rect(Screen.width - 130, Screen.height / 2 + 10, 110f, 90f);
			GUI.Label(position, infoText);
		}
	}

	public void ShowInformation(string text)
	{
		infoText = text;
	}
}
