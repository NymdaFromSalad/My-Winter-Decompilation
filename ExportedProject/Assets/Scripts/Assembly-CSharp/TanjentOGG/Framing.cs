using System;

namespace TanjentOGG
{
	public class Framing
	{
		private static long LONG_MAX = 2147483647L;

		public static int ogg_page_version(Ogg.ogg_page og)
		{
			return og.header.bytes[og.header.offset + 4] & 0xFF;
		}

		public static int ogg_page_continued(Ogg.ogg_page og)
		{
			return og.header.bytes[og.header.offset + 5] & 1;
		}

		public static int ogg_page_bos(Ogg.ogg_page og)
		{
			return og.header.bytes[og.header.offset + 5] & 2;
		}

		public static int ogg_page_eos(Ogg.ogg_page og)
		{
			return og.header.bytes[og.header.offset + 5] & 4;
		}

		public static long ogg_page_granulepos(Ogg.ogg_page og)
		{
			CPtr.BytePtr header = og.header;
			long num = header.bytes[header.offset + 13] & 0xFF;
			num = (num << 8) | (header.bytes[header.offset + 12] & 0xFF);
			num = (num << 8) | (header.bytes[header.offset + 11] & 0xFF);
			num = (num << 8) | (header.bytes[header.offset + 10] & 0xFF);
			num = (num << 8) | (header.bytes[header.offset + 9] & 0xFF);
			num = (num << 8) | (header.bytes[header.offset + 8] & 0xFF);
			num = (num << 8) | (header.bytes[header.offset + 7] & 0xFF);
			return (num << 8) | (header.bytes[header.offset + 6] & 0xFF);
		}

		public static int ogg_page_serialno(Ogg.ogg_page og)
		{
			return (og.header.bytes[og.header.offset + 14] & 0xFF) | ((og.header.bytes[og.header.offset + 15] & 0xFF) << 8) | ((og.header.bytes[og.header.offset + 16] & 0xFF) << 16) | ((og.header.bytes[og.header.offset + 17] & 0xFF) << 24);
		}

		public static int ogg_page_pageno(Ogg.ogg_page og)
		{
			return (og.header.bytes[og.header.offset + 18] & 0xFF) | ((og.header.bytes[og.header.offset + 19] & 0xFF) << 8) | ((og.header.bytes[og.header.offset + 20] & 0xFF) << 16) | ((og.header.bytes[og.header.offset + 21] & 0xFF) << 24);
		}

		public static int ogg_stream_init(Ogg.ogg_stream_state os, int serialno)
		{
			if (os != null)
			{
				os.clear();
				os.body_storage = 16384L;
				os.lacing_storage = 1024L;
				os.body_data = CPtr.BytePtr.malloc(os.body_storage);
				os.lacing_vals = new int[(int)os.lacing_storage];
				os.granule_vals = new long[(int)os.lacing_storage];
				os.serialno = serialno;
				return 0;
			}
			return -1;
		}

		public static int ogg_stream_check(Ogg.ogg_stream_state os)
		{
			if (os == null || os.body_data == null)
			{
				return -1;
			}
			return 0;
		}

		public static int _os_body_expand(Ogg.ogg_stream_state os, long needed)
		{
			if (os.body_storage - needed <= os.body_fill)
			{
				if (os.body_storage > LONG_MAX - needed)
				{
					return -1;
				}
				long num = os.body_storage + needed;
				if (num < LONG_MAX - 1024)
				{
					num += 1024;
				}
				CPtr.BytePtr body_data = CPtr.BytePtr.realloc(os.body_data, (int)num);
				os.body_storage = num;
				os.body_data = body_data;
			}
			return 0;
		}

		public static int _os_lacing_expand(Ogg.ogg_stream_state os, long needed)
		{
			if (os.lacing_storage - needed <= os.lacing_fill)
			{
				if (os.lacing_storage > LONG_MAX - needed)
				{
					return -1;
				}
				long num = os.lacing_storage + needed;
				if (num < LONG_MAX - 32)
				{
					num += 32;
				}
				int[] array = new int[(int)num];
				Array.Copy(os.lacing_vals, 0, array, 0, os.lacing_vals.Length);
				os.lacing_vals = array;
				long[] array2 = new long[(int)num];
				Array.Copy(os.granule_vals, 0, array2, 0, os.granule_vals.Length);
				os.granule_vals = array2;
				os.lacing_storage = num;
			}
			return 0;
		}

		public static int ogg_sync_init(Ogg.ogg_sync_state oy)
		{
			if (oy != null)
			{
				oy.storage = -1;
				oy.clear();
			}
			return 0;
		}

		private static int ogg_sync_check(Ogg.ogg_sync_state oy)
		{
			if (oy.storage < 0)
			{
				return -1;
			}
			return 0;
		}

