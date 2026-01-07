using System.Runtime.InteropServices;
using UnityEngine;

public class ForceFeedback : MonoBehaviour
{
	[HideInInspector]
	public int force;

	private float forceFeedback;

	private bool forceFeedbackEnabled;

	private CarDynamics cardynamics;

	public int factor = 1000;

	public float multiplier = 0.5f;

	public float smoothingFactor = 0.5f;

	public int clampValue = 20;

	public bool invertForceFeedback;

	private int sign = 1;

	private float m_force;

	[DllImport("user32")]
	private static extern int GetForegroundWindow();

	/*[DllImport("UnityForceFeedback")]
	private static extern int InitDirectInput(int HWND);

	[DllImport("UnityForceFeedback")]
	private static extern void Aquire();

	[DllImport("UnityForceFeedback")]
	private static extern int SetDeviceForcesXY(int x, int y);

	[DllImport("UnityForceFeedback")]
	private static extern bool StartEffect();

	[DllImport("UnityForceFeedback")]
	private static extern bool StopEffect();

	[DllImport("UnityForceFeedback")]
	private static extern bool SetAutoCenter(bool autoCentre);

	[DllImport("UnityForceFeedback")]
	private static extern void FreeDirectInput();*/

	public void Start()
	{
		cardynamics = GetComponent<CarDynamics>();
		InitialiseForceFeedback();
		//SetAutoCenter(false);
	}

	public void Update()
	{
		sign = 1;
		if (invertForceFeedback)
		{
			sign = -1;
		}
		forceFeedback = cardynamics.forceFeedback;
		if (Mathf.Abs(forceFeedback) > (float)clampValue)
		{
			forceFeedback = (float)clampValue * Mathf.Sign(forceFeedback);
		}
		force = (int)(forceFeedback * multiplier) * factor * sign;
		//SetDeviceForcesXY(force, 0);
	}

	public void OnApplicationQuit()
	{
		ShutDownForceFeedback();
	}

	private void InitialiseForceFeedback()
	{
		if (forceFeedbackEnabled)
		{
			Debug.LogWarning("UnityCar: Force feedback attempted to initialise but was aleady running!");
			return;
		}
		int foregroundWindow = GetForegroundWindow();
		//InitDirectInput(foregroundWindow);
		//Aquire();
		//StartEffect();
		forceFeedbackEnabled = true;
	}

	private void ShutDownForceFeedback()
	{
		//StopEffect();
		if (forceFeedbackEnabled)
		{
			//FreeDirectInput();
		}
		else
		{
			Debug.LogWarning("UnityCar:  Force feedback attempted to shutdown but wasn't running!");
		}
	}
}
