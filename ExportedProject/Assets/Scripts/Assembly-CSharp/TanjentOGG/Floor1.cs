using System;
using System.Collections.Generic;

namespace TanjentOGG
{
	public class Floor1
	{
		public class vorbis_info_floor1 : Codec.vorbis_info_floor
		{
			public int partitions;

			public int[] partitionclass = new int[VIF_PARTS];

			public int[] class_dim = new int[VIF_CLASS];

			public int[] class_subs = new int[VIF_CLASS];

			public int[] class_book = new int[VIF_CLASS];

			public int[,] class_subbook = new int[VIF_CLASS, 8];

			public int mult;

			public int[] postlist = new int[VIF_POSIT + 2];

			public float maxover;

			public float maxunder;

			public float maxerr;

			public float twofitweight;

			public float twofitatten;

			public int n;
		}

		public class vorbis_look_floor1 : Codec.vorbis_look_floor
		{
			public int[] sorted_index = new int[VIF_POSIT + 2];

			public int[] forward_index = new int[VIF_POSIT + 2];

			public int[] reverse_index = new int[VIF_POSIT + 2];

			public int[] hineighbor = new int[VIF_POSIT];

			public int[] loneighbor = new int[VIF_POSIT];

			public int posts;

			public int n;

			public int quant_q;

			public vorbis_info_floor1 vi;

			public long phrasebits;

			public long postbits;

			public long frames;
		}

		private class floor1_look_sortpointerComparator : IComparer<int>
		{
			public int[] valueList;

			public int Compare(int x, int y)
			{
				return valueList[x].CompareTo(valueList[y]);
			}
		}

		private static int VIF_POSIT = 63;

		private static int VIF_CLASS = 16;

		private static int VIF_PARTS = 31;

