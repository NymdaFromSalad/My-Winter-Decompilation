namespace TanjentOGG
{
	public class Mapping0
	{
		private static int ilog(int v)
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

		public static Codec.vorbis_info_mapping mapping0_unpack(Codec.vorbis_info vi, Ogg.oggpack_buffer opb)
		{
			Registry.vorbis_info_mapping0 vorbis_info_mapping = new Registry.vorbis_info_mapping0();
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			vorbis_info_mapping.clear();
			int num = (int)Bitwise.oggpack_read(opb, 1);
			if (num < 0)
			{
				return null;
			}
			if (num != 0)
			{
				vorbis_info_mapping.submaps = (int)(Bitwise.oggpack_read(opb, 4) + 1);
				if (vorbis_info_mapping.submaps <= 0)
				{
					return null;
				}
			}
			else
			{
				vorbis_info_mapping.submaps = 1;
			}
			num = (int)Bitwise.oggpack_read(opb, 1);
			if (num < 0)
			{
				return null;
			}
			if (num != 0)
			{
				vorbis_info_mapping.coupling_steps = (int)(Bitwise.oggpack_read(opb, 8) + 1);
				if (vorbis_info_mapping.coupling_steps <= 0)
				{
					return null;
				}
				for (int i = 0; i < vorbis_info_mapping.coupling_steps; i++)
				{
					int num2 = (vorbis_info_mapping.coupling_mag[i] = (int)Bitwise.oggpack_read(opb, ilog(vi.channels)));
					int num3 = (vorbis_info_mapping.coupling_ang[i] = (int)Bitwise.oggpack_read(opb, ilog(vi.channels)));
					if (num2 < 0 || num3 < 0 || num2 == num3 || num2 >= vi.channels || num3 >= vi.channels)
					{
						return null;
					}
				}
			}
			if (Bitwise.oggpack_read(opb, 2) != 0L)
			{
				return null;
			}
			if (vorbis_info_mapping.submaps > 1)
			{
				for (int i = 0; i < vi.channels; i++)
				{
					vorbis_info_mapping.chmuxlist[i] = (int)Bitwise.oggpack_read(opb, 4);
					if (vorbis_info_mapping.chmuxlist[i] >= vorbis_info_mapping.submaps || vorbis_info_mapping.chmuxlist[i] < 0)
					{
						return null;
					}
				}
			}
			for (int i = 0; i < vorbis_info_mapping.submaps; i++)
			{
				Bitwise.oggpack_read(opb, 8);
				vorbis_info_mapping.floorsubmap[i] = (int)Bitwise.oggpack_read(opb, 8);
				if (vorbis_info_mapping.floorsubmap[i] >= codec_setup.floors || vorbis_info_mapping.floorsubmap[i] < 0)
				{
					return null;
				}
				vorbis_info_mapping.residuesubmap[i] = (int)Bitwise.oggpack_read(opb, 8);
				if (vorbis_info_mapping.residuesubmap[i] >= codec_setup.residues || vorbis_info_mapping.residuesubmap[i] < 0)
				{
					return null;
				}
			}
			return vorbis_info_mapping;
		}

		public static int mapping0_inverse(Registry r, Codec.vorbis_block vb, Codec.vorbis_info_mapping l)
		{
			Codec.vorbis_dsp_state vd = vb.vd;
			Codec.vorbis_info vi = vd.vi;
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			Codec.private_state backend_state = vd.backend_state;
			Registry.vorbis_info_mapping0 vorbis_info_mapping = (Registry.vorbis_info_mapping0)l;
			long num = (vb.pcmend = (int)codec_setup.blocksizes[(int)vb.W]);
			CPtr.FloatPtr[] array = new CPtr.FloatPtr[vi.channels];
			int[] array2 = new int[vi.channels];
			int[] array3 = new int[vi.channels];
			float[][] array4 = new float[vi.channels][];
			for (int i = 0; i < vi.channels; i++)
			{
				int num2 = vorbis_info_mapping.chmuxlist[i];
				array4[i] = r._floor_P[codec_setup.floor_type[vorbis_info_mapping.floorsubmap[num2]]].inverse1(vb, backend_state.flr[vorbis_info_mapping.floorsubmap[num2]]);
				if (array4[i] != null)
				{
					array3[i] = 1;
				}
				else
				{
					array3[i] = 0;
				}
				CPtr.FloatPtr.memset(vb.pcm[i], 0, num / 2);
			}
			for (int i = 0; i < vorbis_info_mapping.coupling_steps; i++)
			{
				if (array3[vorbis_info_mapping.coupling_mag[i]] != 0 || array3[vorbis_info_mapping.coupling_ang[i]] != 0)
				{
					array3[vorbis_info_mapping.coupling_mag[i]] = 1;
					array3[vorbis_info_mapping.coupling_ang[i]] = 1;
				}
			}
			for (int i = 0; i < vorbis_info_mapping.submaps; i++)
			{
				int num3 = 0;
				for (int j = 0; j < vi.channels; j++)
				{
					if (vorbis_info_mapping.chmuxlist[j] == i)
					{
						if (array3[j] != 0)
						{
							array2[num3] = 1;
						}
						else
						{
							array2[num3] = 0;
						}
						array[num3++] = new CPtr.FloatPtr(vb.pcm[j]);
					}
				}
				r._residue_P[codec_setup.residue_type[vorbis_info_mapping.residuesubmap[i]]].inverse(vb, backend_state.residue[vorbis_info_mapping.residuesubmap[i]], array, array2, num3);
			}
			for (int i = vorbis_info_mapping.coupling_steps - 1; i >= 0; i--)
			{
				CPtr.FloatPtr floatPtr = vb.pcm[vorbis_info_mapping.coupling_mag[i]];
				CPtr.FloatPtr floatPtr2 = vb.pcm[vorbis_info_mapping.coupling_ang[i]];
				for (int j = 0; j < num / 2; j++)
				{
					float num4 = floatPtr.floats[floatPtr.offset + j];
					float num5 = floatPtr2.floats[floatPtr2.offset + j];
					if (num4 > 0f)
					{
						if (num5 > 0f)
						{
							floatPtr.floats[floatPtr.offset + j] = num4;
							floatPtr2.floats[floatPtr2.offset + j] = num4 - num5;
						}
						else
						{
							floatPtr2.floats[floatPtr2.offset + j] = num4;
							floatPtr.floats[floatPtr.offset + j] = num4 + num5;
						}
					}
					else if (num5 > 0f)
					{
						floatPtr.floats[floatPtr.offset + j] = num4;
						floatPtr2.floats[floatPtr2.offset + j] = num4 + num5;
					}
					else
					{
						floatPtr2.floats[floatPtr2.offset + j] = num4;
						floatPtr.floats[floatPtr.offset + j] = num4 - num5;
					}
				}
			}
			for (int i = 0; i < vi.channels; i++)
			{
				CPtr.FloatPtr pout = new CPtr.FloatPtr(vb.pcm[i]);
				int num6 = vorbis_info_mapping.chmuxlist[i];
				r._floor_P[codec_setup.floor_type[vorbis_info_mapping.floorsubmap[num6]]].inverse2(vb, backend_state.flr[vorbis_info_mapping.floorsubmap[num6]], array4[i], pout);
			}
			for (int i = 0; i < vi.channels; i++)
			{
				CPtr.FloatPtr floatPtr3 = new CPtr.FloatPtr(vb.pcm[i]);
				Mdct.mdct_backward(backend_state.transform[(int)vb.W, 0], floatPtr3, floatPtr3);
			}
			return 0;
		}
	}
}
