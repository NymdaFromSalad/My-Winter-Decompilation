using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker")]
	[Tooltip("Uses line renderer to render vector positions of an arraylist")]
	public class LineRenderArrayList : ArrayListActions
	{
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[ActionSection("Set up")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[Tooltip("Start Color")]
		public FsmColor c1;

		[Tooltip("End Color")]
		public FsmColor c2;

		[Tooltip("Start Width")]
		public FsmFloat width1;

		[Tooltip("End Width")]
		public FsmFloat width2;

		[Tooltip("Material")]
		public FsmMaterial lineMaterial;

		public bool everyFrame;

		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent failureEvent;

		private GameObject go;

		private LineRenderer lineRenderer;

		private Vector3 pos;

		public override void OnEnter()
		{
			go = base.Fsm.GetOwnerDefaultTarget(gameObject);
			lineRenderer = go.AddComponent<LineRenderer>();
			lineRenderer.material = lineMaterial.Value;
			lineRenderer.SetColors(c1.Value, c2.Value);
			lineRenderer.SetWidth(width1.Value, width2.Value);
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				renderArrayList();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
			{
				renderArrayList();
			}
		}

		private void tryArrayList()
		{
			if (!isProxyValid())
			{
				Finish();
			}
			try
			{
				pos = (Vector3)proxy.arrayList[0];
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
				base.Fsm.Event(failureEvent);
				Finish();
			}
		}

		private void renderArrayList()
		{
			if (!isProxyValid())
			{
				return;
			}
			int count = proxy.arrayList.Count;
			lineRenderer.SetVertexCount(count);
			for (int i = 0; i < count; i++)
			{
				try
				{
					pos = (Vector3)proxy.arrayList[i];
				}
				catch (Exception ex)
				{
					lineRenderer.SetVertexCount(0);
					Debug.Log(ex.Message);
					base.Fsm.Event(failureEvent);
					break;
				}
				lineRenderer.SetPosition(i, pos);
			}
		}
	}
}
