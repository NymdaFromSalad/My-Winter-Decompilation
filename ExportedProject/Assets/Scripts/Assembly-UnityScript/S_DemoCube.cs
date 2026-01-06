using System;
using UnityEngine;

[Serializable]
public class S_DemoCube : MonoBehaviour
{
	private S_Deformable deformable;

	public virtual void Awake()
	{
		deformable = (S_Deformable)GetComponent(typeof(S_Deformable));
	}

	public virtual void OnMouseDown()
	{
		deformable.Repair(0.25f);
	}

	public virtual void OnGUI()
	{
		GUILayout.Label("Click on the cube to repair it");
	}

	public virtual void Main()
	{
	}
}