		public static CPtr.BytePtr ogg_sync_buffer(Ogg.ogg_sync_state oy, long size)
		{
			if (ogg_sync_check(oy) != 0)
			{
				return null;
			}
			if (oy.returned != 0)
			{
				oy.fill -= oy.returned;
				if (oy.fill > 0)
				{
					CPtr.BytePtr.memmove(oy.data, new CPtr.BytePtr(oy.data, oy.returned), oy.fill);
				}
				oy.returned = 0;
			}
			if (size > oy.storage - oy.fill)
			{
				long num = size + oy.fill + 4096;
				CPtr.BytePtr data = ((oy.data == null) ? CPtr.BytePtr.malloc(num) : CPtr.BytePtr.realloc(oy.data, (int)num));
				oy.data = data;
				oy.storage = (int)num;
			}
			return new CPtr.BytePtr(oy.data, oy.fill);
		}

		public static int ogg_sync_wrote(Ogg.ogg_sync_state oy, long bytes)
		{
			if (ogg_sync_check(oy) != 0)
			{
				return -1;
			}
			if (oy.fill + bytes > oy.storage)
			{
				return -1;
			}
			oy.fill += (int)bytes;
			return 0;
		}

		private static long ogg_sync_pageseek(Ogg.ogg_sync_state oy, Ogg.ogg_page og)
		{
			CPtr.BytePtr bytePtr = new CPtr.BytePtr(oy.data, oy.returned);
			long num = oy.fill - oy.returned;
			if (ogg_sync_check(oy) != 0)
			{
				return 0L;
			}
			if (oy.headerbytes == 0)
			{
				if (num < 27)
				{
					return 0L;
				}
				byte[] data = new byte[4] { 79, 103, 103, 83 };
				if (CPtr.BytePtr.memcmp(bytePtr, new CPtr.BytePtr(data), 4) != 0)
				{
					oy.headerbytes = 0;
					oy.bodybytes = 0;
					CPtr.BytePtr bytePtr2 = CPtr.BytePtr.memchr(new CPtr.BytePtr(bytePtr, 1), 79, (int)(num - 1));
					if (bytePtr2 == null)
					{
						bytePtr2 = new CPtr.BytePtr(oy.data, oy.fill);
					}
					oy.returned = bytePtr2.offset - oy.data.offset;
					return -(bytePtr2.offset - bytePtr.offset);
				}
				int num2 = (bytePtr.bytes[bytePtr.offset + 26] & 0xFF) + 27;
				if (num < num2)
				{
					return 0L;
				}
				for (int i = 0; i < (bytePtr.bytes[bytePtr.offset + 26] & 0xFF); i++)
				{
					oy.bodybytes += bytePtr.bytes[bytePtr.offset + 27 + i] & 0xFF;
				}
				oy.headerbytes = num2;
			}
			if (oy.bodybytes + oy.headerbytes > num)
			{
				return 0L;
			}
			bytePtr = new CPtr.BytePtr(oy.data, oy.returned);
			if (og != null)
			{
				og.header = bytePtr;
				og.header_len = oy.headerbytes;
				og.body = new CPtr.BytePtr(bytePtr, oy.headerbytes);
				og.body_len = oy.bodybytes;
			}
			oy.unsynced = 0;
			oy.returned += (int)(num = oy.headerbytes + oy.bodybytes);
			oy.headerbytes = 0;
			oy.bodybytes = 0;
			return num;
		}

		public static int ogg_sync_pageout(Ogg.ogg_sync_state oy, Ogg.ogg_page og)
		{
			if (ogg_sync_check(oy) != 0)
			{
				return 0;
			}
			do
			{
				long num = ogg_sync_pageseek(oy, og);
				if (num > 0)
				{
					return 1;
				}
				if (num == 0L)
				{
					return 0;
				}
			}
			while (oy.unsynced != 0);
			oy.unsynced = 1;
			return -1;
		}

