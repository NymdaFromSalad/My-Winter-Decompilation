using UnityEngine;

[ExecuteInEditMode]
public class DashBoard : MonoBehaviour
{
	public enum Docking
	{
		Left = 0,
		Right = 1
	}

	public enum Unit
	{
		Kmh = 0,
		Mph = 1
	}

	public enum SpeedoMeterType
	{
		RigidBody = 0,
		Wheel = 1
	}

	public int depth = 2;

	public Texture2D tachoMeter;

	[HideInInspector]
	public bool tachoMeterDisabled;

	public Vector2 tachoMeterPosition;

	public Docking tachoMeterDocking;

	public float tachoMeterDimension = 1f;

	public Texture2D tachoMeterNeedle;

	public Vector2 tachoMeterNeedleSize;

	public float tachoMeterNeedleAngle;

	private float actualtachoMeterNeedleAngle;

	public float RPMFactor = 3.5714f;

	public Texture2D speedoMeter;

	[HideInInspector]
	public bool speedoMeterDisabled;

	public Vector2 speedoMeterPosition;

	public Docking speedoMeterDocking = Docking.Right;

	public float speedoMeterDimension = 1f;

	public Texture2D speedoMeterNeedle;

	public Vector2 speedoMeterNeedleSize;

	public float speedoMeterNeedleAngle;

	private float actualspeedoMeterNeedleAngle;

	public float speedoMeterFactor;

	public SpeedoMeterType speedoMeterType;

	public Unit digitalSpeedoUnit;

	public GUIStyle digitalSpeedoStyle;

	public Vector2 digitalSpeedoPosition;

	public Docking digitalSpeedoDocking;

	public GUIStyle gearMonitorStyle;

	public Vector2 gearMonitorPosition;

	public Docking gearMonitorDocking;

	public Texture2D clutchMonitor;

	public Texture2D throttleMonitor;

	public Texture2D brakeMonitor;

	public Vector2 pedalsMonitorPosition;

	public Texture2D ABS;

	public Texture2D TCS;

	public Texture2D ESP;

	public Vector2 dashboardLightsPosition;

	public Docking dashboardLightsDocking;

	public float dashboardLightsDimension = 1f;

	public GameObject digitalSpeedoOnBoard;

	public GameObject digitalGearOnBoard;

	public GameObject tachoMeterNeedleOnBoard;

	public GameObject speedoMeterNeedleOnBoard;

	private TextMesh textMeshSpeed;

	private TextMesh textMeshGear;

	[HideInInspector]
	public bool showGUIDashboard = true;

	[HideInInspector]
	public CarController carController;

	private Drivetrain drivetrain;

	private Rect tachoMeterRect;

	private Rect tachoMeterNeedleRect;

	private Rect speedoMeterRect;

	private Rect speedoMeterNeedleRect;

	private Rect instrumentalPanelRect;

	private Rect gearRect;

	private Rect speedoRect;

	private Vector2 pivot;

	private float speedoMeterVelo;

	private float digitalSpeedoVelo;

	private float factor;

	private float absVelo;

	private int sign = 1;

	private int shift;

	private void Start()
	{
		drivetrain = base.transform.parent.GetComponent<Drivetrain>();
		if (digitalSpeedoOnBoard != null)
		{
			textMeshSpeed = digitalSpeedoOnBoard.GetComponent<TextMesh>();
		}
		if (digitalGearOnBoard != null)
		{
			textMeshGear = digitalGearOnBoard.GetComponent<TextMesh>();
		}
		if (digitalSpeedoUnit == Unit.Kmh)
		{
			factor = 3.6f;
		}
		else
		{
			factor = 2.237f;
		}
	}

