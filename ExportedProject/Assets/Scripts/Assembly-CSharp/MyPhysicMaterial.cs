using System;
using UnityEngine;

[Serializable]
public class MyPhysicMaterial
{
	public PhysicMaterial physicMaterial;

	public float grip = 1f;

	public float rollingFriction = 0.018f;

	public float staticFriction = 1f;

	public bool isSkidSmoke = true;

	public bool isSkidMark = true;

	public bool isDirty;

	public CarDynamics.SurfaceType surfaceType;
}