		public static int ogg_stream_pagein(Ogg.ogg_stream_state os, Ogg.ogg_page og)
		{
			CPtr.BytePtr header = og.header;
			CPtr.BytePtr body = og.body;
			long num = og.body_len;
			int i = 0;
			int num2 = ogg_page_version(og);
			int num3 = ogg_page_continued(og);
			int num4 = ogg_page_bos(og);
			int num5 = ogg_page_eos(og);
			long num6 = ogg_page_granulepos(og);
			int num7 = ogg_page_serialno(og);
			long num8 = ogg_page_pageno(og);
			int num9 = header.bytes[header.offset + 26] & 0xFF;
			if (ogg_stream_check(os) != 0)
			{
				return -1;
			}
			long lacing_returned = os.lacing_returned;
			long body_returned = os.body_returned;
			if (body_returned != 0L)
			{
				os.body_fill -= body_returned;
				if (os.body_fill != 0L)
				{
					CPtr.BytePtr.memmove(os.body_data, new CPtr.BytePtr(os.body_data, (int)body_returned), (int)os.body_fill);
				}
				os.body_returned = 0L;
			}
			if (lacing_returned != 0L)
			{
				if (os.lacing_fill - lacing_returned != 0L)
				{
					int[] array = new int[os.lacing_vals.Length];
					long[] array2 = new long[os.granule_vals.Length];
					for (int j = 0; j < (int)(os.lacing_fill - lacing_returned); j++)
					{
						array[j] = os.lacing_vals[(int)(j + lacing_returned)];
						array2[j] = os.granule_vals[(int)(j + lacing_returned)];
					}
					os.lacing_vals = array;
					os.granule_vals = array2;
				}
				os.lacing_fill -= lacing_returned;
				os.lacing_packet -= lacing_returned;
				os.lacing_returned = 0L;
			}
			if (num7 != os.serialno)
			{
				return -1;
			}
			if (num2 > 0)
			{
				return -1;
			}
			if (_os_lacing_expand(os, num9 + 1) != 0)
			{
				return -1;
			}
			if (num8 != os.pageno)
			{
				for (int k = (int)os.lacing_packet; k < os.lacing_fill; k++)
				{
					os.body_fill -= os.lacing_vals[k] & 0xFF;
				}
				os.lacing_fill = os.lacing_packet;
				if (os.pageno != -1)
				{
					os.lacing_vals[(int)os.lacing_fill++] = 1024;
					os.lacing_packet++;
				}
			}
			if (num3 != 0 && (os.lacing_fill < 1 || os.lacing_vals[(int)(os.lacing_fill - 1)] == 1024))
			{
				num4 = 0;
				for (; i < num9; i++)
				{
					int num10 = header.bytes[header.offset + 27 + i] & 0xFF;
					body.offset += num10;
					num -= num10;
					if (num10 < 255)
					{
						i++;
						break;
					}
				}
			}
			if (num != 0L)
			{
				if (_os_body_expand(os, num) != 0)
				{
					return -1;
				}
				CPtr.BytePtr.memcpy(new CPtr.BytePtr(os.body_data, (int)os.body_fill), body, (int)num);
				os.body_fill += num;
			}
			int num11 = -1;
			while (i < num9)
			{
				int num12 = header.bytes[header.offset + 27 + i] & 0xFF;
				os.lacing_vals[(int)os.lacing_fill] = num12;
				os.granule_vals[(int)os.lacing_fill] = -1L;
				if (num4 != 0)
				{
					os.lacing_vals[(int)os.lacing_fill] |= 256;
					num4 = 0;
				}
				if (num12 < 255)
				{
					num11 = (int)os.lacing_fill;
				}
				os.lacing_fill++;
				i++;
				if (num12 < 255)
				{
					os.lacing_packet = os.lacing_fill;
				}
			}
			if (num11 != -1)
			{
				os.granule_vals[num11] = num6;
			}
			if (num5 != 0)
			{
				os.e_o_s = 1;
				if (os.lacing_fill > 0)
				{
					os.lacing_vals[(int)(os.lacing_fill - 1)] |= 512;
				}
			}
			os.pageno = num8 + 1;
			return 0;
		}

		private static int _packetout(Ogg.ogg_stream_state os, Ogg.ogg_packet op, int adv)
		{
			int num = (int)os.lacing_returned;
			if (os.lacing_packet <= num)
			{
				return 0;
			}
			if ((os.lacing_vals[num] & 0x400) != 0)
			{
				os.lacing_returned++;
				os.packetno++;
				return -1;
			}
			if (op == null && adv == 0)
			{
				return 1;
			}
			int num2 = os.lacing_vals[num] & 0xFF;
			long num3 = num2;
			int num4 = os.lacing_vals[num] & 0x200;
			int num5 = os.lacing_vals[num] & 0x100;
			while (num2 == 255)
			{
				int num6 = os.lacing_vals[++num];
				num2 = num6 & 0xFF;
				if ((num6 & 0x200) != 0)
				{
					num4 = 512;
				}
				num3 += num2;
			}
			if (op != null)
			{
				op.e_o_s = num4;
				op.b_o_s = num5;
				op.packet = new CPtr.BytePtr(os.body_data, (int)os.body_returned);
				op.packetno = os.packetno;
				op.granulepos = os.granule_vals[num];
				op.bytes = num3;
			}
			if (adv != 0)
			{
				os.body_returned += num3;
				os.lacing_returned = num + 1;
				os.packetno++;
			}
			return 1;
		}

		public static int ogg_stream_packetout(Ogg.ogg_stream_state os, Ogg.ogg_packet op)
		{
			if (ogg_stream_check(os) != 0)
			{
				return 0;
			}
			return _packetout(os, op, 1);
		}
	}
}
