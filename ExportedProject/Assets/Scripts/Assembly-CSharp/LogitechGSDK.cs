using System;
using System.Runtime.InteropServices;
using System.Text;

public class LogitechGSDK
{
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct LogiControllerPropertiesData
	{
		public bool forceEnable;

		public int overallGain;

		public int springGain;

		public int damperGain;

		public bool defaultSpringEnabled;

		public int defaultSpringGain;

		public bool combinePedals;

		public int wheelRange;

		public bool gameSettingsEnabled;

		public bool allowGameSettings;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct DIJOYSTATE2ENGINES
	{
		public int lX;

		public int lY;

		public int lZ;

		public int lRx;

		public int lRy;

		public int lRz;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglSlider;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] rgdwPOV;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] rgbButtons;

		public int lVX;

		public int lVY;

		public int lVZ;

		public int lVRx;

		public int lVRy;

		public int lVRz;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglVSlider;

		public int lAX;

		public int lAY;

		public int lAZ;

		public int lARx;

		public int lARy;

		public int lARz;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglASlider;

		public int lFX;

		public int lFY;

		public int lFZ;

		public int lFRx;

		public int lFRy;

		public int lFRz;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglFSlider;
	}

	public const int LOGI_MAX_CONTROLLERS = 4;

	public const int LOGI_FORCE_NONE = -1;

	public const int LOGI_FORCE_SPRING = 0;

	public const int LOGI_FORCE_CONSTANT = 1;

	public const int LOGI_FORCE_DAMPER = 2;

	public const int LOGI_FORCE_SIDE_COLLISION = 3;

	public const int LOGI_FORCE_FRONTAL_COLLISION = 4;

	public const int LOGI_FORCE_DIRT_ROAD = 5;

	public const int LOGI_FORCE_BUMPY_ROAD = 6;

	public const int LOGI_FORCE_SLIPPERY_ROAD = 7;

	public const int LOGI_FORCE_SURFACE_EFFECT = 8;

	public const int LOGI_NUMBER_FORCE_EFFECTS = 9;

	public const int LOGI_FORCE_SOFTSTOP = 10;

	public const int LOGI_FORCE_CAR_AIRBORNE = 11;

	public const int LOGI_PERIODICTYPE_NONE = -1;

	public const int LOGI_PERIODICTYPE_SINE = 0;

	public const int LOGI_PERIODICTYPE_SQUARE = 1;

	public const int LOGI_PERIODICTYPE_TRIANGLE = 2;

	public const int LOGI_DEVICE_TYPE_NONE = -1;

	public const int LOGI_DEVICE_TYPE_WHEEL = 0;

	public const int LOGI_DEVICE_TYPE_JOYSTICK = 1;

	public const int LOGI_DEVICE_TYPE_GAMEPAD = 2;

	public const int LOGI_DEVICE_TYPE_OTHER = 3;

	public const int LOGI_NUMBER_DEVICE_TYPES = 4;

	public const int LOGI_MANUFACTURER_NONE = -1;

	public const int LOGI_MANUFACTURER_LOGITECH = 0;

	public const int LOGI_MANUFACTURER_MICROSOFT = 1;

	public const int LOGI_MANUFACTURER_OTHER = 2;

	public const int LOGI_MODEL_G27 = 0;

	public const int LOGI_MODEL_DRIVING_FORCE_GT = 1;

	public const int LOGI_MODEL_G25 = 2;

	public const int LOGI_MODEL_MOMO_RACING = 3;

	public const int LOGI_MODEL_MOMO_FORCE = 4;

	public const int LOGI_MODEL_DRIVING_FORCE_PRO = 5;

	public const int LOGI_MODEL_DRIVING_FORCE = 6;

	public const int LOGI_MODEL_NASCAR_RACING_WHEEL = 7;

	public const int LOGI_MODEL_FORMULA_FORCE = 8;

	public const int LOGI_MODEL_FORMULA_FORCE_GP = 9;

	public const int LOGI_MODEL_FORCE_3D_PRO = 10;

	public const int LOGI_MODEL_EXTREME_3D_PRO = 11;

	public const int LOGI_MODEL_FREEDOM_24 = 12;

	public const int LOGI_MODEL_ATTACK_3 = 13;

	public const int LOGI_MODEL_FORCE_3D = 14;

	public const int LOGI_MODEL_STRIKE_FORCE_3D = 15;

	public const int LOGI_MODEL_G940_JOYSTICK = 16;

	public const int LOGI_MODEL_G940_THROTTLE = 17;

	public const int LOGI_MODEL_G940_PEDALS = 18;

	public const int LOGI_MODEL_RUMBLEPAD = 19;

	public const int LOGI_MODEL_RUMBLEPAD_2 = 20;

	public const int LOGI_MODEL_CORDLESS_RUMBLEPAD_2 = 21;

	public const int LOGI_MODEL_CORDLESS_GAMEPAD = 22;

	public const int LOGI_MODEL_DUAL_ACTION_GAMEPAD = 23;

	public const int LOGI_MODEL_PRECISION_GAMEPAD_2 = 24;

	public const int LOGI_MODEL_CHILLSTREAM = 25;

	public const int LOGI_MODEL_G29 = 26;

	public const int LOGI_MODEL_G920 = 27;

	public const int LOGI_NUMBER_MODELS = 28;

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSteeringInitialize(bool ignoreXInputControllers);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiUpdate();

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr LogiGetStateENGINES(int index);

	public static DIJOYSTATE2ENGINES LogiGetStateCSharp(int index)
	{
		DIJOYSTATE2ENGINES result = new DIJOYSTATE2ENGINES
		{
			rglSlider = new int[2],
			rgdwPOV = new uint[4],
			rgbButtons = new byte[128],
			rglVSlider = new int[2],
			rglASlider = new int[2],
			rglFSlider = new int[2]
		};
		try
		{
			result = (DIJOYSTATE2ENGINES)Marshal.PtrToStructure(LogiGetStateENGINES(index), typeof(DIJOYSTATE2ENGINES));
			return result;
		}
		catch (ArgumentException)
		{
		}
		return result;
	}

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGetDevicePath(int index, StringBuilder str, int size);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGetFriendlyProductName(int index, StringBuilder str, int size);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsConnected(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsDeviceConnected(int index, int deviceType);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsManufacturerConnected(int index, int manufacturerName);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsModelConnected(int index, int modelName);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonTriggered(int index, int buttonNbr);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonReleased(int index, int buttonNbr);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonIsPressed(int index, int buttonNbr);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGenerateNonLinearValues(int index, int nonLinCoeff);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGetNonLinearValue(int index, int inputValue);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiHasForceFeedback(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsPlaying(int index, int forceType);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySpringForce(int index, int offsetPercentage, int saturationPercentage, int coefficientPercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSpringForce(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayConstantForce(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopConstantForce(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayDamperForce(int index, int coefficientPercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopDamperForce(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySideCollisionForce(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayFrontalCollisionForce(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayDirtRoadEffect(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopDirtRoadEffect(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayBumpyRoadEffect(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopBumpyRoadEffect(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySlipperyRoadEffect(int index, int magnitudePercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSlipperyRoadEffect(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySurfaceEffect(int index, int type, int magnitudePercentage, int period);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSurfaceEffect(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayCarAirborne(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopCarAirborne(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySoftstopForce(int index, int usableRangePercentage);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSoftstopForce(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSetPreferredControllerProperties(LogiControllerPropertiesData properties);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGetCurrentControllerProperties(int index, ref LogiControllerPropertiesData properties);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGetShifterMode(int index);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSetOperatingRange(int index, int range);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGetOperatingRange(int index, ref int range);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayLeds(int index, float currentRPM, float rpmFirstLedTurnsOn, float rpmRedLine);

	[DllImport("LogitechSteeringWheel", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiSteeringShutdown();
}