		private static float[] FLOOR1_fromdB_LOOKUP = new float[256]
		{
			1.0649863E-07f, 1.1341951E-07f, 1.2079015E-07f, 1.2863978E-07f, 1.369995E-07f, 1.459025E-07f, 1.5538409E-07f, 1.6548181E-07f, 1.7623574E-07f, 1.8768856E-07f,
			1.998856E-07f, 2.128753E-07f, 2.2670913E-07f, 2.4144197E-07f, 2.5713223E-07f, 2.7384212E-07f, 2.9163792E-07f, 3.1059022E-07f, 3.307741E-07f, 3.5226967E-07f,
			3.7516213E-07f, 3.995423E-07f, 4.255068E-07f, 4.5315863E-07f, 4.8260745E-07f, 5.1397E-07f, 5.4737063E-07f, 5.829419E-07f, 6.208247E-07f, 6.611694E-07f,
			7.041359E-07f, 7.4989464E-07f, 7.98627E-07f, 8.505263E-07f, 9.057983E-07f, 9.646621E-07f, 1.0273513E-06f, 1.0941144E-06f, 1.1652161E-06f, 1.2409384E-06f,
			1.3215816E-06f, 1.4074654E-06f, 1.4989305E-06f, 1.5963394E-06f, 1.7000785E-06f, 1.8105592E-06f, 1.9282195E-06f, 2.053526E-06f, 2.1869757E-06f, 2.3290977E-06f,
			2.4804558E-06f, 2.6416496E-06f, 2.813319E-06f, 2.9961443E-06f, 3.1908505E-06f, 3.39821E-06f, 3.619045E-06f, 3.8542307E-06f, 4.1047006E-06f, 4.371447E-06f,
			4.6555283E-06f, 4.958071E-06f, 5.280274E-06f, 5.623416E-06f, 5.988857E-06f, 6.3780467E-06f, 6.7925284E-06f, 7.2339453E-06f, 7.704048E-06f, 8.2047E-06f,
			8.737888E-06f, 9.305725E-06f, 9.910464E-06f, 1.0554501E-05f, 1.1240392E-05f, 1.1970856E-05f, 1.2748789E-05f, 1.3577278E-05f, 1.4459606E-05f, 1.5399271E-05f,
			1.6400005E-05f, 1.7465769E-05f, 1.8600793E-05f, 1.9809577E-05f, 2.1096914E-05f, 2.2467912E-05f, 2.3928002E-05f, 2.5482977E-05f, 2.7139005E-05f, 2.890265E-05f,
			3.078091E-05f, 3.2781227E-05f, 3.4911533E-05f, 3.718028E-05f, 3.9596467E-05f, 4.2169668E-05f, 4.491009E-05f, 4.7828602E-05f, 5.0936775E-05f, 5.424693E-05f,
			5.7772202E-05f, 6.152657E-05f, 6.552491E-05f, 6.9783084E-05f, 7.4317984E-05f, 7.914758E-05f, 8.429104E-05f, 8.976875E-05f, 9.560242E-05f, 0.00010181521f,
			0.00010843174f, 0.00011547824f, 0.00012298267f, 0.00013097477f, 0.00013948625f, 0.00014855085f, 0.00015820454f, 0.00016848555f, 0.00017943469f, 0.00019109536f,
			0.00020351382f, 0.0002167393f, 0.00023082423f, 0.00024582449f, 0.00026179955f, 0.00027881275f, 0.00029693157f, 0.00031622787f, 0.00033677815f, 0.00035866388f,
			0.00038197188f, 0.00040679457f, 0.00043323037f, 0.0004613841f, 0.0004913675f, 0.00052329927f, 0.0005573062f, 0.0005935231f, 0.0006320936f, 0.0006731706f,
			0.000716917f, 0.0007635063f, 0.00081312325f, 0.00086596457f, 0.00092223985f, 0.0009821722f, 0.0010459992f, 0.0011139743f, 0.0011863665f, 0.0012634633f,
			0.0013455702f, 0.0014330129f, 0.0015261382f, 0.0016253153f, 0.0017309374f, 0.0018434235f, 0.0019632196f, 0.0020908006f, 0.0022266726f, 0.0023713743f,
			0.0025254795f, 0.0026895993f, 0.0028643848f, 0.0030505287f, 0.003248769f, 0.0034598925f, 0.0036847359f, 0.0039241905f, 0.0041792067f, 0.004450795f,
			0.004740033f, 0.005048067f, 0.0053761187f, 0.005725489f, 0.0060975635f, 0.0064938175f, 0.0069158226f, 0.0073652514f, 0.007843887f, 0.008353627f,
			0.008896492f, 0.009474637f, 0.010090352f, 0.01074608f, 0.011444421f, 0.012188144f, 0.012980198f, 0.013823725f, 0.014722068f, 0.015678791f,
			0.016697686f, 0.017782796f, 0.018938422f, 0.020169148f, 0.021479854f, 0.022875736f, 0.02436233f, 0.025945531f, 0.027631618f, 0.029427277f,
			0.031339627f, 0.03337625f, 0.035545226f, 0.037855156f, 0.0403152f, 0.042935107f, 0.045725275f, 0.048696756f, 0.05186135f, 0.05523159f,
			0.05882085f, 0.062643364f, 0.06671428f, 0.07104975f, 0.075666964f, 0.08058423f, 0.08582105f, 0.09139818f, 0.097337745f, 0.1036633f,
			0.11039993f, 0.11757434f, 0.12521498f, 0.13335215f, 0.14201812f, 0.15124726f, 0.16107617f, 0.1715438f, 0.18269168f, 0.19456401f,
			0.20720787f, 0.22067343f, 0.23501402f, 0.25028655f, 0.26655158f, 0.28387362f, 0.3023213f, 0.32196787f, 0.34289113f, 0.36517414f,
			0.3889052f, 0.41417846f, 0.44109413f, 0.4697589f, 0.50028646f, 0.53279793f, 0.5674221f, 0.6042964f, 0.64356697f, 0.6853896f,
			0.72993004f, 0.777365f, 0.8278826f, 0.88168305f, 0.9389798f, 1f
		};

		public static int ilog(int v)
		{
			int num = 0;
			while (v != 0)
			{
				num++;
				v >>= 1;
			}
			return num;
		}

		private static int ilog2(int v)
		{
			int num = 0;
			if (v != 0)
			{
				v--;
			}
			while (v != 0)
			{
				num++;
				v >>= 1;
			}
			return num;
		}

