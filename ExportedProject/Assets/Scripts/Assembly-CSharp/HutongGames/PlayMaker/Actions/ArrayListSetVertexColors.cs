using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Set a mesh vertex colors based on colors found in an arrayList")]
	public class ArrayListSetVertexColors : ArrayListActions
	{
		[RequiredField]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[ActionSection("Set up")]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("The GameObject to set the mesh colors to")]
		[ActionSection("Target")]
		[CheckForComponent(typeof(MeshFilter))]
		public FsmGameObject mesh;

		public bool everyFrame;

		private Mesh _mesh;

		private Color[] _colors;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			mesh = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject value = mesh.Value;
			if (value == null)
			{
				Finish();
				return;
			}
			MeshFilter component = value.GetComponent<MeshFilter>();
			if (component == null)
			{
				Finish();
				return;
			}
			_mesh = component.mesh;
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				SetVertexColors();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			SetVertexColors();
		}

		public void SetVertexColors()
		{
			if (!isProxyValid())
			{
				return;
			}
			_colors = new Color[proxy.arrayList.Count];
			int num = 0;
			foreach (Color array in proxy.arrayList)
			{
				_colors[num] = array;
				num++;
			}
			_mesh.colors = _colors;
		}
	}
}
