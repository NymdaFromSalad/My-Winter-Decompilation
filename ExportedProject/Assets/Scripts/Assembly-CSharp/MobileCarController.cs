using System;
using UnityEngine;

public class MobileCarController : CarController
{
	[Serializable]
	public class TouchButton
	{
		public GUITexture texture;

		public float alphaButtonDown = 0.2f;

		public float alphaButtonReleased = 0.3f;
	}

	public TouchButton throttleButton;

	public TouchButton brakeButton;

	public TouchButton handbrakeButton;

	public TouchButton gearUpButton;

	public TouchButton gearDownButton;

	protected override void GetInput(out float throttleInput, out float brakeInput, out float steerInput, out float handbrakeInput, out float clutchInput, out bool startEngineInput, out int targetGear)
	{
		clutchInput = 0f;
		startEngineInput = false;
		steerInput = Mathf.Clamp(0f - Input.acceleration.y, -1f, 1f);
		throttleInput = 0f;
		brakeInput = 0f;
		handbrakeInput = 0f;
		bool flag = false;
		bool flag2 = false;
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (touch.phase != TouchPhase.Ended && throttleButton != null && throttleButton.texture != null && throttleButton.texture.HitTest(touch.position))
			{
				throttleInput = 1f;
				Color color = throttleButton.texture.color;
				color.a = throttleButton.alphaButtonDown;
				throttleButton.texture.color = color;
			}
			if (touch.phase != TouchPhase.Ended && brakeButton != null && brakeButton.texture != null && brakeButton.texture.HitTest(touch.position))
			{
				brakeInput = 1f;
				Color color2 = brakeButton.texture.color;
				color2.a = brakeButton.alphaButtonDown;
				brakeButton.texture.color = color2;
			}
			if (touch.phase != TouchPhase.Ended && handbrakeButton != null && handbrakeButton.texture != null && handbrakeButton.texture.HitTest(touch.position))
			{
				handbrakeInput = 1f;
				Color color3 = handbrakeButton.texture.color;
				color3.a = handbrakeButton.alphaButtonDown;
				handbrakeButton.texture.color = color3;
			}
			if (touch.phase == TouchPhase.Began && gearUpButton != null && gearUpButton.texture != null && gearUpButton.texture.HitTest(touch.position))
			{
				flag = true;
				Color color4 = gearUpButton.texture.color;
				color4.a = gearUpButton.alphaButtonDown;
				gearUpButton.texture.color = color4;
			}
			if (touch.phase == TouchPhase.Began && gearDownButton != null && gearDownButton.texture != null && gearDownButton.texture.HitTest(touch.position))
			{
				flag2 = true;
				Color color5 = gearDownButton.texture.color;
				color5.a = gearDownButton.alphaButtonDown;
				gearDownButton.texture.color = color5;
			}
		}
		targetGear = drivetrain.gear;
		if (flag && !flag2)
		{
			targetGear++;
		}
		else if (flag2 && !flag)
		{
			targetGear--;
		}
		if (throttleButton != null && throttleButton.texture != null && throttleInput < Mathf.Epsilon)
		{
			Color color6 = throttleButton.texture.color;
			color6.a = throttleButton.alphaButtonReleased;
			throttleButton.texture.color = color6;
		}
		if (brakeButton != null && brakeButton.texture != null && brakeInput < Mathf.Epsilon)
		{
			Color color7 = brakeButton.texture.color;
			color7.a = brakeButton.alphaButtonReleased;
			brakeButton.texture.color = color7;
		}
		if (handbrakeButton != null && handbrakeButton.texture != null && handbrakeInput < Mathf.Epsilon)
		{
			Color color8 = handbrakeButton.texture.color;
			color8.a = handbrakeButton.alphaButtonReleased;
			handbrakeButton.texture.color = color8;
		}
		if (gearUpButton != null && gearUpButton.texture != null && !flag)
		{
			Color color9 = gearUpButton.texture.color;
			color9.a = gearUpButton.alphaButtonReleased;
			gearUpButton.texture.color = color9;
		}
		if (gearDownButton != null && gearDownButton.texture != null && !flag2)
		{
			Color color10 = gearDownButton.texture.color;
			color10.a = gearDownButton.alphaButtonReleased;
			gearDownButton.texture.color = color10;
		}
	}
}
