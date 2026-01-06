namespace TanjentOGG
{
	public class Res012
	{
		public class vorbis_info_residue0 : Codec.vorbis_info_residue
		{
			public long begin;

			public long end;

			public int grouping;

			public int partitions;

			public int partvals;

			public int groupbook;

			public int[] secondstages = new int[64];

			public int[] booklist = new int[512];
		}

		public class vorbis_look_residue0 : Codec.vorbis_look_residue
		{
			public vorbis_info_residue0 info;

			public int parts;

			public int stages;

			public Codebook.codebook[] fullbooks;

			public Codebook.codebook phrasebook;

			public Codebook.codebook[][] partbooks;

			public int partvals;

			public int[][] decodemap;
		}

		private static int ilog(int v)
		{
			int num = 0;
			while (v != 0)
			{
				num++;
				v >>= 1;
			}
			return num;
		}

		private static int icount(int v)
		{
			int num = 0;
			while (v != 0)
			{
				num += v & 1;
				v >>= 1;
			}
			return num;
		}

		public static Codec.vorbis_info_residue res0_unpack(Codec.vorbis_info vi, Ogg.oggpack_buffer opb)
		{
			int num = 0;
			vorbis_info_residue0 vorbis_info_residue = new vorbis_info_residue0();
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			vorbis_info_residue.begin = Bitwise.oggpack_read(opb, 24);
			vorbis_info_residue.end = Bitwise.oggpack_read(opb, 24);
			vorbis_info_residue.grouping = (int)(Bitwise.oggpack_read(opb, 24) + 1);
			vorbis_info_residue.partitions = (int)(Bitwise.oggpack_read(opb, 6) + 1);
			vorbis_info_residue.groupbook = (int)Bitwise.oggpack_read(opb, 8);
			if (vorbis_info_residue.groupbook < 0)
			{
				return null;
			}
			for (int i = 0; i < vorbis_info_residue.partitions; i++)
			{
				int num2 = (int)Bitwise.oggpack_read(opb, 3);
				int num3 = (int)Bitwise.oggpack_read(opb, 1);
				if (num3 < 0)
				{
					return null;
				}
				if (num3 != 0)
				{
					int num4 = (int)Bitwise.oggpack_read(opb, 5);
					if (num4 < 0)
					{
						return null;
					}
					num2 |= num4 << 3;
				}
				vorbis_info_residue.secondstages[i] = num2;
				num += icount(num2);
			}
			for (int i = 0; i < num; i++)
			{
				int num5 = (int)Bitwise.oggpack_read(opb, 8);
				if (num5 < 0)
				{
					return null;
				}
				vorbis_info_residue.booklist[i] = num5;
			}
			if (vorbis_info_residue.groupbook >= codec_setup.books)
			{
				return null;
			}
			for (int i = 0; i < num; i++)
			{
				if (vorbis_info_residue.booklist[i] >= codec_setup.books)
				{
					return null;
				}
				if (codec_setup.book_param[vorbis_info_residue.booklist[i]].maptype == 0)
				{
					return null;
				}
			}
			int num6 = (int)codec_setup.book_param[vorbis_info_residue.groupbook].entries;
			int num7 = (int)codec_setup.book_param[vorbis_info_residue.groupbook].dim;
			int num8 = 1;
			if (num7 < 1)
			{
				return null;
			}
			while (num7 > 0)
			{
				num8 *= vorbis_info_residue.partitions;
				if (num8 > num6)
				{
					return null;
				}
				num7--;
			}
			vorbis_info_residue.partvals = num8;
			return vorbis_info_residue;
		}

		public static Codec.vorbis_look_residue res0_look(Codec.vorbis_dsp_state vd, Codec.vorbis_info_residue vr)
		{
			vorbis_info_residue0 vorbis_info_residue = (vorbis_info_residue0)vr;
			vorbis_look_residue0 vorbis_look_residue = new vorbis_look_residue0();
			Codec.codec_setup_info codec_setup = vd.vi.codec_setup;
			int num = 0;
			int num2 = 0;
			vorbis_look_residue.info = vorbis_info_residue;
			vorbis_look_residue.parts = vorbis_info_residue.partitions;
			vorbis_look_residue.fullbooks = codec_setup.fullbooks;
			vorbis_look_residue.phrasebook = codec_setup.fullbooks[vorbis_info_residue.groupbook];
			int num3 = (int)vorbis_look_residue.phrasebook.dim;
			vorbis_look_residue.partbooks = new Codebook.codebook[vorbis_look_residue.parts][];
			for (int i = 0; i < vorbis_look_residue.parts; i++)
			{
				int num4 = ilog(vorbis_info_residue.secondstages[i]);
				if (num4 == 0)
				{
					continue;
				}
				if (num4 > num2)
				{
					num2 = num4;
				}
				vorbis_look_residue.partbooks[i] = new Codebook.codebook[num4];
				for (int j = 0; j < num4; j++)
				{
					if ((vorbis_info_residue.secondstages[i] & (1 << j)) != 0)
					{
						vorbis_look_residue.partbooks[i][j] = codec_setup.fullbooks[vorbis_info_residue.booklist[num++]];
					}
				}
			}
			vorbis_look_residue.partvals = 1;
			for (int i = 0; i < num3; i++)
			{
				vorbis_look_residue.partvals *= vorbis_look_residue.parts;
			}
			vorbis_look_residue.stages = num2;
			vorbis_look_residue.decodemap = new int[vorbis_look_residue.partvals][];
			for (int i = 0; i < vorbis_look_residue.partvals; i++)
			{
				long num5 = i;
				long num6 = vorbis_look_residue.partvals / vorbis_look_residue.parts;
				vorbis_look_residue.decodemap[i] = new int[num3];
				for (int j = 0; j < num3; j++)
				{
					long num7 = num5 / num6;
					num5 -= num7 * num6;
					num6 /= vorbis_look_residue.parts;
					vorbis_look_residue.decodemap[i][j] = (int)num7;
				}
			}
			return vorbis_look_residue;
		}

		private static int _01inverse(Codec.vorbis_block vb, Codec.vorbis_look_residue vl, CPtr.FloatPtr[] pin, int ch, bool use_decodevs)
		{
			vorbis_look_residue0 vorbis_look_residue = (vorbis_look_residue0)vl;
			vorbis_info_residue0 info = vorbis_look_residue.info;
			int grouping = info.grouping;
			int num = (int)vorbis_look_residue.phrasebook.dim;
			int num2 = vb.pcmend >> 1;
			int num3 = (int)((info.end >= num2) ? num2 : info.end);
			int num4 = (int)(num3 - info.begin);
			if (num4 > 0)
			{
				int num5 = num4 / grouping;
				int num6 = (num5 + num - 1) / num;
				int[][][] array = new int[ch][][];
				for (long num7 = 0L; num7 < ch; num7++)
				{
					array[(int)num7] = new int[num6][];
				}
				for (long num8 = 0L; num8 < vorbis_look_residue.stages; num8++)
				{
					long num9 = 0L;
					long num10 = 0L;
					while (num9 < num5)
					{
						if (num8 == 0L)
						{
							for (long num7 = 0L; num7 < ch; num7++)
							{
								int num11 = (int)Codebook.vorbis_book_decode(vorbis_look_residue.phrasebook, vb.opb);
								if (num11 == -1 || num11 >= info.partvals)
								{
									return 0;
								}
								array[(int)num7][(int)num10] = vorbis_look_residue.decodemap[num11];
								if (array[(int)num7][(int)num10] == null)
								{
									return 0;
								}
							}
						}
						long num12 = 0L;
						while (num12 < num && num9 < num5)
						{
							for (long num7 = 0L; num7 < ch; num7++)
							{
								long num13 = info.begin + num9 * grouping;
								if ((info.secondstages[array[(int)num7][(int)num10][(int)num12]] & (1 << (int)num8)) == 0)
								{
									continue;
								}
								Codebook.codebook codebook = vorbis_look_residue.partbooks[array[(int)num7][(int)num10][(int)num12]][(int)num8];
								if (codebook == null)
								{
									continue;
								}
								if (use_decodevs)
								{
									if (Codebook.vorbis_book_decodevs_add(codebook, new CPtr.FloatPtr(pin[(int)num7], (int)num13), vb.opb, grouping) == -1)
									{
										return 0;
									}
								}
								else if (Codebook.vorbis_book_decodev_add(codebook, new CPtr.FloatPtr(pin[(int)num7], (int)num13), vb.opb, grouping) == -1)
								{
									return 0;
								}
							}
							num12++;
							num9++;
						}
						num10++;
					}
				}
			}
			return 0;
		}

		public static int res0_inverse(Codec.vorbis_block vb, Codec.vorbis_look_residue vl, CPtr.FloatPtr[] pin, int[] nonzero, int ch)
		{
			int num = 0;
			for (int i = 0; i < ch; i++)
			{
				if (nonzero[i] != 0)
				{
					pin[num++] = pin[i];
				}
			}
			if (num != 0)
			{
				return _01inverse(vb, vl, pin, num, true);
			}
			return 0;
		}

		public static int res1_inverse(Codec.vorbis_block vb, Codec.vorbis_look_residue vl, CPtr.FloatPtr[] pin, int[] nonzero, int ch)
		{
			int num = 0;
			for (int i = 0; i < ch; i++)
			{
				if (nonzero[i] != 0)
				{
					pin[num++] = pin[i];
				}
			}
			if (num != 0)
			{
				return _01inverse(vb, vl, pin, num, false);
			}
			return 0;
		}

		public static int res2_inverse(Codec.vorbis_block vb, Codec.vorbis_look_residue vl, CPtr.FloatPtr[] pin, int[] nonzero, int ch)
		{
			vorbis_look_residue0 vorbis_look_residue = (vorbis_look_residue0)vl;
			vorbis_info_residue0 info = vorbis_look_residue.info;
			int grouping = info.grouping;
			int num = (int)vorbis_look_residue.phrasebook.dim;
			int num2 = vb.pcmend * ch >> 1;
			int num3 = (int)((info.end >= num2) ? num2 : info.end);
			int num4 = (int)(num3 - info.begin);
			if (num4 > 0)
			{
				int num5 = num4 / grouping;
				int num6 = (num5 + num - 1) / num;
				int[][] array = new int[num6][];
				long num7;
				for (num7 = 0L; num7 < ch && nonzero[(int)num7] == 0; num7++)
				{
				}
				if (num7 == ch)
				{
					return 0;
				}
				for (long num8 = 0L; num8 < vorbis_look_residue.stages; num8++)
				{
					num7 = 0L;
					long num9 = 0L;
					while (num7 < num5)
					{
						if (num8 == 0L)
						{
							int num10 = (int)Codebook.vorbis_book_decode(vorbis_look_residue.phrasebook, vb.opb);
							if (num10 == -1 || num10 >= info.partvals)
							{
								return 0;
							}
							array[(int)num9] = vorbis_look_residue.decodemap[num10];
							if (array[(int)num9] == null)
							{
								return 0;
							}
						}
						long num11 = 0L;
						while (num11 < num && num7 < num5)
						{
							if ((info.secondstages[array[(int)num9][(int)num11]] & (1 << (int)num8)) != 0)
							{
								Codebook.codebook codebook = vorbis_look_residue.partbooks[array[(int)num9][(int)num11]][(int)num8];
								if (codebook != null && Codebook.vorbis_book_decodevv_add(codebook, pin, num7 * grouping + info.begin, ch, vb.opb, grouping) == -1)
								{
									return 0;
								}
							}
							num11++;
							num7++;
						}
						num9++;
					}
				}
			}
			return 0;
		}
	}
}
