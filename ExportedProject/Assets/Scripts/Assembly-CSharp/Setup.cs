using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

[ExecuteInEditMode]
public class Setup : MonoBehaviour
{
	private string setupFileText = string.Empty;

	private string[] dataLines;

	private Drivetrain drivetrain;

	private CarDynamics cardynamics;

	private ForceFeedback forceFeedback;

	private Arcader arcader;

	private AerodynamicResistance aerodynamicResistance;

	private Axles axles;

	private AxisCarController axisCarController;

	private MouseCarController mouseCarController;

	private MobileCarController mobileCarController;

	private FuelTank[] fuelTanks;

	private Wing[] wings;

	[HideInInspector]
	public bool loadingSetup;

	[HideInInspector]
	public bool savingSetup;

	public string filePath = string.Empty;

	public bool usePersistentDataPath;

	private string value;

	private string message;

	private StreamWriter writer;

	private StreamReader reader;

	private int index0;

	private int index1;

	private int index2;

	private int index3;

	private int index4;

	private int index5;

	private int i;

	private void Awake()
	{
		SetComponent();
		if (filePath != string.Empty)
		{
			LoadSetup();
		}
	}

	private void SetComponent()
	{
		drivetrain = GetComponent<Drivetrain>();
		cardynamics = GetComponent<CarDynamics>();
		forceFeedback = GetComponent<ForceFeedback>();
		arcader = GetComponent<Arcader>();
		aerodynamicResistance = GetComponent<AerodynamicResistance>();
		axles = GetComponent<Axles>();
		axisCarController = GetComponent<AxisCarController>();
		mouseCarController = GetComponent<MouseCarController>();
		mobileCarController = GetComponent<MobileCarController>();
		fuelTanks = GetComponentsInChildren<FuelTank>();
		wings = GetComponentsInChildren<Wing>();
	}

	private void OnApplicationQuit()
	{
		loadingSetup = false;
	}

	public void SaveValue(string parameter, string value, string section)
	{
		index1 = setupFileText.IndexOf("[" + section + "]");
		if (index1 != -1)
		{
			index2 = setupFileText.IndexOf('[', index1 + 1);
			if (index2 != -1)
			{
				index2 -= 2;
			}
			else
			{
				index2 = setupFileText.Length;
			}
			string pattern = string.Format("\\b{0}\\b", parameter);
			Regex regex = new Regex(pattern);
			string text = setupFileText.Substring(index1, index2 - index1);
			index5 = regex.Match(text).Index;
			if (index5 != 0)
			{
				index2 = setupFileText.IndexOf("=", index5 + index1) + 1;
				index3 = setupFileText.IndexOf('\n', index5 + index1);
				index4 = setupFileText.IndexOf('#', index5 + index1);
				if (index4 != -1)
				{
					index3 = Mathf.Min(index3, index4);
				}
				if (index3 == -1)
				{
					index3 = text.Length + index1;
				}
				setupFileText = setupFileText.Remove(index2, index3 - index2);
				setupFileText = setupFileText.Insert(index2, value);
			}
			else
			{
				setupFileText = setupFileText.Insert(index2, '\n' + parameter + "=" + value);
			}
		}
		else
		{
			string text2 = string.Empty;
			if (setupFileText.Length != 0)
			{
				text2 = "\n\n";
			}
			setupFileText = setupFileText.Insert(setupFileText.Length, text2 + "[" + section + "]");
			index2 = setupFileText.Length;
			setupFileText = setupFileText.Insert(index2, '\n' + parameter + "=" + value);
		}
	}

	public string LoadValue(string parameter, string section)
	{
		string result = string.Empty;
		index0 = setupFileText.IndexOf("[" + section + "]");
		if (index0 != -1)
		{
			string text = setupFileText.Substring(index0);
			string pattern = string.Format("\\b{0}\\b", parameter);
			Regex regex = new Regex(pattern);
			index1 = regex.Match(text).Index;
			if (index1 != 0)
			{
				index2 = text.Substring(index1).IndexOf('\n');
				if (index2 == -1)
				{
					index2 = text.Length - index1;
				}
				index3 = text.Substring(index1, index2).IndexOf('#');
				if (index3 != -1)
				{
					index2 = index3;
				}
				string text2 = text.Substring(index1 - 2, index2 + 2);
				index3 = text2.IndexOf("#");
				if (index3 == -1)
				{
					index3 = text2.IndexOf("=");
					result = text2.Substring(index3 + 1);
				}
			}
		}
		return result;
	}

