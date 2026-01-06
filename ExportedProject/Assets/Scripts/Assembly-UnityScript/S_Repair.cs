using System;
using UnityEngine;

[Serializable]
public class S_Repair : MonoBehaviour
{
	private S_Deformable deformable;

	public virtual void Awake()
	{
		deformable = (S_Deformable)GetComponent(typeof(S_Deformable));
	}

	public virtual void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(ray, out hitInfo) && hitInfo.collider.transform.parent == transform)
			{
				deformable.Repair(Time.deltaTime, hitInfo.point, 1f);
			}
		}
	}

	public virtual void Main()
	{
	}
}