		public static Codec.vorbis_info_floor floor1_unpack(Codec.vorbis_info vi, Ogg.oggpack_buffer opb)
		{
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			int num = 0;
			int num2 = -1;
			vorbis_info_floor1 vorbis_info_floor = new vorbis_info_floor1();
			vorbis_info_floor.partitions = (int)Bitwise.oggpack_read(opb, 5);
			int i;
			for (i = 0; i < vorbis_info_floor.partitions; i++)
			{
				vorbis_info_floor.partitionclass[i] = (int)Bitwise.oggpack_read(opb, 4);
				if (vorbis_info_floor.partitionclass[i] < 0)
				{
					return null;
				}
				if (num2 < vorbis_info_floor.partitionclass[i])
				{
					num2 = vorbis_info_floor.partitionclass[i];
				}
			}
			int j;
			for (i = 0; i < num2 + 1; i++)
			{
				vorbis_info_floor.class_dim[i] = (int)(Bitwise.oggpack_read(opb, 3) + 1);
				vorbis_info_floor.class_subs[i] = (int)Bitwise.oggpack_read(opb, 2);
				if (vorbis_info_floor.class_subs[i] < 0)
				{
					return null;
				}
				if (vorbis_info_floor.class_subs[i] != 0)
				{
					vorbis_info_floor.class_book[i] = (int)Bitwise.oggpack_read(opb, 8);
				}
				if (vorbis_info_floor.class_book[i] < 0 || vorbis_info_floor.class_book[i] >= codec_setup.books)
				{
					return null;
				}
				for (j = 0; j < 1 << vorbis_info_floor.class_subs[i]; j++)
				{
					vorbis_info_floor.class_subbook[i, j] = (int)(Bitwise.oggpack_read(opb, 8) - 1);
					if (vorbis_info_floor.class_subbook[i, j] < -1 || vorbis_info_floor.class_subbook[i, j] >= codec_setup.books)
					{
						return null;
					}
				}
			}
			vorbis_info_floor.mult = (int)(Bitwise.oggpack_read(opb, 2) + 1);
			int num3 = (int)Bitwise.oggpack_read(opb, 4);
			if (num3 < 0)
			{
				return null;
			}
			i = 0;
			j = 0;
			for (; i < vorbis_info_floor.partitions; i++)
			{
				num += vorbis_info_floor.class_dim[vorbis_info_floor.partitionclass[i]];
				if (num > VIF_POSIT)
				{
					return null;
				}
				for (; j < num; j++)
				{
					int num4 = (vorbis_info_floor.postlist[j + 2] = (int)Bitwise.oggpack_read(opb, num3));
					if (num4 < 0 || num4 >= 1 << num3)
					{
						return null;
					}
				}
			}
			vorbis_info_floor.postlist[0] = 0;
			vorbis_info_floor.postlist[1] = 1 << num3;
			int[] array = new int[num + 2];
			for (i = 0; i < num + 2; i++)
			{
				array[i] = i;
			}
			floor1_look_sortpointerComparator floor1_look_sortpointerComparator = new floor1_look_sortpointerComparator();
			floor1_look_sortpointerComparator.valueList = vorbis_info_floor.postlist;
			Array.Sort(array, 0, num + 2, floor1_look_sortpointerComparator);
			for (i = 1; i < num + 2; i++)
			{
				if (vorbis_info_floor.postlist[array[i - 1]] == vorbis_info_floor.postlist[array[i]])
				{
					return null;
				}
			}
			return vorbis_info_floor;
		}

