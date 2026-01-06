using System;
using UnityEngine;

[Serializable]
public class S_RainDrop : MonoBehaviour
{
	public virtual void Start()
	{
		UnityEngine.Object.Destroy(gameObject, 10f);
	}

	public virtual void Main()
	{
	}
}
