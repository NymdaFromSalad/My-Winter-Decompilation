using System;

namespace TanjentOGG
{
	public class Mdct
	{
		public class mdct_lookup
		{
			public int n;

			public int log2n;

			public float[] trig;

			public int[] bitrev;

			public float scale;
		}

		private static float cPI3_8 = 0.38268343f;

		private static float cPI2_8 = 0.70710677f;

		private static float cPI1_8 = 0.9238795f;

		private static int rint(float x)
		{
			return (int)Math.Floor(x + 0.5f);
		}

		public static void mdct_init(mdct_lookup lookup, int n)
		{
			int[] array = new int[n / 4];
			float[] array2 = new float[n + n / 4];
			int num = n >> 1;
			int num2 = (lookup.log2n = rint((float)(Math.Log((float)n) / Math.Log(2.0))));
			lookup.n = n;
			lookup.trig = array2;
			lookup.bitrev = array;
			for (int i = 0; i < n / 4; i++)
			{
				array2[i * 2] = (float)Math.Cos(Math.PI / (double)n * (double)(4 * i));
				array2[i * 2 + 1] = (float)(0.0 - Math.Sin(Math.PI / (double)n * (double)(4 * i)));
				array2[num + i * 2] = (float)Math.Cos(Math.PI / (double)(2 * n) * (double)(2 * i + 1));
				array2[num + i * 2 + 1] = (float)Math.Sin(Math.PI / (double)(2 * n) * (double)(2 * i + 1));
			}
			for (int i = 0; i < n / 8; i++)
			{
				array2[n + i * 2] = (float)(Math.Cos(Math.PI / (double)n * (double)(4 * i + 2)) * 0.5);
				array2[n + i * 2 + 1] = (float)((0.0 - Math.Sin(Math.PI / (double)n * (double)(4 * i + 2))) * 0.5);
			}
			int num3 = (1 << num2 - 1) - 1;
			int num4 = 1 << num2 - 2;
			for (int i = 0; i < n / 8; i++)
			{
				int num5 = 0;
				for (int j = 0; num4 >> j != 0; j++)
				{
					if (((num4 >> j) & i) != 0)
					{
						num5 |= 1 << (j & 0x1F);
					}
				}
				array[i * 2] = (~num5 & num3) - 1;
				array[i * 2 + 1] = num5;
			}
			lookup.scale = 4f / (float)n;
		}

		public static void mdct_butterfly_8(CPtr.FloatPtr x)
		{
			float num = x.floats[x.offset + 6] + x.floats[x.offset + 2];
			float num2 = x.floats[x.offset + 6] - x.floats[x.offset + 2];
			float num3 = x.floats[x.offset + 4] + x.floats[x.offset];
			float num4 = x.floats[x.offset + 4] - x.floats[x.offset];
			x.floats[x.offset + 6] = num + num3;
			x.floats[x.offset + 4] = num - num3;
			num = x.floats[x.offset + 5] - x.floats[x.offset + 1];
			num3 = x.floats[x.offset + 7] - x.floats[x.offset + 3];
			x.floats[x.offset] = num2 + num;
			x.floats[x.offset + 2] = num2 - num;
			num = x.floats[x.offset + 5] + x.floats[x.offset + 1];
			num2 = x.floats[x.offset + 7] + x.floats[x.offset + 3];
			x.floats[x.offset + 3] = num3 + num4;
			x.floats[x.offset + 1] = num3 - num4;
			x.floats[x.offset + 7] = num2 + num;
			x.floats[x.offset + 5] = num2 - num;
		}