		public static Codec.vorbis_look_floor floor1_look(Codec.vorbis_dsp_state vd, Codec.vorbis_info_floor pin)
		{
			int[] array = new int[VIF_POSIT + 2];
			vorbis_info_floor1 vorbis_info_floor = (vorbis_info_floor1)pin;
			vorbis_look_floor1 vorbis_look_floor = new vorbis_look_floor1();
			int num = 0;
			vorbis_look_floor.vi = vorbis_info_floor;
			vorbis_look_floor.n = vorbis_info_floor.postlist[1];
			for (int i = 0; i < vorbis_info_floor.partitions; i++)
			{
				num += vorbis_info_floor.class_dim[vorbis_info_floor.partitionclass[i]];
			}
			num = (vorbis_look_floor.posts = num + 2);
			for (int i = 0; i < num; i++)
			{
				array[i] = i;
			}
			floor1_look_sortpointerComparator floor1_look_sortpointerComparator = new floor1_look_sortpointerComparator();
			floor1_look_sortpointerComparator.valueList = vorbis_info_floor.postlist;
			Array.Sort(array, 0, num, floor1_look_sortpointerComparator);
			for (int i = 0; i < num; i++)
			{
				vorbis_look_floor.forward_index[i] = array[i];
			}
			for (int i = 0; i < num; i++)
			{
				vorbis_look_floor.reverse_index[vorbis_look_floor.forward_index[i]] = i;
			}
			for (int i = 0; i < num; i++)
			{
				vorbis_look_floor.sorted_index[i] = vorbis_info_floor.postlist[vorbis_look_floor.forward_index[i]];
			}
			switch (vorbis_info_floor.mult)
			{
			case 1:
				vorbis_look_floor.quant_q = 256;
				break;
			case 2:
				vorbis_look_floor.quant_q = 128;
				break;
			case 3:
				vorbis_look_floor.quant_q = 86;
				break;
			case 4:
				vorbis_look_floor.quant_q = 64;
				break;
			}
			for (int i = 0; i < num - 2; i++)
			{
				int num2 = 0;
				int num3 = 1;
				int num4 = 0;
				int num5 = vorbis_look_floor.n;
				int num6 = vorbis_info_floor.postlist[i + 2];
				for (int j = 0; j < i + 2; j++)
				{
					int num7 = vorbis_info_floor.postlist[j];
					if (num7 > num4 && num7 < num6)
					{
						num2 = j;
						num4 = num7;
					}
					if (num7 < num5 && num7 > num6)
					{
						num3 = j;
						num5 = num7;
					}
				}
				vorbis_look_floor.loneighbor[i] = num2;
				vorbis_look_floor.hineighbor[i] = num3;
			}
			return vorbis_look_floor;
		}

		private static int render_point(int x0, int x1, int y0, int y1, int x)
		{
			y0 &= 0x7FFF;
			y1 &= 0x7FFF;
			int num = y1 - y0;
			int num2 = x1 - x0;
			int num3 = Math.Abs(num);
			int num4 = num3 * (x - x0);
			int num5 = num4 / num2;
			if (num < 0)
			{
				return y0 - num5;
			}
			return y0 + num5;
		}

		private static void render_line(int n, int x0, int x1, int y0, int y1, CPtr.FloatPtr d)
		{
			int num = y1 - y0;
			int num2 = x1 - x0;
			int num3 = Math.Abs(num);
			int num4 = num / num2;
			int num5 = ((num >= 0) ? (num4 + 1) : (num4 - 1));
			int num6 = x0;
			int num7 = y0;
			int num8 = 0;
			num3 -= Math.Abs(num4 * num2);
			if (n > x1)
			{
				n = x1;
			}
			if (num6 < n)
			{
				d.floats[d.offset + num6] *= FLOOR1_fromdB_LOOKUP[num7];
			}
			while (++num6 < n)
			{
				num8 += num3;
				if (num8 >= num2)
				{
					num8 -= num2;
					num7 += num5;
				}
				else
				{
					num7 += num4;
				}
				d.floats[d.offset + num6] *= FLOOR1_fromdB_LOOKUP[num7];
			}
		}

