public class AxisCarController : CarController
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
		throttleInput = cInput.GetAxisRaw(throttleAxis);
		brakeInput = cInput.GetAxisRaw(brakeAxis);
		steerInput = cInput.GetAxisRaw(steerAxis);
		handbrakeInput = cInput.GetAxisRaw(handbrakeAxis);
		clutchInput = cInput.GetAxisRaw(clutchAxis);
		if (normalizeThrottleInput)
		{
			throttleInput = (throttleInput + 1f) / 2f;
		}
		if (exponentialThrottleInput)
		{
			throttleInput *= throttleInput;
		}
		if (normalizeBrakesInput)
		{
			brakeInput = (brakeInput + 1f) / 2f;
		}
		if (exponentialBrakesInput)
		{
			brakeInput *= brakeInput;
		}
		if (normalizeClutchInput)
		{
			clutchInput = (clutchInput + 1f) / 2f;
		}
		if (exponentialClutchInput)
		{
			clutchInput *= clutchInput;
		}
		startEngineInput = cInput.GetKeyDown(startEngineButton);
		targetGear = drivetrain.gear;
		if (cInput.GetKeyDown(shiftUpButton))
		{
			targetGear++;
		}
		if (cInput.GetKeyDown(shiftDownButton))
		{
			targetGear--;
		}
		if (drivetrain.shifter)
		{
			if (cInput.GetButton("reverse"))
			{
				targetGear = 0;
			}
			else if (cInput.GetButton("neutral"))
			{
				targetGear = 1;
			}
			else if (cInput.GetButton("first"))
			{
				targetGear = 2;
			}
			else if (cInput.GetButton("second"))
			{
				targetGear = 3;
			}
			else if (cInput.GetButton("third"))
			{
				targetGear = 4;
			}
			else if (cInput.GetButton("fourth"))
			{
				targetGear = 5;
			}
			else if (cInput.GetButton("fifth"))
			{
				targetGear = 6;
			}
			else if (cInput.GetButton("sixth"))
			{
				targetGear = 7;
			}
			else
			{
				targetGear = 1;
			}
		}
	}
}
