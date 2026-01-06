using System;
using UnityEngine;

[Serializable]
public class Axle
{
	public Wheel leftWheel;

	public Wheel rightWheel;

	[HideInInspector]
	public Wheel[] wheels;

	public bool powered;

	public float suspensionTravel = 0.2f;

	public float suspensionRate = 20000f;

	public float bumpRate = 4000f;

	public float reboundRate = 4000f;

	public float fastBumpFactor = 0.3f;

	public float fastReboundFactor = 0.3f;

	public float antiRollBarRate = 10000f;

	public float brakeFrictionTorque = 1500f;

	public float handbrakeFrictionTorque;

	public float maxSteeringAngle;

	public float forwardGripFactor = 1f;

	public float sidewaysGripFactor = 1f;

	public float camber;

	public float caster;

	[HideInInspector]
	public float deltaCamber;

	[HideInInspector]
	public float oldCamber;

	public CarDynamics.Tires tires;

	public float tiresPressure = 200f;

	public float optimalTiresPressure = 200f;
}