		public static void mdct_butterfly_16(CPtr.FloatPtr x)
		{
			float num = x.floats[x.offset + 1] - x.floats[x.offset + 9];
			float num2 = x.floats[x.offset] - x.floats[x.offset + 8];
			x.floats[x.offset + 8] += x.floats[x.offset];
			x.floats[x.offset + 9] += x.floats[x.offset + 1];
			x.floats[x.offset] = (num + num2) * cPI2_8;
			x.floats[x.offset + 1] = (num - num2) * cPI2_8;
			num = x.floats[x.offset + 3] - x.floats[x.offset + 11];
			num2 = x.floats[x.offset + 10] - x.floats[x.offset + 2];
			x.floats[x.offset + 10] += x.floats[x.offset + 2];
			x.floats[x.offset + 11] += x.floats[x.offset + 3];
			x.floats[x.offset + 2] = num;
			x.floats[x.offset + 3] = num2;
			num = x.floats[x.offset + 12] - x.floats[x.offset + 4];
			num2 = x.floats[x.offset + 13] - x.floats[x.offset + 5];
			x.floats[x.offset + 12] += x.floats[x.offset + 4];
			x.floats[x.offset + 13] += x.floats[x.offset + 5];
			x.floats[x.offset + 4] = (num - num2) * cPI2_8;
			x.floats[x.offset + 5] = (num + num2) * cPI2_8;
			num = x.floats[x.offset + 14] - x.floats[x.offset + 6];
			num2 = x.floats[x.offset + 15] - x.floats[x.offset + 7];
			x.floats[x.offset + 14] += x.floats[x.offset + 6];
			x.floats[x.offset + 15] += x.floats[x.offset + 7];
			x.floats[x.offset + 6] = num;
			x.floats[x.offset + 7] = num2;
			mdct_butterfly_8(x);
			mdct_butterfly_8(new CPtr.FloatPtr(x, 8));
		}

		public static void mdct_butterfly_32(CPtr.FloatPtr x)
		{
			float num = x.floats[x.offset + 30] - x.floats[x.offset + 14];
			float num2 = x.floats[x.offset + 31] - x.floats[x.offset + 15];
			x.floats[x.offset + 30] += x.floats[x.offset + 14];
			x.floats[x.offset + 31] += x.floats[x.offset + 15];
			x.floats[x.offset + 14] = num;
			x.floats[x.offset + 15] = num2;
			num = x.floats[x.offset + 28] - x.floats[x.offset + 12];
			num2 = x.floats[x.offset + 29] - x.floats[x.offset + 13];
			x.floats[x.offset + 28] += x.floats[x.offset + 12];
			x.floats[x.offset + 29] += x.floats[x.offset + 13];
			x.floats[x.offset + 12] = num * cPI1_8 - num2 * cPI3_8;
			x.floats[x.offset + 13] = num * cPI3_8 + num2 * cPI1_8;
			num = x.floats[x.offset + 26] - x.floats[x.offset + 10];
			num2 = x.floats[x.offset + 27] - x.floats[x.offset + 11];
			x.floats[x.offset + 26] += x.floats[x.offset + 10];
			x.floats[x.offset + 27] += x.floats[x.offset + 11];
			x.floats[x.offset + 10] = (num - num2) * cPI2_8;
			x.floats[x.offset + 11] = (num + num2) * cPI2_8;
			num = x.floats[x.offset + 24] - x.floats[x.offset + 8];
			num2 = x.floats[x.offset + 25] - x.floats[x.offset + 9];
			x.floats[x.offset + 24] += x.floats[x.offset + 8];
			x.floats[x.offset + 25] += x.floats[x.offset + 9];
			x.floats[x.offset + 8] = num * cPI3_8 - num2 * cPI1_8;
			x.floats[x.offset + 9] = num2 * cPI3_8 + num * cPI1_8;
			num = x.floats[x.offset + 22] - x.floats[x.offset + 6];
			num2 = x.floats[x.offset + 7] - x.floats[x.offset + 23];
			x.floats[x.offset + 22] += x.floats[x.offset + 6];
			x.floats[x.offset + 23] += x.floats[x.offset + 7];
			x.floats[x.offset + 6] = num2;
			x.floats[x.offset + 7] = num;
			num = x.floats[x.offset + 4] - x.floats[x.offset + 20];
			num2 = x.floats[x.offset + 5] - x.floats[x.offset + 21];
			x.floats[x.offset + 20] += x.floats[x.offset + 4];
			x.floats[x.offset + 21] += x.floats[x.offset + 5];
			x.floats[x.offset + 4] = num2 * cPI1_8 + num * cPI3_8;
			x.floats[x.offset + 5] = num2 * cPI3_8 - num * cPI1_8;
			num = x.floats[x.offset + 2] - x.floats[x.offset + 18];
			num2 = x.floats[x.offset + 3] - x.floats[x.offset + 19];
			x.floats[x.offset + 18] += x.floats[x.offset + 2];
			x.floats[x.offset + 19] += x.floats[x.offset + 3];
			x.floats[x.offset + 2] = (num2 + num) * cPI2_8;
			x.floats[x.offset + 3] = (num2 - num) * cPI2_8;
			num = x.floats[x.offset] - x.floats[x.offset + 16];
			num2 = x.floats[x.offset + 1] - x.floats[x.offset + 17];
			x.floats[x.offset + 16] += x.floats[x.offset];
			x.floats[x.offset + 17] += x.floats[x.offset + 1];
			x.floats[x.offset] = num2 * cPI3_8 + num * cPI1_8;
			x.floats[x.offset + 1] = num2 * cPI1_8 - num * cPI3_8;
			mdct_butterfly_16(x);
			mdct_butterfly_16(new CPtr.FloatPtr(x, 16));
		}