	private bool LoadFromFile(string filePath, bool checkIfNotExist)
	{
		try
		{
			if (File.Exists(filePath))
			{
				reader = new StreamReader(filePath);
				setupFileText = reader.ReadToEnd();
				reader.Close();
				return true;
			}
			if (checkIfNotExist || filePath == string.Empty)
			{
				message = "(file '" + filePath + "' couldn't be found)";
				return false;
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private bool SaveToFile(string filePath)
	{
		try
		{
			if (!File.Exists(filePath))
			{
				Debug.Log("UnityCar: file '" + filePath + "' not found, creating it");
			}
			writer = new StreamWriter(filePath);
			writer.Write(setupFileText);
			writer.Close();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public bool SaveSetup()
	{
		bool flag = false;
		message = string.Empty;
		SetComponent();
		string text = filePath;
		if (usePersistentDataPath)
		{
			text = Application.persistentDataPath + "\\" + filePath;
		}
		if (filePath != string.Empty)
		{
			if (LoadFromFile(text, false))
			{
				savingSetup = true;
				SaveBodyData();
				SaveEngineData();
				SaveTransmissionData();
				SaveSuspensionsData();
				SaveBrakesData();
				SaveTiresData();
				SaveWheelsData();
				SaveWingsData();
				SaveControllerTypeData();
				SaveControllersData();
				SavePhysicMaterialsData();
				SaveArcaderData();
				SaveFuelTanksData();
				SaveForceFeedBackData();
				flag = SaveToFile(text);
				if (flag)
				{
					Debug.Log("UnityCar: setup saved succesfully in the file '" + text + "'");
				}
			}
		}
		else
		{
			message = "(file path empty)";
		}
		if (message != string.Empty)
		{
			Debug.LogError("UnityCar: error during setup saving " + message);
		}
		savingSetup = false;
		return flag;
	}

	private void SaveBodyData()
	{
		SaveValue("weight", GetComponent<Rigidbody>().mass + string.Empty, "body");
		SaveValue("weightRepartition", cardynamics.frontRearWeightRepartition + string.Empty, "body");
		if (cardynamics.centerOfMass != null)
		{
			SaveValue("centerOfMass", string.Concat(cardynamics.centerOfMass.localPosition, string.Empty), "body");
		}
		else
		{
			SaveValue("centerOfMass", string.Concat(GetComponent<Rigidbody>().centerOfMass, string.Empty), "body");
		}
		SaveValue("inertiaFactor", cardynamics.inertiaFactor + string.Empty, "body");
		if (aerodynamicResistance != null)
		{
			SaveValue("dragCoefficent", aerodynamicResistance.Cx + string.Empty, "body");
		}
		if (aerodynamicResistance != null)
		{
			SaveValue("dragArea", aerodynamicResistance.Area + string.Empty, "body");
		}
	}

	private void SaveEngineData()
	{
		if (drivetrain != null)
		{
			SaveValue("maxPower", drivetrain.maxPower + string.Empty, "engine");
			SaveValue("maxPowerRPM", drivetrain.maxPowerRPM + string.Empty, "engine");
			SaveValue("maxTorque", drivetrain.maxTorque + string.Empty, "engine");
			SaveValue("maxTorqueRPM", drivetrain.maxTorqueRPM + string.Empty, "engine");
			SaveValue("minRPM", drivetrain.minRPM + string.Empty, "engine");
			SaveValue("maxRPM", drivetrain.maxRPM + string.Empty, "engine");
			SaveValue("revLimiter", drivetrain.revLimiter + string.Empty, "engine");
			SaveValue("revLimiterTime", drivetrain.revLimiterTime + string.Empty, "engine");
			SaveValue("engineInertia", drivetrain.engineInertia + string.Empty, "engine");
			SaveValue("engineFrictionFactor", drivetrain.engineFrictionFactor + string.Empty, "engine");
			SaveValue("engineOrientation", string.Concat(drivetrain.engineOrientation, string.Empty), "engine");
			SaveValue("canStall", drivetrain.canStall + string.Empty, "engine");
			SaveValue("fuelConsumptionAtCostantSpeed", drivetrain.fuelConsumptionAtCostantSpeed + string.Empty, "engine");
			SaveValue("fuelConsumptionSpeed", drivetrain.fuelConsumptionSpeed + string.Empty, "engine");
		}
	}

	private void SaveTransmissionData()
	{
		if (drivetrain != null)
		{
			SaveValue("transmissionType", string.Concat(drivetrain.transmission, string.Empty), "transmission");
			SaveValue("finalDriveRatio", drivetrain.finalDriveRatio + string.Empty, "transmission");
			SaveValue("drivetrainInertia", drivetrain.drivetrainInertia + string.Empty, "transmission");
			SaveValue("differentialLockCoefficient", drivetrain.differentialLockCoefficient + string.Empty, "transmission");
			SaveValue("shifter", drivetrain.shifter + string.Empty, "transmission");
			SaveValue("automatic", drivetrain.automatic + string.Empty, "transmission");
			SaveValue("autoReverse", drivetrain.autoReverse + string.Empty, "transmission");
			SaveValue("shiftDownRPM", drivetrain.shiftDownRPM + string.Empty, "transmission");
			SaveValue("shiftUpRPM", drivetrain.shiftUpRPM + string.Empty, "transmission");
			SaveValue("shiftTime", drivetrain.shiftTime + string.Empty, "transmission");
			SaveValue("clutchMaxTorque", drivetrain.clutchMaxTorque + string.Empty, "transmission");
			SaveValue("autoClutch", drivetrain.autoClutch + string.Empty, "transmission");
			SaveValue("engageRPM", drivetrain.engageRPM + string.Empty, "transmission");
			SaveValue("disengageRPM", drivetrain.disengageRPM + string.Empty, "transmission");
			SaveValue("gears", drivetrain.gearRatios.Length - 2 + string.Empty, "transmission");
			if (drivetrain.gearRatios.Length > 0)
			{
				SaveValue("gear-ratio-r", drivetrain.gearRatios[0] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 2)
			{
				SaveValue("gear-ratio-1", drivetrain.gearRatios[2] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 3)
			{
				SaveValue("gear-ratio-2", drivetrain.gearRatios[3] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 4)
			{
				SaveValue("gear-ratio-3", drivetrain.gearRatios[4] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 5)
			{
				SaveValue("gear-ratio-4", drivetrain.gearRatios[5] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 6)
			{
				SaveValue("gear-ratio-5", drivetrain.gearRatios[6] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 7)
			{
				SaveValue("gear-ratio-6", drivetrain.gearRatios[7] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 8)
			{
				SaveValue("gear-ratio-7", drivetrain.gearRatios[8] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 9)
			{
				SaveValue("gear-ratio-8", drivetrain.gearRatios[9] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 10)
			{
				SaveValue("gear-ratio-9", drivetrain.gearRatios[10] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 11)
			{
				SaveValue("gear-ratio-10", drivetrain.gearRatios[11] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 12)
			{
				SaveValue("gear-ratio-11", drivetrain.gearRatios[12] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 13)
			{
				SaveValue("gear-ratio-12", drivetrain.gearRatios[13] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 14)
			{
				SaveValue("gear-ratio-13", drivetrain.gearRatios[14] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 15)
			{
				SaveValue("gear-ratio-14", drivetrain.gearRatios[15] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 16)
			{
				SaveValue("gear-ratio-15", drivetrain.gearRatios[16] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 17)
			{
				SaveValue("gear-ratio-16", drivetrain.gearRatios[17] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 18)
			{
				SaveValue("gear-ratio-17", drivetrain.gearRatios[18] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 19)
			{
				SaveValue("gear-ratio-18", drivetrain.gearRatios[19] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 20)
			{
				SaveValue("gear-ratio-19", drivetrain.gearRatios[20] + string.Empty, "transmission");
			}
			if (drivetrain.gearRatios.Length > 21)
			{
				SaveValue("gear-ratio-20", drivetrain.gearRatios[21] + string.Empty, "transmission");
			}
		}
	}

	private void SaveSuspensionsData()
	{
		SaveValue("suspensionTravel", axles.frontAxle.suspensionTravel + string.Empty, "suspensions-frontAxle");
		SaveValue("suspensionRate", axles.frontAxle.suspensionRate + string.Empty, "suspensions-frontAxle");
		SaveValue("bumpRate", axles.frontAxle.bumpRate + string.Empty, "suspensions-frontAxle");
		SaveValue("reboundRate", axles.frontAxle.reboundRate + string.Empty, "suspensions-frontAxle");
		SaveValue("fastBumpFactor", axles.frontAxle.fastBumpFactor + string.Empty, "suspensions-frontAxle");
		SaveValue("fastReboundFactor", axles.frontAxle.fastReboundFactor + string.Empty, "suspensions-frontAxle");
		SaveValue("camber", axles.frontAxle.camber + string.Empty, "suspensions-frontAxle");
		SaveValue("caster", axles.frontAxle.caster + string.Empty, "suspensions-frontAxle");
		SaveValue("antiRollBarRate", axles.frontAxle.antiRollBarRate + string.Empty, "suspensions-frontAxle");
		SaveValue("maxSteeringAngle", axles.frontAxle.maxSteeringAngle + string.Empty, "suspensions-frontAxle");
		SaveValue("suspensionTravel", axles.rearAxle.suspensionTravel + string.Empty, "suspensions-rearAxle");
		SaveValue("suspensionRate", axles.rearAxle.suspensionRate + string.Empty, "suspensions-rearAxle");
		SaveValue("bumpRate", axles.rearAxle.bumpRate + string.Empty, "suspensions-rearAxle");
		SaveValue("reboundRate", axles.rearAxle.reboundRate + string.Empty, "suspensions-rearAxle");
		SaveValue("fastBumpFactor", axles.rearAxle.fastBumpFactor + string.Empty, "suspensions-rearAxle");
		SaveValue("fastReboundFactor", axles.rearAxle.fastReboundFactor + string.Empty, "suspensions-rearAxle");
		SaveValue("camber", axles.rearAxle.camber + string.Empty, "suspensions-rearAxle");
		SaveValue("caster", axles.rearAxle.caster + string.Empty, "suspensions-rearAxle");
		SaveValue("antiRollBarRate", axles.rearAxle.antiRollBarRate + string.Empty, "suspensions-rearAxle");
		SaveValue("maxSteeringAngle", axles.rearAxle.maxSteeringAngle + string.Empty, "suspensions-rearAxle");
		this.i = 0;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			this.i++;
			SaveValue("suspensionTravel", axle.suspensionTravel + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("suspensionRate", axle.suspensionRate + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("bumpRate", axle.bumpRate + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("reboundRate", axle.reboundRate + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("fastBumpFactor", axle.fastBumpFactor + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("fastReboundFactor", axle.fastReboundFactor + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("camber", axle.camber + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("caster", axle.caster + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("antiRollBarRate", axle.antiRollBarRate + string.Empty, "suspensions-otherAxle" + this.i);
			SaveValue("maxSteeringAngle", axle.maxSteeringAngle + string.Empty, "suspensions-otherAxle" + this.i);
		}
	}

	private void SaveBrakesData()
	{
		SaveValue("brakeFrictionTorque", axles.frontAxle.brakeFrictionTorque + string.Empty, "brakes-frontAxle");
		SaveValue("handbrakeFrictionTorque", axles.frontAxle.handbrakeFrictionTorque + string.Empty, "brakes-frontAxle");
		SaveValue("BrakeFrictionTorque", axles.rearAxle.brakeFrictionTorque + string.Empty, "brakes-rearAxle");
		SaveValue("HandbrakeFrictionTorque", axles.rearAxle.handbrakeFrictionTorque + string.Empty, "brakes-rearAxle");
		this.i = 0;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			this.i++;
			SaveValue("brakeFrictionTorque", axle.brakeFrictionTorque + string.Empty, "brakes-otherAxle" + this.i);
			SaveValue("handbrakeFrictionTorque", axle.handbrakeFrictionTorque + string.Empty, "brakes-otherAxle" + this.i);
		}
		SaveValue("frontRearBrakeBalance", cardynamics.frontRearBrakeBalance + string.Empty, "brakes");
		SaveValue("frontRearHandBrakeBalance", cardynamics.frontRearHandBrakeBalance + string.Empty, "brakes");
	}

	private void SaveTiresData()
	{
		SaveValue("tireType", string.Concat(axles.frontAxle.tires, string.Empty), "tires-frontAxle");
		SaveValue("forwardGripFactor", axles.frontAxle.forwardGripFactor + string.Empty, "tires-frontAxle");
		SaveValue("sidewaysGripFactor", axles.frontAxle.sidewaysGripFactor + string.Empty, "tires-frontAxle");
		SaveValue("tiresPressure", axles.frontAxle.tiresPressure + string.Empty, "tires-frontAxle");
		SaveValue("optimalTiresPressure", axles.frontAxle.optimalTiresPressure + string.Empty, "tires-frontAxle");
		SaveValue("tireType", string.Concat(axles.rearAxle.tires, string.Empty), "tires-rearAxle");
		SaveValue("forwardGripFactor", axles.rearAxle.forwardGripFactor + string.Empty, "tires-rearAxle");
		SaveValue("sidewaysGripFactor", axles.rearAxle.sidewaysGripFactor + string.Empty, "tires-rearAxle");
		SaveValue("tiresPressure", axles.rearAxle.tiresPressure + string.Empty, "tires-rearAxle");
		SaveValue("optimalTiresPressure", axles.rearAxle.optimalTiresPressure + string.Empty, "tires-rearAxle");
		this.i = 0;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			this.i++;
			SaveValue("tireType", string.Concat(axle.tires, string.Empty), "tires-otherAxle" + this.i);
			SaveValue("forwardGripFactor", axle.forwardGripFactor + string.Empty, "tires-otherAxle" + this.i);
			SaveValue("sidewaysGripFactor", axle.sidewaysGripFactor + string.Empty, "tires-otherAxle" + this.i);
			SaveValue("tiresPressure", axle.tiresPressure + string.Empty, "tires-otherAxle" + this.i);
			SaveValue("optimalTiresPressure", axle.optimalTiresPressure + string.Empty, "tires-otherAxle" + this.i);
		}
	}

	private void SaveWheelsData()
	{
		if (axles.frontAxle.leftWheel != null)
		{
			SaveWheelData(axles.frontAxle.leftWheel, "frontAxle-left");
		}
		if (axles.frontAxle.rightWheel != null)
		{
			SaveWheelData(axles.frontAxle.rightWheel, "frontAxle-right");
		}
		if (axles.rearAxle.leftWheel != null)
		{
			SaveWheelData(axles.rearAxle.leftWheel, "rearAxle-left");
		}
		if (axles.rearAxle.rightWheel != null)
		{
			SaveWheelData(axles.rearAxle.rightWheel, "rearAxle-right");
		}
		this.i = 0;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			this.i++;
			if (axle.leftWheel != null)
			{
				SaveWheelData(axle.leftWheel, "otherAxle" + this.i + "-left");
			}
			if (axle.rightWheel != null)
			{
				SaveWheelData(axle.rightWheel, "otherAxle" + this.i + "-right");
			}
		}
	}

	private void SaveWheelData(Wheel w, string wheelPosition)
	{
		SaveValue("mass", w.mass + string.Empty, "wheels-" + wheelPosition);
		SaveValue("radius", w.radius + string.Empty, "wheels-" + wheelPosition);
		SaveValue("rimRadius", w.rimRadius + string.Empty, "wheels-" + wheelPosition);
		SaveValue("width", w.width + string.Empty, "wheels-" + wheelPosition);
	}

	private void SaveWingsData()
	{
		this.i = 0;
		Wing[] array = wings;
		foreach (Wing wing in array)
		{
			this.i++;
			SaveValue("dragCoefficient", wing.dragCoefficient + string.Empty, "wing" + this.i);
			SaveValue("angleOfAttack", wing.angleOfAttack + string.Empty, "wing" + this.i);
			SaveValue("area", wing.area + string.Empty, "wing" + this.i);
		}
	}

	private void SaveControllerTypeData()
	{
		SaveValue("controller", string.Concat(cardynamics.controller, string.Empty), "controllerType");
	}

	private void SaveControllersData()
	{
		if (axisCarController != null)
		{
			SaveControllerData(axisCarController, CarDynamics.Controller.axis);
		}
		if (mouseCarController != null)
		{
			SaveControllerData(mouseCarController, CarDynamics.Controller.mouse);
		}
		if (mobileCarController != null)
		{
			SaveControllerData(mobileCarController, CarDynamics.Controller.mobile);
		}
	}

	private void SaveControllerData(CarController carController, CarDynamics.Controller controller)
	{
		SaveValue("smoothInput", carController.smoothInput + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("throttleTime", carController.throttleTime + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("throttleReleaseTime", carController.throttleReleaseTime + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("maxThrottleInReverse", carController.maxThrottleInReverse + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("brakesTime", carController.brakesTime + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("brakesReleaseTime", carController.brakesReleaseTime + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("steerTime", carController.steerTime + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("steerReleaseTime", carController.steerReleaseTime + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("veloSteerTime", carController.veloSteerTime + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("veloSteerReleaseTime", carController.veloSteerReleaseTime + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("steerCorrectionFactor", carController.steerCorrectionFactor + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("steerAssistance", carController.steerAssistance + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("SteerAssistanceMinVelocity", carController.SteerAssistanceMinVelocity + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("TCS", carController.TCS + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("TCSAllowedSlip", carController.TCSAllowedSlip + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("TCSMinVelocity", carController.TCSMinVelocity + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("ABS", carController.ABS + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("ABSAllowedSlip", carController.ABSAllowedSlip + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("ABSMinVelocity", carController.ABSMinVelocity + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("ESP", carController.ESP + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("ESPStrength", carController.ESPStrength + string.Empty, string.Concat(controller, "Controller"));
		SaveValue("ESPMinVelocity", carController.ESPMinVelocity + string.Empty, string.Concat(controller, "Controller"));
		if (carController is AxisCarController)
		{
			SaveValue("throttleAxis", ((AxisCarController)carController).throttleAxis + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("brakeAxis", ((AxisCarController)carController).brakeAxis + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("steerAxis", ((AxisCarController)carController).steerAxis + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("handbrakeAxis", ((AxisCarController)carController).handbrakeAxis + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("clutchAxis", ((AxisCarController)carController).clutchAxis + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("shiftUpButton", ((AxisCarController)carController).shiftUpButton + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("shiftDownButton", ((AxisCarController)carController).shiftDownButton + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("startEngineButton", ((AxisCarController)carController).startEngineButton + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("normalizeThrottleInput", ((AxisCarController)carController).normalizeThrottleInput + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("exponentialThrottleInput", ((AxisCarController)carController).exponentialThrottleInput + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("normalizeBrakesInput", ((AxisCarController)carController).normalizeBrakesInput + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("exponentialBrakesInput", ((AxisCarController)carController).exponentialBrakesInput + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("normalizeClutchInput", ((AxisCarController)carController).normalizeClutchInput + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("exponentialClutchInput", ((AxisCarController)carController).exponentialClutchInput + string.Empty, string.Concat(controller, "Controller"));
		}
		else if (carController is MouseCarController)
		{
			SaveValue("clutchAxis", ((MouseCarController)carController).clutchAxis + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("shiftUpButton", ((MouseCarController)carController).shiftUpButton + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("shiftDownButton", ((MouseCarController)carController).shiftDownButton + string.Empty, string.Concat(controller, "Controller"));
			SaveValue("startEngineButton", ((MouseCarController)carController).startEngineButton + string.Empty, string.Concat(controller, "Controller"));
		}
	}

	private void SavePhysicMaterialsData()
	{
		if (cardynamics.physicMaterials.Length > 0)
		{
			MyPhysicMaterial[] physicMaterials = cardynamics.physicMaterials;
			foreach (MyPhysicMaterial myPhysicMaterial in physicMaterials)
			{
				SaveValue("grip", myPhysicMaterial.grip + string.Empty, "physicMaterials-" + myPhysicMaterial.surfaceType);
				SaveValue("rollingFriction", myPhysicMaterial.rollingFriction + string.Empty, "physicMaterials-" + myPhysicMaterial.surfaceType);
				SaveValue("staticFriction", myPhysicMaterial.staticFriction + string.Empty, "physicMaterials-" + myPhysicMaterial.surfaceType);
				SaveValue("isSkidSmoke", myPhysicMaterial.isSkidSmoke + string.Empty, "physicMaterials-" + myPhysicMaterial.surfaceType);
				SaveValue("isSkidMark", myPhysicMaterial.isSkidMark + string.Empty, "physicMaterials-" + myPhysicMaterial.surfaceType);
				SaveValue("isDirty", myPhysicMaterial.isDirty + string.Empty, "physicMaterials-" + myPhysicMaterial.surfaceType);
			}
		}
	}

	private void SaveArcaderData()
	{
		if (arcader != null)
		{
			SaveValue("enabled", arcader.enabled + string.Empty, "arcader");
			SaveValue("minVelocity", arcader.minVelocity + string.Empty, "arcader");
			SaveValue("overallStrength", arcader.overallStrength + string.Empty, "arcader");
			SaveValue("COGHelperStrength", arcader.COGHelperStrength + string.Empty, "arcader");
			SaveValue("torqueHelperStrength", arcader.torqueHelperStrength + string.Empty, "arcader");
			SaveValue("gripHelperStrength", arcader.gripHelperStrength + string.Empty, "arcader");
		}
	}

	private void SaveFuelTanksData()
	{
		this.i = 0;
		FuelTank[] array = fuelTanks;
		foreach (FuelTank fuelTank in array)
		{
			this.i++;
			SaveValue("tankCapacity", fuelTank.tankCapacity + string.Empty, "fuelTank-" + this.i);
			SaveValue("currentFuel", fuelTank.currentFuel + string.Empty, "fuelTank-" + this.i);
			SaveValue("tankWeight", fuelTank.tankWeight + string.Empty, "fuelTank-" + this.i);
			SaveValue("fuelDensity", fuelTank.fuelDensity + string.Empty, "fuelTank-" + this.i);
		}
	}

	private void SaveForceFeedBackData()
	{
		if (forceFeedback != null)
		{
			SaveValue("enabled", cardynamics.enableForceFeedback + string.Empty, "forcefeedback");
			SaveValue("factor", forceFeedback.factor + string.Empty, "forcefeedback");
			SaveValue("multiplier", forceFeedback.multiplier + string.Empty, "forcefeedback");
			SaveValue("smoothingFactor", forceFeedback.smoothingFactor + string.Empty, "forcefeedback");
			SaveValue("clampValue", forceFeedback.clampValue + string.Empty, "forcefeedback");
			SaveValue("invertForceFeedback", forceFeedback.invertForceFeedback + string.Empty, "forcefeedback");
		}
	}

	public void LoadSetup()
	{
		loadingSetup = true;
		message = string.Empty;
		SetComponent();
		string text = filePath;
		if (usePersistentDataPath)
		{
			text = Application.persistentDataPath + "\\" + filePath;
		}
		if (filePath != string.Empty)
		{
			if (LoadFromFile(text, true))
			{
				if (setupFileText.Contains("[body]"))
				{
					LoadBodyData();
				}
				if (setupFileText.Contains("[engine]"))
				{
					LoadEngineData();
				}
				if (setupFileText.Contains("[transmission]"))
				{
					LoadTransmissionData();
				}
				LoadSuspensionsData();
				LoadBrakesData();
				LoadTiresData();
				LoadWheelsData();
				LoadWingsData();
				if (setupFileText.Contains("[controllerType]"))
				{
					LoadControllerTypeData();
				}
				LoadControllersData();
				LoadPhysicMaterialsData();
				if (setupFileText.Contains("[arcader]"))
				{
					LoadArcaderData();
				}
				LoadFuelTanksData();
				if (setupFileText.Contains("[forcefeedback]"))
				{
					LoadForceFeedBackData();
				}
				Debug.Log("UnityCar: setup loaded succesfully from the file '" + text + "'");
			}
		}
		else
		{
			message = "(file path empty)";
		}
		if (message != string.Empty)
		{
			Debug.LogError("UnityCar: error during setup loading " + message);
		}
		loadingSetup = false;
	}

	private void LoadBodyData()
	{
		value = LoadValue("weight", "body");
		if (value != string.Empty)
		{
			GetComponent<Rigidbody>().mass = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("weightRepartition", "body");
		if (value != string.Empty)
		{
			cardynamics.frontRearWeightRepartition = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("centerOfMass", "body");
		if (value != string.Empty)
		{
			value = value.Replace("(", string.Empty).Replace(")", string.Empty);
			float x = float.Parse(value.Split(',')[0], CultureInfo.InvariantCulture.NumberFormat);
			float y = float.Parse(value.Split(',')[1], CultureInfo.InvariantCulture.NumberFormat);
			float z = float.Parse(value.Split(',')[2], CultureInfo.InvariantCulture.NumberFormat);
			Vector3 vector = new Vector3(x, y, z);
			cardynamics.SetCenterOfMass(vector);
		}
		value = LoadValue("inertiaFactor", "body");
		if (value != string.Empty)
		{
			cardynamics.inertiaFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		if (aerodynamicResistance != null)
		{
			value = LoadValue("dragCoefficent", "body");
			if (value != string.Empty)
			{
				aerodynamicResistance.Cx = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (aerodynamicResistance != null)
		{
			value = LoadValue("dragArea", "body");
			if (value != string.Empty)
			{
				aerodynamicResistance.Area = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
	}

	private void LoadEngineData()
	{
		dataLines = setupFileText.Split('\n');
		string text = string.Empty;
		drivetrain.engineTorqueFromFile = false;
		drivetrain.torqueRPMValuesLen = 0;
		value = LoadValue("minRPM", "engine");
		if (value != string.Empty)
		{
			drivetrain.minRPM = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("maxRPM", "engine");
		if (value != string.Empty)
		{
			drivetrain.maxRPM = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		string[] array = dataLines;
		foreach (string text2 in array)
		{
			if (text2.Contains("torque-curve"))
			{
				text = text + text2.Split('=')[1] + "\n";
			}
		}
		if (text != string.Empty)
		{
			text = text.Substring(0, text.Length - 1);
			dataLines = text.Split('\n');
			drivetrain.torqueRPMValues = new float[dataLines.Length, 2];
			int num = 0;
			string[] array2 = dataLines;
			foreach (string text3 in array2)
			{
				drivetrain.torqueRPMValues[num, 0] = float.Parse(text3.Split(',')[0], CultureInfo.InvariantCulture.NumberFormat);
				drivetrain.torqueRPMValues[num, 1] = float.Parse(text3.Split(',')[1], CultureInfo.InvariantCulture.NumberFormat);
				num++;
			}
			drivetrain.torqueRPMValuesLen = drivetrain.torqueRPMValues.GetLength(0);
			if (drivetrain.maxRPM == 0f)
			{
				this.i = drivetrain.torqueRPMValuesLen - 1;
				while (this.i >= 0)
				{
					if (drivetrain.torqueRPMValues[this.i, 1] > 0f)
					{
						drivetrain.maxRPM = drivetrain.torqueRPMValues[this.i, 0];
						break;
					}
					this.i--;
				}
			}
			drivetrain.engineTorqueFromFile = drivetrain.torqueRPMValuesLen > 0;
		}
		LoadEngineValues(drivetrain.engineTorqueFromFile);
		value = LoadValue("canStall", "engine");
		if (value != string.Empty)
		{
			drivetrain.canStall = bool.Parse(value);
		}
		value = LoadValue("revLimiter", "engine");
		if (value != string.Empty)
		{
			drivetrain.revLimiter = bool.Parse(value);
		}
		value = LoadValue("revLimiterTime", "engine");
		if (value != string.Empty)
		{
			drivetrain.revLimiterTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("engineInertia", "engine");
		if (value != string.Empty)
		{
			drivetrain.engineInertia = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("engineFrictionFactor", "engine");
		if (value != string.Empty)
		{
			drivetrain.engineFrictionFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("engineOrientation", "engine");
		if (value != string.Empty)
		{
			value = value.Replace("(", string.Empty).Replace(")", string.Empty);
			float x = float.Parse(value.Split(',')[0], CultureInfo.InvariantCulture.NumberFormat);
			float y = float.Parse(value.Split(',')[1], CultureInfo.InvariantCulture.NumberFormat);
			float z = float.Parse(value.Split(',')[2], CultureInfo.InvariantCulture.NumberFormat);
			drivetrain.engineOrientation = new Vector3(x, y, z);
		}
		value = LoadValue("canStall", "engine");
		if (value != string.Empty)
		{
			drivetrain.canStall = bool.Parse(value);
		}
		value = LoadValue("fuelConsumptionAtCostantSpeed", "engine");
		if (value != string.Empty)
		{
			drivetrain.fuelConsumptionAtCostantSpeed = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("fuelConsumptionSpeed", "engine");
		if (value != string.Empty)
		{
			drivetrain.fuelConsumptionSpeed = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
	}

	public void LoadEngineValues(bool engineTorqueFromFile)
	{
		if (!engineTorqueFromFile)
		{
			value = LoadValue("maxPower", "engine");
			if (value != string.Empty)
			{
				drivetrain.maxPower = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("maxPowerRPM", "engine");
			if (value != string.Empty)
			{
				drivetrain.maxPowerRPM = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("maxTorque", "engine");
			if (value != string.Empty)
			{
				drivetrain.maxTorque = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("maxTorqueRPM", "engine");
			if (value != string.Empty)
			{
				drivetrain.maxTorqueRPM = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
	}

	private void LoadTransmissionData()
	{
		value = LoadValue("transmissionType", "transmission");
		if (value == "RWD")
		{
			drivetrain.transmission = Drivetrain.Transmissions.RWD;
		}
		else if (value == "FWD")
		{
			drivetrain.transmission = Drivetrain.Transmissions.FWD;
		}
		else if (value == "AWD")
		{
			drivetrain.transmission = Drivetrain.Transmissions.AWD;
		}
		value = LoadValue("gears", "transmission");
		if (value != string.Empty)
		{
			drivetrain.gearRatios = new float[int.Parse(value) + 2];
		}
		if (drivetrain.gearRatios.Length > 0)
		{
			value = LoadValue("gear-ratio-r", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[0] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 2)
		{
			value = LoadValue("gear-ratio-1", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[2] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 3)
		{
			value = LoadValue("gear-ratio-2", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[3] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 4)
		{
			value = LoadValue("gear-ratio-3", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[4] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 5)
		{
			value = LoadValue("gear-ratio-4", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[5] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 6)
		{
			value = LoadValue("gear-ratio-5", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[6] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 7)
		{
			value = LoadValue("gear-ratio-6", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[7] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 8)
		{
			value = LoadValue("gear-ratio-7", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[8] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 9)
		{
			value = LoadValue("gear-ratio-8", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[9] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 10)
		{
			value = LoadValue("gear-ratio-9", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[10] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 11)
		{
			value = LoadValue("gear-ratio-10", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[11] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 12)
		{
			value = LoadValue("gear-ratio-11", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[12] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 13)
		{
			value = LoadValue("gear-ratio-12", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[13] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 14)
		{
			value = LoadValue("gear-ratio-13", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[14] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 15)
		{
			value = LoadValue("gear-ratio-14", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[15] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 16)
		{
			value = LoadValue("gear-ratio-15", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[16] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 17)
		{
			value = LoadValue("gear-ratio-16", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[17] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 18)
		{
			value = LoadValue("gear-ratio-17", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[18] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 19)
		{
			value = LoadValue("gear-ratio-18", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[19] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 20)
		{
			value = LoadValue("gear-ratio-19", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[20] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		if (drivetrain.gearRatios.Length > 21)
		{
			value = LoadValue("gear-ratio-20", "transmission");
			if (value != string.Empty)
			{
				drivetrain.gearRatios[21] = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		value = LoadValue("finalDriveRatio", "transmission");
		if (value != string.Empty)
		{
			drivetrain.finalDriveRatio = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("drivetrainInertia", "transmission");
		if (value != string.Empty)
		{
			drivetrain.drivetrainInertia = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("differentialLockCoefficient", "transmission");
		if (value != string.Empty)
		{
			drivetrain.differentialLockCoefficient = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("shifter", "transmission");
		if (value != string.Empty)
		{
			drivetrain.shifter = bool.Parse(value);
		}
		value = LoadValue("automatic", "transmission");
		if (value != string.Empty)
		{
			drivetrain.automatic = bool.Parse(value);
		}
		value = LoadValue("autoReverse", "transmission");
		if (value != string.Empty)
		{
			drivetrain.autoReverse = bool.Parse(value);
		}
		value = LoadValue("shiftDownRPM", "transmission");
		if (value != string.Empty)
		{
			drivetrain.shiftDownRPM = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("shiftUpRPM", "transmission");
		if (value != string.Empty && value != "0")
		{
			drivetrain.shiftUpRPM = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("shiftTime", "transmission");
		if (value != string.Empty)
		{
			drivetrain.shiftTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("clutchMaxTorque", "transmission");
		if (value != string.Empty)
		{
			drivetrain.clutchMaxTorque = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("autoClutch", "transmission");
		if (value != string.Empty)
		{
			drivetrain.autoClutch = bool.Parse(value);
		}
		value = LoadValue("engageRPM", "transmission");
		if (value != string.Empty)
		{
			drivetrain.engageRPM = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("disengageRPM", "transmission");
		if (value != string.Empty)
		{
			drivetrain.disengageRPM = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
	}

	private void LoadSuspensionsData()
	{
		value = LoadValue("suspensionTravel", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.suspensionTravel = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("suspensionRate", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.suspensionRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("bumpRate", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.bumpRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("reboundRate", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.reboundRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("fastBumpFactor", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.fastBumpFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("fastReboundFactor", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.fastReboundFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("camber", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.camber = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("caster", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.caster = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("antiRollBarRate", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.antiRollBarRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("maxSteeringAngle", "suspensions-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.maxSteeringAngle = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("suspensionTravel", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.suspensionTravel = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("suspensionRate", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.suspensionRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("bumpRate", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.bumpRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("reboundRate", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.reboundRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("fastBumpFactor", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.fastBumpFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("fastReboundFactor", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.fastReboundFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("camber", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.camber = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("caster", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.caster = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("antiRollBarRate", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.antiRollBarRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("maxSteeringAngle", "suspensions-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.maxSteeringAngle = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		this.i = 0;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			this.i++;
			value = LoadValue("suspensionTravel", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.suspensionTravel = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("suspensionRate", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.suspensionRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("bumpRate", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.bumpRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("reboundRate", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.reboundRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("fastBumpFactor", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.fastBumpFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("fastReboundFactor", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.fastReboundFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("camber", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.camber = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("caster", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.caster = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("antiRollBarRate", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.antiRollBarRate = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("maxSteeringAngle", "suspensions-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.maxSteeringAngle = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
	}

	private void LoadBrakesData()
	{
		value = LoadValue("brakeFrictionTorque", "brakes-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.brakeFrictionTorque = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("handbrakeFrictionTorque", "brakes-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.handbrakeFrictionTorque = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("brakeFrictionTorque", "brakes-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.brakeFrictionTorque = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("handbrakeFrictionTorque", "brakes-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.handbrakeFrictionTorque = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		this.i = 0;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			this.i++;
			value = LoadValue("brakeFrictionTorque", "brakes-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.brakeFrictionTorque = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("handbrakeFrictionTorque", "brakesotherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.handbrakeFrictionTorque = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		value = LoadValue("frontRearBrakeBalance", "brakes");
		if (value != string.Empty)
		{
			cardynamics.frontRearBrakeBalance = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("frontRearHandBrakeBalance", "brakes");
		if (value != string.Empty)
		{
			cardynamics.frontRearHandBrakeBalance = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
	}

	private void LoadTiresData()
	{
		value = LoadValue("tireType", "tires-frontAxle").ToLower();
		if (value != string.Empty)
		{
			LoadTireType(value, axles.frontAxle);
		}
		value = LoadValue("forwardGripFactor", "tires-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.forwardGripFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("sidewaysGripFactor", "tires-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.sidewaysGripFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("tiresPressure", "tires-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.tiresPressure = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("optimalTiresPressure", "tires-frontAxle");
		if (value != string.Empty)
		{
			axles.frontAxle.optimalTiresPressure = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("tireType", "tires-rearAxle").ToLower();
		if (value != string.Empty)
		{
			LoadTireType(value, axles.rearAxle);
		}
		value = LoadValue("forwardGripFactor", "tires-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.forwardGripFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("sidewaysGripFactor", "tires-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.sidewaysGripFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("tiresPressure", "tires-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.tiresPressure = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("optimalTiresPressure", "tires-rearAxle");
		if (value != string.Empty)
		{
			axles.rearAxle.optimalTiresPressure = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		this.i = 0;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			this.i++;
			value = LoadValue("tireType", "tires-otherAxle" + this.i).ToLower();
			if (value != string.Empty)
			{
				LoadTireType(value, axle);
			}
			value = LoadValue("forwardGripFactor", "tires-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.forwardGripFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("sidewaysGripFactor", "tires-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.sidewaysGripFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("tiresPressure", "tires-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.tiresPressure = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("optimalTiresPressure", "tires-otherAxle" + this.i);
			if (value != string.Empty)
			{
				axle.optimalTiresPressure = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
	}

	private void LoadTireType(string value, Axle axle)
	{
		switch (value)
		{
		case "competition_front":
			axle.tires = CarDynamics.Tires.competition_front;
			return;
		case "competition_rear":
			axle.tires = CarDynamics.Tires.competition_rear;
			return;
		case "supersport_front":
			axle.tires = CarDynamics.Tires.supersport_front;
			return;
		case "supersport_rear":
			axle.tires = CarDynamics.Tires.supersport_rear;
			return;
		case "sport_front":
			axle.tires = CarDynamics.Tires.sport_front;
			return;
		case "sport_rear":
			axle.tires = CarDynamics.Tires.sport_rear;
			return;
		case "touring_front":
			axle.tires = CarDynamics.Tires.touring_front;
			return;
		case "touring_rear":
			axle.tires = CarDynamics.Tires.touring_rear;
			return;
		case "offroad_front":
			axle.tires = CarDynamics.Tires.offroad_front;
			return;
		case "offroad_rear":
			axle.tires = CarDynamics.Tires.offroad_rear;
			return;
		case "truck_front":
			axle.tires = CarDynamics.Tires.truck_front;
			return;
		case "truck_rear":
			axle.tires = CarDynamics.Tires.truck_rear;
			return;
		}
		Debug.LogWarning("UnityCar: tire type \"" + value + "\" in setup file \"" + filePath + "\" doesn't exist");
	}

	private void LoadWheelsData()
	{
		if (axles.frontAxle.leftWheel != null)
		{
			LoadWheelData(axles.frontAxle.leftWheel, "frontAxle-left");
		}
		if (axles.frontAxle.rightWheel != null)
		{
			LoadWheelData(axles.frontAxle.rightWheel, "frontAxle-right");
		}
		if (axles.rearAxle.leftWheel != null)
		{
			LoadWheelData(axles.rearAxle.leftWheel, "rearAxle-left");
		}
		if (axles.rearAxle.rightWheel != null)
		{
			LoadWheelData(axles.rearAxle.rightWheel, "rearAxle-right");
		}
		this.i = 0;
		Axle[] otherAxles = axles.otherAxles;
		foreach (Axle axle in otherAxles)
		{
			this.i++;
			if (axle.leftWheel != null)
			{
				LoadWheelData(axle.leftWheel, "otherAxle" + this.i + "-left");
			}
			if (axle.rightWheel != null)
			{
				LoadWheelData(axle.rightWheel, "otherAxle" + this.i + "-right");
			}
		}
	}

	private void LoadWheelData(Wheel w, string wheelPosition)
	{
		value = LoadValue("mass", "wheels-" + wheelPosition);
		if (value != string.Empty)
		{
			w.mass = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("radius", "wheels-" + wheelPosition);
		if (value != string.Empty)
		{
			w.radius = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("rimRadius", "wheels-" + wheelPosition);
		if (value != string.Empty)
		{
			w.rimRadius = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("width", "wheels-" + wheelPosition);
		if (value != string.Empty)
		{
			w.width = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
	}

	private void LoadWingsData()
	{
		this.i = 0;
		Wing[] array = wings;
		foreach (Wing wing in array)
		{
			this.i++;
			value = LoadValue("dragCoefficient", "wing" + this.i);
			if (value != string.Empty)
			{
				wing.dragCoefficient = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("angleOfAttack", "wing" + this.i);
			if (value != string.Empty)
			{
				wing.angleOfAttack = int.Parse(value);
			}
			value = LoadValue("area", "wing" + this.i);
			if (value != string.Empty)
			{
				wing.area = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
	}

	private void LoadControllerTypeData()
	{
		value = LoadValue("controller", "controllerType").ToLower();
		if (value == "axis")
		{
			cardynamics.controller = CarDynamics.Controller.axis;
		}
		else if (value == "mouse")
		{
			cardynamics.controller = CarDynamics.Controller.mouse;
		}
		else if (value == "mobile")
		{
			cardynamics.controller = CarDynamics.Controller.mobile;
		}
		else if (value == "external")
		{
			cardynamics.controller = CarDynamics.Controller.external;
		}
		cardynamics.SetController(cardynamics.controller.ToString());
	}

	private void LoadControllersData()
	{
		if (axisCarController != null)
		{
			LoadControllerData(axisCarController, CarDynamics.Controller.axis);
		}
		if (mouseCarController != null)
		{
			LoadControllerData(mouseCarController, CarDynamics.Controller.mouse);
		}
		if (mobileCarController != null)
		{
			LoadControllerData(mobileCarController, CarDynamics.Controller.mobile);
		}
	}

	private void LoadControllerData(CarController carController, CarDynamics.Controller controller)
	{
		value = LoadValue("smoothInput", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.smoothInput = bool.Parse(value);
		}
		value = LoadValue("throttleTime", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.throttleTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("throttleReleaseTime", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.throttleReleaseTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("maxThrottleInReverse", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.maxThrottleInReverse = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("brakesTime", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.brakesTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("brakesReleaseTime", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.brakesReleaseTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("steerTime", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.steerTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("steerReleaseTime", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.steerReleaseTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("veloSteerTime", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.veloSteerTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("veloSteerReleaseTime", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.veloSteerReleaseTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("steerCorrectionFactor", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.steerCorrectionFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("steerAssistance", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.steerAssistance = bool.Parse(value);
		}
		value = LoadValue("SteerAssistanceMinVelocity", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.SteerAssistanceMinVelocity = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("TCS", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.TCS = bool.Parse(value);
		}
		value = LoadValue("TCSAllowedSlip", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.TCSAllowedSlip = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("TCSMinVelocity", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.TCSMinVelocity = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("ABS", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.ABS = bool.Parse(value);
		}
		value = LoadValue("ABSAllowedSlip", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.ABSAllowedSlip = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("ABSMinVelocity", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.ABSMinVelocity = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("ESP", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.ESP = bool.Parse(value);
		}
		value = LoadValue("ESPStrength", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.ESPStrength = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		value = LoadValue("ESPMinVelocity", string.Concat(controller, "Controller"));
		if (value != string.Empty)
		{
			carController.ESPMinVelocity = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
		}
		if (carController is AxisCarController)
		{
			value = LoadValue("throttleAxis", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).throttleAxis = value;
			}
			value = LoadValue("brakeAxis", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).brakeAxis = value;
			}
			value = LoadValue("steerAxis", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).steerAxis = value;
			}
			value = LoadValue("handbrakeAxis", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).handbrakeAxis = value;
			}
			value = LoadValue("clutchAxis", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).clutchAxis = value;
			}
			value = LoadValue("shiftUpButton", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).shiftUpButton = value;
			}
			value = LoadValue("shiftDownButton", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).shiftDownButton = value;
			}
			value = LoadValue("startEngineButton", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).startEngineButton = value;
			}
			value = LoadValue("normalizeThrottleInput", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).normalizeThrottleInput = bool.Parse(value);
			}
			value = LoadValue("exponentialThrottleInput", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).exponentialThrottleInput = bool.Parse(value);
			}
			value = LoadValue("normalizeBrakesInput", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).normalizeBrakesInput = bool.Parse(value);
			}
			value = LoadValue("exponentialBrakesInput", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).exponentialBrakesInput = bool.Parse(value);
			}
			value = LoadValue("normalizeClutchInput", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).normalizeClutchInput = bool.Parse(value);
			}
			value = LoadValue("exponentialClutchInput", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((AxisCarController)carController).exponentialClutchInput = bool.Parse(value);
			}
		}
		else if (carController is MouseCarController)
		{
			value = LoadValue("clutchAxis", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((MouseCarController)carController).clutchAxis = value;
			}
			value = LoadValue("shiftUpButton", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((MouseCarController)carController).shiftUpButton = value;
			}
			value = LoadValue("shiftDownButton", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((MouseCarController)carController).shiftDownButton = value;
			}
			value = LoadValue("startEngineButton", string.Concat(controller, "Controller"));
			if (value != string.Empty)
			{
				((MouseCarController)carController).startEngineButton = value;
			}
		}
	}

	private void LoadPhysicMaterialsData()
	{
		if (cardynamics.physicMaterials.Length <= 0)
		{
			return;
		}
		MyPhysicMaterial[] physicMaterials = cardynamics.physicMaterials;
		foreach (MyPhysicMaterial myPhysicMaterial in physicMaterials)
		{
			value = LoadValue("grip", "physicMaterials-" + myPhysicMaterial.surfaceType);
			if (value != string.Empty)
			{
				myPhysicMaterial.grip = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("rollingFriction", "physicMaterials-" + myPhysicMaterial.surfaceType);
			if (value != string.Empty)
			{
				myPhysicMaterial.rollingFriction = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("staticFriction", "physicMaterials-" + myPhysicMaterial.surfaceType);
			if (value != string.Empty)
			{
				myPhysicMaterial.staticFriction = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("isSkidSmoke", "physicMaterials-" + myPhysicMaterial.surfaceType);
			if (value != string.Empty)
			{
				myPhysicMaterial.isSkidSmoke = bool.Parse(value);
			}
			value = LoadValue("isSkidMark", "physicMaterials-" + myPhysicMaterial.surfaceType);
			if (value != string.Empty)
			{
				myPhysicMaterial.isSkidMark = bool.Parse(value);
			}
			value = LoadValue("isDirty", "physicMaterials-" + myPhysicMaterial.surfaceType);
			if (value != string.Empty)
			{
				myPhysicMaterial.isDirty = bool.Parse(value);
			}
		}
	}

	private void LoadArcaderData()
	{
		if (arcader != null)
		{
			value = LoadValue("enabled", "arcader");
			if (value != string.Empty)
			{
				arcader.enabled = bool.Parse(value);
			}
			value = LoadValue("minVelocity", "arcader");
			if (value != string.Empty)
			{
				arcader.minVelocity = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("overallStrength", "arcader");
			if (value != string.Empty)
			{
				arcader.overallStrength = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("COGHelperStrength", "arcader");
			if (value != string.Empty)
			{
				arcader.COGHelperStrength = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("torqueHelperStrength", "arcader");
			if (value != string.Empty)
			{
				arcader.torqueHelperStrength = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("gripHelperStrength", "arcader");
			if (value != string.Empty)
			{
				arcader.gripHelperStrength = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
	}

	private void LoadFuelTanksData()
	{
		this.i = 0;
		FuelTank[] array = fuelTanks;
		foreach (FuelTank fuelTank in array)
		{
			this.i++;
			value = LoadValue("tankCapacity", "fuelTank-" + this.i);
			if (value != string.Empty)
			{
				fuelTank.tankCapacity = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("currentFuel", "fuelTank-" + this.i);
			if (value != string.Empty)
			{
				fuelTank.currentFuel = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("tankWeight", "fuelTank-" + this.i);
			if (value != string.Empty)
			{
				fuelTank.tankWeight = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("fuelDensity", "fuelTank-" + this.i);
			if (value != string.Empty)
			{
				fuelTank.fuelDensity = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
	}

	private void LoadForceFeedBackData()
	{
		if (forceFeedback != null)
		{
			value = LoadValue("enabled", "forcefeedback");
			if (value != string.Empty)
			{
				cardynamics.enableForceFeedback = bool.Parse(value);
			}
			value = LoadValue("factor", "forcefeedback");
			if (value != string.Empty)
			{
				forceFeedback.factor = int.Parse(value);
			}
			value = LoadValue("multiplier", "forcefeedback");
			if (value != string.Empty)
			{
				forceFeedback.multiplier = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("smoothingFactor", "forcefeedback");
			if (value != string.Empty)
			{
				forceFeedback.smoothingFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
			}
			value = LoadValue("clampValue", "forcefeedback");
			if (value != string.Empty)
			{
				forceFeedback.clampValue = int.Parse(value);
			}
			value = LoadValue("invertForcefeedback", "forcefeedback");
			if (value != string.Empty)
			{
				forceFeedback.invertForceFeedback = bool.Parse(value);
			}
		}
	}
}
