using UnityEngine;

public class Repair : MonoBehaviour
{
	private Deformable deformable;

	private void Awake()
	{
		deformable = GetComponent<Deformable>();
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.transform.parent == base.transform)
			{
				deformable.Repair(Time.deltaTime, hitInfo.point, 1f);
			}
		}
	}
}