		public static void mdct_butterfly_first(float[] T, int Toffset, CPtr.FloatPtr x, int points)
		{
			float[] floats = x.floats;
			int num = x.offset + points - 8;
			float[] floats2 = x.floats;
			int num2 = x.offset + (points >> 1) - 8;
			do
			{
				float num3 = floats[num + 6] - floats2[num2 + 6];
				float num4 = floats[num + 7] - floats2[num2 + 7];
				floats[num + 6] += floats2[num2 + 6];
				floats[num + 7] += floats2[num2 + 7];
				floats2[num2 + 6] = num4 * T[Toffset + 1] + num3 * T[Toffset];
				floats2[num2 + 7] = num4 * T[Toffset] - num3 * T[Toffset + 1];
				num3 = floats[num + 4] - floats2[num2 + 4];
				num4 = floats[num + 5] - floats2[num2 + 5];
				floats[num + 4] += floats2[num2 + 4];
				floats[num + 5] += floats2[num2 + 5];
				floats2[num2 + 4] = num4 * T[Toffset + 5] + num3 * T[Toffset + 4];
				floats2[num2 + 5] = num4 * T[Toffset + 4] - num3 * T[Toffset + 5];
				num3 = floats[num + 2] - floats2[num2 + 2];
				num4 = floats[num + 3] - floats2[num2 + 3];
				floats[num + 2] += floats2[num2 + 2];
				floats[num + 3] += floats2[num2 + 3];
				floats2[num2 + 2] = num4 * T[Toffset + 9] + num3 * T[Toffset + 8];
				floats2[num2 + 3] = num4 * T[Toffset + 8] - num3 * T[Toffset + 9];
				num3 = floats[num] - floats2[num2];
				num4 = floats[num + 1] - floats2[num2 + 1];
				floats[num] += floats2[num2];
				floats[num + 1] += floats2[num2 + 1];
				floats2[num2] = num4 * T[Toffset + 13] + num3 * T[Toffset + 12];
				floats2[num2 + 1] = num4 * T[Toffset + 12] - num3 * T[Toffset + 13];
				num -= 8;
				num2 -= 8;
				Toffset += 16;
			}
			while (num2 >= x.offset);
		}

		public static void mdct_butterfly_generic(float[] T, int Toffset, CPtr.FloatPtr x, int points, int trigint)
		{
			float[] floats = x.floats;
			int num = x.offset + points - 8;
			float[] floats2 = x.floats;
			int num2 = x.offset + (points >> 1) - 8;
			do
			{
				float num3 = floats[num + 6] - floats2[num2 + 6];
				float num4 = floats[num + 7] - floats2[num2 + 7];
				floats[num + 6] += floats2[num2 + 6];
				floats[num + 7] += floats2[num2 + 7];
				floats2[num2 + 6] = num4 * T[Toffset + 1] + num3 * T[Toffset];
				floats2[num2 + 7] = num4 * T[Toffset] - num3 * T[Toffset + 1];
				Toffset += trigint;
				num3 = floats[num + 4] - floats2[num2 + 4];
				num4 = floats[num + 5] - floats2[num2 + 5];
				floats[num + 4] += floats2[num2 + 4];
				floats[num + 5] += floats2[num2 + 5];
				floats2[num2 + 4] = num4 * T[Toffset + 1] + num3 * T[Toffset];
				floats2[num2 + 5] = num4 * T[Toffset] - num3 * T[Toffset + 1];
				Toffset += trigint;
				num3 = floats[num + 2] - floats2[num2 + 2];
				num4 = floats[num + 3] - floats2[num2 + 3];
				floats[num + 2] += floats2[num2 + 2];
				floats[num + 3] += floats2[num2 + 3];
				floats2[num2 + 2] = num4 * T[Toffset + 1] + num3 * T[Toffset];
				floats2[num2 + 3] = num4 * T[Toffset] - num3 * T[Toffset + 1];
				Toffset += trigint;
				num3 = floats[num] - floats2[num2];
				num4 = floats[num + 1] - floats2[num2 + 1];
				floats[num] += floats2[num2];
				floats[num + 1] += floats2[num2 + 1];
				floats2[num2] = num4 * T[Toffset + 1] + num3 * T[Toffset];
				floats2[num2 + 1] = num4 * T[Toffset] - num3 * T[Toffset + 1];
				Toffset += trigint;
				num -= 8;
				num2 -= 8;
			}
			while (num2 >= x.offset);
		}

