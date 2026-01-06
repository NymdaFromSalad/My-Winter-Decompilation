namespace TanjentOGG
{
	public class Info
	{
		public static void _v_readstring(Ogg.oggpack_buffer o, CPtr.BytePtr buf, int bytes)
		{
			CPtr.BytePtr bytePtr = new CPtr.BytePtr(buf);
			while (bytes-- != 0)
			{
				bytePtr.bytes[bytePtr.offset++] = (byte)Bitwise.oggpack_read(o, 8);
			}
		}

		public static void vorbis_comment_init(Codec.vorbis_comment vc)
		{
			vc.clear();
		}

		public static void vorbis_info_init(Codec.vorbis_info vi)
		{
			vi.clear();
			vi.codec_setup = new Codec.codec_setup_info();
		}

		public static int _vorbis_unpack_info(Codec.vorbis_info vi, Ogg.oggpack_buffer opb)
		{
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			if (codec_setup == null)
			{
				return Codec.OV_EFAULT;
			}
			vi.version = (int)Bitwise.oggpack_read(opb, 32);
			if (vi.version != 0)
			{
				return Codec.OV_EVERSION;
			}
			vi.channels = (int)Bitwise.oggpack_read(opb, 8);
			vi.rate = Bitwise.oggpack_read(opb, 32);
			vi.bitrate_upper = Bitwise.oggpack_read(opb, 32);
			vi.bitrate_nominal = Bitwise.oggpack_read(opb, 32);
			vi.bitrate_lower = Bitwise.oggpack_read(opb, 32);
			codec_setup.blocksizes[0] = 1 << (int)Bitwise.oggpack_read(opb, 4);
			codec_setup.blocksizes[1] = 1 << (int)Bitwise.oggpack_read(opb, 4);
			if (vi.rate < 1)
			{
				return Codec.OV_EBADHEADER;
			}
			if (vi.channels < 1)
			{
				return Codec.OV_EBADHEADER;
			}
			if (codec_setup.blocksizes[0] < 64)
			{
				return Codec.OV_EBADHEADER;
			}
			if (codec_setup.blocksizes[1] < codec_setup.blocksizes[0])
			{
				return Codec.OV_EBADHEADER;
			}
			if (codec_setup.blocksizes[1] > 8192)
			{
				return Codec.OV_EBADHEADER;
			}
			if (Bitwise.oggpack_read(opb, 1) != 1)
			{
				return Codec.OV_EBADHEADER;
			}
			return 0;
		}

		public static int _vorbis_unpack_comment(Codec.vorbis_comment vc, Ogg.oggpack_buffer opb)
		{
			int num = (int)Bitwise.oggpack_read(opb, 32);
			if (num < 0)
			{
				return Codec.OV_EBADHEADER;
			}
			if (num > opb.storage - 8)
			{
				return Codec.OV_EBADHEADER;
			}
			vc.vendor = new CPtr.BytePtr(new byte[num + 1]);
			_v_readstring(opb, vc.vendor, num);
			int num2 = (int)Bitwise.oggpack_read(opb, 32);
			if (num2 < 0)
			{
				return Codec.OV_EBADHEADER;
			}
			if (num2 > opb.storage - Bitwise.oggpack_bytes(opb) >> 2)
			{
				return Codec.OV_EBADHEADER;
			}
			vc.comments = num2;
			vc.user_comments = new CPtr.BytePtr[vc.comments + 1];
			vc.comment_lengths = new int[vc.comments + 1];
			for (num2 = 0; num2 < vc.comments; num2++)
			{
				int num3 = (int)Bitwise.oggpack_read(opb, 32);
				if (num3 < 0)
				{
					return Codec.OV_EBADHEADER;
				}
				if (num3 > opb.storage - Bitwise.oggpack_bytes(opb))
				{
					return Codec.OV_EBADHEADER;
				}
				vc.comment_lengths[num2] = num3;
				vc.user_comments[num2] = new CPtr.BytePtr(new byte[num3 + 1]);
				_v_readstring(opb, vc.user_comments[num2], num3);
			}
			if (Bitwise.oggpack_read(opb, 1) != 1)
			{
				return Codec.OV_EBADHEADER;
			}
			return 0;
		}

		private static int _vorbis_unpack_books(Registry r, Codec.vorbis_info vi, Ogg.oggpack_buffer opb)
		{
			Codec.codec_setup_info codec_setup = vi.codec_setup;
			if (codec_setup == null)
			{
				return Codec.OV_EFAULT;
			}
			codec_setup.books = (int)(Bitwise.oggpack_read(opb, 8) + 1);
			if (codec_setup.books <= 0)
			{
				return Codec.OV_EBADHEADER;
			}
			for (int i = 0; i < codec_setup.books; i++)
			{
				codec_setup.book_param[i] = Codebook.vorbis_staticbook_unpack(opb);
				if (codec_setup.book_param[i] == null)
				{
					return Codec.OV_EBADHEADER;
				}
			}
			int num = (int)(Bitwise.oggpack_read(opb, 6) + 1);
			if (num <= 0)
			{
				return Codec.OV_EBADHEADER;
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)Bitwise.oggpack_read(opb, 16);
				if (num2 < 0 || num2 >= Registry.VI_TIMEB)
				{
					return Codec.OV_EBADHEADER;
				}
			}
			codec_setup.floors = (int)(Bitwise.oggpack_read(opb, 6) + 1);
			if (codec_setup.floors <= 0)
			{
				return Codec.OV_EBADHEADER;
			}
			for (int i = 0; i < codec_setup.floors; i++)
			{
				codec_setup.floor_type[i] = (int)Bitwise.oggpack_read(opb, 16);
				if (codec_setup.floor_type[i] < 0 || codec_setup.floor_type[i] >= Registry.VI_FLOORB)
				{
					return Codec.OV_EBADHEADER;
				}
				codec_setup.floor_param[i] = r._floor_P[codec_setup.floor_type[i]].unpack(vi, opb);
				if (codec_setup.floor_param[i] == null)
				{
					return Codec.OV_EBADHEADER;
				}
			}
			codec_setup.residues = (int)(Bitwise.oggpack_read(opb, 6) + 1);
			if (codec_setup.residues <= 0)
			{
				return Codec.OV_EBADHEADER;
			}
			for (int i = 0; i < codec_setup.residues; i++)
			{
				codec_setup.residue_type[i] = (int)Bitwise.oggpack_read(opb, 16);
				if (codec_setup.residue_type[i] < 0 || codec_setup.residue_type[i] >= Registry.VI_RESB)
				{
					return Codec.OV_EBADHEADER;
				}
				codec_setup.residue_param[i] = r._residue_P[codec_setup.residue_type[i]].unpack(vi, opb);
				if (codec_setup.residue_param[i] == null)
				{
					return Codec.OV_EBADHEADER;
				}
			}
			codec_setup.maps = (int)(Bitwise.oggpack_read(opb, 6) + 1);
			if (codec_setup.maps <= 0)
			{
				return Codec.OV_EBADHEADER;
			}
			for (int i = 0; i < codec_setup.maps; i++)
			{
				codec_setup.map_type[i] = (int)Bitwise.oggpack_read(opb, 16);
				if (codec_setup.map_type[i] < 0 || codec_setup.map_type[i] >= Registry.VI_MAPB)
				{
					return Codec.OV_EBADHEADER;
				}
				codec_setup.map_param[i] = r._mapping_P[codec_setup.map_type[i]].unpack(vi, opb);
				if (codec_setup.map_param[i] == null)
				{
					return Codec.OV_EBADHEADER;
				}
			}
			codec_setup.modes = (int)(Bitwise.oggpack_read(opb, 6) + 1);
			if (codec_setup.modes <= 0)
			{
				return Codec.OV_EBADHEADER;
			}
			for (int i = 0; i < codec_setup.modes; i++)
			{
				codec_setup.mode_param[i] = new Codec.vorbis_info_mode();
				codec_setup.mode_param[i].blockflag = (int)Bitwise.oggpack_read(opb, 1);
				codec_setup.mode_param[i].windowtype = (int)Bitwise.oggpack_read(opb, 16);
				codec_setup.mode_param[i].transformtype = (int)Bitwise.oggpack_read(opb, 16);
				codec_setup.mode_param[i].mapping = (int)Bitwise.oggpack_read(opb, 8);
				if (codec_setup.mode_param[i].windowtype >= Registry.VI_WINDOWB)
				{
					return Codec.OV_EBADHEADER;
				}
				if (codec_setup.mode_param[i].transformtype >= Registry.VI_WINDOWB)
				{
					return Codec.OV_EBADHEADER;
				}
				if (codec_setup.mode_param[i].mapping >= codec_setup.maps)
				{
					return Codec.OV_EBADHEADER;
				}
				if (codec_setup.mode_param[i].mapping < 0)
				{
					return Codec.OV_EBADHEADER;
				}
			}
			if (Bitwise.oggpack_read(opb, 1) != 1)
			{
				return Codec.OV_EBADHEADER;
			}
			return 0;
		}

		public static int vorbis_synthesis_headerin(Registry r, Codec.vorbis_info vi, Codec.vorbis_comment vc, Ogg.ogg_packet op)
		{
			Ogg.oggpack_buffer oggpack_buffer = new Ogg.oggpack_buffer();
			if (op != null)
			{
				Bitwise.oggpack_readinit(oggpack_buffer, op.packet, (int)op.bytes);
				CPtr.BytePtr bytePtr = new CPtr.BytePtr(new byte[6]);
				int num = (int)Bitwise.oggpack_read(oggpack_buffer, 8);
				_v_readstring(oggpack_buffer, bytePtr, 6);
				byte[] data = new byte[6] { 118, 111, 114, 98, 105, 115 };
				if (CPtr.BytePtr.memcmp(bytePtr, new CPtr.BytePtr(data), 6) != 0)
				{
					return Codec.OV_ENOTVORBIS;
				}
				switch (num)
				{
				case 1:
					if (op.b_o_s == 0L)
					{
						return Codec.OV_EBADHEADER;
					}
					if (vi.rate != 0L)
					{
						return Codec.OV_EBADHEADER;
					}
					return _vorbis_unpack_info(vi, oggpack_buffer);
				case 3:
					if (vi.rate == 0L)
					{
						return Codec.OV_EBADHEADER;
					}
					return _vorbis_unpack_comment(vc, oggpack_buffer);
				case 5:
					if (vi.rate == 0L || vc.vendor == null)
					{
						return Codec.OV_EBADHEADER;
					}
					return _vorbis_unpack_books(r, vi, oggpack_buffer);
				default:
					return Codec.OV_EBADHEADER;
				}
			}
			return Codec.OV_EBADHEADER;
		}
	}
}