	private void OnGUI()
	{
		if (speedoMeterType == SpeedoMeterType.RigidBody)
		{
			absVelo = Mathf.Abs(drivetrain.velo);
		}
		else
		{
			absVelo = Mathf.Abs(drivetrain.wheelTireVelo);
		}
		speedoMeterVelo = absVelo * 3.6f;
		digitalSpeedoVelo = absVelo * factor;
		GUI.depth = depth;
		if (RPMFactor < 0f)
		{
			RPMFactor = 0f;
		}
		if ((bool)drivetrain)
		{
			actualtachoMeterNeedleAngle = drivetrain.rpm * (RPMFactor / 10f) + tachoMeterNeedleAngle;
		}
		if (speedoMeterFactor < 0.5f)
		{
			speedoMeterFactor = 0.5f;
		}
		if ((bool)drivetrain)
		{
			actualspeedoMeterNeedleAngle = speedoMeterVelo * speedoMeterFactor + speedoMeterNeedleAngle;
		}
		if (!Application.isPlaying || showGUIDashboard)
		{
			if ((bool)tachoMeter)
			{
				float num = 0f;
				if (tachoMeterDocking == Docking.Right)
				{
					num = Screen.width - speedoMeter.width;
				}
				tachoMeterRect = new Rect(tachoMeterPosition.x + num, (float)(Screen.height - tachoMeter.height) - tachoMeterPosition.y + 4f, (float)tachoMeter.width * tachoMeterDimension, (float)tachoMeter.height * tachoMeterDimension);
				GUI.DrawTexture(tachoMeterRect, tachoMeter);
				if ((bool)tachoMeterNeedle)
				{
					if (tachoMeterNeedleSize == Vector2.zero)
					{
						tachoMeterNeedleSize.x = tachoMeterNeedle.width;
						tachoMeterNeedleSize.y = tachoMeterNeedle.height;
					}
					tachoMeterNeedleRect = new Rect(tachoMeterRect.x + tachoMeterRect.width / 2f - tachoMeterNeedleSize.x * tachoMeterDimension * 0.5f, tachoMeterRect.y + tachoMeterRect.height / 2f - tachoMeterNeedleSize.y * tachoMeterDimension * 0.5f, tachoMeterNeedleSize.x * tachoMeterDimension, tachoMeterNeedleSize.y * tachoMeterDimension);
					pivot = new Vector2(tachoMeterNeedleRect.xMin + tachoMeterNeedleRect.width * 0.5f, tachoMeterNeedleRect.yMin + tachoMeterNeedleRect.height * 0.5f);
					Matrix4x4 matrix = GUI.matrix;
					GUIUtility.RotateAroundPivot(actualtachoMeterNeedleAngle, pivot);
					GUI.DrawTexture(tachoMeterNeedleRect, tachoMeterNeedle);
					GUI.matrix = matrix;
				}
			}
			if ((bool)speedoMeter)
			{
				float num2 = 0f;
				if (speedoMeterDocking == Docking.Right)
				{
					num2 = Screen.width - speedoMeter.width;
				}
				Rect position = new Rect(speedoMeterPosition.x + num2, (float)(Screen.height - speedoMeter.height) - speedoMeterPosition.y + 4f, (float)speedoMeter.width * speedoMeterDimension, (float)speedoMeter.height * speedoMeterDimension);
				GUI.DrawTexture(position, speedoMeter);
				if ((bool)speedoMeterNeedle)
				{
					if (speedoMeterNeedleSize == Vector2.zero)
					{
						speedoMeterNeedleSize.x = speedoMeterNeedle.width;
						speedoMeterNeedleSize.y = speedoMeterNeedle.height;
					}
					speedoMeterNeedleRect = new Rect(position.x + position.width / 2f - speedoMeterNeedleSize.x * speedoMeterDimension * 0.5f, position.y + position.height / 2f - speedoMeterNeedleSize.y * speedoMeterDimension * 0.5f, speedoMeterNeedleSize.x * speedoMeterDimension, speedoMeterNeedleSize.y * speedoMeterDimension);
					pivot = new Vector2(speedoMeterNeedleRect.xMin + speedoMeterNeedleRect.width * 0.5f, speedoMeterNeedleRect.yMin + speedoMeterNeedleRect.height * 0.5f);
					Matrix4x4 matrix2 = GUI.matrix;
					GUIUtility.RotateAroundPivot(actualspeedoMeterNeedleAngle, pivot);
					GUI.DrawTexture(speedoMeterNeedleRect, speedoMeterNeedle);
					GUI.matrix = matrix2;
				}
			}
			sign = 1;
			shift = 0;
			if (dashboardLightsDocking == Docking.Right)
			{
				sign = -1;
				shift = Mathf.RoundToInt((float)Screen.width - (float)(TCS.width * 3) * dashboardLightsDimension);
			}
			bool flag = carController == null;
			if ((bool)TCS)
			{
				bool flag2 = false;
				if (!flag)
				{
					flag2 = carController.TCSTriggered;
				}
				else if (!Application.isPlaying)
				{
					flag2 = true;
				}
				if (flag2)
				{
					GUI.DrawTexture(new Rect((float)sign * dashboardLightsPosition.x + (float)shift, (float)Screen.height - (float)TCS.height * dashboardLightsDimension - dashboardLightsPosition.y, (float)TCS.width * dashboardLightsDimension, (float)TCS.height * dashboardLightsDimension), TCS);
				}
			}
			if ((bool)ABS)
			{
				bool flag3 = false;
				if (!flag)
				{
					flag3 = carController.ABSTriggered;
				}
				else if (!Application.isPlaying)
				{
					flag3 = true;
				}
				if (flag3)
				{
					GUI.DrawTexture(new Rect((float)TCS.width * dashboardLightsDimension + (float)sign * dashboardLightsPosition.x + (float)shift, (float)Screen.height - (float)ABS.height * dashboardLightsDimension - dashboardLightsPosition.y, (float)ABS.width * dashboardLightsDimension, (float)ABS.height * dashboardLightsDimension), ABS);
				}
			}
			if ((bool)ESP)
			{
				bool flag4 = false;
				if (!flag)
				{
					flag4 = carController.ESPTriggered;
				}
				else if (!Application.isPlaying)
				{
					flag4 = true;
				}
				if (flag4)
				{
					GUI.DrawTexture(new Rect((float)TCS.width * dashboardLightsDimension + (float)ABS.width * dashboardLightsDimension + (float)sign * dashboardLightsPosition.x + (float)shift, (float)Screen.height - (float)ESP.height * dashboardLightsDimension - dashboardLightsPosition.y, (float)ESP.width * dashboardLightsDimension, (float)ESP.height * dashboardLightsDimension), ESP);
				}
			}
			if ((bool)throttleMonitor)
			{
				float num3 = 0f;
				if (!flag)
				{
					num3 = carController.throttle;
				}
				else if (!Application.isPlaying)
				{
					num3 = 1f;
				}
				Rect position2 = new Rect(pedalsMonitorPosition.x + (float)Screen.width - 10f, (float)Screen.height - pedalsMonitorPosition.y, 10f, (float)(-throttleMonitor.height) * num3);
				GUI.DrawTexture(position2, throttleMonitor);
			}
			if ((bool)brakeMonitor)
			{
				float num4 = 0f;
				if (!flag)
				{
					num4 = carController.brake;
				}
				else if (!Application.isPlaying)
				{
					num4 = 1f;
				}
				Rect position3 = new Rect(pedalsMonitorPosition.x - 12f + (float)Screen.width - 10f, (float)Screen.height - pedalsMonitorPosition.y, 10f, (float)(-brakeMonitor.height) * num4);
				GUI.DrawTexture(position3, brakeMonitor);
			}
			if ((bool)clutchMonitor)
			{
				float num5 = 0f;
				if (drivetrain != null && drivetrain.clutch != null)
				{
					num5 = drivetrain.clutch.GetClutchPosition();
				}
				else if (!Application.isPlaying)
				{
					num5 = 0f;
				}
				Rect position4 = new Rect(pedalsMonitorPosition.x - 24f + (float)Screen.width - 10f, (float)Screen.height - pedalsMonitorPosition.y, 10f, (float)(-clutchMonitor.height) * (1f - num5));
				GUI.DrawTexture(position4, clutchMonitor);
			}
			if ((bool)drivetrain)
			{
				sign = 1;
				shift = 0;
				if (gearMonitorDocking == Docking.Right)
				{
					sign = -1;
					shift = Screen.width - 25;
				}
				gearRect = new Rect((float)sign * gearMonitorPosition.x + (float)shift, 0f - gearMonitorPosition.y + (float)Screen.height - 50f, 50f, 50f);
				if (drivetrain.gear < drivetrain.neutral)
				{
					GUI.Label(gearRect, "R", gearMonitorStyle);
				}
				else if (drivetrain.gear == drivetrain.neutral)
				{
					GUI.Label(gearRect, "H", gearMonitorStyle);
				}
				else
				{
					GUI.Label(gearRect, string.Empty + (drivetrain.gear - drivetrain.neutral), gearMonitorStyle);
				}
				sign = 1;
				shift = 0;
				if (digitalSpeedoDocking == Docking.Right)
				{
					sign = -1;
					shift = Screen.width - 25;
				}
				speedoRect = new Rect((float)sign * digitalSpeedoPosition.x + (float)shift, 0f - digitalSpeedoPosition.y + (float)Screen.height - 50f, 50f, 50f);
				GUI.Label(speedoRect, string.Empty + Mathf.Round(digitalSpeedoVelo), digitalSpeedoStyle);
			}
		}
		if (textMeshSpeed != null)
		{
			textMeshSpeed.text = string.Empty + Mathf.Round(digitalSpeedoVelo);
		}
		if (tachoMeterNeedleOnBoard != null)
		{
			tachoMeterNeedleOnBoard.transform.localRotation = Quaternion.Euler(tachoMeterNeedleOnBoard.transform.localEulerAngles.x, tachoMeterNeedleOnBoard.transform.localEulerAngles.y, actualtachoMeterNeedleAngle);
		}
		if (speedoMeterNeedleOnBoard != null)
		{
			speedoMeterNeedleOnBoard.transform.localRotation = Quaternion.Euler(speedoMeterNeedleOnBoard.transform.localEulerAngles.x, speedoMeterNeedleOnBoard.transform.localEulerAngles.y, actualspeedoMeterNeedleAngle);
		}
		if (textMeshGear != null)
		{
			if (drivetrain.gear < drivetrain.neutral)
			{
				textMeshGear.text = "R";
			}
			else if (drivetrain.gear == drivetrain.neutral)
			{
				textMeshGear.text = "H";
			}
			else
			{
				textMeshGear.text = string.Empty + (drivetrain.gear - drivetrain.neutral);
			}
		}
	}
}
