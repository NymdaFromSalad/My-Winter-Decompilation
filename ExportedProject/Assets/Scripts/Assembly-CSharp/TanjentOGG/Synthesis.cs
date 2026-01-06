namespace TanjentOGG
{
	public class Synthesis
	{
		public static int vorbis_synthesis(Registry r, Codec.vorbis_block vb, Ogg.ogg_packet op)
		{
			Codec.vorbis_dsp_state vorbis_dsp_state = ((vb == null) ? null : vb.vd);
			Codec.private_state private_state = ((vorbis_dsp_state == null) ? null : vorbis_dsp_state.backend_state);
			Codec.vorbis_info vorbis_info = ((vorbis_dsp_state == null) ? null : vorbis_dsp_state.vi);
			Codec.codec_setup_info codec_setup_info = ((vorbis_info == null) ? null : vorbis_info.codec_setup);
			Ogg.oggpack_buffer oggpack_buffer = ((vb == null) ? null : vb.opb);
			if (vorbis_dsp_state == null || private_state == null || vorbis_info == null || codec_setup_info == null || oggpack_buffer == null)
			{
				return Codec.OV_EBADPACKET;
			}
			Bitwise.oggpack_readinit(oggpack_buffer, op.packet, (int)op.bytes);
			if (Bitwise.oggpack_read(oggpack_buffer, 1) != 0L)
			{
				return Codec.OV_ENOTAUDIO;
			}
			int num = (int)Bitwise.oggpack_read(oggpack_buffer, private_state.modebits);
			if (num == -1)
			{
				return Codec.OV_EBADPACKET;
			}
			vb.mode = num;
			if (codec_setup_info.mode_param[num] == null)
			{
				return Codec.OV_EBADPACKET;
			}
			vb.W = codec_setup_info.mode_param[num].blockflag;
			if (vb.W != 0L)
			{
				vb.lW = Bitwise.oggpack_read(oggpack_buffer, 1);
				vb.nW = Bitwise.oggpack_read(oggpack_buffer, 1);
				if (vb.nW == -1)
				{
					return Codec.OV_EBADPACKET;
				}
			}
			else
			{
				vb.lW = 0L;
				vb.nW = 0L;
			}
			vb.granulepos = op.granulepos;
			vb.sequence = op.packetno;
			vb.eofflag = (int)op.e_o_s;
			vb.pcmend = (int)codec_setup_info.blocksizes[(int)vb.W];
			vb.pcm = new CPtr.FloatPtr[vorbis_info.channels];
			for (int i = 0; i < vorbis_info.channels; i++)
			{
				vb.pcm[i] = new CPtr.FloatPtr(new float[vb.pcmend]);
			}
			int num2 = codec_setup_info.map_type[codec_setup_info.mode_param[num].mapping];
			return r._mapping_P[num2].inverse(r, vb, codec_setup_info.map_param[codec_setup_info.mode_param[num].mapping]);
		}
	}
}
