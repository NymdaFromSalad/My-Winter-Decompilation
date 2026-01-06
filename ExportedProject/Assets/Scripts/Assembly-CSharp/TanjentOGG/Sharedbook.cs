using System;
using System.Collections.Generic;

namespace TanjentOGG
{
	public class Sharedbook
	{
		private class codepComparator : IComparer<int>
		{
			public long[] codes;

			public int Compare(int x, int y)
			{
				long num = codes[x];
				long num2 = codes[y];
				int num3 = ((num > num2) ? 1 : 0);
				int num4 = ((num < num2) ? 1 : 0);
				return num3 - num4;
			}
		}

		private static int VQ_FEXP = 10;

		private static int VQ_FMAN = 21;

		private static int VQ_FEXP_BIAS = 768;

		public static int _ilog(int v)
		{
			int num = 0;
			while (v != 0)
			{
				num++;
				v >>= 1;
			}
			return num;
		}

		private static double ldexp(double x, int exp)
		{
			return x * Math.Pow(2.0, exp);
		}

		private static float _float32_unpack(long val)
		{
			double num = val & 0x1FFFFF;
			int num2 = (int)(val & 0x80000000u);
			long num3 = (val & 0x7FE00000) >> VQ_FMAN;
			if (num2 != 0)
			{
				num = 0.0 - num;
			}
			return (float)ldexp(num, (int)(num3 - (VQ_FMAN - 1) - VQ_FEXP_BIAS));
		}

		public static long[] _make_words(byte[] l, long n, long sparsecount)
		{
			long num = 0L;
			long[] array = new long[33];
			int num2 = (int)((sparsecount == 0L) ? n : sparsecount);
			long[] array2 = new long[num2];
			long num3;
			for (num3 = 0L; num3 < n; num3++)
			{
				long num4 = (int)l[(int)num3];
				if (num4 > 0)
				{
					long num5 = array[(int)num4];
					if (num4 < 32 && num5 >> (int)num4 != 0L)
					{
						return null;
					}
					array2[(int)num++] = num5;
					for (long num6 = num4; num6 > 0; num6--)
					{
						if ((array[(int)num6] & 1) != 0L)
						{
							if (num6 == 1)
							{
								array[1]++;
							}
							else
							{
								array[(int)num6] = array[(int)(num6 - 1)] << 1;
							}
							break;
						}
						array[(int)num6]++;
					}
					for (long num6 = num4 + 1; num6 < 33 && array[(int)num6] >> 1 == num5; num6++)
					{
						num5 = array[(int)num6];
						array[(int)num6] = array[(int)(num6 - 1)] << 1;
					}
				}
				else if (sparsecount == 0L)
				{
					num++;
				}
			}
			if (sparsecount != 1)
			{
				for (num3 = 1L; num3 < 33; num3++)
				{
					if ((array[(int)num3] & (4294967295L >> 32 - (int)num3)) != 0L)
					{
						return null;
					}
				}
			}
			num3 = 0L;
			num = 0L;
			for (; num3 < n; num3++)
			{
				int num7 = 0;
				for (long num6 = 0L; num6 < (int)l[(int)num3]; num6++)
				{
					num7 <<= 1;
					num7 |= (int)(array2[(int)num] >> (int)num6) & 1;
				}
				if (sparsecount != 0L)
				{
					if (l[(int)num3] != 0)
					{
						array2[(int)num++] = num7;
					}
				}
				else
				{
					array2[(int)num++] = num7;
				}
			}
			return array2;
		}

		public static long _book_maptype1_quantvals(Codebook.static_codebook b)
		{
			long num = (long)Math.Floor(Math.Pow((float)b.entries, 1f / (float)b.dim));
			while (true)
			{
				long num2 = 1L;
				long num3 = 1L;
				for (int i = 0; i < b.dim; i++)
				{
					num2 *= num;
					num3 *= num + 1;
				}
				if (num2 <= b.entries && num3 > b.entries)
				{
					break;
				}
				num = ((num2 <= b.entries) ? (num + 1) : (num - 1));
			}
			return num;
		}

		public static float[] _book_unquantize(Codebook.static_codebook b, int n, int[] sparsemap)
		{
			long num = 0L;
			if (b.maptype == 1 || b.maptype == 2)
			{
				float num2 = _float32_unpack(b.q_min);
				float num3 = _float32_unpack(b.q_delta);
				float[] array = new float[(int)(n * b.dim)];
				switch (b.maptype)
				{
				case 1:
				{
					int num7 = (int)_book_maptype1_quantvals(b);
					for (long num4 = 0L; num4 < b.entries; num4++)
					{
						if ((sparsemap == null || b.lengthlist[(int)num4] == 0) && sparsemap != null)
						{
							continue;
						}
						float num8 = 0f;
						int num9 = 1;
						for (long num6 = 0L; num6 < b.dim; num6++)
						{
							int num10 = (int)(num4 / num9 % num7);
							float value2 = b.quantlist[num10];
							value2 = Math.Abs(value2) * num3 + num2 + num8;
							if (b.q_sequencep != 0)
							{
								num8 = value2;
							}
							if (sparsemap != null)
							{
								array[(int)(sparsemap[(int)num] * b.dim + num6)] = value2;
							}
							else
							{
								array[(int)(num * b.dim + num6)] = value2;
							}
							num9 *= num7;
						}
						num++;
					}
					break;
				}
				case 2:
				{
					for (long num4 = 0L; num4 < b.entries; num4++)
					{
						if ((sparsemap == null || b.lengthlist[(int)num4] == 0) && sparsemap != null)
						{
							continue;
						}
						float num5 = 0f;
						for (long num6 = 0L; num6 < b.dim; num6++)
						{
							float value = b.quantlist[(int)(num4 * b.dim + num6)];
							value = Math.Abs(value) * num3 + num2 + num5;
							if (b.q_sequencep != 0)
							{
								num5 = value;
							}
							if (sparsemap != null)
							{
								array[(int)(sparsemap[(int)num] * b.dim + num6)] = value;
							}
							else
							{
								array[(int)(num * b.dim + num6)] = value;
							}
						}
						num++;
					}
					break;
				}
				}
				return array;
			}
			return null;
		}

