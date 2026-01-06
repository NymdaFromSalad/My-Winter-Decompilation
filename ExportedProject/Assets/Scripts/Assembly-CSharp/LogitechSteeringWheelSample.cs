using System.Text;
using UnityEngine;

public class LogitechSteeringWheelSample : MonoBehaviour
{
	private LogitechGSDK.LogiControllerPropertiesData properties;

	private string actualState;

	private string activeForces;

	private string propertiesEdit;

	private string buttonStatus;

	private string forcesLabel;

	private string[] activeForceAndEffect;

	private void Start()
	{
		activeForces = string.Empty;
		propertiesEdit = string.Empty;
		actualState = string.Empty;
		buttonStatus = string.Empty;
		forcesLabel = "Press the following keys to activate forces and effects on the steering wheel / gaming controller \n";
		forcesLabel += "Spring force : S\n";
		forcesLabel += "Constant force : C\n";
		forcesLabel += "Damper force : D\n";
		forcesLabel += "Side collision : Left or Right Arrow\n";
		forcesLabel += "Front collision : Up arrow\n";
		forcesLabel += "Dirt road effect : I\n";
		forcesLabel += "Bumpy road effect : B\n";
		forcesLabel += "Slippery road effect : L\n";
		forcesLabel += "Surface effect : U\n";
		forcesLabel += "Car Airborne effect : A\n";
		forcesLabel += "Soft Stop Force : O\n";
		forcesLabel += "Set example controller properties : PageUp\n";
		forcesLabel += "Play Leds : P\n";
		activeForceAndEffect = new string[9];
		Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(true));
	}

	private void OnApplicationQuit()
	{
		LogitechGSDK.LogiSteeringShutdown();
	}

	private void OnGUI()
	{
		activeForces = GUI.TextArea(new Rect(10f, 10f, 180f, 200f), activeForces, 400);
		propertiesEdit = GUI.TextArea(new Rect(200f, 10f, 200f, 200f), propertiesEdit, 400);
		actualState = GUI.TextArea(new Rect(410f, 10f, 300f, 200f), actualState, 1000);
		buttonStatus = GUI.TextArea(new Rect(720f, 10f, 300f, 200f), buttonStatus, 1000);
		GUI.Label(new Rect(10f, 400f, 800f, 400f), forcesLabel);
	}

	private void Update()
	{
		if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			LogitechGSDK.LogiGetFriendlyProductName(0, stringBuilder, 256);
			propertiesEdit = string.Concat("Current Controller : ", stringBuilder, "\n");
			propertiesEdit += "Current controller properties : \n\n";
			LogitechGSDK.LogiControllerPropertiesData logiControllerPropertiesData = default(LogitechGSDK.LogiControllerPropertiesData);
			LogitechGSDK.LogiGetCurrentControllerProperties(0, ref logiControllerPropertiesData);
			string text = propertiesEdit;
			propertiesEdit = text + "forceEnable = " + logiControllerPropertiesData.forceEnable + "\n";
			text = propertiesEdit;
			propertiesEdit = text + "overallGain = " + logiControllerPropertiesData.overallGain + "\n";
			text = propertiesEdit;
			propertiesEdit = text + "springGain = " + logiControllerPropertiesData.springGain + "\n";
			text = propertiesEdit;
			propertiesEdit = text + "damperGain = " + logiControllerPropertiesData.damperGain + "\n";
			text = propertiesEdit;
			propertiesEdit = text + "defaultSpringEnabled = " + logiControllerPropertiesData.defaultSpringEnabled + "\n";
			text = propertiesEdit;
			propertiesEdit = text + "combinePedals = " + logiControllerPropertiesData.combinePedals + "\n";
			text = propertiesEdit;
			propertiesEdit = text + "wheelRange = " + logiControllerPropertiesData.wheelRange + "\n";
			text = propertiesEdit;
			propertiesEdit = text + "gameSettingsEnabled = " + logiControllerPropertiesData.gameSettingsEnabled + "\n";
			text = propertiesEdit;
			propertiesEdit = text + "allowGameSettings = " + logiControllerPropertiesData.allowGameSettings + "\n";
			actualState = "Steering wheel current state : \n\n";
			LogitechGSDK.DIJOYSTATE2ENGINES dIJOYSTATE2ENGINES = LogitechGSDK.LogiGetStateCSharp(0);
			text = actualState;
			actualState = text + "x-axis position :" + dIJOYSTATE2ENGINES.lX + "\n";
			text = actualState;
			actualState = text + "y-axis position :" + dIJOYSTATE2ENGINES.lY + "\n";
			text = actualState;
			actualState = text + "z-axis position :" + dIJOYSTATE2ENGINES.lZ + "\n";
			text = actualState;
			actualState = text + "x-axis rotation :" + dIJOYSTATE2ENGINES.lRx + "\n";
			text = actualState;
			actualState = text + "y-axis rotation :" + dIJOYSTATE2ENGINES.lRy + "\n";
			text = actualState;
			actualState = text + "z-axis rotation :" + dIJOYSTATE2ENGINES.lRz + "\n";
			text = actualState;
			actualState = text + "extra axes positions 1 :" + dIJOYSTATE2ENGINES.rglSlider[0] + "\n";
			text = actualState;
			actualState = text + "extra axes positions 2 :" + dIJOYSTATE2ENGINES.rglSlider[1] + "\n";
			switch (dIJOYSTATE2ENGINES.rgdwPOV[0])
			{
			case 0u:
				actualState += "POV : UP\n";
				break;
			case 4500u:
				actualState += "POV : UP-RIGHT\n";
				break;
			case 9000u:
				actualState += "POV : RIGHT\n";
				break;
			case 13500u:
				actualState += "POV : DOWN-RIGHT\n";
				break;
			case 18000u:
				actualState += "POV : DOWN\n";
				break;
			case 22500u:
				actualState += "POV : DOWN-LEFT\n";
				break;
			case 27000u:
				actualState += "POV : LEFT\n";
				break;
			case 31500u:
				actualState += "POV : UP-LEFT\n";
				break;
			default:
				actualState += "POV : CENTER\n";
				break;
			}
			buttonStatus = "Button pressed : \n\n";
			for (int i = 0; i < 128; i++)
			{
				if (dIJOYSTATE2ENGINES.rgbButtons[i] == 128)
				{
					text = buttonStatus;
					buttonStatus = text + "Button " + i + " pressed\n";
				}
			}
			int num = LogitechGSDK.LogiGetShifterMode(0);
			string empty = string.Empty;
			switch (num)
			{
			case 1:
				empty = "Gated";
				break;
			case 0:
				empty = "Sequential";
				break;
			default:
				empty = "Unknown";
				break;
			}
			actualState = actualState + "\nSHIFTER MODE:" + empty;
			activeForces = "Active forces and effects :\n";
			if (Input.GetKeyUp(KeyCode.S))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 0))
				{
					LogitechGSDK.LogiStopSpringForce(0);
					activeForceAndEffect[0] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlaySpringForce(0, 50, 50, 50);
					activeForceAndEffect[0] = "Spring Force\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.C))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 1))
				{
					LogitechGSDK.LogiStopConstantForce(0);
					activeForceAndEffect[1] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlayConstantForce(0, 50);
					activeForceAndEffect[1] = "Constant Force\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.D))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 2))
				{
					LogitechGSDK.LogiStopDamperForce(0);
					activeForceAndEffect[2] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlayDamperForce(0, 50);
					activeForceAndEffect[2] = "Damper Force\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
			{
				LogitechGSDK.LogiPlaySideCollisionForce(0, 60);
			}
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				LogitechGSDK.LogiPlayFrontalCollisionForce(0, 60);
			}
			if (Input.GetKeyUp(KeyCode.I))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 5))
				{
					LogitechGSDK.LogiStopDirtRoadEffect(0);
					activeForceAndEffect[3] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlayDirtRoadEffect(0, 50);
					activeForceAndEffect[3] = "Dirt Road Effect\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.B))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 6))
				{
					LogitechGSDK.LogiStopBumpyRoadEffect(0);
					activeForceAndEffect[4] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlayBumpyRoadEffect(0, 50);
					activeForceAndEffect[4] = "Bumpy Road Effect\n";
				}
			}
			if (Input.GetKeyUp(KeyCode.L))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 7))
				{
					LogitechGSDK.LogiStopSlipperyRoadEffect(0);
					activeForceAndEffect[5] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlaySlipperyRoadEffect(0, 50);
					activeForceAndEffect[5] = "Slippery Road Effect\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.U))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 8))
				{
					LogitechGSDK.LogiStopSurfaceEffect(0);
					activeForceAndEffect[6] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlaySurfaceEffect(0, 1, 50, 1000);
					activeForceAndEffect[6] = "Surface Effect\n";
				}
			}
			if (Input.GetKeyUp(KeyCode.A))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 11))
				{
					LogitechGSDK.LogiStopCarAirborne(0);
					activeForceAndEffect[7] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlayCarAirborne(0);
					activeForceAndEffect[7] = "Car Airborne\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.O))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 10))
				{
					LogitechGSDK.LogiStopSoftstopForce(0);
					activeForceAndEffect[8] = string.Empty;
				}
				else
				{
					LogitechGSDK.LogiPlaySoftstopForce(0, 20);
					activeForceAndEffect[8] = "Soft Stop Force\n";
				}
			}
			if (Input.GetKeyUp(KeyCode.PageUp))
			{
				properties.wheelRange = 90;
				properties.forceEnable = true;
				properties.overallGain = 80;
				properties.springGain = 80;
				properties.damperGain = 80;
				properties.allowGameSettings = true;
				properties.combinePedals = false;
				properties.defaultSpringEnabled = true;
				properties.defaultSpringGain = 80;
				LogitechGSDK.LogiSetPreferredControllerProperties(properties);
			}
			if (Input.GetKeyUp(KeyCode.P))
			{
				LogitechGSDK.LogiPlayLeds(0, 20f, 20f, 20f);
			}
			for (int j = 0; j < 9; j++)
			{
				activeForces += activeForceAndEffect[j];
			}
		}
		else if (!LogitechGSDK.LogiIsConnected(0))
		{
			actualState = "PLEASE PLUG IN A STEERING WHEEL OR A FORCE FEEDBACK CONTROLLER";
		}
		else
		{
			actualState = "THIS WINDOW NEEDS TO BE IN FOREGROUND IN ORDER FOR THE SDK TO WORK PROPERLY";
		}
	}
}