		public static float[] floor1_inverse1(Codec.vorbis_block vb, Codec.vorbis_look_floor pin)
		{
			vorbis_look_floor1 vorbis_look_floor = (vorbis_look_floor1)pin;
			vorbis_info_floor1 vi = vorbis_look_floor.vi;
			Codec.codec_setup_info codec_setup = vb.vd.vi.codec_setup;
			Codebook.codebook[] fullbooks = codec_setup.fullbooks;
			if (Bitwise.oggpack_read(vb.opb, 1) == 1)
			{
				int[] array = new int[vorbis_look_floor.posts];
				array[0] = (int)Bitwise.oggpack_read(vb.opb, ilog(vorbis_look_floor.quant_q - 1));
				array[1] = (int)Bitwise.oggpack_read(vb.opb, ilog(vorbis_look_floor.quant_q - 1));
				int i = 0;
				int num = 2;
				for (; i < vi.partitions; i++)
				{
					int num2 = vi.partitionclass[i];
					int num3 = vi.class_dim[num2];
					int num4 = vi.class_subs[num2];
					int num5 = 1 << num4;
					int num6 = 0;
					if (num4 != 0)
					{
						num6 = (int)Codebook.vorbis_book_decode(fullbooks[vi.class_book[num2]], vb.opb);
						if (num6 == -1)
						{
							return null;
						}
					}
					for (int j = 0; j < num3; j++)
					{
						int num7 = vi.class_subbook[num2, num6 & (num5 - 1)];
						num6 >>= num4;
						if (num7 >= 0)
						{
							if ((array[num + j] = (int)Codebook.vorbis_book_decode(fullbooks[num7], vb.opb)) == -1)
							{
								return null;
							}
						}
						else
						{
							array[num + j] = 0;
						}
					}
					num += num3;
				}
				for (i = 2; i < vorbis_look_floor.posts; i++)
				{
					int num8 = render_point(vi.postlist[vorbis_look_floor.loneighbor[i - 2]], vi.postlist[vorbis_look_floor.hineighbor[i - 2]], array[vorbis_look_floor.loneighbor[i - 2]], array[vorbis_look_floor.hineighbor[i - 2]], vi.postlist[i]);
					int num9 = vorbis_look_floor.quant_q - num8;
					int num10 = num8;
					int num11 = ((num9 >= num10) ? num10 : num9) << 1;
					int num12 = array[i];
					if (num12 != 0)
					{
						num12 = ((num12 >= num11) ? ((num9 <= num10) ? (-1 - (num12 - num9)) : (num12 - num10)) : (((num12 & 1) == 0) ? (num12 >> 1) : (-(num12 + 1 >> 1))));
						array[i] = (num12 + num8) & 0x7FFF;
						array[vorbis_look_floor.loneighbor[i - 2]] &= 32767;
						array[vorbis_look_floor.hineighbor[i - 2]] &= 32767;
					}
					else
					{
						array[i] = num8 | 0x8000;
					}
				}
				float[] array2 = new float[array.Length];
				for (int k = 0; k < array.Length; k++)
				{
					array2[k] = array[k];
				}
				return array2;
			}
			return null;
		}

		public static int floor1_inverse2(Codec.vorbis_block vb, Codec.vorbis_look_floor pin, float[] memo, CPtr.FloatPtr pout)
		{
			vorbis_look_floor1 vorbis_look_floor = (vorbis_look_floor1)pin;
			vorbis_info_floor1 vi = vorbis_look_floor.vi;
			Codec.codec_setup_info codec_setup = vb.vd.vi.codec_setup;
			int num = (int)(codec_setup.blocksizes[(int)vb.W] / 2);
			if (memo != null)
			{
				int[] array = new int[memo.Length];
				for (int i = 0; i < memo.Length; i++)
				{
					array[i] = (int)memo[i];
				}
				int num2 = 0;
				int x = 0;
				int num3 = array[0] * vi.mult;
				num3 = ((num3 >= 0) ? ((num3 <= 255) ? num3 : 255) : 0);
				for (int j = 1; j < vorbis_look_floor.posts; j++)
				{
					int num4 = vorbis_look_floor.forward_index[j];
					int num5 = array[num4] & 0x7FFF;
					if (num5 == array[num4])
					{
						num2 = vi.postlist[num4];
						num5 *= vi.mult;
						num5 = ((num5 >= 0) ? ((num5 <= 255) ? num5 : 255) : 0);
						render_line(num, x, num2, num3, num5, pout);
						x = num2;
						num3 = num5;
					}
				}
				for (int j = num2; j < num; j++)
				{
					pout.floats[pout.offset + j] *= FLOOR1_fromdB_LOOKUP[num3];
				}
				return 1;
			}
			CPtr.FloatPtr.memset(pout, 0, pout.floats.Length - pout.offset);
			return 0;
		}
	}
}
