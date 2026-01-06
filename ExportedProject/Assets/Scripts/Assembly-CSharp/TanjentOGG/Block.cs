namespace TanjentOGG
{
	public class Block
	{
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

		public static int vorbis_block_init(Codec.vorbis_dsp_state v, Codec.vorbis_block vb)
		{
			vb.clear();
			vb.vd = v;
			return 0;
		}

		private static int _vds_shared_init(Registry r, Codec.vorbis_dsp_state v, Codec.vorbis_info vi, int encp)
		{
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			if (codec_setup == null)
			{
				return 1;
			}
			int halfrate_flag = codec_setup.halfrate_flag;
			v.clear();
			Codec.private_state private_state = (v.backend_state = new Codec.private_state());
			v.vi = vi;
			private_state.modebits = ilog2(codec_setup.modes);
			private_state.transform[0, 0] = new Mdct.mdct_lookup();
			private_state.transform[1, 0] = new Mdct.mdct_lookup();
			Mdct.mdct_init(private_state.transform[0, 0], (int)(codec_setup.blocksizes[0] >> halfrate_flag));
			Mdct.mdct_init(private_state.transform[1, 0], (int)(codec_setup.blocksizes[1] >> halfrate_flag));
			private_state.window[0] = ilog2((int)codec_setup.blocksizes[0]) - 6;
			private_state.window[1] = ilog2((int)codec_setup.blocksizes[1]) - 6;
			if (codec_setup.fullbooks == null)
			{
				codec_setup.fullbooks = new Codebook.codebook[codec_setup.books];
				for (int i = 0; i < codec_setup.books; i++)
				{
					codec_setup.fullbooks[i] = new Codebook.codebook();
				}
				for (int j = 0; j < codec_setup.books; j++)
				{
					if (codec_setup.book_param[j] == null)
					{
						return -1;
					}
					if (Sharedbook.vorbis_book_init_decode(codec_setup.fullbooks[j], codec_setup.book_param[j]) != 0)
					{
						return -1;
					}
					codec_setup.book_param[j] = null;
				}
			}
			v.pcm_storage = (int)codec_setup.blocksizes[1];
			v.pcm = new CPtr.FloatPtr[vi.channels];
			v.pcmret = new CPtr.FloatPtr[vi.channels];
			for (int j = 0; j < vi.channels; j++)
			{
				v.pcm[j] = new CPtr.FloatPtr(new float[v.pcm_storage]);
			}
			v.lW = 0L;
			v.W = 0L;
			v.centerW = codec_setup.blocksizes[1] / 2;
			v.pcm_current = (int)v.centerW;
			private_state.flr = new Codec.vorbis_look_floor[codec_setup.floors];
			private_state.residue = new Codec.vorbis_look_residue[codec_setup.residues];
			for (int j = 0; j < codec_setup.floors; j++)
			{
				private_state.flr[j] = r._floor_P[codec_setup.floor_type[j]].look(v, codec_setup.floor_param[j]);
			}
			for (int j = 0; j < codec_setup.residues; j++)
			{
				private_state.residue[j] = r._residue_P[codec_setup.residue_type[j]].look(v, codec_setup.residue_param[j]);
			}
			return 0;
		}

		private static int vorbis_synthesis_restart(Codec.vorbis_dsp_state v)
		{
			Codec.vorbis_info vi = v.vi;
			if (v.backend_state == null)
			{
				return -1;
			}
			if (vi == null)
			{
				return -1;
			}
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			if (codec_setup == null)
			{
				return -1;
			}
			int halfrate_flag = codec_setup.halfrate_flag;
			v.centerW = codec_setup.blocksizes[1] >> halfrate_flag + 1;
			v.pcm_current = (int)(v.centerW >> halfrate_flag);
			v.pcm_returned = -1;
			v.granulepos = -1L;
			v.sequence = -1L;
			v.eofflag = 0;
			v.backend_state.sample_count = -1L;
			return 0;
		}

		public static int vorbis_synthesis_init(Registry r, Codec.vorbis_dsp_state v, Codec.vorbis_info vi)
		{
			if (_vds_shared_init(r, v, vi, 0) != 0)
			{
				return 1;
			}
			vorbis_synthesis_restart(v);
			return 0;
		}

		public static int vorbis_synthesis_blockin(Registry r, Codec.vorbis_dsp_state v, Codec.vorbis_block vb)
		{
			Codec.vorbis_info vi = v.vi;
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			Codec.private_state backend_state = v.backend_state;
			int halfrate_flag = codec_setup.halfrate_flag;
			if (vb == null)
			{
				return Codec.OV_EINVAL;
			}
			if (v.pcm_current > v.pcm_returned && v.pcm_returned != -1)
			{
				return Codec.OV_EINVAL;
			}
			v.lW = v.W;
			v.W = vb.W;
			v.nW = -1L;
			if (v.sequence == -1 || v.sequence + 1 != vb.sequence)
			{
				v.granulepos = -1L;
				backend_state.sample_count = -1L;
			}
			v.sequence = vb.sequence;
			if (vb.pcm != null)
			{
				int num = (int)(codec_setup.blocksizes[(int)v.W] >> halfrate_flag + 1);
				int num2 = (int)(codec_setup.blocksizes[0] >> halfrate_flag + 1);
				int num3 = (int)(codec_setup.blocksizes[1] >> halfrate_flag + 1);
				v.glue_bits += vb.glue_bits;
				v.time_bits += vb.time_bits;
				v.floor_bits += vb.floor_bits;
				v.res_bits += vb.res_bits;
				int num4;
				int num5;
				if (v.centerW != 0L)
				{
					num4 = num3;
					num5 = 0;
				}
				else
				{
					num4 = 0;
					num5 = num3;
				}
				for (int i = 0; i < vi.channels; i++)
				{
					if (v.lW != 0L)
					{
						if (v.W != 0L)
						{
							float[] array = r.vwin[backend_state.window[1] - halfrate_flag];
							CPtr.FloatPtr floatPtr = new CPtr.FloatPtr(v.pcm[i], num5);
							CPtr.FloatPtr floatPtr2 = new CPtr.FloatPtr(vb.pcm[i]);
							for (int j = 0; j < num3; j++)
							{
								floatPtr.floats[floatPtr.offset + j] = floatPtr.floats[floatPtr.offset + j] * array[num3 - j - 1] + floatPtr2.floats[floatPtr2.offset + j] * array[j];
							}
						}
						else
						{
							float[] array2 = r.vwin[backend_state.window[0] - halfrate_flag];
							CPtr.FloatPtr floatPtr3 = new CPtr.FloatPtr(v.pcm[i], num5 + num3 / 2 - num2 / 2);
							CPtr.FloatPtr floatPtr4 = new CPtr.FloatPtr(vb.pcm[i]);
							for (int j = 0; j < num2; j++)
							{
								floatPtr3.floats[floatPtr3.offset + j] = floatPtr3.floats[floatPtr3.offset + j] * array2[num2 - j - 1] + floatPtr4.floats[floatPtr4.offset + j] * array2[j];
							}
						}
					}
					else if (v.W != 0L)
					{
						float[] array3 = r.vwin[backend_state.window[0] - halfrate_flag];
						CPtr.FloatPtr floatPtr5 = new CPtr.FloatPtr(v.pcm[i], num5);
						CPtr.FloatPtr floatPtr6 = new CPtr.FloatPtr(vb.pcm[i], num3 / 2 - num2 / 2);
						int j;
						for (j = 0; j < num2; j++)
						{
							floatPtr5.floats[floatPtr5.offset + j] = floatPtr5.floats[floatPtr5.offset + j] * array3[num2 - j - 1] + floatPtr6.floats[floatPtr6.offset + j] * array3[j];
						}
						for (; j < num3 / 2 + num2 / 2; j++)
						{
							floatPtr5.floats[floatPtr5.offset + j] = floatPtr6.floats[floatPtr6.offset + j];
						}
					}
					else
					{
						float[] array4 = r.vwin[backend_state.window[0] - halfrate_flag];
						CPtr.FloatPtr floatPtr7 = new CPtr.FloatPtr(v.pcm[i], num5);
						CPtr.FloatPtr floatPtr8 = new CPtr.FloatPtr(vb.pcm[i]);
						for (int j = 0; j < num2; j++)
						{
							floatPtr7.floats[floatPtr7.offset + j] = floatPtr7.floats[floatPtr7.offset + j] * array4[num2 - j - 1] + floatPtr8.floats[floatPtr8.offset + j] * array4[j];
						}
					}
					CPtr.FloatPtr floatPtr9 = new CPtr.FloatPtr(v.pcm[i], num4);
					CPtr.FloatPtr floatPtr10 = new CPtr.FloatPtr(vb.pcm[i], num);
					for (int j = 0; j < num; j++)
					{
						floatPtr9.floats[floatPtr9.offset + j] = floatPtr10.floats[floatPtr10.offset + j];
					}
				}
				if (v.centerW != 0L)
				{
					v.centerW = 0L;
				}
				else
				{
					v.centerW = num3;
				}
				if (v.pcm_returned == -1)
				{
					v.pcm_returned = num4;
					v.pcm_current = num4;
				}
				else
				{
					v.pcm_returned = num5;
					v.pcm_current = (int)(num5 + (codec_setup.blocksizes[(int)v.lW] / 4 + codec_setup.blocksizes[(int)v.W] / 4 >> halfrate_flag));
				}
			}
			if (backend_state.sample_count == -1)
			{
				backend_state.sample_count = 0L;
			}
			else
			{
				backend_state.sample_count += codec_setup.blocksizes[(int)v.lW] / 4 + codec_setup.blocksizes[(int)v.W] / 4;
			}
			if (v.granulepos == -1)
			{
				if (vb.granulepos != -1)
				{
					v.granulepos = vb.granulepos;
					if (backend_state.sample_count > v.granulepos)
					{
						long num6 = backend_state.sample_count - vb.granulepos;
						if (num6 < 0)
						{
							num6 = 0L;
						}
						if (vb.eofflag != 0)
						{
							if (num6 > v.pcm_current - v.pcm_returned << halfrate_flag)
							{
								num6 = v.pcm_current - v.pcm_returned << halfrate_flag;
							}
							v.pcm_current -= (int)(num6 >> halfrate_flag);
						}
						else
						{
							v.pcm_returned += (int)(num6 >> halfrate_flag);
							if (v.pcm_returned > v.pcm_current)
							{
								v.pcm_returned = v.pcm_current;
							}
						}
					}
				}
			}
			else
			{
				v.granulepos += codec_setup.blocksizes[(int)v.lW] / 4 + codec_setup.blocksizes[(int)v.W] / 4;
				if (vb.granulepos != -1 && v.granulepos != vb.granulepos)
				{
					if (v.granulepos > vb.granulepos)
					{
						long num7 = v.granulepos - vb.granulepos;
						if (num7 != 0L && vb.eofflag != 0)
						{
							if (num7 > v.pcm_current - v.pcm_returned << halfrate_flag)
							{
								num7 = v.pcm_current - v.pcm_returned << halfrate_flag;
							}
							if (num7 < 0)
							{
								num7 = 0L;
							}
							v.pcm_current -= (int)(num7 >> halfrate_flag);
						}
					}
					v.granulepos = vb.granulepos;
				}
			}
			if (vb.eofflag != 0)
			{
				v.eofflag = 1;
			}
			return 0;
		}

		public static int vorbis_synthesis_pcmout(Codec.vorbis_dsp_state v, CPtr.FloatPtr[][] pcm)
		{
			Codec.vorbis_info vi = v.vi;
			if (v.pcm_returned > -1 && v.pcm_returned < v.pcm_current)
			{
				if (pcm != null)
				{
					for (int i = 0; i < vi.channels; i++)
					{
						v.pcmret[i] = new CPtr.FloatPtr(v.pcm[i], v.pcm_returned);
					}
					pcm[0] = v.pcmret;
				}
				return v.pcm_current - v.pcm_returned;
			}
			return 0;
		}

		public static int vorbis_synthesis_read(Codec.vorbis_dsp_state v, int n)
		{
			if (n != 0 && v.pcm_returned + n > v.pcm_current)
			{
				return Codec.OV_EINVAL;
			}
			v.pcm_returned += n;
			return 0;
		}
	}
}