		public static void mdct_butterflies(mdct_lookup init, CPtr.FloatPtr x, int points)
		{
			float[] trig = init.trig;
			int num = init.log2n - 5;
			if (--num > 0)
			{
				mdct_butterfly_first(trig, 0, x, points);
			}
			int num2 = 1;
			while (--num > 0)
			{
				for (int i = 0; i < 1 << num2; i++)
				{
					mdct_butterfly_generic(trig, 0, new CPtr.FloatPtr(x, (points >> num2) * i), points >> num2, 4 << num2);
				}
				num2++;
			}
			for (int i = 0; i < points; i += 32)
			{
				mdct_butterfly_32(new CPtr.FloatPtr(x, i));
			}
		}

		public static void mdct_bitreverse(mdct_lookup init, CPtr.FloatPtr x)
		{
			int n = init.n;
			int[] bitrev = init.bitrev;
			int num = 0;
			float[] floats = x.floats;
			int num2 = x.offset;
			float[] floats2 = x.floats;
			int num3 = num2 + (n >> 1);
			int num4 = num3;
			float[] trig = init.trig;
			int num5 = n;
			do
			{
				float[] floats3 = x.floats;
				int num6 = num3 + bitrev[num];
				float[] floats4 = x.floats;
				int num7 = num3 + bitrev[num + 1];
				float num8 = floats3[num6 + 1] - floats4[num7 + 1];
				float num9 = floats3[num6] + floats4[num7];
				float num10 = num9 * trig[num5] + num8 * trig[num5 + 1];
				float num11 = num9 * trig[num5 + 1] - num8 * trig[num5];
				num4 -= 4;
				num8 = 0.5f * (floats3[num6 + 1] + floats4[num7 + 1]);
				num9 = 0.5f * (floats3[num6] - floats4[num7]);
				floats[num2] = num8 + num10;
				floats2[num4 + 2] = num8 - num10;
				floats[num2 + 1] = num9 + num11;
				floats2[num4 + 3] = num11 - num9;
				floats3 = x.floats;
				num6 = num3 + bitrev[num + 2];
				floats4 = x.floats;
				num7 = num3 + bitrev[num + 3];
				num8 = floats3[num6 + 1] - floats4[num7 + 1];
				num9 = floats3[num6] + floats4[num7];
				num10 = num9 * trig[num5 + 2] + num8 * trig[num5 + 3];
				num11 = num9 * trig[num5 + 3] - num8 * trig[num5 + 2];
				num8 = 0.5f * (floats3[num6 + 1] + floats4[num7 + 1]);
				num9 = 0.5f * (floats3[num6] - floats4[num7]);
				floats[num2 + 2] = num8 + num10;
				floats2[num4] = num8 - num10;
				floats[num2 + 3] = num9 + num11;
				floats2[num4 + 1] = num11 - num9;
				num5 += 4;
				num += 4;
				num2 += 4;
			}
			while (num2 < num4);
		}

