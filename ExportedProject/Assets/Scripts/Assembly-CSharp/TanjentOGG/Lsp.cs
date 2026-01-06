using System;

namespace TanjentOGG
{
	public class Lsp
	{
		private static float fromdB(float x)
		{
			return (float)Math.Exp(x * 0.11512925f);
		}

		public static void vorbis_lsp_to_curve(CPtr.FloatPtr curve, int[] map, int n, int ln, float[] lsp, int m, float amp, float ampoffset)
		{
			float num = (float)(Math.PI / (double)ln);
			int i;
			for (i = 0; i < m; i++)
			{
				lsp[i] = (float)(2.0 * Math.Cos(lsp[i]));
			}
			i = 0;
			while (i < n)
			{
				int num2 = map[i];
				float num3 = 0.5f;
				float num4 = 0.5f;
				float num5 = (float)(2.0 * Math.Cos(num * (float)num2));
				int j;
				for (j = 1; j < m; j += 2)
				{
					num4 *= num5 - lsp[j - 1];
					num3 *= num5 - lsp[j];
				}
				if (j == m)
				{
					num4 *= num5 - lsp[j - 1];
					num3 *= num3 * (4f - num5 * num5);
					num4 *= num4;
				}
				else
				{
					num3 *= num3 * (2f - num5);
					num4 *= num4 * (2f + num5);
				}
				num4 = fromdB((float)((double)amp / Math.Sqrt(num3 + num4) - (double)ampoffset));
				curve.floats[curve.offset + i] *= num4;
				while (map[++i] == num2)
				{
					curve.floats[curve.offset + i] *= num4;
				}
			}
		}
	}
}
