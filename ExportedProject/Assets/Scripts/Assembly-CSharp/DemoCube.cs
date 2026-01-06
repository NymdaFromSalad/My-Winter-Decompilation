using UnityEngine;

public class DemoCube : MonoBehaviour
{
	private Deformable deformable;

	private void Awake()
	{
		deformable = GetComponent<Deformable>();
	}

	private void OnMouseDown()
	{
		deformable.Repair(0.25f);
	}

	private void OnGUI()
	{
		GUILayout.Label("Click on the cube to repair it");
	}
}