		public static void mdct_backward(mdct_lookup init, CPtr.FloatPtr pin, CPtr.FloatPtr pout)
		{
			int n = init.n;
			int num = n >> 1;
			int num2 = n >> 2;
			float[] floats = pin.floats;
			int num3 = pin.offset + num - 7;
			float[] floats2 = pout.floats;
			int num4 = pout.offset + num + num2;
			float[] trig = init.trig;
			int num5 = num2;
			do
			{
				num4 -= 4;
				floats2[num4] = (0f - floats[num3 + 2]) * trig[num5 + 3] - floats[num3] * trig[num5 + 2];
				floats2[num4 + 1] = floats[num3] * trig[num5 + 3] - floats[num3 + 2] * trig[num5 + 2];
				floats2[num4 + 2] = (0f - floats[num3 + 6]) * trig[num5 + 1] - floats[num3 + 4] * trig[num5];
				floats2[num4 + 3] = floats[num3 + 4] * trig[num5 + 1] - floats[num3 + 6] * trig[num5];
				num3 -= 8;
				num5 += 4;
			}
			while (num3 >= pin.offset);
			floats = pin.floats;
			num3 = pin.offset + num - 8;
			floats2 = pout.floats;
			num4 = pout.offset + num + num2;
			trig = init.trig;
			num5 = num2;
			do
			{
				num5 -= 4;
				floats2[num4] = floats[num3 + 4] * trig[num5 + 3] + floats[num3 + 6] * trig[num5 + 2];
				floats2[num4 + 1] = floats[num3 + 4] * trig[num5 + 2] - floats[num3 + 6] * trig[num5 + 3];
				floats2[num4 + 2] = floats[num3] * trig[num5 + 1] + floats[num3 + 2] * trig[num5];
				floats2[num4 + 3] = floats[num3] * trig[num5] - floats[num3 + 2] * trig[num5 + 1];
				num3 -= 8;
				num4 += 4;
			}
			while (num3 >= pin.offset);
			mdct_butterflies(init, new CPtr.FloatPtr(pout, num), num);
			mdct_bitreverse(init, pout);
			float[] floats3 = pout.floats;
			int num6 = pout.offset + num + num2;
			float[] floats4 = pout.floats;
			int num7 = pout.offset + num + num2;
			floats = pout.floats;
			num3 = pout.offset;
			trig = init.trig;
			num5 = num;
			do
			{
				num6 -= 4;
				floats3[num6 + 3] = floats[num3] * trig[num5 + 1] - floats[num3 + 1] * trig[num5];
				floats4[num7] = 0f - (floats[num3] * trig[num5] + floats[num3 + 1] * trig[num5 + 1]);
				floats3[num6 + 2] = floats[num3 + 2] * trig[num5 + 3] - floats[num3 + 3] * trig[num5 + 2];
				floats4[num7 + 1] = 0f - (floats[num3 + 2] * trig[num5 + 2] + floats[num3 + 3] * trig[num5 + 3]);
				floats3[num6 + 1] = floats[num3 + 4] * trig[num5 + 5] - floats[num3 + 5] * trig[num5 + 4];
				floats4[num7 + 2] = 0f - (floats[num3 + 4] * trig[num5 + 4] + floats[num3 + 5] * trig[num5 + 5]);
				floats3[num6] = floats[num3 + 6] * trig[num5 + 7] - floats[num3 + 7] * trig[num5 + 6];
				floats4[num7 + 3] = 0f - (floats[num3 + 6] * trig[num5 + 6] + floats[num3 + 7] * trig[num5 + 7]);
				num7 += 4;
				num3 += 8;
				num5 += 8;
			}
			while (num3 < num6);
			floats = pout.floats;
			num3 = pout.offset + num + num2;
			floats3 = pout.floats;
			num6 = pout.offset + num2;
			floats4 = floats3;
			num7 = num6;
			do
			{
				num6 -= 4;
				num3 -= 4;
				floats4[num7] = 0f - (floats3[num6 + 3] = floats[num3 + 3]);
				floats4[num7 + 1] = 0f - (floats3[num6 + 2] = floats[num3 + 2]);
				floats4[num7 + 2] = 0f - (floats3[num6 + 1] = floats[num3 + 1]);
				floats4[num7 + 3] = 0f - (floats3[num6] = floats[num3]);
				num7 += 4;
			}
			while (num7 < num3);
			floats = pout.floats;
			num3 = pout.offset + num + num2;
			floats3 = pout.floats;
			num6 = pout.offset + num + num2;
			num7 = pout.offset + num;
			do
			{
				num6 -= 4;
				floats3[num6] = floats[num3 + 3];
				floats3[num6 + 1] = floats[num3 + 2];
				floats3[num6 + 2] = floats[num3 + 1];
				floats3[num6 + 3] = floats[num3];
				num3 += 4;
			}
			while (num6 > num7);
		}
	}
}
