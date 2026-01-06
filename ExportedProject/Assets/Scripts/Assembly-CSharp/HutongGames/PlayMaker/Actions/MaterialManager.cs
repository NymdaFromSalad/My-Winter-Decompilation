using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("♥ Caligula ♥")]
	[Tooltip("Handels material management.")]
	public class MaterialManager : FsmStateAction
	{
		private static bool[] available = new bool[7];

		[Tooltip("Specifies what materials are available")]
		[ActionSection("Setup")]
		public static FsmMaterial[] materials = new FsmMaterial[7];

		[Tooltip("Material recently used. Doesn't need to be initialized!")]
		public static FsmMaterial usematerial;

		public static Material use;

		public override void Reset()
		{
			for (int i = 0; i < available.Length; i++)
			{
				available[i] = true;
			}
			CubeCreated();
		}

		public override void OnEnter()
		{
		}

		public override void OnUpdate()
		{
		}

		public static void CubeCreated()
		{
			for (int i = 0; i < available.Length; i++)
			{
				if (available[i])
				{
					available[i] = false;
					usematerial = materials[i];
					break;
				}
			}
		}

		public static void setMaterial()
		{
			use = usematerial.Value;
		}

		public static void CubeDestroyed()
		{
			for (int i = 0; i < available.Length; i++)
			{
				if (!available[i])
				{
					available[i] = true;
					break;
				}
			}
		}
	}
}
