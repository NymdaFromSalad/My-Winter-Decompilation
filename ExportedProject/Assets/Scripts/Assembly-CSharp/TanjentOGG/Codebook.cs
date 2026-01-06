namespace TanjentOGG
{
	public class Codebook
	{
		public class static_codebook
		{
			public long dim;

			public long entries;

			public byte[] lengthlist;

			public int maptype;

			public long q_min;

			public long q_delta;

			public int q_quant;

			public int q_sequencep;

			public long[] quantlist;

			public int allocedp;
		}

		public class codebook
		{
			public long dim;

			public long entries;

			public long used_entries;

			public static_codebook c;

			public float[] valuelist;

			public long[] codelist;

			public int[] dec_index;

			public byte[] dec_codelengths;

			public long[] dec_firsttable;

			public int dec_firsttablen;

			public int dec_maxlength;

			public int quantvals;

			public int minval;

			public int delta;

			public void clear()
			{
				dim = 0L;
				entries = 0L;
				used_entries = 0L;
				c = null;
				valuelist = null;
				codelist = null;
				dec_index = null;
				dec_codelengths = null;
				dec_firsttable = null;
				dec_firsttablen = 0;
				dec_maxlength = 0;
				quantvals = 0;
				minval = 0;
				delta = 0;
			}
		}

		public static static_codebook vorbis_staticbook_unpack(Ogg.oggpack_buffer opb)
		{
			static_codebook static_codebook = new static_codebook();
			static_codebook.allocedp = 1;
			if (Bitwise.oggpack_read(opb, 24) != 5653314)
			{
				return null;
			}
			static_codebook.dim = Bitwise.oggpack_read(opb, 16);
			static_codebook.entries = Bitwise.oggpack_read(opb, 24);
			if (static_codebook.entries == -1)
			{
				return null;
			}
			if (Sharedbook._ilog((int)static_codebook.dim) + Sharedbook._ilog((int)static_codebook.entries) > 24)
			{
				return null;
			}
			switch ((int)Bitwise.oggpack_read(opb, 1))
			{
			case 0:
			{
				long num5 = Bitwise.oggpack_read(opb, 1);
				if (static_codebook.entries * ((num5 != 0L) ? 1 : 5) + 7 >> 3 > opb.storage - Bitwise.oggpack_bytes(opb))
				{
					return null;
				}
				static_codebook.lengthlist = new byte[(int)static_codebook.entries];
				if (num5 != 0L)
				{
					for (long num2 = 0L; num2 < static_codebook.entries; num2++)
					{
						if (Bitwise.oggpack_read(opb, 1) != 0L)
						{
							long num6 = Bitwise.oggpack_read(opb, 5);
							if (num6 == -1)
							{
								return null;
							}
							static_codebook.lengthlist[(int)num2] = (byte)(num6 + 1);
						}
						else
						{
							static_codebook.lengthlist[(int)num2] = 0;
						}
					}
					break;
				}
				for (long num2 = 0L; num2 < static_codebook.entries; num2++)
				{
					long num7 = Bitwise.oggpack_read(opb, 5);
					if (num7 == -1)
					{
						return null;
					}
					static_codebook.lengthlist[(int)num2] = (byte)(num7 + 1);
				}
				break;
			}
			case 1:
			{
				long num = Bitwise.oggpack_read(opb, 5) + 1;
				if (num == 0L)
				{
					return null;
				}
				static_codebook.lengthlist = new byte[(int)static_codebook.entries];
				long num2 = 0L;
				while (num2 < static_codebook.entries)
				{
					long num3 = Bitwise.oggpack_read(opb, Sharedbook._ilog((int)(static_codebook.entries - num2)));
					if (num3 == -1)
					{
						return null;
					}
					if (num > 32 || num3 > static_codebook.entries - num2 || (num3 > 0 && num3 - 1 >> (int)(num - 1) > 1))
					{
						return null;
					}
					if (num > 32)
					{
						return null;
					}
					long num4 = 0L;
					while (num4 < num3)
					{
						static_codebook.lengthlist[(int)num2] = (byte)num;
						num4++;
						num2++;
					}
					num++;
				}
				break;
			}
			default:
				return null;
			}
			switch (static_codebook.maptype = (int)Bitwise.oggpack_read(opb, 4))
			{
			case 1:
			case 2:
			{
				static_codebook.q_min = Bitwise.oggpack_read(opb, 32);
				static_codebook.q_delta = Bitwise.oggpack_read(opb, 32);
				static_codebook.q_quant = (int)(Bitwise.oggpack_read(opb, 4) + 1);
				static_codebook.q_sequencep = (int)Bitwise.oggpack_read(opb, 1);
				if (static_codebook.q_sequencep == -1)
				{
					return null;
				}
				int num8 = 0;
				switch (static_codebook.maptype)
				{
				case 1:
					num8 = (int)((static_codebook.dim != 0L) ? Sharedbook._book_maptype1_quantvals(static_codebook) : 0);
					break;
				case 2:
					num8 = (int)(static_codebook.entries * static_codebook.dim);
					break;
				}
				if (num8 * static_codebook.q_quant + 7 >> 3 > opb.storage - Bitwise.oggpack_bytes(opb))
				{
					return null;
				}
				static_codebook.quantlist = new long[num8];
				for (long num2 = 0L; num2 < num8; num2++)
				{
					static_codebook.quantlist[(int)num2] = Bitwise.oggpack_read(opb, static_codebook.q_quant);
				}
				if (num8 != 0 && static_codebook.quantlist[num8 - 1] == -1)
				{
					return null;
				}
				break;
			}
			default:
				return null;
			case 0:
				break;
			}
			return static_codebook;
		}

		private static long bitreverse(long x)
		{
			x = ((x >> 16) & 0xFFFF) | ((x << 16) & 0xFFFF0000u);
			x = ((x >> 8) & 0xFF00FF) | ((x << 8) & 0xFF00FF00u);
			x = ((x >> 4) & 0xF0F0F0F) | ((x << 4) & 0xF0F0F0F0u);
			x = ((x >> 2) & 0x33333333) | ((x << 2) & 0xCCCCCCCCu);
			return ((x >> 1) & 0x55555555) | ((x << 1) & 0xAAAAAAAAu);
		}

		private static long decode_packed_entry_number(codebook book, Ogg.oggpack_buffer b)
		{
			int num = book.dec_maxlength;
			long num2 = Bitwise.oggpack_look(b, book.dec_firsttablen);
			long num4;
			long num5;
			if (num2 >= 0)
			{
				long num3 = (int)book.dec_firsttable[(int)num2];
				if ((num3 & 0x80000000u) == 0L)
				{
					Bitwise.oggpack_adv(b, book.dec_codelengths[(int)(num3 - 1)]);
					return num3 - 1;
				}
				num4 = (num3 >> 15) & 0x7FFF;
				num5 = book.used_entries - (num3 & 0x7FFF);
			}
			else
			{
				num4 = 0L;
				num5 = book.used_entries;
			}
			num2 = Bitwise.oggpack_look(b, num);
			while (num2 < 0 && num > 1)
			{
				num2 = Bitwise.oggpack_look(b, --num);
			}
			if (num2 < 0)
			{
				return -1L;
			}
			long num6 = bitreverse(num2);
			while (num5 - num4 > 1)
			{
				long num7 = num5 - num4 >> 1;
				long num8 = ((book.codelist[(int)(num4 + num7)] > num6) ? 1 : 0);
				num4 += num7 & (num8 - 1);
				num5 -= num7 & -num8;
			}
			if (book.dec_codelengths[(int)num4] <= num)
			{
				Bitwise.oggpack_adv(b, book.dec_codelengths[(int)num4]);
				return num4;
			}
			Bitwise.oggpack_adv(b, num);
			return -1L;
		}

		public static long vorbis_book_decode(codebook book, Ogg.oggpack_buffer b)
		{
			if (book.used_entries > 0)
			{
				long num = decode_packed_entry_number(book, b);
				if (num >= 0)
				{
					return book.dec_index[(int)num];
				}
			}
			return -1L;
		}

		public static long vorbis_book_decodevs_add(codebook book, CPtr.FloatPtr a, Ogg.oggpack_buffer b, int n)
		{
			if (book.used_entries > 0)
			{
				int num = (int)(n / book.dim);
				long[] array = new long[num];
				CPtr.FloatPtr[] array2 = new CPtr.FloatPtr[num];
				int i;
				for (i = 0; i < num; i++)
				{
					array[i] = decode_packed_entry_number(book, b);
					if (array[i] == -1)
					{
						return -1L;
					}
					array2[i] = new CPtr.FloatPtr(book.valuelist, (int)(array[i] * book.dim));
				}
				i = 0;
				int num2 = 0;
				while (i < book.dim)
				{
					for (int j = 0; j < num; j++)
					{
						a.floats[a.offset + num2 + j] += array2[j].floats[array2[j].offset + i];
					}
					i++;
					num2 += num;
				}
			}
			return 0L;
		}

		public static long vorbis_book_decodev_add(codebook book, CPtr.FloatPtr a, Ogg.oggpack_buffer b, int n)
		{
			if (book.used_entries > 0)
			{
				if (book.dim > 8)
				{
					int num = 0;
					while (num < n)
					{
						int num2 = (int)decode_packed_entry_number(book, b);
						if (num2 == -1)
						{
							return -1L;
						}
						CPtr.FloatPtr floatPtr = new CPtr.FloatPtr(book.valuelist, (int)(num2 * book.dim));
						int num3 = 0;
						while (num3 < book.dim)
						{
							a.floats[a.offset + num++] += floatPtr.floats[floatPtr.offset + num3++];
						}
					}
				}
				else
				{
					int num = 0;
					while (num < n)
					{
						int num2 = (int)decode_packed_entry_number(book, b);
						if (num2 == -1)
						{
							return -1L;
						}
						CPtr.FloatPtr floatPtr = new CPtr.FloatPtr(book.valuelist, (int)(num2 * book.dim));
						int num3 = 0;
						while (num3 < book.dim)
						{
							a.floats[a.offset + num++] += floatPtr.floats[floatPtr.offset + num3++];
						}
					}
				}
			}
			return 0L;
		}

		public static long vorbis_book_decodev_set(codebook book, float[] a, Ogg.oggpack_buffer b, int n)
		{
			if (book.used_entries > 0)
			{
				int num = 0;
				while (num < n)
				{
					int num2 = (int)decode_packed_entry_number(book, b);
					if (num2 == -1)
					{
						return -1L;
					}
					int num3 = 0;
					while (num < n && num3 < book.dim)
					{
						a[num++] = book.valuelist[(int)(num2 * book.dim + num3++)];
					}
				}
			}
			else
			{
				int num4 = 0;
				while (num4 < n)
				{
					a[num4++] = 0f;
				}
			}
			return 0L;
		}

		public static long vorbis_book_decodevv_add(codebook book, CPtr.FloatPtr[] a, long offset, int ch, Ogg.oggpack_buffer b, int n)
		{
			int num = 0;
			if (book.used_entries > 0)
			{
				long num2 = offset / ch;
				while (num2 < (offset + n) / ch)
				{
					long num3 = decode_packed_entry_number(book, b);
					if (num3 == -1)
					{
						return -1L;
					}
					CPtr.FloatPtr floatPtr = new CPtr.FloatPtr(book.valuelist, (int)(num3 * book.dim));
					for (long num4 = 0L; num4 < book.dim; num4++)
					{
						a[num].floats[(int)(a[num].offset + num2)] += floatPtr.floats[(int)(floatPtr.offset + num4)];
						num++;
						if (num == ch)
						{
							num = 0;
							num2++;
						}
					}
				}
			}
			return 0L;
		}
	}
}
