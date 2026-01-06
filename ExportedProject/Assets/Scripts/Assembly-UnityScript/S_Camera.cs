using System;
using UnityEngine;

[Serializable]
public class S_Camera : MonoBehaviour
{
	public GameObject Car;

	public virtual void Update()
	{
		transform.LookAt(Car.transform);
	}

	public virtual void Main()
	{
	}
}
