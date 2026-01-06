public class TestCarController : CarController
{
	public string throttleAxis = "Throttle";

	public string brakeAxis = "Brake";

	public string steerAxis = "Horizontal";

	public string handbrakeAxis = "Handbrake";

	public string clutchAxis = "Clutch";

	public string shiftUpButton = "ShiftUp";

	public string shiftDownButton = "ShiftDown";

	public string startEngineButton = "StartEngine";

	public bool normalizeThrottleInput;

	public bool exponentialThrottleInput;

	public bool normalizeBrakesInput;

	public bool exponentialBrakesInput;

	public bool normalizeClutchInput;

	public bool exponentialClutchInput;

	protected override void GetInput(out float throttleInput, out float brakeInput, out float steerInput, out float handbrakeInput, out float clutchInput, out bool startEngineInput, out int targetGear)
	{
		throttleInput = 1f;
		brakeInput = 0f;
		steerInput = 0f;
		handbrakeInput = 0f;
		clutchInput = 0f;
		startEngineInput = false;
		targetGear = 2;
	}
}
