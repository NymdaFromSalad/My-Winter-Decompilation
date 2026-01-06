using System.Collections;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
	private bool toggleTxt;

	public GameObject selectedCar;

	private bool isEnabled;

	public bool stressTest;

	private Rect GridRect;

	private int GridInt;

	private int oldGridInt;

	private string[] GridEntrys;

	private Rect ToggleRect;

	private Vector2 BumpSize;

	private Vector2 StartSize = new Vector2(1024f, 768f);

	public Texture2D grid;

	public int gridWidth = 354;

	public int gridHeight = 262;

	public GUIStyle powerStyle;

	public GUIStyle torqueStyle;

	private Color[] buffer;

	private float Offset;

	private Rect[] NameLabelRect;

	private Rect[] SliderRect;

	private Rect GearsWindowRect;

	private Rect SettingsWindowRect;

	private Rect ScrollRect;

	private Vector2 ScrollPosition;

	private Rect ScrollViewRect;

	private Rect[] ScrollLabelsRect;

	private Rect[] ScrollSlidersRect;

	private Color[] colors;

	private Rigidbody mrigidbody;

	private Transform mTransform;

	private CarDynamics carDynamics;

	private Drivetrain drivetrain;

	private CarDebug carDebug;

	private CarDamage carDamage;

	private AerodynamicResistance aerodynamicResistance;

	private CarController carController;

	private DashBoard dashBoard;

	private Arcader arcader;

	private Setup setup;

	private Axles axles;

	private FuelTank[] fuelTanks;

	private float[] currentFuels;

	private bool showForces;

	private bool TCSChanged;

	private bool ESPChanged;

	private string[] tireTypes = new string[12]
	{
		"Competition Front", "Competition Rear", "SuperSport Front", "SuperSport Rear", "Sport Front", "Sport Rear", "Touring Front", "Touring Rear", "Offroad Front", "Offroad Rear",
		"Truck Front", "Truck Rear"
	};

	private int tiresTypeFront;

	private int tiresTypeRear;

	private float mass;

	private string[] transmissionTypes = new string[3] { "RWD", "FWD", "AWD" };

	private int transmissionType;

	private int oldTransmissionType;

	private bool engineTorqueFromFile;

	private bool mouseControllerEnabled;

	private bool oldMouseControllerEnabled;

	private bool oldArcaderEnabled;

	private float timer;

	private float timer1;

	private bool steerAssistanceStateChanged;

	private bool startTimer;

	private bool startTimer1;

	private bool ESPStateChanged;

	private bool ABSStateChanged;

	private bool TCSStateChanged;

	[HideInInspector]
	public int carsNumber;

	private float engageRPM;

	private int maxRPM;

	private float factor;

	private float m_maxTorque;

	private float oldactualMaxPower;

	private float actualMaxPower;

	private int i;

	private int floor;

	private int top;

	private int maxKmh;

	private bool ESP;

	private int entrysCount;

	private bool showPopupDialog;

	private string message;

	private Vector3 boundingSize;

	private float zlocalPositionLimit;

	private void Awake()
	{
		NameLabelRect = new Rect[30];
		SliderRect = new Rect[30];
		GridInt = 0;
		transmissionTypes = new string[3] { "RWD", "FWD", "AWD" };
		colors = new Color[8]
		{
			Color.red,
			Color.blue,
			Color.blue,
			Color.green,
			Color.yellow,
			Color.magenta,
			Color.black,
			Color.gray
		};
		ScrollPosition = Vector2.zero;
		if (grid != null)
		{
			buffer = grid.GetPixels();
		}
	}

	private void Start()
	{
		if (selectedCar != null)
		{
			StartCoroutine(ChangeCar(selectedCar));
		}
	}

	public IEnumerator ChangeCar(GameObject mselectedCar)
	{
		if (!(mselectedCar != null))
		{
			yield break;
		}
		mTransform = mselectedCar.transform;
		mrigidbody = mselectedCar.GetComponent<Rigidbody>();
		carDynamics = mselectedCar.GetComponent<CarDynamics>();
		drivetrain = mselectedCar.GetComponent<Drivetrain>();
		aerodynamicResistance = mselectedCar.GetComponent<AerodynamicResistance>();
		carDebug = mTransform.GetComponent<CarDebug>();
		carDamage = mselectedCar.GetComponent<CarDamage>();
		carController = mselectedCar.GetComponent<CarDynamics>().carController;
		dashBoard = mselectedCar.transform.GetComponentInChildren<DashBoard>();
		arcader = mselectedCar.transform.GetComponentInChildren<Arcader>();
		setup = mselectedCar.GetComponent<Setup>();
		axles = mselectedCar.GetComponent<Axles>();
		fuelTanks = mselectedCar.GetComponentsInChildren<FuelTank>();
		currentFuels = new float[fuelTanks.Length];
		engineTorqueFromFile = drivetrain.engineTorqueFromFile;
		if (setup != null && setup.enabled)
		{
			while (setup.loadingSetup)
			{
				yield return new WaitForSeconds(0.02f);
			}
		}
		if (engineTorqueFromFile)
		{
			drivetrain.CalcValues(factor, engineTorqueFromFile);
		}
		if (setup != null)
		{
			if (Application.isEditor && setup.enabled)
			{
				GridEntrys = new string[8] { "Engine", "Transmission", "Suspensions", "Brakes", "Tires", "Body", "Assistance", "Save Setup" };
				entrysCount = 8;
			}
			else
			{
				GridEntrys = new string[7] { "Engine", "Transmission", "Suspensions", "Brakes", "Tires", "Body", "Assistance" };
				entrysCount = 7;
			}
		}
		else
		{
			GridEntrys = new string[7] { "Engine", "Transmission", "Suspensions", "Brakes", "Tires", "Body", "Assistance" };
			entrysCount = 7;
		}
		m_maxTorque = drivetrain.maxTorque;
		ESP = carController.ESP;
		selectedCar = mselectedCar;
		tiresTypeFront = (int)axles.frontAxle.tires;
		tiresTypeRear = (int)axles.rearAxle.tires;
		transmissionType = (oldTransmissionType = (int)drivetrain.transmission);
		boundingSize = carDynamics.BoundingSize(selectedCar.GetComponentsInChildren<Collider>());
		zlocalPositionLimit = 0.8f * boundingSize.x / 4.5f;
		engageRPM = drivetrain.engageRPM;
		maxRPM = (Mathf.CeilToInt(drivetrain.maxRPM / 1000f) + 1) * 1000;
		maxKmh = Mathf.RoundToInt((float)maxRPM * axles.frontAxle.leftWheel.radius * 2f * 0.1885f / (drivetrain.gearRatios[drivetrain.gearRatios.Length - 1] * drivetrain.finalDriveRatio));
		mass = mrigidbody.mass;
		StartSize = new Vector2(Screen.width, Screen.height);
		if (grid != null)
		{
			floor = (grid.height - gridHeight) / 2;
		}
		top = gridHeight + Mathf.RoundToInt((float)gridHeight * 0.17f) + floor;
		RectCalculation(StartSize);
		ScrollRectCalculation(StartSize, drivetrain.gearRatios.Length - 2);
		factor = 1f;
		if (grid != null)
		{
			switch (GridInt)
			{
			case 0:
				ApplyEngineTorque();
				break;
			case 1:
				ApplyGears();
				break;
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.B))
		{
			isEnabled = !isEnabled;
		}
		if (Input.GetKeyUp(KeyCode.N) && dashBoard != null)
		{
			dashBoard.gameObject.active = !dashBoard.gameObject.active;
		}
		if (Input.GetKeyUp(KeyCode.G))
		{
			showForces = !showForces;
			Wheel[] allWheels = axles.allWheels;
			foreach (Wheel wheel in allWheels)
			{
				wheel.showForces = showForces;
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Restore();
			Application.LoadLevel(0);
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			drivetrain.engageRPM = drivetrain.maxPowerRPM;
			if (carController.TCS)
			{
				carController.TCS = false;
				TCSChanged = true;
			}
			if (carController.ESP)
			{
				carController.ESP = false;
				ESPChanged = true;
			}
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			drivetrain.engageRPM = engageRPM;
			if (TCSChanged)
			{
				carController.TCS = true;
				TCSChanged = false;
			}
			if (ESPChanged)
			{
				carController.ESP = true;
				ESPChanged = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				carDamage.repair = true;
			}
			else
			{
				ResetCarPosition();
			}
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (Time.timeScale != 1f)
			{
				Time.timeScale = 1f;
			}
			else
			{
				Time.timeScale = 0.05f;
			}
		}
		if (Input.GetKeyDown(KeyCode.F2) && Application.isEditor)
		{
			if (carDebug == null)
			{
				carDebug = mTransform.gameObject.AddComponent<CarDebug>();
			}
			else
			{
				carDebug.enabled = !carDebug.enabled;
			}
		}
		if (startTimer)
		{
			timer += Time.deltaTime;
			if (timer >= 2.5f)
			{
				timer = 0f;
				startTimer = false;
			}
		}
		if (carController != null)
		{
			if (carController.handbrakeInput != 0f)
			{
				startTimer = true;
				if (carController.steerAssistance)
				{
					carController.steerAssistance = false;
					steerAssistanceStateChanged = true;
				}
				if (carController.ESP)
				{
					carController.ESP = false;
					ESPStateChanged = true;
				}
				if (carController.ABS)
				{
					carController.ABS = false;
					ABSStateChanged = true;
				}
				if (drivetrain.autoClutch)
				{
					drivetrain.clutch.SetClutchPosition(0f);
				}
				if (carController.TCS)
				{
					TCSStateChanged = true;
				}
			}
			else
			{
				if (steerAssistanceStateChanged && !startTimer)
				{
					carController.steerAssistance = !carController.steerAssistance;
					steerAssistanceStateChanged = false;
				}
				if (ESPStateChanged && !startTimer)
				{
					carController.ESP = !carController.ESP;
					ESPStateChanged = false;
				}
				if (ABSStateChanged)
				{
					carController.ABS = !carController.ABS;
					ABSStateChanged = false;
				}
				if (TCSStateChanged && !startTimer)
				{
					TCSStateChanged = false;
				}
			}
		}
		if (carDynamics != null)
		{
			if (!carDynamics.AllWheelsOnGround() && mrigidbody.velocity.sqrMagnitude <= 1f)
			{
				startTimer1 = true;
			}
			else
			{
				startTimer1 = false;
			}
		}
		if (startTimer1)
		{
			timer1 += Time.deltaTime;
			if (timer1 >= 2f)
			{
				ResetCarPosition();
				timer1 = 0f;
				startTimer1 = false;
			}
		}
		else
		{
			timer1 = 0f;
		}
	}

	private void ResetCarPosition()
	{
		mTransform.position = new Vector3(mTransform.position.x, mTransform.position.y + 3f, mTransform.position.z);
		mTransform.eulerAngles = new Vector3(0f, mTransform.eulerAngles.y, 0f);
		mrigidbody.angularVelocity = Vector3.zero;
		mrigidbody.velocity = Vector3.zero;
	}

	private void ShowPopupDialog(int windowID)
	{
		GUI.Label(new Rect(20f, 20f, 200f, 40f), message);
		if (GUI.Button(new Rect(80f, 60f, 60f, 30f), "OK"))
		{
			showPopupDialog = !showPopupDialog;
		}
	}

	private void OnGUI()
	{
		if (showPopupDialog)
		{
			GUI.Window(1, new Rect(Screen.width / 2 - 110, Screen.height / 2 - 40, 220f, 100f), ShowPopupDialog, "UnityCar 2.0 Pro");
			GUI.BringWindowToFront(1);
		}
		int num = 1;
		if (stressTest)
		{
			num = 2;
		}
		GUI.Box(new Rect(Screen.width / 2 - 245, Screen.height - 20 * num, 480f, 40 * num), string.Empty);
		if (stressTest)
		{
			GUI.Label(new Rect(Screen.width / 2 - 140, Screen.height - 40, Screen.width, 40f), "Press F10 to instantiate a car (number of cars: " + carsNumber + ")");
		}
		if (mTransform != null)
		{
			GUI.Label(new Rect(Screen.width / 2 - 245, Screen.height - 20, Screen.width, 40f), "Press B to toggle Car Setup Window, PAGE UP and PAGE DOWN to change car");
			if (isEnabled)
			{
				SettingsWindowRect = new Rect(10f, 5f, StartSize.x - 20f, StartSize.y - 20f);
			}
			else
			{
				SettingsWindowRect = new Rect(10f, 5f, StartSize.x - 20f, 30f);
			}
			SettingsWindowRect = GUI.Window(0, SettingsWindowRect, SettingsWindow, "Car Settings");
		}
	}

	private void SettingsWindow(int windowID)
	{
		if (isEnabled)
		{
			oldGridInt = GridInt;
			GridInt = GUI.SelectionGrid(GridRect, GridInt, GridEntrys, entrysCount);
			switch (GridInt)
			{
			case 0:
				GridInt = 0;
				Engine();
				break;
			case 1:
				GridInt = 1;
				Transmission();
				break;
			case 2:
				GridInt = 2;
				Suspensions();
				break;
			case 3:
				GridInt = 3;
				Brakes();
				break;
			case 4:
				GridInt = 4;
				Tires();
				break;
			case 5:
				GridInt = 5;
				Body();
				break;
			case 6:
				GridInt = 6;
				Assistance();
				break;
			case 7:
				GridInt = 7;
				SaveSetup();
				break;
			}
		}
		string text = ((!isEnabled) ? "Expand Setup Window" : "Minimize Setup Window");
		isEnabled = GUI.Toggle(ToggleRect, isEnabled, text);
	}

	private void SaveSetup()
	{
		GridInt = oldGridInt;
		if (setup.filePath != string.Empty)
		{
			if (setup.SaveSetup())
			{
				message = "Setup succesfully saved.";
			}
			else
			{
				message = "An error occurred during saving setup.";
			}
		}
		else
		{
			message = "No setup file set.";
		}
		showPopupDialog = true;
	}

	private void Engine()
	{
		GUI.Label(NameLabelRect[0], "Power Moltiplier: " + drivetrain.powerMultiplier + "X");
		drivetrain.powerMultiplier = RoundTo(GUI.HorizontalSlider(SliderRect[0], drivetrain.powerMultiplier, 0.1f, 5f), 1);
		GUI.Label(NameLabelRect[1], "max Power: " + Mathf.Round(drivetrain.maxPower * drivetrain.powerMultiplier) + " HP (Max NET Power: " + Mathf.Round(drivetrain.maxNetPower * drivetrain.powerMultiplier) + " HP @ " + drivetrain.maxNetPowerRPM + " RPM)");
		if (!engineTorqueFromFile)
		{
			drivetrain.maxPower = RoundTo(GUI.HorizontalSlider(SliderRect[1], drivetrain.maxPower, 10f, drivetrain.maxTorque), 0);
		}
		GUI.Label(NameLabelRect[2], "max Power RPM: " + drivetrain.maxPowerRPM + " RPM");
		if (!engineTorqueFromFile)
		{
			drivetrain.maxPowerRPM = RoundTo(GUI.HorizontalSlider(SliderRect[2], drivetrain.maxPowerRPM, drivetrain.maxTorqueRPM + drivetrain.maxTorqueRPM * 0.25f, drivetrain.maxRPM), 0);
		}
		GUI.Label(NameLabelRect[3], "max Torque: " + m_maxTorque * drivetrain.powerMultiplier + " Nm (Max NET Torque: " + Mathf.Round(drivetrain.maxNetTorque * drivetrain.powerMultiplier) + " Nm @ " + drivetrain.maxNetTorqueRPM + " RPM)");
		if (!engineTorqueFromFile)
		{
			drivetrain.maxTorque = RoundTo(GUI.HorizontalSlider(SliderRect[3], drivetrain.maxTorque, drivetrain.maxPower, drivetrain.maxPower * 8f), 0);
		}
		GUI.Label(NameLabelRect[4], "max Torque RPM: " + drivetrain.maxTorqueRPM + " RPM");
		if (!engineTorqueFromFile)
		{
			drivetrain.maxTorqueRPM = RoundTo(GUI.HorizontalSlider(SliderRect[4], drivetrain.maxTorqueRPM, 500f, drivetrain.maxPowerRPM - drivetrain.maxPowerRPM * 0.25f), 0);
		}
		GUI.Label(NameLabelRect[5], "Engine Friction: " + drivetrain.engineFrictionFactor);
		drivetrain.engineFrictionFactor = RoundTo(GUI.HorizontalSlider(SliderRect[5], drivetrain.engineFrictionFactor, 0.1f, 0.5f), 2);
		GUI.Label(NameLabelRect[6], "Engine Inertia: " + drivetrain.engineInertia);
		drivetrain.engineInertia = RoundTo(GUI.HorizontalSlider(SliderRect[6], drivetrain.engineInertia, 0.1f, 10f), 2);
		drivetrain.revLimiter = GUI.Toggle(NameLabelRect[7], drivetrain.revLimiter, " Rev Limiter");
		drivetrain.canStall = GUI.Toggle(new Rect(NameLabelRect[7].x, NameLabelRect[7].y + 20f, NameLabelRect[7].width, NameLabelRect[7].height), drivetrain.canStall, " Engine Can Stall");
		Rect position = new Rect(NameLabelRect[20].x + 50f, NameLabelRect[20].y, 800f, 600f);
		if (grid != null)
		{
			GUI.BeginGroup(position);
			float num = (grid.width - gridWidth) / 2;
			float num2 = (grid.height - gridHeight) / 2 - 10;
			if (drivetrain.torqueRPMValuesLen > 0)
			{
				engineTorqueFromFile = GUI.Toggle(new Rect(num, (float)(-gridHeight) * 0.2f + num2 - 48f, 340f, 20f), engineTorqueFromFile, " Use Table Data");
			}
			GUI.Label(new Rect(num + 85f, (float)(-gridHeight) * 0.2f + num2, 340f, 20f), "Current NET Power: " + Mathf.Round(drivetrain.currentPower) + " HP @ " + Mathf.Round(drivetrain.rpm) + " RPM", powerStyle);
			GUI.Label(new Rect((float)gridWidth + num, (float)gridHeight * 0.33f + num2, 140f, 20f), string.Empty + Mathf.Round(m_maxTorque * drivetrain.powerMultiplier * 1.33f) + " Nm", torqueStyle);
			GUI.Label(new Rect((float)gridWidth + num, (float)gridHeight * 0.5f + num2, 140f, 20f), string.Empty + Mathf.Round(m_maxTorque * drivetrain.powerMultiplier) + " Nm", torqueStyle);
			GUI.Label(new Rect((float)gridWidth + num, (float)gridHeight * 0.67f + num2, 140f, 20f), string.Empty + Mathf.Round(m_maxTorque * drivetrain.powerMultiplier * 0.66f) + " Nm", torqueStyle);
			GUI.Label(new Rect((float)gridWidth + num, (float)gridHeight * 0.84f + num2, 140f, 20f), string.Empty + Mathf.Round(m_maxTorque * drivetrain.powerMultiplier * 0.33f) + " Nm", torqueStyle);
			GUI.Label(new Rect((float)gridWidth + num, (float)(gridHeight * 1) + num2 - 5f, 140f, 20f), string.Empty + 0 + " Nm", torqueStyle);
			GUI.Label(new Rect(num - 8f, (float)(-gridHeight) * 0.17f + num2, 140f, 20f), string.Empty + Mathf.Round(drivetrain.maxPower * drivetrain.powerMultiplier * 1.166f) + " CV", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)(-gridHeight) * 0.01f + num2, 140f, 20f), string.Empty + Mathf.Round(drivetrain.maxPower * drivetrain.powerMultiplier) + " CV", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.16f + num2, 140f, 20f), string.Empty + Mathf.Round(drivetrain.maxPower * drivetrain.powerMultiplier * 0.83f) + " CV", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.33f + num2, 140f, 20f), string.Empty + Mathf.Round(drivetrain.maxPower * drivetrain.powerMultiplier * 0.664f) + " CV", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.5f + num2, 140f, 20f), string.Empty + Mathf.Round(drivetrain.maxPower * drivetrain.powerMultiplier * 0.498f) + " CV", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.67f + num2, 140f, 20f), string.Empty + Mathf.Round(drivetrain.maxPower * drivetrain.powerMultiplier * 0.332f) + " CV", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.84f + num2, 140f, 20f), string.Empty + Mathf.Round(drivetrain.maxPower * drivetrain.powerMultiplier * 0.166f) + " CV", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)(gridHeight * 1) + num2 - 5f, 140f, 20f), string.Empty + 0 + " CV", powerStyle);
			float num3 = (float)gridWidth * 0.04f;
			GUI.Label(new Rect(num, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + 0, torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.125f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.125f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.25f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.25f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.375f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.375f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.5f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.5f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.625f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.625f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.75f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.75f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.875f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.875f), torqueStyle);
			GUI.Label(new Rect(num + (float)(gridWidth * 1) - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + maxRPM + " RPM", torqueStyle);
			Rect position2 = new Rect(0f, 0f, grid.width, grid.height);
			GUI.DrawTexture(position2, grid);
			GUI.EndGroup();
		}
		if (GUI.changed)
		{
			if (!engineTorqueFromFile && setup != null && drivetrain.engineTorqueFromFile != engineTorqueFromFile)
			{
				setup.LoadEngineValues(engineTorqueFromFile);
			}
			drivetrain.engineTorqueFromFile = engineTorqueFromFile;
			if (grid != null)
			{
				ApplyEngineTorque();
			}
			drivetrain.externalMultiplier = drivetrain.maxPower / drivetrain.originalMaxPower;
			drivetrain.CalcIdleThrottle();
			drivetrain.CalcClutchTorque();
			carController.externalTCSThreshold = Mathf.Clamp01((drivetrain.powerMultiplier * drivetrain.externalMultiplier - 1f) / 3f);
		}
	}

	private void OnApplicationQuit()
	{
		Restore();
	}

	private void Restore()
	{
		if (grid != null)
		{
			grid.SetPixels(buffer);
			grid.Apply(false);
		}
	}

	private void ApplyEngineTorque()
	{
		grid.SetPixels(buffer);
		factor = 1f;
		drivetrain.CalcValues(factor, engineTorqueFromFile);
		for (float num = 0f; num <= (float)maxRPM; num += 1f)
		{
			float num2 = drivetrain.CalcEngineTorque(1f, num);
			oldactualMaxPower = actualMaxPower;
			actualMaxPower = num2 * num * drivetrain.RPM2angularVelo * 0.001f * drivetrain.KW2CV;
			if (actualMaxPower > drivetrain.maxPower && actualMaxPower > oldactualMaxPower && num2 < drivetrain.maxTorque)
			{
				factor = drivetrain.maxPower / actualMaxPower;
			}
		}
		m_maxTorque = Mathf.Round(drivetrain.maxTorque * factor);
		for (float num3 = 0f; num3 <= (float)maxRPM; num3 += 1f)
		{
			float num2 = drivetrain.CalcEngineTorque(drivetrain.powerMultiplier * factor, num3);
			if (num2 > m_maxTorque * drivetrain.powerMultiplier)
			{
				num2 = m_maxTorque * drivetrain.powerMultiplier;
			}
			float num4 = num2 * num3 * drivetrain.RPM2angularVelo * 0.001f * drivetrain.KW2CV;
			float num5 = drivetrain.CalcEngineTorqueInt_reference(drivetrain.powerMultiplier, num3);
			int value = Mathf.RoundToInt(num2 / (m_maxTorque * drivetrain.powerMultiplier) * (float)gridHeight * 0.5f + (float)floor);
			int value2 = Mathf.RoundToInt(num4 / (drivetrain.maxPower * drivetrain.powerMultiplier) * (float)gridHeight + (float)floor);
			int value3 = Mathf.RoundToInt(num5 / (drivetrain.maxTorque * drivetrain.powerMultiplier) * (float)gridHeight * 0.5f + (float)floor);
			float num6 = num2 - drivetrain.CalcEngineFrictionTorque(drivetrain.powerMultiplier, num3);
			int value4 = Mathf.RoundToInt(num6 / (m_maxTorque * drivetrain.powerMultiplier) * (float)gridHeight * 0.5f + (float)floor);
			float num7 = num6 * num3 * drivetrain.RPM2angularVelo * 0.001f * drivetrain.KW2CV;
			int value5 = Mathf.RoundToInt(num7 / (drivetrain.maxPower * drivetrain.powerMultiplier) * (float)gridHeight + (float)floor);
			int x = Mathf.RoundToInt(num3 / (float)maxRPM * (float)gridWidth + (float)((grid.width - gridWidth) / 2) - 1f);
			grid.SetPixel(x, Mathf.Clamp(value, floor, top), Color.black);
			grid.SetPixel(x, Mathf.Clamp(value2, floor, top), Color.red);
			grid.SetPixel(x, Mathf.Clamp(value4, floor, top), Color.black);
			grid.SetPixel(x, Mathf.Clamp(value5, floor, top), Color.red);
			if (!engineTorqueFromFile)
			{
				grid.SetPixel(x, Mathf.Clamp(value3, floor, top), Color.blue);
			}
		}
		grid.Apply(false);
	}

	private void ApplyGears()
	{
		grid.SetPixels(buffer);
		int num = 0;
		maxKmh = Mathf.RoundToInt((float)maxRPM * axles.frontAxle.leftWheel.radius * 2f * 0.1885f / (drivetrain.gearRatios[drivetrain.gearRatios.Length - 1] * drivetrain.finalDriveRatio));
		for (int i = 0; i < drivetrain.gearRatios.Length; i++)
		{
			if (num == colors.Length)
			{
				num = 0;
			}
			for (float num2 = 0f; num2 <= (float)maxRPM; num2 += 1f)
			{
				if (i != 1)
				{
					float num3 = num2 * axles.frontAxle.leftWheel.radius * 2f * 0.1885f / (Mathf.Abs(drivetrain.gearRatios[i]) * drivetrain.finalDriveRatio);
					int num4 = Mathf.RoundToInt(num3 / (float)maxKmh * (float)gridHeight + (float)floor);
					if (num4 <= top)
					{
						int x = Mathf.RoundToInt(num2 / (float)maxRPM * (float)gridWidth + (float)((grid.width - gridWidth) / 2) - 1f);
						grid.SetPixel(x, Mathf.Clamp(num4, floor, top), colors[num]);
					}
				}
			}
			num++;
		}
		grid.Apply(false);
	}

	private void Transmission()
	{
		GUI.Label(NameLabelRect[0], "Differential Lock Coefficient: " + RoundTo(drivetrain.differentialLockCoefficient, 0) + "%");
		drivetrain.differentialLockCoefficient = GUI.HorizontalSlider(SliderRect[0], RoundTo(drivetrain.differentialLockCoefficient, 0), 0f, 100f);
		GUI.Label(NameLabelRect[1], "Type of drive: " + drivetrain.transmission);
		transmissionType = GUI.SelectionGrid(SliderRect[1], transmissionType, transmissionTypes, 3);
		Rect position = new Rect(NameLabelRect[1].x - 25f, NameLabelRect[1].y - 30f, 800f, 600f);
		if (grid != null)
		{
			GUI.BeginGroup(position);
			float num = (grid.width - gridWidth) / 2;
			float num2 = (grid.height - gridHeight) / 2 - 10;
			GUI.Label(new Rect(num - 8f, (float)(-gridHeight) * 0.17f + num2, 140f, 20f), string.Empty + maxKmh + " kmh", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)(-gridHeight) * 0.01f + num2, 140f, 20f), string.Empty + Mathf.Round((float)maxKmh * 0.857f) + " kmh", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.16f + num2, 140f, 20f), string.Empty + Mathf.Round((float)maxKmh * 0.714f) + " kmh", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.33f + num2, 140f, 20f), string.Empty + Mathf.Round((float)maxKmh * 0.571f) + " kmh", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.5f + num2, 140f, 20f), string.Empty + Mathf.Round((float)maxKmh * 0.42857f) + " kmh", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.67f + num2, 140f, 20f), string.Empty + Mathf.Round((float)maxKmh * 0.2857f) + " kmh", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)gridHeight * 0.84f + num2, 140f, 20f), string.Empty + Mathf.Round((float)maxKmh * 0.143f) + " kmh", powerStyle);
			GUI.Label(new Rect(num - 8f, (float)(gridHeight * 1) + num2 - 5f, 140f, 20f), string.Empty + 0 + " kmh", powerStyle);
			float num3 = (float)gridWidth * 0.04f;
			GUI.Label(new Rect(num, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + 0, torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.125f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.125f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.25f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.25f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.375f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.375f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.5f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.5f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.625f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.625f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.75f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.75f), torqueStyle);
			GUI.Label(new Rect(num + (float)gridWidth * 0.875f - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round((float)maxRPM * 0.875f), torqueStyle);
			GUI.Label(new Rect(num + (float)(gridWidth * 1) - num3, (float)gridHeight + num2 + 15f, 140f, 20f), string.Empty + Mathf.Round(maxRPM) + " RPM", torqueStyle);
			Rect position2 = new Rect(0f, 0f, grid.width, grid.height);
			GUI.DrawTexture(position2, grid);
			GUI.EndGroup();
		}
		string text = ((drivetrain.gear < drivetrain.neutral) ? "R" : ((drivetrain.gear != drivetrain.neutral) ? (string.Empty + (drivetrain.gear - drivetrain.neutral)) : "H"));
		GUI.Label(new Rect(NameLabelRect[20].x + 40f, NameLabelRect[20].y, NameLabelRect[20].width, NameLabelRect[20].height), "Current Gear Engaged: " + text + " @ " + Mathf.Round(drivetrain.rpm) + " RPM");
		GUI.Label(new Rect(NameLabelRect[20].x + 40f, NameLabelRect[20].y + 30f, NameLabelRect[20].width, NameLabelRect[20].height), "Current Speed: " + Mathf.Round(Mathf.Abs(drivetrain.velo) * 3.6f) + " km/h");
		GUI.Label(NameLabelRect[10], "Number of Gears: " + (drivetrain.gearRatios.Length - 2));
		GUI.Label(NameLabelRect[11], "Final Drive Ratio: " + drivetrain.finalDriveRatio);
		drivetrain.finalDriveRatio = RoundTo(GUI.HorizontalSlider(SliderRect[11], drivetrain.finalDriveRatio, 0.5f, 15f), 2);
		GUI.Label(NameLabelRect[12], "Reverse Gear Ratio: " + drivetrain.gearRatios[0]);
		drivetrain.gearRatios[0] = RoundTo(GUI.HorizontalSlider(SliderRect[12], drivetrain.gearRatios[0], -0.5f, -15f), 2);
		ScrollPosition = GUI.BeginScrollView(ScrollViewRect, ScrollPosition, ScrollRect);
		for (int i = 0; i < drivetrain.gearRatios.Length - 2; i++)
		{
			GUI.Label(ScrollLabelsRect[i], "Gear " + (i + 1) + " Ratio: " + drivetrain.gearRatios[i + 2]);
			drivetrain.gearRatios[i + 2] = RoundTo(GUI.HorizontalSlider(ScrollSlidersRect[i], drivetrain.gearRatios[i + 2], 0.5f, 15f), 3);
		}
		GUI.EndScrollView();
		if (GUI.changed)
		{
			if (grid != null)
			{
				ApplyGears();
			}
			if (oldTransmissionType != transmissionType)
			{
				SetTransmission();
			}
		}
	}

	private void Suspensions()
	{
		GUI.Label(NameLabelRect[0], "Front Suspension Travel: " + axles.frontAxle.suspensionTravel + " m");
		axles.frontAxle.suspensionTravel = RoundTo(GUI.HorizontalSlider(SliderRect[0], axles.frontAxle.suspensionTravel, 0.1f, 0.5f), 2);
		GUI.Label(NameLabelRect[1], "Front Suspension Stiffness: " + RoundTo(axles.frontAxle.suspensionRate / 1000f, 1) + " N/mm");
		axles.frontAxle.suspensionRate = RoundTo(GUI.HorizontalSlider(SliderRect[1], axles.frontAxle.suspensionRate, 0f, 150000f), 1);
		GUI.Label(NameLabelRect[2], "Front Bump Damping: " + RoundTo(axles.frontAxle.bumpRate / 1000f, 1) + " Ns/mm");
		axles.frontAxle.bumpRate = RoundTo(GUI.HorizontalSlider(SliderRect[2], axles.frontAxle.bumpRate, 0f, 12000f), 1);
		GUI.Label(NameLabelRect[3], "Front Rebound Damping: " + RoundTo(axles.frontAxle.reboundRate / 1000f, 1) + " Ns/mm");
		axles.frontAxle.reboundRate = RoundTo(GUI.HorizontalSlider(SliderRect[3], axles.frontAxle.reboundRate, 0f, 12000f), 1);
		GUI.Label(NameLabelRect[4], "Front Antirollbar Stiffness: " + RoundTo(axles.frontAxle.antiRollBarRate / 1000f, 1) + " N/mm");
		axles.frontAxle.antiRollBarRate = RoundTo(GUI.HorizontalSlider(SliderRect[4], axles.frontAxle.antiRollBarRate, 0f, 100000f), 1);
		GUI.Label(NameLabelRect[5], "Front Camber: " + RoundTo(axles.frontAxle.camber, 0) + "°");
		axles.frontAxle.camber = RoundTo(GUI.HorizontalSlider(SliderRect[5], axles.frontAxle.camber, -10f, 10f), 0);
		GUI.Label(NameLabelRect[6], "Front Caster: " + RoundTo(axles.frontAxle.caster, 0) + "°");
		axles.frontAxle.caster = RoundTo(GUI.HorizontalSlider(SliderRect[6], axles.frontAxle.caster, -10f, 10f), 0);
		GUI.Label(NameLabelRect[10], "Rear Suspension Travel: " + axles.rearAxle.suspensionTravel + " m");
		axles.rearAxle.suspensionTravel = RoundTo(GUI.HorizontalSlider(SliderRect[10], axles.rearAxle.suspensionTravel, 0.1f, 0.5f), 2);
		GUI.Label(NameLabelRect[11], "Rear Suspension Stiffness: " + RoundTo(axles.rearAxle.suspensionRate / 1000f, 1) + " N/mm");
		axles.rearAxle.suspensionRate = RoundTo(GUI.HorizontalSlider(SliderRect[11], axles.rearAxle.suspensionRate, 0f, 150000f), 1);
		GUI.Label(NameLabelRect[12], "Rear Bump Damping: " + RoundTo(axles.rearAxle.bumpRate / 1000f, 1) + " Ns/mm");
		axles.rearAxle.bumpRate = RoundTo(GUI.HorizontalSlider(SliderRect[12], axles.rearAxle.bumpRate, 0f, 12000f), 1);
		GUI.Label(NameLabelRect[13], "Rear Rebound Damping: " + RoundTo(axles.rearAxle.reboundRate / 1000f, 1) + " Ns/mm");
		axles.rearAxle.reboundRate = RoundTo(GUI.HorizontalSlider(SliderRect[13], axles.rearAxle.reboundRate, 0f, 12000f), 1);
		GUI.Label(NameLabelRect[14], "Rear Antirollbar Stiffness: " + RoundTo(axles.rearAxle.antiRollBarRate / 1000f, 1) + " N/mm");
		axles.rearAxle.antiRollBarRate = RoundTo(GUI.HorizontalSlider(SliderRect[14], axles.rearAxle.antiRollBarRate, 0f, 100000f), 1);
		GUI.Label(NameLabelRect[15], "Rear Camber: " + RoundTo(axles.rearAxle.camber, 0) + "°");
		axles.rearAxle.camber = RoundTo(GUI.HorizontalSlider(SliderRect[15], axles.rearAxle.camber, -10f, 10f), 0);
		GUI.Label(NameLabelRect[20], "Front Fast Bump Factor: " + axles.frontAxle.fastBumpFactor);
		axles.frontAxle.fastBumpFactor = RoundTo(GUI.HorizontalSlider(SliderRect[20], axles.frontAxle.fastBumpFactor, 0f, 1f), 1);
		GUI.Label(NameLabelRect[21], "Front Fast Rebound Factor: " + axles.frontAxle.fastReboundFactor);
		axles.frontAxle.fastReboundFactor = RoundTo(GUI.HorizontalSlider(SliderRect[21], axles.frontAxle.fastReboundFactor, 0f, 1f), 1);
		GUI.Label(NameLabelRect[22], "Front Wheels Maximum Steering Angle: " + RoundTo(axles.frontAxle.maxSteeringAngle, 0) + "°");
		axles.frontAxle.maxSteeringAngle = RoundTo(GUI.HorizontalSlider(SliderRect[22], axles.frontAxle.maxSteeringAngle, 0f, 45f), 0);
		GUI.Label(NameLabelRect[23], "Rear Wheels Maximum Steering Angle: " + RoundTo(axles.rearAxle.maxSteeringAngle, 0) + "°");
		axles.rearAxle.maxSteeringAngle = RoundTo(GUI.HorizontalSlider(SliderRect[23], axles.rearAxle.maxSteeringAngle, 0f, 45f), 0);
		if (GUI.changed)
		{
			carDynamics.SetWheelsParams();
		}
	}

	private void Brakes()
	{
		GUI.Label(NameLabelRect[0], "Front Maximum Brake Torque: " + axles.frontAxle.brakeFrictionTorque + " Nm");
		axles.frontAxle.brakeFrictionTorque = RoundTo(GUI.HorizontalSlider(SliderRect[0], axles.frontAxle.brakeFrictionTorque, 0f, 10000f), 0);
		GUI.Label(NameLabelRect[1], "Rear Maximum Brake Torque: " + axles.rearAxle.brakeFrictionTorque + " Nm");
		axles.rearAxle.brakeFrictionTorque = RoundTo(GUI.HorizontalSlider(SliderRect[1], axles.rearAxle.brakeFrictionTorque, 0f, 10000f), 0);
		GUI.Label(NameLabelRect[2], "Brake Balance (front: " + carDynamics.frontRearBrakeBalance * 100f + "% rear: " + (1f - carDynamics.frontRearBrakeBalance) * 100f + "%)");
		carDynamics.frontRearBrakeBalance = RoundTo(GUI.HorizontalSlider(SliderRect[2], carDynamics.frontRearBrakeBalance, 0f, 1f), 2);
		GUI.Label(NameLabelRect[3], "Front Maximum Handbrake Torque: " + axles.frontAxle.handbrakeFrictionTorque + " Nm");
		axles.frontAxle.handbrakeFrictionTorque = RoundTo(GUI.HorizontalSlider(SliderRect[3], axles.frontAxle.handbrakeFrictionTorque, 0f, 10000f), 0);
		GUI.Label(NameLabelRect[4], "Rear Maximum Handbrake Torque: " + axles.rearAxle.handbrakeFrictionTorque + " Nm");
		axles.rearAxle.handbrakeFrictionTorque = RoundTo(GUI.HorizontalSlider(SliderRect[4], axles.rearAxle.handbrakeFrictionTorque, 0f, 10000f), 0);
		GUI.Label(NameLabelRect[5], "Handbrake Balance (front: " + carDynamics.frontRearHandBrakeBalance * 100f + "% rear: " + (1f - carDynamics.frontRearHandBrakeBalance) * 100f + "%)");
		carDynamics.frontRearHandBrakeBalance = RoundTo(GUI.HorizontalSlider(SliderRect[5], carDynamics.frontRearHandBrakeBalance, 0f, 1f), 2);
		if (GUI.changed)
		{
			carDynamics.SetBrakes();
		}
	}

	private void Tires()
	{
		GUI.Label(NameLabelRect[0], "Front Forward Grip: " + axles.frontAxle.forwardGripFactor);
		axles.frontAxle.forwardGripFactor = RoundTo(GUI.HorizontalSlider(SliderRect[0], axles.frontAxle.forwardGripFactor, 0.1f, 2f), 1);
		GUI.Label(NameLabelRect[1], "Front Sideways Grip: " + axles.frontAxle.sidewaysGripFactor);
		axles.frontAxle.sidewaysGripFactor = RoundTo(GUI.HorizontalSlider(SliderRect[1], axles.frontAxle.sidewaysGripFactor, 0.1f, 2f), 1);
		GUI.Label(NameLabelRect[2], "Rear Forward Grip: " + axles.rearAxle.forwardGripFactor);
		axles.rearAxle.forwardGripFactor = RoundTo(GUI.HorizontalSlider(SliderRect[2], axles.rearAxle.forwardGripFactor, 0.1f, 2f), 1);
		GUI.Label(NameLabelRect[3], "Rear Sideways Grip: " + axles.rearAxle.sidewaysGripFactor);
		axles.rearAxle.sidewaysGripFactor = RoundTo(GUI.HorizontalSlider(SliderRect[3], axles.rearAxle.sidewaysGripFactor, 0.1f, 2f), 1);
		if (Time.fixedDeltaTime > 0.02f)
		{
			GUI.enabled = false;
			GUI.Label(NameLabelRect[4], "[Tire Pressure Calculation Disabled. Decrease TimeStep to enable it]");
			axles.frontAxle.tiresPressure = RoundTo(GUI.HorizontalSlider(SliderRect[4], axles.frontAxle.tiresPressure, 0f, 400.5f), 2);
			GUI.Label(NameLabelRect[5], "[Tire Pressure Calculation Disabled. Decrease TimeStep to enable it]");
			axles.rearAxle.tiresPressure = RoundTo(GUI.HorizontalSlider(SliderRect[5], axles.rearAxle.tiresPressure, 0f, 400.5f), 2);
		}
		else
		{
			GUI.Label(NameLabelRect[4], "Front Tires Pressure: " + RoundTo(axles.frontAxle.tiresPressure, 0) + " kPa (" + RoundTo(axles.frontAxle.tiresPressure / 101.325f, 1) + " atm)");
			float num = RoundTo(GUI.HorizontalSlider(SliderRect[4], axles.frontAxle.tiresPressure, 0f, 400.5f), 2);
			if (num < 40f && num > 0f)
			{
				num = 40f;
			}
			axles.frontAxle.tiresPressure = num;
			GUI.Label(NameLabelRect[5], "Rear Tires Pressure: " + RoundTo(axles.rearAxle.tiresPressure, 0) + " kPa (" + RoundTo(axles.rearAxle.tiresPressure / 101.325f, 1) + " atm)");
			float num2 = RoundTo(GUI.HorizontalSlider(SliderRect[5], axles.rearAxle.tiresPressure, 0f, 400.5f), 2);
			if (num2 < 40f && num2 > 0f)
			{
				num2 = 40f;
			}
			axles.rearAxle.tiresPressure = num2;
		}
		GUI.enabled = true;
		GUI.Label(NameLabelRect[10], "Tires Type Front:");
		Rect position = new Rect(SliderRect[10].x, SliderRect[10].y, SliderRect[10].width, SliderRect[10].height + 150f);
		tiresTypeFront = GUI.SelectionGrid(position, tiresTypeFront, tireTypes, 2);
		GUI.Label(NameLabelRect[14], "Tires Type Rear:");
		Rect position2 = new Rect(SliderRect[14].x, SliderRect[14].y, SliderRect[14].width, SliderRect[14].height + 150f);
		tiresTypeRear = GUI.SelectionGrid(position2, tiresTypeRear, tireTypes, 2);
		if (GUI.changed)
		{
			if (Time.fixedDeltaTime > 0.02f)
			{
				carDynamics.tirePressureEnabled = false;
			}
			else
			{
				carDynamics.tirePressureEnabled = true;
			}
			carDynamics.SetWheelsParams();
			SetTiresType(tiresTypeFront, axles.frontAxle);
			SetTiresType(tiresTypeRear, axles.rearAxle);
			carDynamics.SetTiresType();
		}
	}

	private void Body()
	{
		GUI.Label(NameLabelRect[0], "Vehicle Weight: " + mrigidbody.mass + " Kg");
		mrigidbody.mass = GUI.HorizontalSlider(SliderRect[0], RoundTo(mrigidbody.mass, 0), 500f, 10000f);
		GUI.Label(NameLabelRect[1], "Weight Repartition (Front:Rear) " + carDynamics.frontRearWeightRepartition * 100f + ":" + (100f - carDynamics.frontRearWeightRepartition * 100f));
		carDynamics.zlocalPosition = GUI.HorizontalSlider(SliderRect[1], carDynamics.zlocalPosition, zlocalPositionLimit, 0f - zlocalPositionLimit);
		if (aerodynamicResistance != null)
		{
			GUI.Label(NameLabelRect[2], "Drag Coefficient, Cx: " + aerodynamicResistance.Cx);
			aerodynamicResistance.Cx = RoundTo(GUI.HorizontalSlider(SliderRect[2], aerodynamicResistance.Cx, 0.01f, 1f), 2);
			GUI.Label(NameLabelRect[3], "Drag Area, CxA: " + aerodynamicResistance.Area + " square meters");
			aerodynamicResistance.Area = RoundTo(GUI.HorizontalSlider(SliderRect[3], aerodynamicResistance.Area, 0.001f, 20f), 3);
		}
		if (fuelTanks.Length != 0)
		{
			this.i = 0;
			for (int i = 0; i < fuelTanks.Length; i++)
			{
				GUI.Label(NameLabelRect[20 + this.i], "Tank" + (i + 1) + " Capacity: " + fuelTanks[i].tankCapacity + " liters");
				fuelTanks[i].tankCapacity = RoundTo(GUI.HorizontalSlider(SliderRect[20 + this.i], fuelTanks[i].tankCapacity, 0f, 800f), 0);
				GUI.Label(NameLabelRect[21 + this.i], "Tank" + (i + 1) + " Current Fuel: " + fuelTanks[i].currentFuel + " liters");
				currentFuels[i] = RoundTo(GUI.HorizontalSlider(SliderRect[21 + this.i], fuelTanks[i].currentFuel, 0f, fuelTanks[i].tankCapacity), 0);
				this.i += 2;
			}
			GUI.Label(NameLabelRect[10], "Fuel Consumption At " + drivetrain.fuelConsumptionSpeed + " km/h: " + drivetrain.fuelConsumptionAtCostantSpeed + " liters per 100 kms");
			drivetrain.fuelConsumptionAtCostantSpeed = RoundTo(GUI.HorizontalSlider(SliderRect[10], drivetrain.fuelConsumptionAtCostantSpeed, 0f, 50f), 1);
			GUI.Label(NameLabelRect[11], "Fuel Consumption Speed: " + drivetrain.fuelConsumptionSpeed + " km/h");
			drivetrain.fuelConsumptionSpeed = RoundTo(GUI.HorizontalSlider(SliderRect[11], drivetrain.fuelConsumptionSpeed, 1f, 200f), 0);
			GUI.Label(NameLabelRect[12], "Current Consumption: " + RoundTo(drivetrain.currentConsumption, 2) + " liters per 100 kms");
		}
		if (!GUI.changed)
		{
			return;
		}
		carDynamics.FixPhysX();
		SetCOGPosition(carDynamics.zlocalPosition);
		for (int j = 0; j < fuelTanks.Length; j++)
		{
			fuelTanks[j].currentFuel = currentFuels[j];
		}
		drivetrain.RPMAtSpeedInLastGear = drivetrain.CalcRPMAtSpeedInLastGear(drivetrain.fuelConsumptionSpeed);
		float num = mrigidbody.mass / mass;
		if (num > 1f)
		{
			Wheel[] allWheels = axles.allWheels;
			foreach (Wheel wheel in allWheels)
			{
				wheel.mass = wheel.originalMass * num;
				wheel.rotationalInertia = wheel.mass / 2f * wheel.radius * wheel.radius;
			}
		}
	}

	private void Assistance()
	{
		carController.ABS = GUI.Toggle(new Rect(NameLabelRect[0].x, NameLabelRect[0].y, NameLabelRect[0].width - 150f, NameLabelRect[0].height), carController.ABS, "ABS (AntiLock Braking)");
		carController.TCS = GUI.Toggle(new Rect(NameLabelRect[1].x, NameLabelRect[1].y, NameLabelRect[1].width - 150f, NameLabelRect[1].height), carController.TCS, "TCS (Traction Control)");
		carController.ESP = GUI.Toggle(new Rect(NameLabelRect[2].x, NameLabelRect[2].y, NameLabelRect[2].width - 150f, NameLabelRect[2].height), carController.ESP, "ESP (Stability Control)");
		carController.steerAssistance = GUI.Toggle(new Rect(NameLabelRect[3].x, NameLabelRect[3].y, NameLabelRect[3].width - 150f, NameLabelRect[3].height), carController.steerAssistance, "Steer Assistance");
		drivetrain.automatic = GUI.Toggle(new Rect(NameLabelRect[4].x, NameLabelRect[4].y, NameLabelRect[4].width - 150f, NameLabelRect[4].height), drivetrain.automatic, "Automatic Transmission");
		drivetrain.autoClutch = GUI.Toggle(new Rect(NameLabelRect[5].x, NameLabelRect[5].y, NameLabelRect[5].width - 150f, NameLabelRect[5].height), drivetrain.autoClutch, "Automatic Clutch");
		drivetrain.autoReverse = GUI.Toggle(new Rect(NameLabelRect[6].x, NameLabelRect[6].y, NameLabelRect[6].width - 150f, NameLabelRect[6].height), drivetrain.autoReverse, "Automatic Reverse");
		mouseControllerEnabled = GUI.Toggle(new Rect(NameLabelRect[7].x, NameLabelRect[7].y, NameLabelRect[7].width - 150f, NameLabelRect[7].height), carDynamics.controller == CarDynamics.Controller.mouse, "Mouse Controller");
		GUI.Label(NameLabelRect[20], "Steer Correction Factor: " + carController.steerCorrectionFactor);
		carController.steerCorrectionFactor = RoundTo(GUI.HorizontalSlider(SliderRect[20], carController.steerCorrectionFactor, 0f, 10f), 0);
		GUI.Label(NameLabelRect[21], "Fixed TimeStep: " + Time.fixedDeltaTime + " secs (" + 1f / Time.fixedDeltaTime + " Hz)");
		Time.fixedDeltaTime = RoundTo(GUI.HorizontalSlider(SliderRect[21], Time.fixedDeltaTime, 0.01f, 0.05f), 2);
		if (arcader != null)
		{
			arcader.enabled = GUI.Toggle(NameLabelRect[22], arcader.enabled, "Arcade Mode");
			if (arcader.enabled)
			{
				GUI.Label(NameLabelRect[23], "Minimum Velocity: " + arcader.minVelocity + " km/h");
				arcader.minVelocity = RoundTo(GUI.HorizontalSlider(SliderRect[23], arcader.minVelocity, 0f, 50f), 0);
				GUI.Label(NameLabelRect[24], "Overall Strength: " + arcader.overallStrength);
				arcader.overallStrength = RoundTo(GUI.HorizontalSlider(SliderRect[24], arcader.overallStrength, 0f, 1f), 1);
				GUI.Label(NameLabelRect[25], "COG Helper Strength: " + arcader.COGHelperStrength);
				arcader.COGHelperStrength = RoundTo(GUI.HorizontalSlider(SliderRect[25], arcader.COGHelperStrength, 0f, 2f), 1);
				GUI.Label(NameLabelRect[26], "Torque Helper Strength: " + arcader.torqueHelperStrength);
				arcader.torqueHelperStrength = RoundTo(GUI.HorizontalSlider(SliderRect[26], arcader.torqueHelperStrength, 0f, 2f), 1);
				GUI.Label(NameLabelRect[27], "Grip Helper Strength: " + arcader.gripHelperStrength);
				arcader.gripHelperStrength = RoundTo(GUI.HorizontalSlider(SliderRect[27], arcader.gripHelperStrength, 0f, 2f), 1);
			}
		}
		GUI.Label(NameLabelRect[10], "Time to fully engage throttle: " + carController.throttleTime + " secs");
		carController.throttleTime = RoundTo(GUI.HorizontalSlider(SliderRect[10], carController.throttleTime, 0.001f, 1f), 3);
		GUI.Label(NameLabelRect[11], "Time to fully release throttle: " + carController.throttleReleaseTime + " secs");
		carController.throttleReleaseTime = RoundTo(GUI.HorizontalSlider(SliderRect[11], carController.throttleReleaseTime, 0.001f, 1f), 3);
		GUI.Label(NameLabelRect[12], "Time to fully engage brakes: " + carController.brakesTime + " secs");
		carController.brakesTime = RoundTo(GUI.HorizontalSlider(SliderRect[12], carController.brakesTime, 0.001f, 1f), 3);
		GUI.Label(NameLabelRect[13], "Time to fully release brakes: " + carController.brakesReleaseTime + " secs");
		carController.brakesReleaseTime = RoundTo(GUI.HorizontalSlider(SliderRect[13], carController.brakesReleaseTime, 0.001f, 1f), 3);
		GUI.Label(NameLabelRect[14], "Time to fully turn steering wheel: " + carController.steerTime + " secs");
		carController.steerTime = RoundTo(GUI.HorizontalSlider(SliderRect[14], carController.steerTime, 0.01f, 1f), 2);
		GUI.Label(NameLabelRect[15], "Time to fully release steering wheel: " + carController.steerReleaseTime + " secs");
		carController.steerReleaseTime = RoundTo(GUI.HorizontalSlider(SliderRect[15], carController.steerReleaseTime, 0.01f, 1f), 2);
		GUI.Label(NameLabelRect[16], "Turn steering wheel slow down factor: " + carController.veloSteerTime);
		carController.veloSteerTime = RoundTo(GUI.HorizontalSlider(SliderRect[16], carController.veloSteerTime, 0f, 1f), 2);
		GUI.Label(NameLabelRect[17], "Release steering wheel slow down factor: " + carController.veloSteerReleaseTime);
		carController.veloSteerReleaseTime = RoundTo(GUI.HorizontalSlider(SliderRect[17], carController.veloSteerReleaseTime, 0f, 1f), 2);
		if (GUI.changed)
		{
			if (Time.fixedDeltaTime > 0.02f)
			{
				carDynamics.tirePressureEnabled = false;
			}
			else
			{
				carDynamics.tirePressureEnabled = true;
			}
			carDynamics.SetWheelsParams();
			if (arcader != null)
			{
				if (arcader.enabled)
				{
					if (arcader.enabled != oldArcaderEnabled)
					{
						carController.ESP = false;
					}
				}
				else if (arcader.enabled != oldArcaderEnabled)
				{
					carController.ESP = ESP;
				}
			}
			if (oldMouseControllerEnabled != mouseControllerEnabled)
			{
				if (mouseControllerEnabled)
				{
					carDynamics.controller = CarDynamics.Controller.mouse;
				}
				else
				{
					carDynamics.controller = CarDynamics.Controller.axis;
				}
				if (carDynamics.carController != null)
				{
					carDynamics.SetController(carDynamics.controller.ToString());
					carController = carDynamics.carController;
				}
			}
		}
		oldMouseControllerEnabled = mouseControllerEnabled;
		if (arcader != null)
		{
			oldArcaderEnabled = arcader.enabled;
		}
	}

	private void RectCalculation(Vector2 Size)
	{
		ToggleRect = new Rect(8f, 0f, 200f, 20f);
		SettingsWindowRect = new Rect(10f, 5f, Size.x - 20f, Size.y - 20f);
		GridRect = new Rect(10f, 20f, SettingsWindowRect.width - 20f, 20f);
		float num = (SettingsWindowRect.height - 50f) / 20f / 5f;
		float num2 = num;
		for (int i = 0; i < 10; i++)
		{
			NameLabelRect[i] = new Rect(10f, 50f + (float)i * (50f + num), SettingsWindowRect.width / 3f + 100f, 22f);
			SliderRect[i] = new Rect(NameLabelRect[i].x, NameLabelRect[i].y + 20f, SettingsWindowRect.width / 3f - 10f, NameLabelRect[i].height);
		}
		for (int j = 0; j < 10; j++)
		{
			NameLabelRect[10 + j] = new Rect(SettingsWindowRect.width / 3f * 2f, 50f + (float)j * (50f + num2), SettingsWindowRect.width / 3f + 100f, 22f);
			SliderRect[10 + j] = new Rect(NameLabelRect[10 + j].x, NameLabelRect[10 + j].y + 20f, SettingsWindowRect.width / 3f - 10f, NameLabelRect[j].height);
		}
		for (int k = 0; k < 10; k++)
		{
			NameLabelRect[20 + k] = new Rect(SettingsWindowRect.width / 3f, 50f + (float)k * (50f + num2), SettingsWindowRect.width / 3f + 100f, 22f);
			SliderRect[20 + k] = new Rect(NameLabelRect[20 + k].x, NameLabelRect[20 + k].y + 20f, SettingsWindowRect.width / 3f - 10f, NameLabelRect[k].height);
		}
		ScrollViewRect = new Rect(SettingsWindowRect.width / 3f * 2f, 50f + 3f * (50f + num2), SettingsWindowRect.width / 3f - 10f, SettingsWindowRect.height - SliderRect[23].y);
	}

	private void ScrollRectCalculation(Vector2 Size, int AmountOfGears)
	{
		ScrollLabelsRect = new Rect[AmountOfGears];
		ScrollSlidersRect = new Rect[AmountOfGears];
		for (int i = 0; i < AmountOfGears; i++)
		{
			ScrollLabelsRect[i] = new Rect(NameLabelRect[0].x, NameLabelRect[0].y - 50f + (float)(50 * i), NameLabelRect[0].width - 50f, NameLabelRect[0].height);
			ScrollSlidersRect[i] = new Rect(SliderRect[0].x, SliderRect[0].y - 50f + (float)(50 * i), SliderRect[0].width - 50f, SliderRect[0].height);
		}
		ScrollRect = new Rect(0f, 0f, ScrollViewRect.width - 50f, 50f + (float)AmountOfGears * 50f);
	}

	private void SetCOGPosition(float zlocalPosition)
	{
		carDynamics.centerOfMass.localPosition = new Vector3(carDynamics.centerOfMass.localPosition.x, carDynamics.ylocalPosition, zlocalPosition);
		mrigidbody.centerOfMass = carDynamics.centerOfMass.localPosition;
	}

	private void SetTiresType(int tiresType, Axle axle)
	{
		switch (tiresType)
		{
		case 0:
			axle.tires = CarDynamics.Tires.competition_front;
			break;
		case 1:
			axle.tires = CarDynamics.Tires.competition_rear;
			break;
		case 2:
			axle.tires = CarDynamics.Tires.supersport_front;
			break;
		case 3:
			axle.tires = CarDynamics.Tires.supersport_rear;
			break;
		case 4:
			axle.tires = CarDynamics.Tires.sport_front;
			break;
		case 5:
			axle.tires = CarDynamics.Tires.sport_rear;
			break;
		case 6:
			axle.tires = CarDynamics.Tires.touring_front;
			break;
		case 7:
			axle.tires = CarDynamics.Tires.touring_rear;
			break;
		case 8:
			axle.tires = CarDynamics.Tires.offroad_front;
			break;
		case 9:
			axle.tires = CarDynamics.Tires.offroad_rear;
			break;
		case 10:
			axle.tires = CarDynamics.Tires.truck_front;
			break;
		case 11:
			axle.tires = CarDynamics.Tires.truck_rear;
			break;
		}
	}

	private void SetTransmission()
	{
		switch (transmissionType)
		{
		case 0:
			drivetrain.transmission = Drivetrain.Transmissions.RWD;
			break;
		case 1:
			drivetrain.transmission = Drivetrain.Transmissions.FWD;
			break;
		case 2:
			drivetrain.transmission = Drivetrain.Transmissions.AWD;
			break;
		}
		drivetrain.SetTransmission(drivetrain.transmission);
		oldTransmissionType = transmissionType;
	}

	private float RoundTo(float value, int precision)
	{
		int num = 1;
		for (int i = 1; i <= precision; i++)
		{
			num *= 10;
		}
		return Mathf.Round(value * (float)num) / (float)num;
	}
}
