using System;

namespace TanjentOGG
{
	public class Floor0
	{
		public class vorbis_look_floor0 : Codec.vorbis_look_floor
		{
			public int ln;

			public int m;

			public int[][] linearmap;

			public int[] n = new int[2];

			public vorbis_info_floor0 vi;
		}

		public class vorbis_info_floor0 : Codec.vorbis_info_floor
		{
			public int order;

			public long rate;

			public long barkmap;

			public int ampbits;

			public int ampdB;

			public int numbooks;

			public int[] books = new int[16];
		}

		private static float toBARK(float n)
		{
			return (float)(13.100000381469727 * Math.Atan(0.00074f * n) + 2.240000009536743 * Math.Atan(n * n * 1.85E-08f) + (double)(0.0001f * n));
		}

		public static Codec.vorbis_info_floor floor0_unpack(Codec.vorbis_info vi, Ogg.oggpack_buffer opb)
		{
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			vorbis_info_floor0 vorbis_info_floor = new vorbis_info_floor0();
			vorbis_info_floor.order = (int)Bitwise.oggpack_read(opb, 8);
			vorbis_info_floor.rate = Bitwise.oggpack_read(opb, 16);
			vorbis_info_floor.barkmap = Bitwise.oggpack_read(opb, 16);
			vorbis_info_floor.ampbits = (int)Bitwise.oggpack_read(opb, 6);
			vorbis_info_floor.ampdB = (int)Bitwise.oggpack_read(opb, 8);
			vorbis_info_floor.numbooks = (int)(Bitwise.oggpack_read(opb, 4) + 1);
			if (vorbis_info_floor.order < 1)
			{
				return null;
			}
			if (vorbis_info_floor.rate < 1)
			{
				return null;
			}
			if (vorbis_info_floor.barkmap < 1)
			{
				return null;
			}
			if (vorbis_info_floor.numbooks < 1)
			{
				return null;
			}
			for (int i = 0; i < vorbis_info_floor.numbooks; i++)
			{
				vorbis_info_floor.books[i] = (int)Bitwise.oggpack_read(opb, 8);
				if (vorbis_info_floor.books[i] < 0 || vorbis_info_floor.books[i] >= codec_setup.books)
				{
					return null;
				}
				if (codec_setup.book_param[vorbis_info_floor.books[i]].maptype == 0)
				{
					return null;
				}
				if (codec_setup.book_param[vorbis_info_floor.books[i]].dim < 1)
				{
					return null;
				}
			}
			return vorbis_info_floor;
		}

		private static void floor0_map_lazy_init(Codec.vorbis_block vb, Codec.vorbis_info_floor infoX, vorbis_look_floor0 look)
		{
			if (look.linearmap[(int)vb.W] != null)
			{
				return;
			}
			Codec.vorbis_dsp_state vd = vb.vd;
			Codec.vorbis_info vi = vd.vi;
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			vorbis_info_floor0 vorbis_info_floor = (vorbis_info_floor0)infoX;
			int num = (int)vb.W;
			int num2 = (int)(codec_setup.blocksizes[num] / 2);
			float num3 = (float)look.ln / toBARK((float)vorbis_info_floor.rate / 2f);
			look.linearmap[num] = new int[num2 + 1];
			int i;
			for (i = 0; i < num2; i++)
			{
				int num4 = (int)Math.Floor(toBARK((float)vorbis_info_floor.rate / 2f / (float)num2 * (float)i) * num3);
				if (num4 >= look.ln)
				{
					num4 = look.ln - 1;
				}
				look.linearmap[num][i] = num4;
			}
			look.linearmap[num][i] = -1;
			look.n[num] = num2;
		}

		public static Codec.vorbis_look_floor floor0_look(Codec.vorbis_dsp_state vd, Codec.vorbis_info_floor i)
		{
			vorbis_info_floor0 vorbis_info_floor = (vorbis_info_floor0)i;
			vorbis_look_floor0 vorbis_look_floor = new vorbis_look_floor0();
			vorbis_look_floor.m = vorbis_info_floor.order;
			vorbis_look_floor.ln = (int)vorbis_info_floor.barkmap;
			vorbis_look_floor.vi = vorbis_info_floor;
			vorbis_look_floor.linearmap = new int[2][];
			return vorbis_look_floor;
		}

		public static float[] floor0_inverse1(Codec.vorbis_block vb, Codec.vorbis_look_floor i)
		{
			vorbis_look_floor0 vorbis_look_floor = (vorbis_look_floor0)i;
			vorbis_info_floor0 vi = vorbis_look_floor.vi;
			int num = (int)Bitwise.oggpack_read(vb.opb, vi.ampbits);
			if (num > 0)
			{
				long num2 = (1 << vi.ampbits) - 1;
				float num3 = (float)num / (float)num2 * (float)vi.ampdB;
				int num4 = (int)Bitwise.oggpack_read(vb.opb, Sharedbook._ilog(vi.numbooks));
				if (num4 != -1 && num4 < vi.numbooks)
				{
					Codec.codec_setup_info codec_setup = vb.vd.vi.codec_setup;
					Codebook.codebook codebook = codec_setup.fullbooks[vi.books[num4]];
					float num5 = 0f;
					float[] array = new float[(int)(vorbis_look_floor.m + codebook.dim + 1)];
					if (Codebook.vorbis_book_decodev_set(codebook, array, vb.opb, vorbis_look_floor.m) == -1)
					{
						return null;
					}
					int j = 0;
					while (j < vorbis_look_floor.m)
					{
						int num6 = 0;
						for (; j < vorbis_look_floor.m; j++)
						{
							if (num6 >= codebook.dim)
							{
								break;
							}
							array[j] += num5;
							num6++;
						}
						num5 = array[j - 1];
					}
					array[vorbis_look_floor.m] = num3;
					return array;
				}
			}
			return null;
		}

		public static int floor0_inverse2(Codec.vorbis_block vb, Codec.vorbis_look_floor i, float[] memo, CPtr.FloatPtr pout)
		{
			vorbis_look_floor0 vorbis_look_floor = (vorbis_look_floor0)i;
			vorbis_info_floor0 vi = vorbis_look_floor.vi;
			floor0_map_lazy_init(vb, vi, vorbis_look_floor);
			if (memo != null)
			{
				float amp = memo[vorbis_look_floor.m];
				Lsp.vorbis_lsp_to_curve(pout, vorbis_look_floor.linearmap[(int)vb.W], vorbis_look_floor.n[(int)vb.W], vorbis_look_floor.ln, memo, vorbis_look_floor.m, amp, vi.ampdB);
				return 1;
			}
			CPtr.FloatPtr.memset(pout, 0, pout.floats.Length - pout.offset);
			return 0;
		}
	}
}
