using System.Collections.Generic;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/Effects/Outline", 15)]
	public class Outline : Shadow
	{
		protected Outline()
		{
		}

		public override void ModifyVertices(List<UIVertex> verts)
		{
			if (IsActive())
			{
				int start = 0;
				int count = verts.Count;
				ApplyShadow(verts, base.effectColor, start, verts.Count, base.effectDistance.x, base.effectDistance.y);
				start = count;
				count = verts.Count;
				ApplyShadow(verts, base.effectColor, start, verts.Count, base.effectDistance.x, 0f - base.effectDistance.y);
				start = count;
				count = verts.Count;
				ApplyShadow(verts, base.effectColor, start, verts.Count, 0f - base.effectDistance.x, base.effectDistance.y);
				start = count;
				count = verts.Count;
				ApplyShadow(verts, base.effectColor, start, verts.Count, 0f - base.effectDistance.x, 0f - base.effectDistance.y);
			}
		}
	}
}
