using System.Collections;
using UnityEngine;

public class StartGame : MonoBehaviour
{
	public CarCamerasController carCamerasController;

	public CarCamerasController mapCameraController;

	private GameObject StartTimer;

	private SettingsMenu settingsMenu;

	public GameObject[] cars;

	private GameObject selectedCar;

	private bool altNormalForce;

	public Skidmarks skidmarks;

	public float fixedTimeStep = 0.02f;

	private int index;

	private int lastIndex;

	private float oldFixedDeltaTime;

	private CarCameras carCameras;

	private Light mlight;

	private DashBoard dashBoard;

	private GameObject unityCar;

	private void Awake()
	{
		if (carCamerasController == null)
		{
			carCamerasController = Camera.main.GetComponent<CarCamerasController>();
		}
		settingsMenu = GetComponent<SettingsMenu>();
		carCameras = Camera.main.GetComponent<CarCameras>();
		if (mapCameraController == null)
		{
			if ((bool)GameObject.FindWithTag("MapCamera"))
			{
				mapCameraController = GameObject.FindWithTag("MapCamera").GetComponent<CarCamerasController>();
			}
			else if ((bool)GameObject.Find("MapCamera"))
			{
				mapCameraController = GameObject.Find("MapCamera").GetComponent<CarCamerasController>();
			}
		}
		object[] array = Object.FindObjectsOfType(typeof(GameObject));
		object[] array2 = array;
		foreach (object obj in array2)
		{
			GameObject gameObject = (GameObject)obj;
			if (gameObject.GetComponent<Light>() != null)
			{
				mlight = gameObject.GetComponent<Light>();
			}
		}
		Time.fixedDeltaTime = fixedTimeStep;
		if (cars.Length == 0)
		{
			cars = GameObject.FindGameObjectsWithTag("Car");
		}
		GameObject[] array3 = cars;
		foreach (GameObject gameObject2 in array3)
		{
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
				if (gameObject2.transform.GetComponent<CarDebug>() != null)
				{
					gameObject2.transform.GetComponent<CarDebug>().enabled = false;
				}
				if (gameObject2.transform.GetComponent<Setup>() != null)
				{
					gameObject2.transform.GetComponent<Setup>().loadingSetup = false;
					gameObject2.transform.GetComponent<Setup>().enabled = false;
				}
				if (gameObject2.transform.GetComponent<CarDynamics>().skidmarks == null && (bool)this.skidmarks)
				{
					Skidmarks skidmarks = Object.Instantiate(this.skidmarks, Vector3.zero, Quaternion.identity) as Skidmarks;
					gameObject2.transform.GetComponent<CarDynamics>().skidmarks = skidmarks;
				}
			}
		}
	}

	private void Start()
	{
		GameObject[] array = cars;
		foreach (GameObject gameObject in array)
		{
			if (gameObject != null)
			{
				DisableObject(gameObject);
			}
		}
		if (cars.Length != 0 && cars[0] != null)
		{
			lastIndex = -1;
			selectedCar = ChangeCar(0, lastIndex);
		}
	}

	private void DisableObjects(GameObject selectedCar)
	{
		GameObject[] array = cars;
		foreach (GameObject gameObject in array)
		{
			if (gameObject != selectedCar)
			{
				DisableObject(gameObject);
			}
		}
	}

	private void DisableObject(GameObject car)
	{
		if (settingsMenu != null)
		{
			if (!settingsMenu.stressTest)
			{
				car.transform.GetComponent<CarDynamics>().SetController("axis");
			}
		}
		else
		{
			car.transform.GetComponent<CarDynamics>().SetController("axis");
		}
		dashBoard = car.GetComponentInChildren<DashBoard>();
		if (dashBoard != null)
		{
			dashBoard.gameObject.active = false;
		}
		if (car.transform.GetComponent<CarDebug>() != null)
		{
			car.transform.GetComponent<CarDebug>().enabled = false;
		}
		if (settingsMenu != null)
		{
			settingsMenu.ChangeCar(null);
		}
	}

	private void EnableObject(GameObject car)
	{
		car.SetActive(true);
		if (car.transform.GetComponent<Setup>() != null)
		{
			car.transform.GetComponent<Setup>().enabled = true;
		}
		if (settingsMenu != null)
		{
			if (!settingsMenu.stressTest)
			{
				car.transform.GetComponent<CarDynamics>().SetController("axis");
			}
		}
		else
		{
			car.transform.GetComponent<CarDynamics>().SetController("axis");
		}
		dashBoard = car.GetComponentInChildren<DashBoard>();
		if (dashBoard != null)
		{
			dashBoard.gameObject.active = true;
		}
		if (settingsMenu != null)
		{
			StartCoroutine(settingsMenu.ChangeCar(car));
		}
	}

	private void SelectNextCar(bool next)
	{
		lastIndex = index;
		if (next)
		{
			index++;
		}
		else
		{
			index--;
		}
		if (index > cars.Length - 1)
		{
			index = 0;
		}
		if (index < 0)
		{
			index = cars.Length - 1;
		}
		selectedCar = ChangeCar(index, lastIndex);
	}

	private void Update()
	{
		if (cars.Length >= 1)
		{
			if (Input.GetKeyUp(KeyCode.PageUp))
			{
				SelectNextCar(true);
			}
			else if (Input.GetKeyUp(KeyCode.PageDown))
			{
				SelectNextCar(false);
			}
		}
		if (Input.GetKeyUp(KeyCode.M) && mapCameraController != null)
		{
			mapCameraController.gameObject.active = !mapCameraController.gameObject.active;
		}
		if (Input.GetKeyDown(KeyCode.F10) && settingsMenu.stressTest)
		{
			StartCoroutine(CreateCar());
		}
		if (Input.GetKeyDown(KeyCode.F12) && mlight != null)
		{
			if (mlight.shadows == LightShadows.None)
			{
				mlight.shadows = LightShadows.Soft;
			}
			else
			{
				mlight.shadows = LightShadows.None;
			}
		}
		if (dashBoard != null)
		{
			if (carCameras.driverView)
			{
				dashBoard.showGUIDashboard = false;
			}
			else
			{
				dashBoard.showGUIDashboard = true;
			}
		}
		if (Time.fixedDeltaTime != oldFixedDeltaTime)
		{
			checkFixedDeltaTime();
		}
		oldFixedDeltaTime = Time.fixedDeltaTime;
	}

	private void checkFixedDeltaTime()
	{
		GameObject[] array = cars;
		foreach (GameObject gameObject in array)
		{
			if (gameObject != null)
			{
				if (Time.fixedDeltaTime > 0.02f)
				{
					gameObject.transform.GetComponent<CarDynamics>().tirePressureEnabled = false;
					continue;
				}
				gameObject.transform.GetComponent<CarDynamics>().tirePressureEnabled = true;
				gameObject.transform.GetComponent<CarDynamics>().SetWheelsParams();
			}
		}
	}

	private IEnumerator CreateCar()
	{
		unityCar = UnityCar.CreateNewCar();
		unityCar.transform.parent = base.transform;
		int sign = ((!(Random.value < 0.5f)) ? 1 : (-1));
		unityCar.transform.position = Camera.main.transform.TransformPoint(Vector3.forward * (10f + Random.value * 10f) + Vector3.up * 3f + sign * Vector3.right * Random.value * 10f);
		unityCar.transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);
		Resize(ref cars, cars.Length + 1);
		cars[cars.Length - 1] = unityCar;
		settingsMenu.carsNumber = cars.Length;
		if (carCameras.target == null)
		{
			carCameras.target = unityCar.transform;
			while (unityCar.GetComponent<CarDynamics>() == null)
			{
				yield return new WaitForSeconds(0.02f);
			}
			SelectNextCar(true);
		}
	}

	private GameObject ChangeCar(int index, int lastIndex)
	{
		if (lastIndex != -1)
		{
			DisableObject(cars[lastIndex]);
		}
		selectedCar = cars[index];
		EnableObject(selectedCar);
		carCamerasController.externalSizex = (carCamerasController.externalSizey = (carCamerasController.externalSizez = 0f));
		if (selectedCar.transform.tag == "Truck" && selectedCar.transform.GetComponent<CharacterJoint>() != null)
		{
			carCamerasController.externalSizez = 6f;
		}
		carCamerasController.SetCamera(0, selectedCar.transform, true);
		if (mapCameraController != null)
		{
			mapCameraController.SetCamera(7, selectedCar.transform, true);
		}
		return selectedCar;
	}

	public static void Resize<T>(ref T[] array, int newSize)
	{
		T[] array2 = array;
		if (array2 == null)
		{
			array = new T[newSize];
		}
		else if (array2.Length != newSize)
		{
			T[] array3 = new T[newSize];
			array2.CopyTo(array3, 0);
			array = array3;
		}
	}
}
