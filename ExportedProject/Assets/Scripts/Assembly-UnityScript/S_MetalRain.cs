using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class S_MetalRain : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024Rain_002429 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal S_MetalRain _0024self__002430;

			public _0024(S_MetalRain self_)
			{
				_0024self__002430 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					UnityEngine.Object.Instantiate(_0024self__002430.RainObject, new Vector3((UnityEngine.Random.value - 0.5f) * 20f, UnityEngine.Random.value * 40f + 10f, (UnityEngine.Random.value - 0.5f) * 20f), Quaternion.identity);
					result = (Yield(2, new WaitForSeconds(UnityEngine.Random.value * 0.2f + 0.1f)) ? 1 : 0);
					break;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal S_MetalRain _0024self__002431;

		public _0024Rain_002429(S_MetalRain self_)
		{
			_0024self__002431 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__002431);
		}
	}

	public GameObject RainObject;

	public Texture2D HardnessMap;

	private S_Deformable deformable;

	private bool map;

	private bool showMap;

	public virtual void Start()
	{
		StartCoroutine_Auto(Rain());
		deformable = (S_Deformable)GetComponent(typeof(S_Deformable));
	}

	public virtual IEnumerator Rain()
	{
		return new _0024Rain_002429(this).GetEnumerator();
	}

	public virtual void OnMouseDown()
	{
		deformable.Repair(0.25f);
	}

	public virtual void OnGUI()
	{
		GUILayout.Label("Hardness:");
		deformable.Hardness = GUILayout.HorizontalSlider(deformable.Hardness, 0.1f, 1f);
		deformable.DeformMeshCollider = GUILayout.Toggle(deformable.DeformMeshCollider, " Update mesh collider (high CPU usage)");
		bool flag = GUILayout.Toggle(map, " Use hardness map");
		if (flag != map)
		{
			map = flag;
			deformable.Repair(1f);
			if (map)
			{
				deformable.HardnessMap = HardnessMap;
			}
			else
			{
				deformable.HardnessMap = null;
				GetComponent<Renderer>().material.mainTexture = null;
				showMap = false;
			}
		}
		if (map)
		{
			showMap = GUILayout.Toggle(showMap, " Show hardness map");
			if (showMap)
			{
				GetComponent<Renderer>().material.mainTexture = HardnessMap;
			}
			else
			{
				GetComponent<Renderer>().material.mainTexture = null;
			}
		}
		GUILayout.Label("Click on the object to repair it");
	}

	public virtual void Main()
	{
	}
}