		private static long bitreverse(long x)
		{
			x = ((x >> 16) & 0xFFFF) | ((x << 16) & 0xFFFF0000u);
			x = ((x >> 8) & 0xFF00FF) | ((x << 8) & 0xFF00FF00u);
			x = ((x >> 4) & 0xF0F0F0F) | ((x << 4) & 0xF0F0F0F0u);
			x = ((x >> 2) & 0x33333333) | ((x << 2) & 0xCCCCCCCCu);
			return ((x >> 1) & 0x55555555) | ((x << 1) & 0xAAAAAAAAu);
		}

		public static int vorbis_book_init_decode(Codebook.codebook c, Codebook.static_codebook s)
		{
			int num = 0;
			c.clear();
			for (int i = 0; i < s.entries; i++)
			{
				if (s.lengthlist[i] > 0)
				{
					num++;
				}
			}
			c.entries = s.entries;
			c.used_entries = num;
			c.dim = s.dim;
			if (num > 0)
			{
				long[] array = _make_words(s.lengthlist, s.entries, c.used_entries);
				if (array == null)
				{
					return -1;
				}
				int[] array2 = new int[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = bitreverse(array[i]);
					array2[i] = i;
				}
				codepComparator codepComparator = new codepComparator();
				codepComparator.codes = array;
				Array.Sort(array2, 0, num, codepComparator);
				int[] array3 = new int[num];
				c.codelist = new long[num];
				for (int i = 0; i < num; i++)
				{
					int num2 = array2[i];
					array3[num2] = i;
				}
				for (int i = 0; i < num; i++)
				{
					c.codelist[array3[i]] = array[i];
				}
				c.valuelist = _book_unquantize(s, num, array3);
				c.dec_index = new int[num];
				num = 0;
				for (int i = 0; i < s.entries; i++)
				{
					if (s.lengthlist[i] > 0)
					{
						c.dec_index[array3[num++]] = i;
					}
				}
				c.dec_codelengths = new byte[num];
				num = 0;
				for (int i = 0; i < s.entries; i++)
				{
					if (s.lengthlist[i] > 0)
					{
						c.dec_codelengths[array3[num++]] = s.lengthlist[i];
					}
				}
				c.dec_firsttablen = _ilog((int)c.used_entries) - 4;
				if (c.dec_firsttablen < 5)
				{
					c.dec_firsttablen = 5;
				}
				if (c.dec_firsttablen > 8)
				{
					c.dec_firsttablen = 8;
				}
				int num3 = 1 << c.dec_firsttablen;
				c.dec_firsttable = new long[num3];
				c.dec_maxlength = 0;
				for (int i = 0; i < num; i++)
				{
					if (c.dec_maxlength < c.dec_codelengths[i])
					{
						c.dec_maxlength = c.dec_codelengths[i];
					}
					if (c.dec_codelengths[i] <= c.dec_firsttablen)
					{
						long num4 = bitreverse(c.codelist[i]);
						for (int j = 0; j < 1 << c.dec_firsttablen - c.dec_codelengths[i]; j++)
						{
							c.dec_firsttable[(int)(num4 | (j << (int)c.dec_codelengths[i]))] = i + 1;
						}
					}
				}
				long num5 = (long)(4294967294uL << 31 - c.dec_firsttablen);
				num5 &= 0xFFFFFFFFu;
				long num6 = 0L;
				long num7 = 0L;
				for (int i = 0; i < num3; i++)
				{
					long num8 = (long)i << 32 - c.dec_firsttablen;
					if (c.dec_firsttable[(int)bitreverse(num8)] == 0L)
					{
						for (; num6 + 1 < num && c.codelist[(int)(num6 + 1)] <= num8; num6++)
						{
						}
						for (; num7 < num && num8 >= (c.codelist[(int)num7] & num5); num7++)
						{
						}
						long num9 = num6;
						long num10 = num - num7;
						if (num9 > 32767)
						{
							num9 = 32767L;
						}
						if (num10 > 32767)
						{
							num10 = 32767L;
						}
						c.dec_firsttable[(int)bitreverse(num8)] = 0x80000000u | (num9 << 15) | num10;
					}
				}
			}
			return 0;
		}
	}
}
